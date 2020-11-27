using System;
using System.Linq;
using AutoFixture.Xunit2;
using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.PaymentStrategy;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace ClearBank.DeveloperTest.Tests
{
    public class PaymentServiceTests
    {
        [Theory, AutoData]
        public void Should_retrieve_data_store(string dataStoreType)
        {
            var options = new OptionsWrapper<PaymentServiceOptions>(new PaymentServiceOptions
                {DataStoreType = dataStoreType});

            var dataStoreFactory = Substitute.For<IAccountDataStoreFactory>();

            var paymentSchemeStrategy = Substitute.For<IPaymentSchemeStrategy>();
            paymentSchemeStrategy.MakePayment(default, default).ReturnsForAnyArgs(new MakePaymentResult());
            var paymentSchemeStrategies = new[] {paymentSchemeStrategy};

            var paymentService = new PaymentService(dataStoreFactory, paymentSchemeStrategies, options);

            paymentService.MakePayment(new MakePaymentRequest());

            dataStoreFactory.Received().MakeDataStore(dataStoreType);
        }

        [Theory, AutoData]
        public void Should_get_the_account(string accountNumber, PaymentServiceOptions paymentServiceOptions)
        {
            var options = Substitute.For<IOptions<PaymentServiceOptions>>();
            options.Value.Returns(paymentServiceOptions);

            var dataStore = Substitute.For<IAccountDataStore>();
            var dataStoreFactory = Substitute.For<IAccountDataStoreFactory>();
            dataStoreFactory.MakeDataStore(default).ReturnsForAnyArgs(dataStore);

            var paymentSchemeStrategy = Substitute.For<IPaymentSchemeStrategy>();
            paymentSchemeStrategy.MakePayment(default, default).ReturnsForAnyArgs(new MakePaymentResult());
            var paymentSchemeStrategies = new[] {paymentSchemeStrategy};

            var paymentService = new PaymentService(dataStoreFactory, paymentSchemeStrategies, options);

            paymentService.MakePayment(new MakePaymentRequest {DebtorAccountNumber = accountNumber});

            dataStore.Received().GetAccount(accountNumber);
        }

        [Theory, AutoData]
        public void Should_make_the_payment_using_the_right_strategy(Account account, PaymentServiceOptions
            paymentServiceOptions)
        {
            var options = Substitute.For<IOptions<PaymentServiceOptions>>();
            options.Value.Returns(paymentServiceOptions);

            var dataStore = Substitute.For<IAccountDataStore>();
            dataStore.GetAccount(default).ReturnsForAnyArgs(account);

            var dataStoreFactory = Substitute.For<IAccountDataStoreFactory>();
            dataStoreFactory.MakeDataStore(default).ReturnsForAnyArgs(dataStore);

            var strategies = Enum.GetValues(typeof(PaymentScheme)).Cast<PaymentScheme>().Select(
                scheme =>
                {
                    var strategy = Substitute.For<IPaymentSchemeStrategy>();
                    strategy.Accepts.Returns(scheme);
                    strategy.MakePayment(default, default).ReturnsForAnyArgs(new MakePaymentResult());
                    return strategy;
                }).ToList();

            var paymentService = new PaymentService(dataStoreFactory, strategies, options);

            var request = new MakePaymentRequest {PaymentScheme = PaymentScheme.Bacs};
            paymentService.MakePayment(request);

            strategies.Single(strategy => strategy.Accepts == PaymentScheme.Bacs).Received()
                .MakePayment(account, request);
            strategies.Single(strategy => strategy.Accepts == PaymentScheme.FasterPayments).DidNotReceiveWithAnyArgs
                ().MakePayment(default, default);
            strategies.Single(strategy => strategy.Accepts == PaymentScheme.Chaps).DidNotReceiveWithAnyArgs
                ().MakePayment(default, default);
        }

        [Theory, AutoData]
        public void Should_deduct_the_balance(PaymentServiceOptions paymentServiceOptions)
        {
            var options = Substitute.For<IOptions<PaymentServiceOptions>>();
            options.Value.Returns(paymentServiceOptions);

            var dataStore = Substitute.For<IAccountDataStore>();
            var account = new Account {Balance = 100};
            dataStore.GetAccount(default).ReturnsForAnyArgs(account);

            var dataStoreFactory = Substitute.For<IAccountDataStoreFactory>();
            dataStoreFactory.MakeDataStore(default).ReturnsForAnyArgs(dataStore);

            var paymentSchemeStrategy = Substitute.For<IPaymentSchemeStrategy>();
            paymentSchemeStrategy.MakePayment(default, default)
                .ReturnsForAnyArgs(new MakePaymentResult {Success = true});
            var strategies = new[] {paymentSchemeStrategy};

            var paymentService = new PaymentService(dataStoreFactory, strategies, options);

            paymentService.MakePayment(new MakePaymentRequest {Amount = 20}).Success.Should().BeTrue();

            dataStore.Received().UpdateAccount(Arg.Is<Account>(a => a.Balance == 80));
        }
        [Theory, AutoData]
        public void Should_not_deduct_the_balance(PaymentServiceOptions paymentServiceOptions)
        {
            var options = Substitute.For<IOptions<PaymentServiceOptions>>();
            options.Value.Returns(paymentServiceOptions);

            var dataStore = Substitute.For<IAccountDataStore>();
            var account = new Account {Balance = 100};
            dataStore.GetAccount(default).ReturnsForAnyArgs(account);

            var dataStoreFactory = Substitute.For<IAccountDataStoreFactory>();
            dataStoreFactory.MakeDataStore(default).ReturnsForAnyArgs(dataStore);

            var paymentSchemeStrategy = Substitute.For<IPaymentSchemeStrategy>();
            paymentSchemeStrategy.MakePayment(default, default)
                .ReturnsForAnyArgs(new MakePaymentResult {Success = false});
            var strategies = new[] {paymentSchemeStrategy};

            var paymentService = new PaymentService(dataStoreFactory, strategies, options);

            paymentService.MakePayment(new MakePaymentRequest {Amount = 20}).Success.Should().BeFalse();

            dataStore.DidNotReceiveWithAnyArgs().UpdateAccount(default);
        }
    }
}