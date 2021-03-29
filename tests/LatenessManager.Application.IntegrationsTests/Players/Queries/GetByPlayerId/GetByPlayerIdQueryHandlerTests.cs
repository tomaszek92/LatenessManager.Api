using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using LatenessManager.Application.Players.Queries.GetPlayerById;
using LatenessManager.Domain.Entities.PlayerAggregate;
using Xunit;

namespace LatenessManager.Application.IntegrationsTests.Players.Queries.GetByPlayerId
{
    public class GetByPlayerIdQueryHandlerTests : TestBase
    {
        [Fact]
        public async Task gets_player_by_given_id()
        {
            // Arrange
            var otherPlayers = Fixture.CreateMany<Player>().ToArray();
            
            foreach (var player in otherPlayers)
            {
                player.AddPenalty(Fixture.Create<DateTime>());
            }
            
            await AddAsync(otherPlayers);

            var expectedPlayer = Fixture.Create<Player>();
            expectedPlayer.AddPenalty(Fixture.Create<DateTime>());
            expectedPlayer.CarryOutPenalty(Fixture.Create<DateTime>());

            await AddAsync(expectedPlayer);

            var query = new GetPlayerByIdQuery(expectedPlayer.Id);

            var expected = new PlayerDetailsDto
            {
                Id = expectedPlayer.Id,
                FirstName = expectedPlayer.Name.FirstName,
                LastName = expectedPlayer.Name.LastName,
                Penalties = expectedPlayer.Penalties
                    .Select(penalty => new PenaltyDto
                    {
                        Date = penalty.Date.Date,
                        Count = penalty.Count
                    })
                    .ToList()
            };

            // Act
            var playerDetailsDto = await SendAsync(query);

            // Assert
            playerDetailsDto.Should().BeEquivalentTo(expected);
        }
    }
}