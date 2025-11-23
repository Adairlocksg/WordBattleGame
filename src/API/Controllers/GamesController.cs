using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WordBattle.API.Controllers.Abstractions;
using WordBattle.Application.Abstractions.Messaging;
using WordBattle.Application.UseCases.Games.AddPlayer;
using WordBattle.Application.UseCases.Games.NewGame;
using WordBattle.Application.UseCases.Games.RegisterAnswer;
using WordBattle.Application.UseCases.Games.Start;
using WordBattle.Application.UseCases.Games.StartNextRound;
using WordBattle.Domain.ValueObjects;

namespace WordBattle.API.Controllers
{
    [Route("api/[controller]")]
    public class GamesController(
        ICommandHandler<NewGameCommand, Guid> createGameHandler,
        ICommandHandler<AddPlayerCommand, Guid> addPlayerHandler,
        ICommandHandler<StartGameCommand, Guid> startGameHandler,
        ICommandHandler<StartNextRoundCommand, StartNextRoundResponse> startNextRoundHandler,
        ICommandHandler<RegisterAnswerCommand, AnswerResult> registerAnswerHandler
        ) : ApiController
    {
        /// <summary>
        /// Cria um novo lobby de jogo.
        /// </summary>
        /// <returns>ID do jogo criado</returns>
        [HttpPost]
        [SwaggerOperation(Summary = "Inicia um novo lobby", Description = "Cria a instância do jogo para começar a adicionar jogadores.")]
        [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            var command = new NewGameCommand();
            return await HandleResult(async () => await createGameHandler.HandleAsync(command, ct));
        }

        /// <summary>
        /// Adiciona um jogador ao lobby.
        /// </summary>
        [HttpPost("{id}/players")]
        [SwaggerOperation(Summary = "Adiciona jogador", Description = "Registra um novo participante no jogo informado.")]
        [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddPlayer(
            [FromRoute] Guid id,
            [FromBody] AddPlayerDto input,
            CancellationToken ct)
        {
            var command = new AddPlayerCommand(id, input.Name);
            return await HandleResult(async () => await addPlayerHandler.HandleAsync(command, ct));
        }

        /// <summary>
        /// Inicia a partida (trava entrada de novos jogadores).
        /// </summary>
        [HttpPut("{id:guid}/start")]
        [SwaggerOperation(Summary = "Começa o jogo", Description = "Muda o status para Em Progresso e define a ordem dos turnos.")]
        [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status200OK)]
        public async Task<IActionResult> StartGame([FromRoute] Guid id, CancellationToken ct)
        {
            var command = new StartGameCommand(id);
            return await HandleResult(async () => await startGameHandler.HandleAsync(command, ct));
        }

        /// <summary>
        /// Inicia uma nova rodada (Sorteia categoria).
        /// </summary>
        [HttpPost("{id}/rounds")]
        [SwaggerOperation(Summary = "Girar Roleta / Nova Rodada", Description = "Cria uma nova rodada, sorteia uma categoria e define o primeiro jogador.")]
        [ProducesResponseType(typeof(ApiResponse<StartNextRoundResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> StartNextRound([FromRoute] Guid id, CancellationToken ct)
        {
            var command = new StartNextRoundCommand(id);
            return await HandleResult(async () => await startNextRoundHandler.HandleAsync(command, ct));
        }

        /// <summary>
        /// Registra a resposta de um jogador.
        /// </summary>
        [HttpPost("{id}/answers")]
        [SwaggerOperation(Summary = "Responder", Description = "Envia a palavra do jogador atual. Retorna se acertou, se foi eliminado e quem é o próximo.")]
        [ProducesResponseType(typeof(ApiResponse<AnswerResult>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Answer(
            [FromRoute] Guid id,
            [FromBody] RegisterAnswerDto input,
            CancellationToken ct)
        {
            var command = new RegisterAnswerCommand(id, input.PlayerId, input.Word);
            return await HandleResult(async () => await registerAnswerHandler.HandleAsync(command, ct));
        }
    }

    // DTOs auxiliares para não sujar os Commands com atributos de rota/body misturados
    public record AddPlayerDto(string Name);
    public record RegisterAnswerDto(Guid PlayerId, string Word);
}
