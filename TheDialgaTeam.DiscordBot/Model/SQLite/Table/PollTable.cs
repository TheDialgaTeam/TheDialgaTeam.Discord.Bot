﻿using System;
using SQLite;

namespace TheDialgaTeam.DiscordBot.Model.SQLite.Table
{
    [Table("Poll")]
    internal sealed class PollTable : BaseTable, IDatabaseTable
    {
        public string MessageId { get; set; }

        public DateTimeOffset StartDateTime { get; set; }

        public TimeSpan Duration { get; set; }

        [Indexed]
        public long DiscordChannelId { get; set; }
    }
}