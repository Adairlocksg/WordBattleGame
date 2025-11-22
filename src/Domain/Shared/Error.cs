namespace WordBattle.Domain.Shared
{
    public record Error(string Code, string Message)
    {
        public static Error None = new(string.Empty, string.Empty);
        public static Error NullValue = new("Error.NullValue", "Um valor nulo foi fornecido.");
        public static Error NotFound = new("Error.NotFound", "O recurso solicitado não foi encontrado.");
    }
}
