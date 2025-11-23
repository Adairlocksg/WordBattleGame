using WordBattle.Domain.Entities.Games.Players;
using WordBattle.Domain.Entities.Games.Rounds.Answers;
using WordBattle.Domain.Shared;
using WordBattle.Domain.ValueObjects;

namespace WordBattle.Domain.Entities.Games.Rounds
{
    public class Round : Entity
    {
        internal protected Round() { }

        private Round(int number, Guid categoryId, Guid gameId, int expectedAnswers)
        {
            Number = number;
            CategoryId = categoryId;
            GameId = gameId;
            CurrentTurnStartedAt = DateTime.UtcNow;
            ExpectedAnswers = expectedAnswers;
        }

        internal static Round Create(int number, Guid categoryId, Guid gameId, int expectedAnswers)
        {
            return new Round(number, categoryId, gameId, expectedAnswers);
        }

        private readonly List<Answer> _answers = [];
        public IReadOnlyCollection<Answer> Answers => _answers.AsReadOnly();

        public int Number { get; private set; }
        public Guid CategoryId { get; private set; }
        public Guid GameId { get; private set; }
        public DateTime CurrentTurnStartedAt { get; private set; }
        public bool IsFinished { get; private set; }
        public int ExpectedAnswers { get; private set; }

        public void Finish()
        {
            IsFinished = true;
        }

        public Result<Answer> ProcessAnswer(Player player, Word word)
        {
            var now = DateTime.UtcNow;
            var timeElapsed = (now - CurrentTurnStartedAt).TotalSeconds;
            bool isValidTime = timeElapsed <= 10.5;
           
            var newAnswer = Answer.Create(Id, player.Id, word, isValidTime);

            bool isDuplicate = _answers.Any(x => x.Word.Equals(word));

            if (isDuplicate)
                newAnswer.MarkAsInvalidContent();

            _answers.Add(newAnswer);

            CurrentTurnStartedAt = DateTime.UtcNow;

            return newAnswer;
        }
    }
}
