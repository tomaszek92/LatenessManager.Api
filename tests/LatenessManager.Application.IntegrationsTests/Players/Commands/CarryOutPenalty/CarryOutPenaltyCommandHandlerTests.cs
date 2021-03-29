using System;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using LatenessManager.Application.Common;
using LatenessManager.Application.Common.Exceptions;
using LatenessManager.Application.Players.Commands.AddPenalty;
using LatenessManager.Application.Players.Commands.CarryOutPenalty;
using LatenessManager.Domain.Entities.PlayerAggregate;
using Xunit;

namespace LatenessManager.Application.IntegrationsTests.Players.Commands.CarryOutPenalty
{
    public class CarryOutPenaltyCommandHandlerTests : TestBase
    {
        [Fact]
        public async Task carries_out_penalty_to_given_player()
        {
            // Arrange
            var otherPlayer = Fixture.Create<Player>();

            var player = Fixture.Create<Player>();
            player.AddPenalty(Fixture.Create<DateTime>());

            await AddAsync(otherPlayer, player);

            var command = new CarryOutPenaltyCommand
            {
                Id = player.Id,
                Date = Fixture.Create<DateTime>()
            };

            // Act
            await SendAsync(command);

            // Assert
            var updatedPlayer = await FindAsync<Player>(player.Id, p => p.Penalties);
            updatedPlayer.Name.FirstName.Should().Be(player.Name.FirstName);
            updatedPlayer.Name.LastName.Should().Be(player.Name.LastName);
            updatedPlayer.Penalties.Should().HaveCount(2);
            updatedPlayer.Penalties.Should().ContainSingle(penalty => penalty.Count == -Penalty.Unit &&
                                                                      penalty.Date == command.Date.Date);
        }

        [Fact]
        public void has_error_if_player_does_not_exist_for_given_id()
        {
            // Arrange
            var command = new CarryOutPenaltyCommand
            {
                Id = Fixture.Create<int>(),
                Date = Fixture.Create<DateTime>()
            };

            Func<Task> act = async () => await SendAsync(command);

            // Act
            var exceptionAssertions = act.Should().Throw<ValidationException>();

            // Assert
            exceptionAssertions
                .Which
                .Errors
                .Should()
                .ContainKey(nameof(CarryOutPenaltyCommand.Id))
                .WhichValue
                .Should()
                .ContainSingle(x => x.Code == ErrorCode.Player.NotExist);
        }
    }
}