using AutoFixture;
using AutoFixture.Xunit2;
using ClearBank.DeveloperTest.PaymentStrategy;
using ClearBank.DeveloperTest.Types;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace ClearBank.DeveloperTest.Tests
{
    public class FasterPaymentStrategyTests
    {
        [Fact]
        public void Should_accept_faster_payment()
        {
            new FasterPaymentStrategy(null).Accepts.Should().Be(PaymentScheme.FasterPayments);
        }

        [Theory]
        [InlineData(100, 99)]
        [InlineData(100, 100)]
        public void Should_make_payment(decimal balance, decimal paymentAmount)
        {
            var account = new Fixture().Build<Account>()
                .With(a => a.Balance, balance)
                .Create();

            var predicate = Substitute.For<IPaymentSchemePredicate>();
            predicate.IsAllowedScheme(account.AllowedPaymentSchemes, AllowedPaymentSchemes.FasterPayments)
                .Returns(true);

            var strategy = new FasterPaymentStrategy(predicate);

            strategy.MakePayment(account, new MakePaymentRequest {Amount = paymentAmount}).Success.Should().BeTrue();
        }

        [Theory, AutoData]
        public void Should_not_make_payment_when_not_allowed_by_scheme(Fixture fixture)
        {
            var account = fixture.Build<Account>()
                .With(a => a.Balance, 100)
                .Create();

            var predicate = Substitute.For<IPaymentSchemePredicate>();
            predicate.IsAllowedScheme(account.AllowedPaymentSchemes, AllowedPaymentSchemes.FasterPayments)
                .Returns(false);

            var strategy = new FasterPaymentStrategy(predicate);

            strategy.MakePayment(account, new MakePaymentRequest {Amount = 99}).Success.Should().BeFalse();
            predicate.Received().IsAllowedScheme(account.AllowedPaymentSchemes, AllowedPaymentSchemes.FasterPayments);
        }

        [Theory, AutoData]
        public void Should_not_make_payment_when_insufficient_balance(Fixture fixture)
        {
            var account = fixture.Build<Account>()
                .With(a => a.Balance, 99)
                .Create();

            var predicate = Substitute.For<IPaymentSchemePredicate>();
            predicate.IsAllowedScheme(account.AllowedPaymentSchemes, AllowedPaymentSchemes.FasterPayments)
                .Returns(true);

            var strategy = new FasterPaymentStrategy(predicate);

            strategy.MakePayment(account, new MakePaymentRequest {Amount = 100}).Success.Should().BeFalse();
        }
    }
}