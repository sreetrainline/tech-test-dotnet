using ClearBank.DeveloperTest.Rules;
using ClearBank.DeveloperTest.Types;
using Xunit;

namespace ClearBank.DeveloperTest.Tests
{
    public class FasterPaymentsRuleTests
    {
        [Fact]
        public void IsValid_WhenAllowedAndSufficientBalance_ReturnsTrue()
        {
            var rule = new FasterPaymentsRule();

            var account = new Account
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
                Balance = 100
            };

            var request = new MakePaymentRequest
            {
                Amount = 50,
                PaymentScheme = PaymentScheme.FasterPayments
            };

            var ok = rule.IsValid(account, request);

            Assert.True(ok);
        }

        [Fact]
        public void IsValid_WhenInsufficientBalance_ReturnsFalse()
        {
            var rule = new FasterPaymentsRule();

            var account = new Account
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
                Balance = 10
            };

            var request = new MakePaymentRequest
            {
                Amount = 50,
                PaymentScheme = PaymentScheme.FasterPayments
            };

            var ok = rule.IsValid(account, request);

            Assert.False(ok);
        }
    }
}