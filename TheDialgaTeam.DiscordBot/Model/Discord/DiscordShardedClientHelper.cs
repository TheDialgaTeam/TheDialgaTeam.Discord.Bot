﻿using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace TheDialgaTeam.DiscordBot.Model.Discord
{
    public interface IDiscordShardedClientHelper
    {
        event Func<IDiscordShardedClientHelper, LogMessage, Task> Log;

        event Func<IDiscordShardedClientHelper, Task> LoggedIn;

        event Func<IDiscordShardedClientHelper, Task> LoggedOut;

        event Func<IDiscordShardedClientHelper, SocketChannel, Task> ChannelCreated;

        event Func<IDiscordShardedClientHelper, SocketChannel, Task> ChannelDestroyed;

        event Func<IDiscordShardedClientHelper, SocketChannel, SocketChannel, Task> ChannelUpdated;

        event Func<IDiscordShardedClientHelper, SocketMessage, Task> MessageReceived;

        event Func<IDiscordShardedClientHelper, Cacheable<IMessage, ulong>, ISocketMessageChannel, Task> MessageDeleted;

        event Func<IDiscordShardedClientHelper, Cacheable<IMessage, ulong>, SocketMessage, ISocketMessageChannel, Task> MessageUpdated;

        event Func<IDiscordShardedClientHelper, Cacheable<IUserMessage, ulong>, ISocketMessageChannel, SocketReaction, Task> ReactionAdded;

        event Func<IDiscordShardedClientHelper, Cacheable<IUserMessage, ulong>, ISocketMessageChannel, SocketReaction, Task> ReactionRemoved;

        event Func<IDiscordShardedClientHelper, Cacheable<IUserMessage, ulong>, ISocketMessageChannel, Task> ReactionsCleared;

        event Func<IDiscordShardedClientHelper, SocketRole, Task> RoleCreated;

        event Func<IDiscordShardedClientHelper, SocketRole, Task> RoleDeleted;

        event Func<IDiscordShardedClientHelper, SocketRole, SocketRole, Task> RoleUpdated;

        event Func<IDiscordShardedClientHelper, SocketGuild, Task> JoinedGuild;

        event Func<IDiscordShardedClientHelper, SocketGuild, Task> LeftGuild;

        event Func<IDiscordShardedClientHelper, SocketGuild, Task> GuildAvailable;

        event Func<IDiscordShardedClientHelper, SocketGuild, Task> GuildUnavailable;

        event Func<IDiscordShardedClientHelper, SocketGuild, Task> GuildMembersDownloaded;

        event Func<IDiscordShardedClientHelper, SocketGuild, SocketGuild, Task> GuildUpdated;

        event Func<IDiscordShardedClientHelper, SocketGuildUser, Task> UserJoined;

        event Func<IDiscordShardedClientHelper, SocketGuildUser, Task> UserLeft;

        event Func<IDiscordShardedClientHelper, SocketUser, SocketGuild, Task> UserBanned;

        event Func<IDiscordShardedClientHelper, SocketUser, SocketGuild, Task> UserUnbanned;

        event Func<IDiscordShardedClientHelper, SocketUser, SocketUser, Task> UserUpdated;

        event Func<IDiscordShardedClientHelper, SocketGuildUser, SocketGuildUser, Task> GuildMemberUpdated;

        event Func<IDiscordShardedClientHelper, SocketUser, SocketVoiceState, SocketVoiceState, Task> UserVoiceStateUpdated;

        event Func<IDiscordShardedClientHelper, SocketSelfUser, SocketSelfUser, Task> CurrentUserUpdated;

        event Func<IDiscordShardedClientHelper, SocketUser, ISocketMessageChannel, Task> UserIsTyping;

        event Func<IDiscordShardedClientHelper, SocketGroupUser, Task> RecipientAdded;

        event Func<IDiscordShardedClientHelper, SocketGroupUser, Task> RecipientRemoved;

        event Func<IDiscordShardedClientHelper, DiscordSocketClient, Task> ShardConnected;

        event Func<IDiscordShardedClientHelper, Exception, DiscordSocketClient, Task> ShardDisconnected;

        event Func<IDiscordShardedClientHelper, DiscordSocketClient, Task> ShardReady;

        event Func<IDiscordShardedClientHelper, int, int, DiscordSocketClient, Task> ShardLatencyUpdated;

        DiscordShardedClient DiscordShardedClient { get; }

        Task StartListeningAsync();

        Task StopListeningAsync();
    }

    internal sealed class DiscordShardedClientHelper : IDiscordShardedClientHelper
    {
        public event Func<IDiscordShardedClientHelper, LogMessage, Task> Log;

        public event Func<IDiscordShardedClientHelper, Task> LoggedIn;

        public event Func<IDiscordShardedClientHelper, Task> LoggedOut;

        public event Func<IDiscordShardedClientHelper, SocketChannel, Task> ChannelCreated;

        public event Func<IDiscordShardedClientHelper, SocketChannel, Task> ChannelDestroyed;

        public event Func<IDiscordShardedClientHelper, SocketChannel, SocketChannel, Task> ChannelUpdated;

        public event Func<IDiscordShardedClientHelper, SocketMessage, Task> MessageReceived;

        public event Func<IDiscordShardedClientHelper, Cacheable<IMessage, ulong>, ISocketMessageChannel, Task> MessageDeleted;

        public event Func<IDiscordShardedClientHelper, Cacheable<IMessage, ulong>, SocketMessage, ISocketMessageChannel, Task> MessageUpdated;

        public event Func<IDiscordShardedClientHelper, Cacheable<IUserMessage, ulong>, ISocketMessageChannel, SocketReaction, Task> ReactionAdded;

        public event Func<IDiscordShardedClientHelper, Cacheable<IUserMessage, ulong>, ISocketMessageChannel, SocketReaction, Task> ReactionRemoved;

        public event Func<IDiscordShardedClientHelper, Cacheable<IUserMessage, ulong>, ISocketMessageChannel, Task> ReactionsCleared;

        public event Func<IDiscordShardedClientHelper, SocketRole, Task> RoleCreated;

        public event Func<IDiscordShardedClientHelper, SocketRole, Task> RoleDeleted;

        public event Func<IDiscordShardedClientHelper, SocketRole, SocketRole, Task> RoleUpdated;

        public event Func<IDiscordShardedClientHelper, SocketGuild, Task> JoinedGuild;

        public event Func<IDiscordShardedClientHelper, SocketGuild, Task> LeftGuild;

        public event Func<IDiscordShardedClientHelper, SocketGuild, Task> GuildAvailable;

        public event Func<IDiscordShardedClientHelper, SocketGuild, Task> GuildUnavailable;

        public event Func<IDiscordShardedClientHelper, SocketGuild, Task> GuildMembersDownloaded;

        public event Func<IDiscordShardedClientHelper, SocketGuild, SocketGuild, Task> GuildUpdated;

        public event Func<IDiscordShardedClientHelper, SocketGuildUser, Task> UserJoined;

        public event Func<IDiscordShardedClientHelper, SocketGuildUser, Task> UserLeft;

        public event Func<IDiscordShardedClientHelper, SocketUser, SocketGuild, Task> UserBanned;

        public event Func<IDiscordShardedClientHelper, SocketUser, SocketGuild, Task> UserUnbanned;

        public event Func<IDiscordShardedClientHelper, SocketUser, SocketUser, Task> UserUpdated;

        public event Func<IDiscordShardedClientHelper, SocketGuildUser, SocketGuildUser, Task> GuildMemberUpdated;

        public event Func<IDiscordShardedClientHelper, SocketUser, SocketVoiceState, SocketVoiceState, Task> UserVoiceStateUpdated;

        public event Func<IDiscordShardedClientHelper, SocketSelfUser, SocketSelfUser, Task> CurrentUserUpdated;

        public event Func<IDiscordShardedClientHelper, SocketUser, ISocketMessageChannel, Task> UserIsTyping;

        public event Func<IDiscordShardedClientHelper, SocketGroupUser, Task> RecipientAdded;

        public event Func<IDiscordShardedClientHelper, SocketGroupUser, Task> RecipientRemoved;

        public event Func<IDiscordShardedClientHelper, DiscordSocketClient, Task> ShardConnected;

        public event Func<IDiscordShardedClientHelper, Exception, DiscordSocketClient, Task> ShardDisconnected;

        public event Func<IDiscordShardedClientHelper, DiscordSocketClient, Task> ShardReady;

        public event Func<IDiscordShardedClientHelper, int, int, DiscordSocketClient, Task> ShardLatencyUpdated;

        public DiscordShardedClient DiscordShardedClient { get; }

        private string BotToken { get; }

        public DiscordShardedClientHelper(string botToken)
        {
            DiscordShardedClient = new DiscordShardedClient(new DiscordSocketConfig { LogLevel = LogSeverity.Verbose });
            BotToken = botToken;
        }

        public async Task StartListeningAsync()
        {
            while (DiscordShardedClient.LoginState == LoginState.LoggingOut)
                await Task.Delay(1);

            if (DiscordShardedClient.LoginState == LoginState.LoggedOut)
            {
                DiscordShardedClient.Log += DiscordShardedClientOnLog;
                DiscordShardedClient.LoggedIn += DiscordShardedClientOnLoggedIn;
                DiscordShardedClient.LoggedOut += DiscordShardedClientOnLoggedOut;
                DiscordShardedClient.ChannelCreated += DiscordShardedClientOnChannelCreated;
                DiscordShardedClient.ChannelDestroyed += DiscordShardedClientOnChannelDestroyed;
                DiscordShardedClient.ChannelUpdated += DiscordShardedClientOnChannelUpdated;
                DiscordShardedClient.MessageReceived += DiscordShardedClientOnMessageReceived;
                DiscordShardedClient.MessageDeleted += DiscordShardedClientOnMessageDeleted;
                DiscordShardedClient.MessageUpdated += DiscordShardedClientOnMessageUpdated;
                DiscordShardedClient.ReactionAdded += DiscordShardedClientOnReactionAdded;
                DiscordShardedClient.ReactionRemoved += DiscordShardedClientOnReactionRemoved;
                DiscordShardedClient.ReactionsCleared += DiscordShardedClientOnReactionsCleared;
                DiscordShardedClient.RoleCreated += DiscordShardedClientOnRoleCreated;
                DiscordShardedClient.RoleDeleted += DiscordShardedClientOnRoleDeleted;
                DiscordShardedClient.RoleUpdated += DiscordShardedClientOnRoleUpdated;
                DiscordShardedClient.JoinedGuild += DiscordShardedClientOnJoinedGuild;
                DiscordShardedClient.LeftGuild += DiscordShardedClientOnLeftGuild;
                DiscordShardedClient.GuildAvailable += DiscordShardedClientOnGuildAvailable;
                DiscordShardedClient.GuildUnavailable += DiscordShardedClientOnGuildUnavailable;
                DiscordShardedClient.GuildMembersDownloaded += DiscordShardedClientOnGuildMembersDownloaded;
                DiscordShardedClient.GuildUpdated += DiscordShardedClientOnGuildUpdated;
                DiscordShardedClient.UserJoined += DiscordShardedClientOnUserJoined;
                DiscordShardedClient.UserLeft += DiscordShardedClientOnUserLeft;
                DiscordShardedClient.UserBanned += DiscordShardedClientOnUserBanned;
                DiscordShardedClient.UserUnbanned += DiscordShardedClientOnUserUnbanned;
                DiscordShardedClient.UserUpdated += DiscordShardedClientOnUserUpdated;
                DiscordShardedClient.GuildMemberUpdated += DiscordShardedClientOnGuildMemberUpdated;
                DiscordShardedClient.UserVoiceStateUpdated += DiscordShardedClientOnUserVoiceStateUpdated;
                DiscordShardedClient.CurrentUserUpdated += DiscordShardedClientOnCurrentUserUpdated;
                DiscordShardedClient.UserIsTyping += DiscordShardedClientOnUserIsTyping;
                DiscordShardedClient.RecipientAdded += DiscordShardedClientOnRecipientAdded;
                DiscordShardedClient.RecipientRemoved += DiscordShardedClientOnRecipientRemoved;
                DiscordShardedClient.ShardConnected += DiscordShardedClientOnShardConnected;
                DiscordShardedClient.ShardDisconnected += DiscordShardedClientOnShardDisconnected;
                DiscordShardedClient.ShardReady += DiscordShardedClientOnShardReady;
                DiscordShardedClient.ShardLatencyUpdated += DiscordShardedClientOnShardLatencyUpdated;

                await DiscordShardedClient.LoginAsync(TokenType.Bot, BotToken);
                await DiscordShardedClient.StartAsync();
            }
        }

        public async Task StopListeningAsync()
        {
            while (DiscordShardedClient.LoginState == LoginState.LoggingIn)
                await Task.Delay(1);

            if (DiscordShardedClient.LoginState == LoginState.LoggedIn)
            {
                await DiscordShardedClient.LogoutAsync();
                await DiscordShardedClient.StopAsync();

                DiscordShardedClient.ShardLatencyUpdated -= DiscordShardedClientOnShardLatencyUpdated;
                DiscordShardedClient.ShardReady -= DiscordShardedClientOnShardReady;
                DiscordShardedClient.ShardDisconnected -= DiscordShardedClientOnShardDisconnected;
                DiscordShardedClient.ShardConnected -= DiscordShardedClientOnShardConnected;
                DiscordShardedClient.RecipientRemoved -= DiscordShardedClientOnRecipientRemoved;
                DiscordShardedClient.RecipientAdded -= DiscordShardedClientOnRecipientAdded;
                DiscordShardedClient.UserIsTyping -= DiscordShardedClientOnUserIsTyping;
                DiscordShardedClient.CurrentUserUpdated -= DiscordShardedClientOnCurrentUserUpdated;
                DiscordShardedClient.UserVoiceStateUpdated -= DiscordShardedClientOnUserVoiceStateUpdated;
                DiscordShardedClient.GuildMemberUpdated -= DiscordShardedClientOnGuildMemberUpdated;
                DiscordShardedClient.UserUpdated -= DiscordShardedClientOnUserUpdated;
                DiscordShardedClient.UserUnbanned -= DiscordShardedClientOnUserUnbanned;
                DiscordShardedClient.UserBanned -= DiscordShardedClientOnUserBanned;
                DiscordShardedClient.UserLeft -= DiscordShardedClientOnUserLeft;
                DiscordShardedClient.UserJoined -= DiscordShardedClientOnUserJoined;
                DiscordShardedClient.GuildUpdated -= DiscordShardedClientOnGuildUpdated;
                DiscordShardedClient.GuildMembersDownloaded -= DiscordShardedClientOnGuildMembersDownloaded;
                DiscordShardedClient.GuildUnavailable -= DiscordShardedClientOnGuildUnavailable;
                DiscordShardedClient.GuildAvailable -= DiscordShardedClientOnGuildAvailable;
                DiscordShardedClient.LeftGuild -= DiscordShardedClientOnLeftGuild;
                DiscordShardedClient.JoinedGuild -= DiscordShardedClientOnJoinedGuild;
                DiscordShardedClient.RoleUpdated -= DiscordShardedClientOnRoleUpdated;
                DiscordShardedClient.RoleDeleted -= DiscordShardedClientOnRoleDeleted;
                DiscordShardedClient.RoleCreated -= DiscordShardedClientOnRoleCreated;
                DiscordShardedClient.ReactionsCleared -= DiscordShardedClientOnReactionsCleared;
                DiscordShardedClient.ReactionRemoved -= DiscordShardedClientOnReactionRemoved;
                DiscordShardedClient.ReactionAdded -= DiscordShardedClientOnReactionAdded;
                DiscordShardedClient.MessageUpdated -= DiscordShardedClientOnMessageUpdated;
                DiscordShardedClient.MessageDeleted -= DiscordShardedClientOnMessageDeleted;
                DiscordShardedClient.MessageReceived -= DiscordShardedClientOnMessageReceived;
                DiscordShardedClient.ChannelUpdated -= DiscordShardedClientOnChannelUpdated;
                DiscordShardedClient.ChannelDestroyed -= DiscordShardedClientOnChannelDestroyed;
                DiscordShardedClient.ChannelCreated -= DiscordShardedClientOnChannelCreated;
                DiscordShardedClient.LoggedOut -= DiscordShardedClientOnLoggedOut;
                DiscordShardedClient.LoggedIn -= DiscordShardedClientOnLoggedIn;
                DiscordShardedClient.Log -= DiscordShardedClientOnLog;
            }
        }

        private async Task DiscordShardedClientOnLog(LogMessage arg)
        {
            if (Log != null)
                await Log.Invoke(this, arg).ConfigureAwait(false);
        }

        private async Task DiscordShardedClientOnLoggedIn()
        {
            if (LoggedIn != null)
                await LoggedIn.Invoke(this).ConfigureAwait(false);
        }

        private async Task DiscordShardedClientOnLoggedOut()
        {
            if (LoggedOut != null)
                await LoggedOut.Invoke(this).ConfigureAwait(false);
        }

        private async Task DiscordShardedClientOnChannelCreated(SocketChannel arg)
        {
            if (ChannelCreated != null)
                await ChannelCreated.Invoke(this, arg).ConfigureAwait(false);
        }

        private async Task DiscordShardedClientOnChannelDestroyed(SocketChannel arg)
        {
            if (ChannelDestroyed != null)
                await ChannelDestroyed.Invoke(this, arg).ConfigureAwait(false);
        }

        private async Task DiscordShardedClientOnChannelUpdated(SocketChannel arg1, SocketChannel arg2)
        {
            if (ChannelUpdated != null)
                await ChannelUpdated.Invoke(this, arg1, arg2).ConfigureAwait(false);
        }

        private async Task DiscordShardedClientOnMessageReceived(SocketMessage arg)
        {
            if (MessageReceived != null)
                await MessageReceived.Invoke(this, arg).ConfigureAwait(false);
        }

        private async Task DiscordShardedClientOnMessageDeleted(Cacheable<IMessage, ulong> arg1, ISocketMessageChannel arg2)
        {
            if (MessageDeleted != null)
                await MessageDeleted.Invoke(this, arg1, arg2).ConfigureAwait(false);
        }

        private async Task DiscordShardedClientOnMessageUpdated(Cacheable<IMessage, ulong> arg1, SocketMessage arg2, ISocketMessageChannel arg3)
        {
            if (MessageUpdated != null)
                await MessageUpdated.Invoke(this, arg1, arg2, arg3).ConfigureAwait(false);
        }

        private async Task DiscordShardedClientOnReactionAdded(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            if (ReactionAdded != null)
                await ReactionAdded.Invoke(this, arg1, arg2, arg3).ConfigureAwait(false);
        }

        private async Task DiscordShardedClientOnReactionRemoved(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            if (ReactionRemoved != null)
                await ReactionRemoved.Invoke(this, arg1, arg2, arg3).ConfigureAwait(false);
        }

        private async Task DiscordShardedClientOnReactionsCleared(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2)
        {
            if (ReactionsCleared != null)
                await ReactionsCleared.Invoke(this, arg1, arg2).ConfigureAwait(false);
        }

        private async Task DiscordShardedClientOnRoleCreated(SocketRole arg)
        {
            if (RoleCreated != null)
                await RoleCreated.Invoke(this, arg).ConfigureAwait(false);
        }

        private async Task DiscordShardedClientOnRoleDeleted(SocketRole arg)
        {
            if (RoleDeleted != null)
                await RoleDeleted.Invoke(this, arg).ConfigureAwait(false);
        }

        private async Task DiscordShardedClientOnRoleUpdated(SocketRole arg1, SocketRole arg2)
        {
            if (RoleUpdated != null)
                await RoleUpdated.Invoke(this, arg1, arg2).ConfigureAwait(false);
        }

        private async Task DiscordShardedClientOnJoinedGuild(SocketGuild arg)
        {
            if (JoinedGuild != null)
                await JoinedGuild.Invoke(this, arg).ConfigureAwait(false);
        }

        private async Task DiscordShardedClientOnLeftGuild(SocketGuild arg)
        {
            if (LeftGuild != null)
                await LeftGuild.Invoke(this, arg).ConfigureAwait(false);
        }

        private async Task DiscordShardedClientOnGuildAvailable(SocketGuild arg)
        {
            if (GuildAvailable != null)
                await GuildAvailable.Invoke(this, arg).ConfigureAwait(false);
        }

        private async Task DiscordShardedClientOnGuildUnavailable(SocketGuild arg)
        {
            if (GuildUnavailable != null)
                await GuildUnavailable.Invoke(this, arg).ConfigureAwait(false);
        }

        private async Task DiscordShardedClientOnGuildMembersDownloaded(SocketGuild arg)
        {
            if (GuildMembersDownloaded != null)
                await GuildMembersDownloaded.Invoke(this, arg).ConfigureAwait(false);
        }

        private async Task DiscordShardedClientOnGuildUpdated(SocketGuild arg1, SocketGuild arg2)
        {
            if (GuildUpdated != null)
                await GuildUpdated.Invoke(this, arg1, arg2).ConfigureAwait(false);
        }

        private async Task DiscordShardedClientOnUserJoined(SocketGuildUser arg)
        {
            if (UserJoined != null)
                await UserJoined.Invoke(this, arg).ConfigureAwait(false);
        }

        private async Task DiscordShardedClientOnUserLeft(SocketGuildUser arg)
        {
            if (UserLeft != null)
                await UserLeft.Invoke(this, arg).ConfigureAwait(false);
        }

        private async Task DiscordShardedClientOnUserBanned(SocketUser arg1, SocketGuild arg2)
        {
            if (UserBanned != null)
                await UserBanned.Invoke(this, arg1, arg2).ConfigureAwait(false);
        }

        private async Task DiscordShardedClientOnUserUnbanned(SocketUser arg1, SocketGuild arg2)
        {
            if (UserUnbanned != null)
                await UserUnbanned.Invoke(this, arg1, arg2).ConfigureAwait(false);
        }

        private async Task DiscordShardedClientOnUserUpdated(SocketUser arg1, SocketUser arg2)
        {
            if (UserUpdated != null)
                await UserUpdated.Invoke(this, arg1, arg2).ConfigureAwait(false);
        }

        private async Task DiscordShardedClientOnGuildMemberUpdated(SocketGuildUser arg1, SocketGuildUser arg2)
        {
            if (GuildMemberUpdated != null)
                await GuildMemberUpdated.Invoke(this, arg1, arg2).ConfigureAwait(false);
        }

        private async Task DiscordShardedClientOnUserVoiceStateUpdated(SocketUser arg1, SocketVoiceState arg2, SocketVoiceState arg3)
        {
            if (UserVoiceStateUpdated != null)
                await UserVoiceStateUpdated.Invoke(this, arg1, arg2, arg3).ConfigureAwait(false);
        }

        private async Task DiscordShardedClientOnCurrentUserUpdated(SocketSelfUser arg1, SocketSelfUser arg2)
        {
            if (CurrentUserUpdated != null)
                await CurrentUserUpdated.Invoke(this, arg1, arg2).ConfigureAwait(false);
        }

        private async Task DiscordShardedClientOnUserIsTyping(SocketUser arg1, ISocketMessageChannel arg2)
        {
            if (UserIsTyping != null)
                await UserIsTyping.Invoke(this, arg1, arg2).ConfigureAwait(false);
        }

        private async Task DiscordShardedClientOnRecipientAdded(SocketGroupUser arg)
        {
            if (RecipientAdded != null)
                await RecipientAdded.Invoke(this, arg).ConfigureAwait(false);
        }

        private async Task DiscordShardedClientOnRecipientRemoved(SocketGroupUser arg)
        {
            if (RecipientRemoved != null)
                await RecipientRemoved.Invoke(this, arg).ConfigureAwait(false);
        }

        private async Task DiscordShardedClientOnShardConnected(DiscordSocketClient arg)
        {
            if (ShardConnected != null)
                await ShardConnected.Invoke(this, arg).ConfigureAwait(false);
        }

        private async Task DiscordShardedClientOnShardDisconnected(Exception arg1, DiscordSocketClient arg2)
        {
            if (ShardDisconnected != null)
                await ShardDisconnected.Invoke(this, arg1, arg2).ConfigureAwait(false);
        }

        private async Task DiscordShardedClientOnShardReady(DiscordSocketClient arg)
        {
            if (ShardReady != null)
                await ShardReady.Invoke(this, arg).ConfigureAwait(false);
        }

        private async Task DiscordShardedClientOnShardLatencyUpdated(int arg1, int arg2, DiscordSocketClient arg3)
        {
            if (ShardLatencyUpdated != null)
                await ShardLatencyUpdated.Invoke(this, arg1, arg2, arg3).ConfigureAwait(false);
        }
    }
}