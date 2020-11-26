using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.PaymentStrategy
{
    public interface IPaymentSchemeStrategy
    {
        PaymentScheme Accepts { get; }

        MakePaymentResult MakePayment(Account account, MakePaymentRequest request);
    }
}