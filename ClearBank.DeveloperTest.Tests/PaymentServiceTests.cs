using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using Moq;
using Xunit;

namespace ClearBank.DeveloperTest.Tests
{
    public class PaymentServiceTests
    {
        private readonly Mock<IDataStoreFactory> _dataStoreFactory;
        private readonly Mock<IAccountDataStore> _accountDataStore;
        private readonly Mock<IPaymentValidator> _paymentValidator;
        private readonly PaymentService _sut;

        private readonly MakePaymentRequest _request;
        private readonly Account _account;

        public PaymentServiceTests()
        {
            _dataStoreFactory = new Mock<IDataStoreFactory>();
            _accountDataStore = new Mock<IAccountDataStore>();
            _paymentValidator = new Mock<IPaymentValidator>();

            _dataStoreFactory
                .Setup(x => x.Create())
                .Returns(_accountDataStore.Object);

            _request = new MakePaymentRequest
            {
                DebtorAccountNumber = "Account1",
                Amount = 50,
                PaymentScheme = PaymentScheme.FasterPayments
            };

            _account = new Account
            {
                Balance = 100
            };

            _accountDataStore
                .Setup(x => x.GetAccount(_request.DebtorAccountNumber))
                .Returns(_account);

            _sut = new PaymentService(
                _dataStoreFactory.Object,
                _paymentValidator.Object);
        }

        [Fact]
        public void MakePayment_WhenValidationFails_ReturnsFailure_DoesNotUpdateAccount()
        {
            _paymentValidator
                .Setup(x => x.Validate(_account, _request))
                .Returns(false);

            Assert.False(_sut.MakePayment(_request).Success);
            _accountDataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }

        [Fact]
        public void MakePayment_WhenValidationPasses_ReturnsSuccess_AndUpdatesAccount()
        {
            _paymentValidator
                .Setup(x => x.Validate(_account, _request))
                .Returns(true);

            Assert.True (_sut.MakePayment(_request).Success);
            Assert.Equal(50, _account.Balance);
            _accountDataStore.Verify(x => x.UpdateAccount(_account), Times.Once);
        }
    }
}

