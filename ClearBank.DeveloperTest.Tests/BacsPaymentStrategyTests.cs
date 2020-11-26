using AutoFixture.Xunit2;
using ClearBank.DeveloperTest.PaymentStrategy;
using ClearBank.DeveloperTest.Types;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace ClearBank.DeveloperTest.Tests
{
    public class BacsPaymentStrategyTests
    {
        [Fact]
        public void Should_accept_bacs()
        {
            new BacsPaymentStrategy(null).Accepts.Should().Be(PaymentScheme.Bacs);
        }

        [Theory, AutoData]
        public void Should_make_payment(Account account)
        {
            var predicate = Substitute.For<IPaymentSchemePredicate>();
            predicate.IsAllowedScheme(account.AllowedPaymentSchemes, AllowedPaymentSchemes.Bacs).Returns(true);
            
            var strategy = new BacsPaymentStrategy(predicate);

            strategy.MakePayment(account, null).Success.Should().BeTrue();
        }
        
        [Theory, AutoData]
        public void Should_not_make_payment(Account account)
        {
            var predicate = Substitute.For<IPaymentSchemePredicate>();
            predicate.IsAllowedScheme(account.AllowedPaymentSchemes, AllowedPaymentSchemes.Bacs).Returns(false);
            
            var strategy = new BacsPaymentStrategy(predicate);

            strategy.MakePayment(account, null).Success.Should().BeFalse();
        }
    }
}