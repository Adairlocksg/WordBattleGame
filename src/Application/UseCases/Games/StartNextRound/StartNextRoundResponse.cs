namespace WordBattle.Application.UseCases.Games.StartNextRound
{
    public record StartNextRoundResponse(Guid RoundId, int RoundNumber, string CategoryDescription, string PlayerName, Guid PlayerId);
}
