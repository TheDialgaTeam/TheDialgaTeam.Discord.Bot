﻿using SQLite;

namespace TheDialgaTeam.DiscordBot.Model.SQLite.Table
{
    [Table("DiscordGuild")]
    internal sealed class DiscordGuildTable : BaseTable, IDatabaseTable
    {
        public string GuildId { get; set; }

        public string Prefix { get; set; }

        [Indexed]
        public long DiscordAppId { get; set; }
    }
}