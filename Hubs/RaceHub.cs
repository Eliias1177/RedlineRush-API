using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace DragRacingAPI.Hubs
{
    public sealed class RaceHub : Hub
    {
        private readonly MatchmakingService _matchmaking;

        public RaceHub(MatchmakingService matchmaking)
        {
            _matchmaking = matchmaking;
        }

        public async Task JoinMatchmaking(string playerId)
        {
            Context.Items["PlayerId"] = playerId;

            var (isMatched, opponentConnectionId, matchId) =
                await _matchmaking.TryMatchAsync(Context.ConnectionId, playerId);

            if (!isMatched)
            {
                await Clients.Caller.SendAsync("WaitingForOpponent");
                return;
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, matchId!);
            await Groups.AddToGroupAsync(opponentConnectionId!, matchId!);
            await Clients.Group(matchId!).SendAsync("MatchFound", matchId);
        }

        public async Task SendTelemetry(string matchId, float distance) {
            await Clients.OthersInGroup(matchId).SendAsync("ReceiveOpponentData", distance);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var playerId = Context.Items["PlayerId"] as string;
            await _matchmaking.RemoveFromWaitingAsync(Context.ConnectionId, playerId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}