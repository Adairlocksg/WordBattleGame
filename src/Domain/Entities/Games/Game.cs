using WordBattle.Domain.Entities.Games.Players;
using WordBattle.Domain.Entities.Games.Rounds;
using WordBattle.Domain.Errors;
using WordBattle.Domain.Shared;
using WordBattle.Domain.ValueObjects;

namespace WordBattle.Domain.Entities.Games
{
    public class Game : Entity
    {
        internal protected Game() { }

        public DateTime? StartedAt { get; private set; }
        public DateTime? FinishedAt { get; private set; }
        public Guid? WinnerPlayerId { get; private set; }
        public Round? CurrentRound => _rounds.OrderByDescending(r => r.Number).FirstOrDefault();

        public IReadOnlyCollection<Player> Players => _players.AsReadOnly();
        private readonly List<Player> _players = [];

        public IReadOnlyCollection<Round> Rounds => _rounds.AsReadOnly();
        private readonly List<Round> _rounds = [];

        public static Game Create()
        {
            return new Game();
        }

        public Result<AnswerResult> RegisterAnswer(Guid playerId, string wordStr)
        {
            if (!StartedAt.HasValue)
                return Result.Failure<AnswerResult>(DomainErrors.Game.GameNotStarted);

            if (FinishedAt.HasValue)
                return Result.Failure<AnswerResult>(DomainErrors.Game.GameAlreadyFinished);

            if (CurrentRound is null)
                return Result.Failure<AnswerResult>(DomainErrors.Game.RoundNotStarted);

            var player = _players.FirstOrDefault(p => p.Id == playerId);
            if (player is null)
                return Result.Failure<AnswerResult>(DomainErrors.Game.PlayerNotFound);

            if (!player.IsPlaying)
                return Result.Failure<AnswerResult>(DomainErrors.Game.PlayerAlreadyEliminated);

            var word = new Word(wordStr);
            var resultAnswer = CurrentRound.ProcessAnswer(player, word);
            if (resultAnswer.IsFailure)
                return Result.Failure<AnswerResult>(resultAnswer.Error);

            var answer = resultAnswer.Value;

            if (!answer.IsValidContent || !answer.IsValidTime)
            {
                player.Eliminate();
            }

            var activePlayers = _players.Where(p => p.IsPlaying).ToList();

            var answersInRound = CurrentRound.Answers.Count;

            bool roundFinished = false;
            if (answersInRound >= CurrentRound.ExpectedAnswers)
            {
                CurrentRound.Finish();
                roundFinished = true;

                if (activePlayers.Count is 1 or 0)
                {
                    var winner = activePlayers.FirstOrDefault();
                    FinishGame(winner);
                }
            }

            var nextPlayer = _players.OrderBy(x => x.Sequence).FirstOrDefault(x => x.Sequence > player.Sequence && x.IsPlaying == true);

            return new AnswerResult(
                playerId,
                word, answer.IsValidTime,
                answer.IsValidContent,
                !player.IsPlaying,
                roundFinished,
                FinishedAt.HasValue,
                WinnerPlayerId,
                nextPlayer?.Id ?? Guid.Empty
             );
        }

        public Result<Game> StartNextRound(Guid categoryId)
        {
            if (!StartedAt.HasValue)
                return Result.Failure<Game>(DomainErrors.Game.GameNotStarted);

            if (FinishedAt.HasValue)
                return Result.Failure<Game>(DomainErrors.Game.GameAlreadyFinished);

            if (CurrentRound is not null && !CurrentRound.IsFinished)
                return Result.Failure<Game>(DomainErrors.Game.GameRoundNotFinished);

            var activePlayers = _players.Count(x => x.IsPlaying);
            var roundNumber = _rounds.Count + 1;
            var round = Round.Create(roundNumber, categoryId, Id, activePlayers);

            _rounds.Add(round);

            return Result.Success(this);
        }

        public Player? GetFirstActivePlayer()
        {
            return _players.OrderBy(x => x.Sequence).FirstOrDefault(x => x.IsPlaying);
        }

        public Result<Game> AddPlayer(string name)
        {
            if (StartedAt.HasValue)
                return Result.Failure<Game>(DomainErrors.Game.GameAlreadyStarted);

            if (FinishedAt.HasValue)
                return Result.Failure<Game>(DomainErrors.Game.GameAlreadyFinished);

            if (_players.Any(x => x.Name == name))
                return Result.Failure<Game>(DomainErrors.Game.PlayerNameAlreadyExists);

            var countPlayers = _players.Count;
            var resultPlayer = Player.Create(name, countPlayers + 1, Id);

            if (resultPlayer.IsFailure)
                return Result.Failure<Game>(resultPlayer.Error);

            _players.Add(resultPlayer.Value);

            return Result.Success(this);
        }

        public Result StartGame()
        {
            if (_players.Count < 2)
                return Result.Failure(DomainErrors.Game.NotEnoughPlayersToStart);

            StartedAt = DateTime.UtcNow;

            return Result.Success();
        }

        private void FinishGame(Player? winner)
        {
            WinnerPlayerId = winner?.Id;
            FinishedAt = DateTime.UtcNow;
        }
    }
}
