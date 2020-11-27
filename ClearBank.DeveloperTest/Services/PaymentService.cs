using System.Collections.Generic;
using System.Linq;
using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.PaymentStrategy;
using ClearBank.DeveloperTest.Types;
using Microsoft.Extensions.Options;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IAccountDataStoreFactory _accountDataStoreFactory;
        private readonly IEnumerable<IPaymentSchemeStrategy> _paymentSchemes;
        private readonly IOptions<PaymentServiceOptions> _options;

        public PaymentService(IAccountDataStoreFactory accountDataStoreFactory,
            IEnumerable<IPaymentSchemeStrategy> paymentSchemes, IOptions<PaymentServiceOptions> options)
        {
            _accountDataStoreFactory = accountDataStoreFactory;
            _paymentSchemes = paymentSchemes;
            _options = options;
        }

        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var dataStoreType = _options.Value.DataStoreType; 

            var accountDataStore = _accountDataStoreFactory.MakeDataStore(dataStoreType);

            var account = accountDataStore.GetAccount(request.DebtorAccountNumber);

            var paymentStrategy = _paymentSchemes.Single(strategy => strategy.Accepts == request.PaymentScheme);

            var result = paymentStrategy.MakePayment(account, request);

            if (result.Success)
            {
                account.Balance -= request.Amount;
                accountDataStore.UpdateAccount(account);
            }

            return result;
        }
    }
}