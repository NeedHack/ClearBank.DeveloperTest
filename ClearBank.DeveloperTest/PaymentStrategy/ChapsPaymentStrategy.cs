using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.PaymentStrategy
{
    public class ChapsPaymentStrategy : IPaymentSchemeStrategy
    {
        private readonly IPaymentSchemePredicate _predicate;

        public ChapsPaymentStrategy(IPaymentSchemePredicate predicate)
        {
            _predicate = predicate;
        }

        public PaymentScheme Accepts { get; } = PaymentScheme.Chaps;

        public MakePaymentResult MakePayment(Account account, MakePaymentRequest request)
        {
            return new MakePaymentResult
            {
                Success = _predicate.IsAllowedScheme(account.AllowedPaymentSchemes, AllowedPaymentSchemes.Chaps)
                          && account.Status == AccountStatus.Live
            };
        }
    }
}