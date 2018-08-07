﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Discord;
using TheDialgaTeam.Discord.Bot.Model.Discord;
using TheDialgaTeam.Discord.Bot.Model.Discord.Command;
using TheDialgaTeam.Discord.Bot.Model.SQLite.Table;
using TheDialgaTeam.Discord.Bot.Service.Logger;
using TheDialgaTeam.Discord.Bot.Service.SQLite;

namespace TheDialgaTeam.Discord.Bot.Service.Discord
{
    internal class DiscordAppService
    {
        private LoggerService LoggerService { get; }

        private SQLiteService SQLiteService { get; }

        private Task BackgroundTask { get; }

        private List<DiscordAppInstance> DiscordAppInstances { get; } = new List<DiscordAppInstance>();

        private Stopwatch Stopwatch { get; } = new Stopwatch();

        private ConcurrentQueue<(ulong clientId, Action<CommandExecuteResult> callbackAction)> StartDiscordAppQueue { get; } = new ConcurrentQueue<(ulong clientId, Action<CommandExecuteResult> callbackAction)>();

        private ConcurrentQueue<(ulong clientId, Action<CommandExecuteResult> callbackAction)> StopDiscordAppQueue { get; } = new ConcurrentQueue<(ulong clientId, Action<CommandExecuteResult> callbackAction)>();

        private ConcurrentQueue<(Func<List<DiscordAppInstance>, Task> callbackFunc, Action<bool> callbackAction)> GeneralQueue { get; } = new ConcurrentQueue<(Func<List<DiscordAppInstance>, Task> callbackFunc, Action<bool> callbackAction)>();

        public DiscordAppService(LoggerService loggerService, SQLiteService sqliteService)
        {
            LoggerService = loggerService;
            SQLiteService = sqliteService;
            BackgroundTask = DiscordAppServiceTask();
        }

        public async Task<CommandExecuteResult> StartDiscordAppAsync(ulong clientId)
        {
            var taskEnded = false;
            CommandExecuteResult result = null;

            StartDiscordAppQueue.Enqueue((clientId, callbackResult =>
            {
                taskEnded = true;
                result = callbackResult;
            }));

            while (!taskEnded)
                await Task.Delay(1).ConfigureAwait(false);

            return result;
        }

        public async Task<CommandExecuteResult> StopDiscordAppAsync(ulong clientId)
        {
            var taskEnded = false;
            CommandExecuteResult result = null;

            StopDiscordAppQueue.Enqueue((clientId, callbackResult =>
            {
                taskEnded = true;
                result = callbackResult;
            }));

            while (!taskEnded)
                await Task.Delay(1).ConfigureAwait(false);

            return result;
        }

        public async Task RequestDiscordAppInstanceAsync(Func<List<DiscordAppInstance>, Task> callbackFunc)
        {
            var taskEnded = false;

            GeneralQueue.Enqueue((callbackFunc, result => taskEnded = result));

            while (!taskEnded)
                await Task.Delay(1).ConfigureAwait(false);
        }

        private Task DiscordAppServiceTask()
        {
            return Task.Factory.StartNew(async () =>
            {
                Stopwatch.Start();

                while (true)
                {
                    // Process start discord app queue.
                    while (StartDiscordAppQueue.Count > 0)
                    {
                        if (StartDiscordAppQueue.TryDequeue(out var item))
                            await StartDiscordAppInstanceAsync(item.clientId, item.callbackAction);
                        else
                            await WaitForNextSecondAsync().ConfigureAwait(false);
                    }

                    // Process stop discord app queue.
                    while (StopDiscordAppQueue.Count > 0)
                    {
                        if (StopDiscordAppQueue.TryDequeue(out var item))
                            await StopDiscordAppInstanceAsync(item.clientId, item.callbackAction);
                        else
                            await WaitForNextSecondAsync().ConfigureAwait(false);
                    }

                    // Process General Task on queue.
                    while (GeneralQueue.Count > 0)
                    {
                        if (GeneralQueue.TryDequeue(out var item))
                        {
                            try
                            {
                                await item.callbackFunc.Invoke(DiscordAppInstances).ConfigureAwait(false);
                            }
                            catch (Exception ex)
                            {
                                await LoggerService.LogErrorMessageAsync(ex);
                            }

                            item.callbackAction?.Invoke(true);
                        }
                        else
                            await WaitForNextSecondAsync().ConfigureAwait(false);
                    }

                    foreach (var discordAppInstance in DiscordAppInstances)
                    {
                        // check if discord app is verified.
                        if (!discordAppInstance.IsVerified)
                        {
                            if (discordAppInstance.DiscordShardedClient.CurrentUser == null)
                                continue;

                            var clientIdString = discordAppInstance.ClientId.ToString();

                            if (discordAppInstance.ClientId != discordAppInstance.DiscordShardedClient.CurrentUser.Id)
                            {
                                await LoggerService.LogMessageAsync(discordAppInstance.DiscordShardedClient, new LogMessage(LogSeverity.Error, "", "Discord App client id mismatch found! Forced application to stop!")).ConfigureAwait(false);
                                await StopDiscordAppInstanceAsync(discordAppInstance.ClientId).ConfigureAwait(false);
                                await SQLiteService.SQLiteAsyncConnection.Table<DiscordAppTable>().DeleteAsync(a => a.ClientId == clientIdString).ConfigureAwait(false);
                                continue;
                            }

                            var discordAppTable = await SQLiteService.SQLiteAsyncConnection.Table<DiscordAppTable>().Where(a => a.ClientId == clientIdString).FirstOrDefaultAsync().ConfigureAwait(false);
                            var discordAppInstanceInfo = await discordAppInstance.DiscordShardedClient.GetApplicationInfoAsync().ConfigureAwait(false);

                            discordAppTable.AppName = discordAppInstanceInfo.Name;
                            discordAppTable.AppDescription = discordAppInstanceInfo.Description;
                            discordAppTable.LastUpdateCheck = DateTimeOffset.Now;

                            await SQLiteService.SQLiteAsyncConnection.UpdateAsync(discordAppTable).ConfigureAwait(false);

                            discordAppInstance.IsVerified = true;
                        }

                        if (discordAppInstance.NextCheck == null)
                            discordAppInstance.NextCheck = DateTimeOffset.Now.AddMinutes(15);

                        if (DateTimeOffset.Now < discordAppInstance.NextCheck)
                            continue;

                        // Check if discord app is logged in.
                        try
                        {
                            if (discordAppInstance.DiscordShardedClient.LoginState == LoginState.LoggingOut || discordAppInstance.DiscordShardedClient.LoginState == LoginState.LoggedOut)
                                await discordAppInstance.DiscordAppLoginAsync().ConfigureAwait(false);
                        }
                        catch (Exception ex)
                        {
                            await LoggerService.LogErrorMessageAsync(ex);
                        }

                        // Check if discord app is connected.
                        foreach (var discordSocketClient in discordAppInstance.DiscordShardedClient.Shards)
                        {
                            try
                            {
                                if (discordSocketClient.ConnectionState == ConnectionState.Disconnected || discordSocketClient.ConnectionState == ConnectionState.Disconnecting)
                                    await discordSocketClient.StartAsync().ConfigureAwait(false);
                            }
                            catch (Exception ex)
                            {
                                await LoggerService.LogErrorMessageAsync(ex);
                            }
                        }
                    }

                    // Process discord app to remove.
                    for (var i = DiscordAppInstances.Count - 1; i >= 0; i--)
                    {
                        var discordAppInstance = DiscordAppInstances[i];

                        if (discordAppInstance.IsStarted || discordAppInstance.IsLoggedIn)
                            continue;

                        discordAppInstance.Dispose();
                        DiscordAppInstances.RemoveAt(i);
                    }

                    await WaitForNextSecondAsync().ConfigureAwait(false);
                }
            }, TaskCreationOptions.LongRunning);
        }

        private async Task WaitForNextSecondAsync()
        {
            Stopwatch.Stop();

            if (Stopwatch.Elapsed > TimeSpan.FromSeconds(1))
                await Task.Delay(1).ConfigureAwait(false);
            else
                await Task.Delay(TimeSpan.FromSeconds(1) - Stopwatch.Elapsed).ConfigureAwait(false);

            Stopwatch.Restart();
        }

        private async Task StartDiscordAppInstanceAsync(ulong clientId, Action<CommandExecuteResult> callbackAction = null)
        {
            await StopDiscordAppInstanceAsync(clientId);

            var clientIdString = clientId.ToString();
            var discordAppTable = await SQLiteService.SQLiteAsyncConnection.Table<DiscordAppTable>().Where(a => a.ClientId == clientIdString).FirstOrDefaultAsync().ConfigureAwait(false);

            if (string.IsNullOrEmpty(discordAppTable?.BotToken))
            {
                callbackAction?.Invoke(CommandExecuteResult.FromError("Discord App is not registered in the database."));
                return;
            }

            var discordApp = new DiscordAppInstance(Convert.ToUInt64(discordAppTable.ClientId), discordAppTable.BotToken);

            AddListener(discordApp);

            try
            {
                await discordApp.DiscordAppLoginAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await discordApp.DiscordAppLogoutAsync().ConfigureAwait(false);
                await LoggerService.LogErrorMessageAsync(ex);
            }

            if (!discordApp.IsLoggedIn)
            {
                callbackAction?.Invoke(CommandExecuteResult.FromError("Discord App encountered an error while trying to authenticate."));
                return;
            }

            try
            {
                await discordApp.DiscordAppStartAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await LoggerService.LogErrorMessageAsync(ex);
            }

            if (!discordApp.IsStarted)
            {
                callbackAction?.Invoke(CommandExecuteResult.FromError("Discord App encountered an error while trying to connect to the discord gateway server."));
                return;
            }

            DiscordAppInstances.Add(discordApp);

            callbackAction?.Invoke(CommandExecuteResult.FromSuccess("Discord App is now starting!"));
        }

        private async Task StopDiscordAppInstanceAsync(ulong clientId, Action<CommandExecuteResult> callbackAction = null)
        {
            foreach (var discordAppInstance in DiscordAppInstances)
            {
                if (discordAppInstance.ClientId != clientId)
                    continue;

                var instanceName = discordAppInstance.DiscordShardedClient?.CurrentUser?.ToString();

                try
                {
                    await discordAppInstance.DiscordAppStopAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    await LoggerService.LogErrorMessageAsync(ex);
                }

                try
                {
                    await discordAppInstance.DiscordAppLogoutAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    await LoggerService.LogErrorMessageAsync(ex);
                }

                RemoveListener(discordAppInstance);
                callbackAction?.Invoke(CommandExecuteResult.FromSuccess($"Discord App {instanceName} has stopped."));
                return;
            }

            callbackAction?.Invoke(CommandExecuteResult.FromError("Discord App is not running!"));
        }

        private void AddListener(DiscordAppInstance discordAppInstance)
        {
            discordAppInstance.Log += DiscordAppInstanceOnLog;
        }

        private void RemoveListener(DiscordAppInstance discordAppInstance)
        {
            discordAppInstance.Log -= DiscordAppInstanceOnLog;
        }

        private Task DiscordAppInstanceOnLog(DiscordAppInstance discordAppInstance, LogMessage logMessage)
        {
            Task.Run(async () => await LoggerService.LogMessageAsync(discordAppInstance.DiscordShardedClient, logMessage).ConfigureAwait(false)).ConfigureAwait(false);
            return Task.CompletedTask;
        }
    }
}