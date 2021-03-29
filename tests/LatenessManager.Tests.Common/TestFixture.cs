using System;
using System.Linq;
using AutoFixture;

namespace LatenessManager.Tests.Common
{
    public static class TestFixture
    {
        public static IFixture Get()
        {
            var fixture = new Fixture();

            fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));

            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            fixture.Customize<decimal>(c => c.FromFactory<int>(i => (decimal)(i * new Random().NextDouble())));
            fixture.Customize<float>(c => c.FromFactory<int>(i => (float)(i * new Random().NextDouble())));
            fixture.Customize<double>(c => c.FromFactory<int>(i => i * new Random().NextDouble()));

            return fixture;
        }
    }
}