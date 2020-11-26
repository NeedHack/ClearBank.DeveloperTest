using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.PaymentStrategy
{
    public class PaymentSchemePredicate : IPaymentSchemePredicate
    {
        public bool IsAllowedScheme(AllowedPaymentSchemes schemes, AllowedPaymentSchemes scheme)
        {
            return schemes.HasFlag(AllowedPaymentSchemes.Bacs);
        }
    }
}