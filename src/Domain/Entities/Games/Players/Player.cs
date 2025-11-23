using WordBattle.Domain.Errors;
using WordBattle.Domain.Shared;

namespace WordBattle.Domain.Entities.Games.Players
{
    public class Player : Entity
    {
        internal protected Player() { }

        private Player(string name, int sequence, Guid gameId)
        {
            Name = name;
            Sequence = sequence;
            GameId = gameId;
            IsPlaying = true;
        }

        internal static Result<Player> Create(string name, int sequence, Guid gameId)
        {
            if (string.IsNullOrEmpty(name))
                return Result.Failure<Player>(DomainErrors.Player.EmptyName);

            if (name.Length > 200)
                return Result.Failure<Player>(DomainErrors.Player.LongName);

            return new Player(name, sequence, gameId);
        }

        public void Eliminate()
        {
            IsPlaying = false;
            EliminatedAt = DateTime.UtcNow;
        }

        public string Name { get; private set; }

        public Guid GameId { get; private set; }
        public int Sequence { get; private set; }
        public bool IsPlaying { get; private set; }
        public DateTime? EliminatedAt { get; private set; }
    }
}
