using AutoFixture;
using AutoFixture.Xunit2;
using ClearBank.DeveloperTest.PaymentStrategy;
using ClearBank.DeveloperTest.Types;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace ClearBank.DeveloperTest.Tests
{
    public class ChapsPaymentStrategyTests
    {
        [Fact]
        public void Should_accept_chaps()
        {
            new ChapsPaymentStrategy(null).Accepts.Should().Be(PaymentScheme.Chaps);
        }

        [Theory, AutoData]
        public void Should_make_payment(Fixture fixture)
        {
            var account = fixture.Build<Account>()
                .With(a => a.Status, AccountStatus.Live)
                .Create();

            var predicate = Substitute.For<IPaymentSchemePredicate>();
            predicate.IsAllowedScheme(account.AllowedPaymentSchemes, AllowedPaymentSchemes.Chaps).Returns(true);

            var strategy = new ChapsPaymentStrategy(predicate);

            strategy.MakePayment(account, null).Success.Should().BeTrue();
        }

        [Theory]
        [InlineData(AccountStatus.Disabled)]
        [InlineData(AccountStatus.InboundPaymentsOnly)]
        public void Should_not_make_payment_when_not_live(AccountStatus accountStatus)
        {
            var account = new Fixture().Build<Account>()
                .With(a => a.Status, accountStatus)
                .Create();

            var predicate = Substitute.For<IPaymentSchemePredicate>();
            predicate.IsAllowedScheme(account.AllowedPaymentSchemes, AllowedPaymentSchemes.Chaps).Returns(true);

            var strategy = new ChapsPaymentStrategy(predicate);

            strategy.MakePayment(account, null).Success.Should().BeFalse();
        }

        [Theory, AutoData]
        public void Should_not_make_payment_when_not_allowed_scheme(AllowedPaymentSchemes schemes)
        {
            var account = new Fixture().Build<Account>()
                .With(a => a.AllowedPaymentSchemes, schemes)
                .With(a => a.Status, AccountStatus.Live)
                .Create();

            var predicate = Substitute.For<IPaymentSchemePredicate>();
            var strategy = new ChapsPaymentStrategy(predicate);

            strategy.MakePayment(account, null).Success.Should().BeFalse();
            predicate.Received().IsAllowedScheme(schemes, AllowedPaymentSchemes.Chaps);
        }
    }
}