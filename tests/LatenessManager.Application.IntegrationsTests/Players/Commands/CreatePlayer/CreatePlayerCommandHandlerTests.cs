using System;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using LatenessManager.Application.Common;
using LatenessManager.Application.Common.Exceptions;
using LatenessManager.Application.Players.Commands.CreatePlayer;
using LatenessManager.Domain.Entities.PlayerAggregate;
using Xunit;

namespace LatenessManager.Application.IntegrationsTests.Players.Commands.CreatePlayer
{
    public class CreatePlayerCommandHandlerTests : TestBase
    {
        [Fact]
        public async Task adds_player_to_database_without_penalty()
        {
            // Arrange
            var command = Fixture
                .Build<CreatePlayerCommand>()
                .Without(c => c.InitialPenaltyCount)
                .Create();

            // Act
            var id = await SendAsync(command);

            // Assert
            var player = await FindAsync<Player>(id, p => p.Penalties);
            player.Name.FirstName.Should().Be(command.FirstName);
            player.Name.LastName.Should().Be(command.LastName);
            player.Penalties.Should().BeEmpty();
        }
        
        [Fact]
        public async Task adds_player_to_database_with_penalty()
        {
            // Arrange
            var command = Fixture
                .Build<CreatePlayerCommand>()
                .With(c => c.InitialPenaltyCount, Penalty.Unit)
                .Create();

            // Act
            var id = await SendAsync(command);

            // Assert
            var player = await FindAsync<Player>(id, p => p.Penalties);
            player.Name.FirstName.Should().Be(command.FirstName);
            player.Name.LastName.Should().Be(command.LastName);
            player.Penalties.Should().ContainSingle(penalty => penalty.Count == Penalty.Unit &&
                                                               penalty.Date == UtcNow.ToLocalTime().Date);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(99)]
        public void has_error_if_initial_penalty_count_is_invalid(int initialPenaltyCount)
        {
            // Arrange
            var command = Fixture
                .Build<CreatePlayerCommand>()
                .With(c => c.InitialPenaltyCount, initialPenaltyCount)
                .Create();
            
            Func<Task> act = async () => await SendAsync(command);

            // Act
            var exceptionAssertions = act.Should().Throw<ValidationException>();

            // Assert
            exceptionAssertions
                .Which
                .Errors
                .Should()
                .ContainKey(nameof(CreatePlayerCommand.InitialPenaltyCount))
                .WhichValue
                .Should()
                .ContainSingle(x => x.Code == ErrorCode.Penalty.InvalidCount);
        }
    }
}