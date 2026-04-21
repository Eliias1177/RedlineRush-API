using System;
using System.Threading;
using System.Threading.Tasks;

namespace DragRacingAPI.Hubs
{
    public record WaitingPlayer(string ConnectionId, string PlayerId, DateTime JoinedAt);

    public sealed class MatchmakingService
    {
        private WaitingPlayer? _waitingPlayer;
        private readonly SemaphoreSlim _lock = new(initialCount: 1, maxCount: 1);

        public async Task<(bool IsMatched, string? OpponentConnectionId, string? MatchId)>
            TryMatchAsync(string connectionId, string playerId)
        {
            await _lock.WaitAsync();
            try
            {
                // CASO 1: Si eres el mismo jugador (reconectando), actualiza tu conexión
                if (_waitingPlayer is not null && _waitingPlayer.PlayerId == playerId)
                {
                    _waitingPlayer = new WaitingPlayer(connectionId, playerId, DateTime.UtcNow);
                    return (false, null, null);
                }

                // CASO 2: Sala vacía, te sientas a esperar
                if (_waitingPlayer is null)
                {
                    _waitingPlayer = new WaitingPlayer(connectionId, playerId, DateTime.UtcNow);
                    return (false, null, null);
                }

                // CASO 3: Hay un oponente diferente. ¡Emparejar!
                var opponent = _waitingPlayer;
                _waitingPlayer = null;
                var matchId = Guid.NewGuid().ToString("N");
                return (true, opponent.ConnectionId, matchId);
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task RemoveFromWaitingAsync(string connectionId, string? playerId)
        {
            await _lock.WaitAsync();
            try
            {
                if (_waitingPlayer is null) return;

                bool isMatch = _waitingPlayer.ConnectionId == connectionId
                            || (playerId is not null && _waitingPlayer.PlayerId == playerId);

                if (isMatch) _waitingPlayer = null;
            }
            finally
            {
                _lock.Release();
            }
        }
    }
}