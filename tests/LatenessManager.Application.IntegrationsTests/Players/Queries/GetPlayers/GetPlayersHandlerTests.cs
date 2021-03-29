using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using LatenessManager.Application.Players.Queries.GetPlayers;
using LatenessManager.Domain.Entities.PlayerAggregate;
using Xunit;

namespace LatenessManager.Application.IntegrationsTests.Players.Queries.GetPlayers
{
    [Collection("Integration tests")]
    public class GetPlayersHandlerTests : TestBase
    {
        [Fact]
        public async Task gets_all_players()
        {
            // Arrange
            var player1 = Fixture.Create<Player>();
            player1.SetInitialPenalty(Fixture.Create<DateTime>(), 500);

            var player2 = Fixture.Create<Player>();
            player2.AddPenalty(Fixture.Create<DateTime>());
            player2.AddPenalty(Fixture.Create<DateTime>());
            player2.CarryOutPenalty(Fixture.Create<DateTime>());
            
            await AddAsync(player1, player2);

            var query = new GetPlayersQuery();

            var expected = new List<PlayerDto>
            {
                new()
                {
                    Id = player1.Id,
                    FirstName = player1.Name.FirstName,
                    LastName = player1.Name.LastName,
                    PenaltiesCount = 500
                },
                new()
                {
                    Id = player2.Id,
                    FirstName = player2.Name.FirstName,
                    LastName = player2.Name.LastName,
                    PenaltiesCount = 100
                }
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}