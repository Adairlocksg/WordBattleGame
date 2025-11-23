using FluentAssertions;
using WordBattle.Domain.Entities.Games;
using WordBattle.Domain.Errors;

namespace WordBattle.Domain.Tests.Entities
{
    public class GameTests
    {
        [Fact]
        public void Create_ShouldInitialize_WithEmptyCollections()
        {
            // Act
            var game = Game.Create();

            // Assert
            game.Id.Should().NotBeEmpty();
            game.Players.Should().BeEmpty();
            game.Rounds.Should().BeEmpty();
            game.StartedAt.Should().BeNull();
            game.FinishedAt.Should().BeNull();
        }

        [Fact]
        public void AddPlayer_ShouldReturnSuccess_WhenGameNotStarted()
        {
            // Arrange
            var game = Game.Create();

            // Act
            var result = game.AddPlayer("Player 1");

            // Assert
            result.IsSuccess.Should().BeTrue();
            game.Players.Should().HaveCount(1);
            game.Players.First().Name.Should().Be("Player 1");
        }

        [Fact]
        public void AddPlayer_ShouldFail_WhenNameIsDuplicate()
        {
            // Arrange
            var game = Game.Create();
            game.AddPlayer("Player 1");

            // Act
            var result = game.AddPlayer("Player 1"); // Mesmo nome

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(DomainErrors.Game.PlayerNameAlreadyExists);
        }

        [Fact]
        public void StartGame_ShouldFail_WhenInsufficientPlayers()
        {
            // Arrange
            var game = Game.Create();
            game.AddPlayer("Solo Player");

            // Act
            var result = game.StartGame();

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(DomainErrors.Game.NotEnoughPlayersToStart);
        }

        [Fact]
        public void StartNextRound_ShouldCreateRound_WhenGameIsInProgress()
        {
            // Arrange
            var game = Game.Create();
            game.AddPlayer("P1");
            game.AddPlayer("P2");
            game.StartGame();
            var categoryId = Guid.NewGuid();

            // Act
            var result = game.StartNextRound(categoryId);

            // Assert
            result.IsSuccess.Should().BeTrue();
            game.Rounds.Should().HaveCount(1);
            game.CurrentRound.Should().NotBeNull();
            game.CurrentRound!.Number.Should().Be(1);
        }

        [Fact]
        public void RegisterAnswer_ShouldSaveAnswer_WhenValid()
        {
            // Arrange (Cenário completo: Jogo rolando com Round 1)
            var game = Game.Create();
            game.AddPlayer("P1");
            game.AddPlayer("P2");
            game.StartGame();
            game.StartNextRound(Guid.NewGuid());

            var player1 = game.Players.First(p => p.Name == "P1");

            // Act
            var result = game.RegisterAnswer(player1.Id, "Teste");

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.IsValidContent.Should().BeTrue();

            // Verifica se salvou dentro do Round
            game.CurrentRound.Answers.Should().HaveCount(1);
            game.CurrentRound.Answers.First().Word.Value.Should().Be("Teste");
        }

        [Fact]
        public void RegisterAnswer_ShouldEliminatePlayer_WhenWordIsDuplicate()
        {
            // Arrange
            var game = Game.Create();
            game.AddPlayer("P1");
            game.AddPlayer("P2");
            game.StartGame();
            game.StartNextRound(Guid.NewGuid());

            var p1 = game.Players.First(p => p.Name == "P1");
            var p2 = game.Players.First(p => p.Name == "P2");

            // Act 1: P1 responde "Banana"
            game.RegisterAnswer(p1.Id, "Banana");

            // Act 2: P2 tenta responder "Banana" também
            var resultP2 = game.RegisterAnswer(p2.Id, "Banana");

            // Assert
            resultP2.IsSuccess.Should().BeTrue();

            var answerResult = resultP2.Value;
            answerResult.IsValidContent.Should().BeFalse();
            answerResult.IsEliminated.Should().BeTrue();

            p2.IsPlaying.Should().BeFalse(); // Verifica o estado da entidade Player
        }

        [Fact]
        public void RegisterAnswer_ShouldFinishRound_WhenAllActivePlayersAnswered()
        {
            // Arrange
            var game = Game.Create();
            game.AddPlayer("P1");
            game.AddPlayer("P2");
            game.StartGame();
            game.StartNextRound(Guid.NewGuid());

            var p1 = game.Players.First(p => p.Name == "P1");
            var p2 = game.Players.First(p => p.Name == "P2");

            // Act
            var res1 = game.RegisterAnswer(p1.Id, "Um");
            var res2 = game.RegisterAnswer(p2.Id, "Dois");

            // Assert
            res1.Value.IsRoundFinished.Should().BeFalse();
            res2.Value.IsRoundFinished.Should().BeTrue();

            game.CurrentRound.IsFinished.Should().BeTrue();
        }

        [Fact]
        public void RegisterAnswer_ShouldNotFinishRound_WhenFirstPlayerPlays_InTwoPlayerGame()
        {
            // Arrange
            var game = Game.Create();
            game.AddPlayer("Jogador 1"); // Sequence 1
            game.AddPlayer("Jogador 2"); // Sequence 2
            game.StartGame();

            // Inicia o Round (ExpectedAnswers deve ser 2)
            game.StartNextRound(Guid.NewGuid());

            var p1 = game.Players.First(p => p.Name == "Jogador 1");

            // Act - Jogador 1 responde
            var result = game.RegisterAnswer(p1.Id, "Abacaxi");

            // Assert
            result.IsSuccess.Should().BeTrue();
            
            result.Value.IsRoundFinished.Should().BeFalse();
            result.Value.IsGameFinished.Should().BeFalse();
            result.Value.NextPlayerId.Should().NotBeEmpty();

            game.CurrentRound.IsFinished.Should().BeFalse();
        }

        [Fact]
        public void RegisterAnswer_ShouldFinishGame_WhenLastPlayerPlays_AndOnlyOneRemains()
        {
            // Arrange
            var game = Game.Create();
            game.AddPlayer("Vencedor"); // P1
            game.AddPlayer("Perdedor"); // P2
            game.StartGame();
            game.StartNextRound(Guid.NewGuid());

            var p1 = game.Players.First(p => p.Name == "Vencedor");
            var p2 = game.Players.First(p => p.Name == "Perdedor");

 
            game.RegisterAnswer(p1.Id, "Carro");

            var result = game.RegisterAnswer(p2.Id, "Carro");

            // Assert
            result.IsSuccess.Should().BeTrue();
            var finalState = result.Value;

            // Verificações de Estado
            finalState.IsRoundFinished.Should().BeTrue();
            finalState.IsGameFinished.Should().BeTrue()
            finalState.WinnerId.Should().Be(p1.Id);

            game.FinishedAt.Should().NotBeNull();
            game.WinnerPlayerId.Should().Be(p1.Id);
        }
    }
}
