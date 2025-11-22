using FluentAssertions;
using WordBattle.Domain.Entities.Categories;
using WordBattle.Domain.Errors;

namespace WordBattle.Domain.Tests.Entities
{
    public class CategoryTests
    {
        [Fact]
        public void Create_ShouldReturnSuccess_WhenDescriptionIsValid()
        {
            var validDescription = "Teste";

            var result = Category.Create(validDescription);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Description.Should().Be(validDescription);
            result.Value.Id.Should().NotBeEmpty();
        }

        [Fact]
        public void Create_ShouldReturnFailure_WhenDescriptionIsEmpty()
        {
            var invalidDescription = string.Empty;

            var result = Category.Create(invalidDescription);

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(DomainErrors.Category.EmptyDescription);
        }

        [Theory] // Theory permite passar vários cenários (InlineData)
        [InlineData("oi")] // Menor que 3
        [InlineData("ab")]
        public void Create_ShouldReturnFailure_WhenDescriptionIsTooShort(string shortDescription)
        {
            var result = Category.Create(shortDescription);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(DomainErrors.Category.ShortDescription);
        }

        [Fact]
        public void Create_ShouldReturnFailure_WhenDescriptionIsTooLong()
        {
            var invalidDescription = new string('a', 201);

            var result = Category.Create(invalidDescription);

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(DomainErrors.Category.LongDescription);
        }
    }
}
