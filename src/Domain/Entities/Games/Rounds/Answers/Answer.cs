using WordBattle.Domain.ValueObjects;

namespace WordBattle.Domain.Entities.Games.Rounds.Answers
{
    public class Answer : Entity
    {
        protected Answer() { }

        private Answer(Guid roundId, Guid playerId, Word word, bool isValidTime)
        {
            RoundId = roundId;
            PlayerId = playerId;
            Word = word;
            IsValidTime = isValidTime;
            SumbitedAt = DateTime.UtcNow;
            IsValidContent = true;
        }

        internal static Answer Create(Guid roundId, Guid playerId, Word word, bool isValidTime)
        {
            return new Answer(roundId, playerId, word, isValidTime);
        }

        public void MarkAsInvalidContent()
        {
            IsValidContent = false;
        }

        public Word Word { get; private set; }
        public Guid RoundId { get; private set; }
        public Guid PlayerId { get; private set; }
        public bool IsValidTime { get; private set; }
        public bool IsValidContent { get; private set; }
        public DateTime SumbitedAt { get; private set; }
    }
}
