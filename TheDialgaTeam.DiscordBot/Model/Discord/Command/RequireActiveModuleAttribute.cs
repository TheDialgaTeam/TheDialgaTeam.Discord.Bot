﻿using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using SQLiteNetExtensionsAsync.Extensions;
using TheDialgaTeam.DiscordBot.Model.SQLite.Table;
using TheDialgaTeam.DiscordBot.Services.SQLite;

namespace TheDialgaTeam.DiscordBot.Model.Discord.Command
{
    internal sealed class RequireActiveModuleAttribute : PreconditionAttribute
    {
        public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            if (context.Message.Channel is SocketDMChannel || context.Message.Channel is SocketGroupChannel)
                return PreconditionResult.FromError("This command requires to be in Guild context.");

            var sqliteService = services.GetRequiredService<ISQLiteService>();

            var clientId = context.Client.CurrentUser.Id.ToString();
            var guildId = context.Guild.Id.ToString();
            var moduleName = command.Module.Name;

            var discordAppDetailTableId = await sqliteService.GetDiscordAppDetailTableIdAsync(context.Client.CurrentUser.Id.ToString());

            if (discordAppDetailTableId == null)
                return PreconditionResult.FromError("Missing DiscordAppDetail record from the database.");

            return discordGuildModuleModel?.Active ?? false ? PreconditionResult.FromSuccess() : PreconditionResult.FromError($"This command require {command.Module.Name} to be active in this guild.");
        }
    }
}