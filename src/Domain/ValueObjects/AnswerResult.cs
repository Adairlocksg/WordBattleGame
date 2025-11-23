namespace WordBattle.Domain.ValueObjects
{
    public record AnswerResult(
        Guid PlayerId,
        Word Word,
        bool IsValidTime,
        bool IsValidContent,
        bool IsEliminated,
        bool IsRoundFinished,
        bool IsGameFinished,
        Guid? WinnerId,
        Guid NextPlayerId
    );
}
