﻿using SQLite;

namespace TheDialgaTeam.DiscordBot.Model.SQLite.Table
{
    [Table("FreeGameNotification")]
    internal sealed class FreeGameNotificationTable : BaseTable, IDatabaseTable
    {
        public bool Active { get; set; }

        public string RoleId { get; set; }

        [Indexed]
        public long DiscordChannelId { get; set; }
    }
}