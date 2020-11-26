using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.PaymentStrategy
{
    public interface IPaymentSchemePredicate
    {
        bool IsAllowedScheme(AllowedPaymentSchemes schemes, AllowedPaymentSchemes scheme);
    }
}