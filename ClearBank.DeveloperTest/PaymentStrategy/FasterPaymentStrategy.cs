using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.PaymentStrategy
{
    public class FasterPaymentStrategy : IPaymentSchemeStrategy
    {
        public PaymentScheme Accepts { get; } = PaymentScheme.FasterPayments;

        public MakePaymentResult MakePayment(Account account, MakePaymentRequest request)
        {
            return new MakePaymentResult
            {
                Success = account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments) &&
                          account.Balance < request.Amount
            };
        }
    }
}