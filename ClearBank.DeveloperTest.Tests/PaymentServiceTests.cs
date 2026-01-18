using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using Moq;
using Xunit;

namespace ClearBank.DeveloperTest.Tests
{
    public class PaymentServiceTests
    {
        private readonly Mock<IDataStoreFactory> _dataStoreFactory = new();
        private readonly Mock<IAccountDataStore> _accountDataStore = new();
        private readonly Mock<IPaymentValidator> _validator = new();

        private PaymentService CreateSut()
        {
            _dataStoreFactory
                .Setup(f => f.Create())
                .Returns(_accountDataStore.Object);

            return new PaymentService(
                _dataStoreFactory.Object,
                _validator.Object);
        }

        [Fact]
        public void MakePayment_BacsPayment_WithValidAccount_ReturnsSuccess()
        {
            var account = new Account { Balance = 200 };

            var request = new MakePaymentRequest
            {
                DebtorAccountNumber = "12345",
                Amount = 100,
                PaymentScheme = PaymentScheme.Bacs
            };

            _accountDataStore.Setup(d => d.GetAccount("12345")).Returns(account);
            _validator.Setup(v => v.Validate(account, request)).Returns(true);

            var sut = CreateSut();

            var result = sut.MakePayment(request);

            Assert.True(result.Success);
            Assert.Equal(100, account.Balance);

            _accountDataStore.Verify(d => d.UpdateAccount(account), Times.Once);
        }

        [Fact]
        public void MakePayment_BacsPayment_WithNullAccount_ReturnsFalse()
        {
            var request = new MakePaymentRequest
            {
                DebtorAccountNumber = "99999",
                Amount = 100,
                PaymentScheme = PaymentScheme.Bacs
            };

            _accountDataStore.Setup(d => d.GetAccount("99999")).Returns((Account?)null);
            _validator.Setup(v => v.Validate(null, request)).Returns(false);

            var sut = CreateSut();

            var result = sut.MakePayment(request);

            Assert.False(result.Success);
            _accountDataStore.Verify(d => d.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }

        [Fact]
        public void MakePayment_FasterPayments_WithInsufficientBalance_ReturnsFalse()
        {
            var account = new Account { Balance = 50 };

            var request = new MakePaymentRequest
            {
                DebtorAccountNumber = "12345",
                Amount = 100,
                PaymentScheme = PaymentScheme.FasterPayments
            };

            _accountDataStore.Setup(d => d.GetAccount("12345")).Returns(account);
            _validator.Setup(v => v.Validate(account, request)).Returns(false);

            var sut = CreateSut();

            var result = sut.MakePayment(request);

            Assert.False(result.Success);
            Assert.Equal(50, account.Balance); // unchanged
        }

        [Fact]
        public void MakePayment_ChapsPayment_WithNonLiveAccount_ReturnsFalse()
        {
            var account = new Account { Status = AccountStatus.Disabled };

            var request = new MakePaymentRequest
            {
                DebtorAccountNumber = "12345",
                Amount = 100,
                PaymentScheme = PaymentScheme.Chaps
            };

            _accountDataStore.Setup(d => d.GetAccount("12345"))
                .Returns(account);
            _validator.Setup(v => v.Validate(account, request)).Returns(false);

            var sut = CreateSut();

            var result = sut.MakePayment(request);

            Assert.False(result.Success);
        }
    }
}
