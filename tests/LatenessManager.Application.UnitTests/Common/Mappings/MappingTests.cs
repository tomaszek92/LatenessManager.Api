using System;
using System.Collections.Generic;
using AutoFixture;
using AutoFixture.Kernel;
using AutoMapper;
using LatenessManager.Application.Common.Mappings;
using LatenessManager.Application.Players.Queries.GetPlayerById;
using LatenessManager.Application.Players.Queries.GetPlayers;
using LatenessManager.Domain.Entities.PlayerAggregate;
using LatenessManager.Tests.Common;
using Xunit;

namespace LatenessManager.Application.UnitTests.Common.Mappings
{
    public class MappingTests
    {
        private readonly IFixture _fixture = TestFixture.Get();
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;

        public MappingTests()
        {
            _configuration = new MapperConfiguration(cfg => { cfg.AddProfile<MappingProfile>(); });

            _mapper = _configuration.CreateMapper();
        }

        [Fact]
        public void has_valid_configuration()
        {
            _configuration.AssertConfigurationIsValid();
        }

        public static IEnumerable<object[]> MappingData =>
            new List<object[]>
            {
                new object[] { typeof(Player), typeof(PlayerDto) },
                new object[] { typeof(Player), typeof(PlayerDetailsDto) },
                new object[] { typeof(Penalty), typeof(PenaltyDto) }
            };
        
        [Theory]
        [MemberData(nameof(MappingData))]
        public void supports_mapping_from_source_to_destination(Type source, Type destination)
        {
            var instance = GetInstanceOf(source);

            _mapper.Map(instance, source, destination);
        }

        private object GetInstanceOf(Type type) => _fixture.Create(type, new SpecimenContext(_fixture));
    }
}