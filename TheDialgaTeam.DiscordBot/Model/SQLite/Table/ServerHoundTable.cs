﻿using System;
using SQLite;

namespace TheDialgaTeam.DiscordBot.Model.SQLite.Table
{
    [Table("ServerHound")]
    internal sealed class ServerHoundTable : BaseTable, IDatabaseTable
    {
        public bool Dbans { get; set; }

        public DateTimeOffset LastChecked { get; set; }

        [Indexed]
        public long DiscordGuildId { get; set; }
    }
}