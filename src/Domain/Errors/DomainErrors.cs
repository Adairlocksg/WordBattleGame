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
        }
    }
}
