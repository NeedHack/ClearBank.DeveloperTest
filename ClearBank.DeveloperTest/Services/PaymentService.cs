using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IAccountDataStoreFactory _accountDataStoreFactory;
        private readonly IConfiguration _configuration;

        public PaymentService(IAccountDataStoreFactory accountDataStoreFactory, IConfiguration configuration)
        {
            _accountDataStoreFactory = accountDataStoreFactory;
            _configuration = configuration;
        }

        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var dataStoreType = _configuration.AppSettings("DataStoreType");

            var accountDataStore = _accountDataStoreFactory.MakeDataStore(dataStoreType);

            var account = accountDataStore.GetAccount(request.DebtorAccountNumber);

            var result = new MakePaymentResult();

            // switch (request.PaymentScheme)
            // {
            //     case PaymentScheme.Bacs:
            //         if (account == null)
            //         {
            //             result.Success = false;
            //         }
            //         else if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs))
            //         {
            //             result.Success = false;
            //         }
            //
            //         break;
            //
            //     case PaymentScheme.FasterPayments:
            //         if (account == null)
            //         {
            //             result.Success = false;
            //         }
            //         else if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments))
            //         {
            //             result.Success = false;
            //         }
            //         else if (account.Balance < request.Amount)
            //         {
            //             result.Success = false;
            //         }
            //
            //         break;
            //
            //     case PaymentScheme.Chaps:
            //         if (account == null)
            //         {
            //             result.Success = false;
            //         }
            //         else if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps))
            //         {
            //             result.Success = false;
            //         }
            //         else if (account.Status != AccountStatus.Live)
            //         {
            //             result.Success = false;
            //         }
            //
            //         break;
            // }

            if (result.Success)
            {
                account.Balance -= request.Amount;
                accountDataStore.UpdateAccount(account);
            }

            return result;
        }
    }
}