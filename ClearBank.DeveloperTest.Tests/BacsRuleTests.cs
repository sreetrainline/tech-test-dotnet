using ClearBank.DeveloperTest.Rules;
using ClearBank.DeveloperTest.Types;
using Xunit;

namespace ClearBank.DeveloperTest.Tests
{
    public class BacsRuleTests
    {
        [Fact]
        public void IsValid_WhenBacsIsAllowed_ReturnsTrue()
        {
            var rule = new BacsRule();

            var account = new Account
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs
            };

            var request = new MakePaymentRequest
            {
                PaymentScheme = PaymentScheme.Bacs
            };

            var ok = rule.IsValid(account, request);

            Assert.True(ok);
        }

        [Fact]
        public void IsValid_WhenBacsIsNotAllowed_ReturnsFalse()
        {
            var rule = new BacsRule();

            var account = new Account
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments
            };

            var request = new MakePaymentRequest
            {
                PaymentScheme = PaymentScheme.Bacs
            };

            var ok = rule.IsValid(account, request);

            Assert.False(ok);
        }
    }
}