﻿using Nancy;
using TheDialgaTeam.Discord.Bot.Model.SQLite.Table;
using TheDialgaTeam.Discord.Bot.Service.SQLite;

namespace TheDialgaTeam.Discord.Bot.Nancy
{
    public sealed class IndexController : NancyModule
    {
        public IndexController(SQLiteService sqliteService)
        {
            Get("/getDiscordAppTable", async args =>
            {
                var discordAppTables = await sqliteService.SQLiteAsyncConnection.Table<DiscordAppTable>().ToArrayAsync().ConfigureAwait(false);
                return Response.AsJson(discordAppTables);
            });

            Get("/getDiscordAppTable/clientId/{clientId}", async args =>
            {
                string clientId = args["clientId"];
                var discordAppTables = await sqliteService.SQLiteAsyncConnection.Table<DiscordAppTable>().Where(a => a.ClientId == clientId).ToArrayAsync().ConfigureAwait(false);
                return Response.AsJson(discordAppTables);
            });
        }
    }
}