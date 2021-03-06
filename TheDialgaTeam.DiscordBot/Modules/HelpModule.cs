﻿using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TheDialgaTeam.DiscordBot.Model.Discord.Command;

namespace TheDialgaTeam.DiscordBot.Modules
{
    [Name("Help")]
    public sealed class HelpModule : ModuleHelper
    {
        private IServiceProvider ServiceProvider { get; }

        private CommandService CommandService { get; }

        public HelpModule(IProgram program)
        {
            ServiceProvider = program.ServiceProvider;
            CommandService = program.CommandService;
        }

        [Command("Help")]
        public async Task HelpAsync()
        {
            var helpMessage = new EmbedBuilder()
                              .WithTitle("Available Command:")
                              .WithColor(Color.Orange)
                              .WithDescription($"To find out more about each command, use `{Context.Client.CurrentUser.Mention} help <CommandName>`")
                              .WithThumbnailUrl(Context.Client.CurrentUser.GetAvatarUrl());

            foreach (var module in CommandService.Modules)
            {
                var moduleName = $"{module.Name} Module";

                if (moduleName == "Help Module")
                    continue;

                var commandInfo = new StringBuilder();

                foreach (var command in module.Commands)
                {
                    var preconditionResult = await command.CheckPreconditionsAsync(Context, ServiceProvider);

                    if (!preconditionResult.IsSuccess)
                        continue;

                    commandInfo.Append($"`{command.Name}`");

                    if (command.Aliases.Count > 0)
                    {
                        foreach (var commandAliase in command.Aliases)
                        {
                            if (!commandAliase.Equals(command.Name, StringComparison.OrdinalIgnoreCase))
                                commandInfo.Append($" `{commandAliase}`");
                        }
                    }

                    commandInfo.AppendLine($": {command.Summary}");
                }

                if (commandInfo.Length > 0)
                    helpMessage = helpMessage.AddField(moduleName, commandInfo.ToString());
            }

            await ReplyDMAsync("", false, helpMessage.Build());
        }

        [Command("Help")]
        public async Task HelpAsync([Remainder] string commandName)
        {
            foreach (var commandServiceModule in CommandService.Modules)
            {
                var moduleName = $"{commandServiceModule.Name} Module";

                if (moduleName == "Help Module")
                    continue;

                foreach (var command in commandServiceModule.Commands)
                {
                    if (!CheckCommandEquals(command, commandName))
                        continue;

                    var helpMessage = new EmbedBuilder()
                                      .WithTitle("Command Info:")
                                      .WithColor(Color.Orange)
                                      .WithDescription($"To find out more about each command, use `{Context.Client.CurrentUser.Mention} help <CommandName>`");

                    var requiredPermission = RequiredPermissions.GuildMember;
                    var requiredContext = ContextType.Guild | ContextType.DM | ContextType.Group;
                    string requiredContextString;

                    foreach (var commandAttribute in command.Preconditions)
                    {
                        switch (commandAttribute)
                        {
                            case RequirePermissionAttribute requirePermissionAttribute:
                                requiredPermission = requirePermissionAttribute.RequiredPermission;
                                break;

                            case RequireContextAttribute requireContextAttribute:
                                requiredContext = requireContextAttribute.Contexts;
                                break;
                        }
                    }

                    switch (requiredContext)
                    {
                        case ContextType.Guild | ContextType.DM | ContextType.Group:
                            requiredContextString = $"{ContextType.Guild}, {ContextType.DM}, {ContextType.Group}";
                            break;

                        case ContextType.Guild | ContextType.DM:
                            requiredContextString = $"{ContextType.Guild}, {ContextType.DM}";
                            break;

                        case ContextType.Guild | ContextType.Group:
                            requiredContextString = $"{ContextType.Guild}, {ContextType.Group}";
                            break;

                        case ContextType.DM | ContextType.Group:
                            requiredContextString = $"{ContextType.DM}, {ContextType.Group}";
                            break;

                        case ContextType.Guild:
                            requiredContextString = $"{ContextType.Guild}";
                            break;

                        case ContextType.DM:
                            requiredContextString = $"{ContextType.DM}";
                            break;

                        case ContextType.Group:
                            requiredContextString = $"{ContextType.Group}";
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    var commandInfo = new StringBuilder($"Usage: {Context.Client.CurrentUser.Mention} {command.Name}");
                    var argsInfo = new StringBuilder();

                    foreach (var commandParameter in command.Parameters)
                    {
                        if (commandParameter.IsMultiple)
                            commandInfo.Append($" `params {commandParameter.Type.Name}[] {commandParameter.Name}`");
                        else if (commandParameter.IsOptional)
                            commandInfo.Append($" `{(commandParameter.IsRemainder ? "Remainder " : "")}{commandParameter.Type.Name} {commandParameter.Name} = {commandParameter.DefaultValue ?? "null"}`");
                        else
                            commandInfo.Append($" `{(commandParameter.IsRemainder ? "Remainder " : "")}{commandParameter.Type.Name} {commandParameter.Name}`");

                        argsInfo.AppendLine($"{commandParameter.Type.Name} {commandParameter.Name}: {commandParameter.Summary}");
                    }

                    commandInfo.AppendLine($"\nDescription: {command.Summary}");
                    commandInfo.AppendLine($"Required Permission: {requiredPermission.ToString()}");
                    commandInfo.AppendLine($"Required Context: {requiredContextString}");

                    if (argsInfo.Length > 0)
                    {
                        commandInfo.AppendLine("\nArguments Info:");
                        commandInfo.Append(argsInfo);
                    }

                    commandInfo.AppendLine("\nNote:");
                    commandInfo.Append(AppendNotes(command));

                    helpMessage = helpMessage.AddField($"{command.Name} command:", commandInfo.ToString());

                    await ReplyAsync("", false, helpMessage.Build());
                    return;
                }
            }
        }

        private static bool CheckCommandEquals(CommandInfo command, string commandName)
        {
            if (command.Name.Equals(commandName, StringComparison.OrdinalIgnoreCase))
                return true;

            foreach (var commandAliase in command.Aliases)
            {
                if (commandAliase.Equals(commandName, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        private static string AppendNotes(CommandInfo commandInfo)
        {
            var stringBuilder = new StringBuilder();
            var ignoredTypes = new List<Type>();

            foreach (var commandInfoParameter in commandInfo.Parameters)
            {
                if (commandInfoParameter.Type.Name == typeof(bool).Name && !ignoredTypes.Contains(typeof(bool)))
                {
                    stringBuilder.AppendLine($"{typeof(bool).Name} arguments can be true or false.\n");
                    ignoredTypes.Add(typeof(bool));
                }
                else if (commandInfoParameter.Type.Name == typeof(char).Name && !ignoredTypes.Contains(typeof(char)))
                {
                    stringBuilder.AppendLine($"{typeof(char).Name} arguments only accepts one single character without quotes.\n");
                    ignoredTypes.Add(typeof(char));
                }
                else if (commandInfoParameter.Type.Name == typeof(sbyte).Name && !ignoredTypes.Contains(typeof(sbyte)))
                {
                    stringBuilder.AppendLine($"{typeof(sbyte).Name} arguments only accepts {sbyte.MinValue} to {sbyte.MaxValue}.\n");
                    ignoredTypes.Add(typeof(sbyte));
                }
                else if (commandInfoParameter.Type.Name == typeof(byte).Name && !ignoredTypes.Contains(typeof(byte)))
                {
                    stringBuilder.AppendLine($"{typeof(byte).Name} arguments only accepts {byte.MinValue} to {byte.MaxValue}.\n");
                    ignoredTypes.Add(typeof(byte));
                }
                else if (commandInfoParameter.Type.Name == typeof(ushort).Name && !ignoredTypes.Contains(typeof(ushort)))
                {
                    stringBuilder.AppendLine($"{typeof(ushort).Name} arguments only accepts {ushort.MinValue} to {ushort.MaxValue}.\n");
                    ignoredTypes.Add(typeof(ushort));
                }
                else if (commandInfoParameter.Type.Name == typeof(short).Name && !ignoredTypes.Contains(typeof(short)))
                {
                    stringBuilder.AppendLine($"{typeof(short).Name} arguments only accepts {short.MinValue} to {short.MaxValue}.\n");
                    ignoredTypes.Add(typeof(short));
                }
                else if (commandInfoParameter.Type.Name == typeof(uint).Name && !ignoredTypes.Contains(typeof(uint)))
                {
                    stringBuilder.AppendLine($"{typeof(uint).Name} arguments only accepts {uint.MinValue} to {uint.MaxValue}.\n");
                    ignoredTypes.Add(typeof(uint));
                }
                else if (commandInfoParameter.Type.Name == typeof(int).Name && !ignoredTypes.Contains(typeof(int)))
                {
                    stringBuilder.AppendLine($"{typeof(int).Name} arguments only accepts {int.MinValue} to {int.MaxValue}.\n");
                    ignoredTypes.Add(typeof(int));
                }
                else if (commandInfoParameter.Type.Name == typeof(ulong).Name && !ignoredTypes.Contains(typeof(ulong)))
                {
                    stringBuilder.AppendLine($"{typeof(ulong).Name} arguments only accepts {ulong.MinValue} to {ulong.MaxValue}.\n");
                    ignoredTypes.Add(typeof(ulong));
                }
                else if (commandInfoParameter.Type.Name == typeof(long).Name && !ignoredTypes.Contains(typeof(long)))
                {
                    stringBuilder.AppendLine($"{typeof(long).Name} arguments only accepts {long.MinValue} to {long.MaxValue}.\n");
                    ignoredTypes.Add(typeof(long));
                }
                else if (commandInfoParameter.Type.Name == typeof(float).Name && !ignoredTypes.Contains(typeof(float)))
                {
                    stringBuilder.AppendLine($"{typeof(float).Name} arguments only accepts {float.MinValue} to {float.MaxValue}.\n");
                    ignoredTypes.Add(typeof(float));
                }
                else if (commandInfoParameter.Type.Name == typeof(double).Name && !ignoredTypes.Contains(typeof(double)))
                {
                    stringBuilder.AppendLine($"{typeof(double).Name} arguments only accepts {double.MinValue} to {double.MaxValue}.\n");
                    ignoredTypes.Add(typeof(double));
                }
                else if (commandInfoParameter.Type.Name == typeof(decimal).Name && !ignoredTypes.Contains(typeof(decimal)))
                {
                    stringBuilder.AppendLine($"{typeof(decimal).Name} arguments only accepts {decimal.MinValue} to {decimal.MaxValue}.\n");
                    ignoredTypes.Add(typeof(decimal));
                }
                else if (commandInfoParameter.Type.Name == typeof(string).Name && !ignoredTypes.Contains(typeof(string)))
                {
                    stringBuilder.AppendLine($"{typeof(string).Name} arguments must be double quoted except for the remainder string type.\n");
                    ignoredTypes.Add(typeof(string));
                }
                else if (commandInfoParameter.Type.Name == typeof(TimeSpan).Name && !ignoredTypes.Contains(typeof(TimeSpan)))
                {
                    stringBuilder.AppendLine($@"{typeof(TimeSpan).Name} arguments must be in one of these format: (May include `-` sign)
`d`, `hh:mm`, `hh:mm:ss`, `d.hh:mm`, `d.hh:mm:ss`, `hh:mm:ss.ff`, `d.hh:mm:ss.ff`

d: Days, ranging from 0 to 10675199.
hh: Hours, ranging from 0 to 23.
mm: Minutes, ranging from 0 to 59.
ss: Optional seconds, ranging from 0 to 59.
ff: Optional fractional seconds, consisting of one to seven decimal digits.
");
                    ignoredTypes.Add(typeof(TimeSpan));
                }
                else if (commandInfoParameter.Type.Name == typeof(IChannel).Name && !ignoredTypes.Contains(typeof(IChannel)))
                {
                    stringBuilder.AppendLine($"{typeof(IChannel).Name} arguments can be #channel, channel id, channel name of any scope.\n");
                    ignoredTypes.Add(typeof(IChannel));
                }
                else if (commandInfoParameter.Type.Name == typeof(IUser).Name && !ignoredTypes.Contains(typeof(IUser)))
                {
                    stringBuilder.AppendLine($"{typeof(IUser).Name} arguments can be @user, user id, username, nickname of any scope.\n");
                    ignoredTypes.Add(typeof(IUser));
                }
                else if (commandInfoParameter.Type.Name == typeof(IRole).Name && !ignoredTypes.Contains(typeof(IRole)))
                {
                    stringBuilder.AppendLine($"{typeof(IRole).Name} arguments can be @role, role id, role name.\n");
                    ignoredTypes.Add(typeof(IRole));
                }
                else if (commandInfoParameter.Type.Name == typeof(IEmote).Name && !ignoredTypes.Contains(typeof(IEmote)))
                {
                    stringBuilder.AppendLine($"{typeof(IEmote).Name} arguments is discord emoji.\n");
                    ignoredTypes.Add(typeof(IEmote));
                }
            }

            return stringBuilder.ToString();
        }
    }
}