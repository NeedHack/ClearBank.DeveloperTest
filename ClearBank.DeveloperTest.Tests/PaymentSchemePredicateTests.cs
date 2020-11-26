using ClearBank.DeveloperTest.PaymentStrategy;
using ClearBank.DeveloperTest.Types;
using FluentAssertions;
using Xunit;

namespace ClearBank.DeveloperTest.Tests
{
    public class PaymentSchemePredicateTests
    {
        [Theory]
        [InlineData(AllowedPaymentSchemes.Bacs)]
        [InlineData(AllowedPaymentSchemes.Bacs | AllowedPaymentSchemes.Chaps)]
        [InlineData(AllowedPaymentSchemes.Bacs | AllowedPaymentSchemes.Chaps | AllowedPaymentSchemes.FasterPayments)]
        public void Should_return_true(AllowedPaymentSchemes allowedPaymentSchemes)
        {
            var predicate = new PaymentSchemePredicate();

            predicate.IsAllowedScheme(allowedPaymentSchemes, AllowedPaymentSchemes.Bacs).Should().BeTrue();
        }        
        
        [Theory]
        [InlineData(AllowedPaymentSchemes.Chaps)]
        [InlineData(AllowedPaymentSchemes.FasterPayments)]
        [InlineData(AllowedPaymentSchemes.Chaps | AllowedPaymentSchemes.FasterPayments)]
        public void Should_return_false(AllowedPaymentSchemes allowedPaymentSchemes)
        {
            var predicate = new PaymentSchemePredicate();

            predicate.IsAllowedScheme(allowedPaymentSchemes, AllowedPaymentSchemes.Bacs).Should().BeFalse();
        }        
    }
}