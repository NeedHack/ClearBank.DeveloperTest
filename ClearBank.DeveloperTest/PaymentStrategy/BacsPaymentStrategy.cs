using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.PaymentStrategy
{
    public class BacsPaymentStrategy : IPaymentSchemeStrategy
    {
        private readonly IPaymentSchemePredicate _paymentSchemePredicate;
        public PaymentScheme Accepts { get; } = PaymentScheme.Bacs;

        public BacsPaymentStrategy(IPaymentSchemePredicate paymentSchemePredicate)
        {
            _paymentSchemePredicate = paymentSchemePredicate;
        }

        public MakePaymentResult MakePayment(Account account, MakePaymentRequest request)
        {
            return new MakePaymentResult
            {
                Success = _paymentSchemePredicate.IsAllowedScheme(account.AllowedPaymentSchemes, AllowedPaymentSchemes.Bacs)
            };
        }
    }
}