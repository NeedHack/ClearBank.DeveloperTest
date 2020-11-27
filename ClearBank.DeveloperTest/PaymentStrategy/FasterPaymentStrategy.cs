using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.PaymentStrategy
{
    public class FasterPaymentStrategy : IPaymentSchemeStrategy
    {
        private readonly IPaymentSchemePredicate _predicate;
        public PaymentScheme Accepts { get; } = PaymentScheme.FasterPayments;

        public FasterPaymentStrategy(IPaymentSchemePredicate predicate)
        {
            _predicate = predicate;
        }

        public MakePaymentResult MakePayment(Account account, MakePaymentRequest request)
        {
            return new MakePaymentResult
            {
                Success = _predicate.IsAllowedScheme(account.AllowedPaymentSchemes,
                              AllowedPaymentSchemes.FasterPayments) &&
                          account.Balance >= request.Amount
            };
        }
    }
}