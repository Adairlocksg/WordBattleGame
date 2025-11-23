using System;
using System.Collections.Generic;
using System.Text;
using WordBattle.Domain.Shared;

namespace WordBattle.Domain.Errors
{
    public static class DomainErrors
    {
        public static class Category
        {
            public static readonly Error EmptyDescription = new("Category.EmptyDescription", "A descrição da cateoria não pode ser vazia");
            public static readonly Error ShortDescription = new("Category.ShortDescription", "A descrição da categoria tem que possuir mais de 3 caracteres");
            public static readonly Error LongDescription = new("Category.LongDescription", "A descrição da categoria não pode possuir mais de 100 caracteres");
            public static readonly Error NoActiveCategory = new("Category.NoActiveCategory", "Não existem categorias ativas para iniciar um novo round");
        }

        public static class Player
        {
            public static readonly Error EmptyName = new("Player.EmptyName", "O nome do jogador é obrigatório");
            public static readonly Error LongName = new("Player.LongName", "O nome do jogador não pode ter mais de 100 caracteres");
        }

        public static class Game
        {
            public static readonly Error NotEnoughPlayersToStart = new("Game.NotEnoughPlayersToStart", "Não é possível iniciar um jogo com menos de 2 jogadores");
            public static readonly Error GameAlreadyStarted = new("Game.GameAlreadyStarted", "O jogo já foi iniciado");
            public static readonly Error GameNotStarted = new("Game.GameNotStarted", "O jogo ainda não foi iniciado");
            public static readonly Error GameAlreadyFinished = new("Game.GameAlreadyFinished", "O jogo já finalizou");
            public static readonly Error GameRoundNotFinished = new("Game.GameRoundNotFinished", "O round ainda não finalizou");
            public static readonly Error PlayerNameAlreadyExists = new("Game.PlayerNameAlreadyExists", "Já existe um jogador com esse nome no jogo");
            public static readonly Error PlayerNotFound = new("Game.PlayerNotFound", "Não foi encontrado o jogador informado");
            public static readonly Error PlayerAlreadyEliminated = new("Game.PlayerAlreadyEliminated", "O jogador já foi eliminado");
            public static readonly Error RoundNotStarted = new("Game.RoundNotStarted", "O round ainda não foi iniciado");
            public static readonly Error RoundNotFinished = new("Game.RoundNotFinished", "O round ainda não acabou");
        }
    }
}
