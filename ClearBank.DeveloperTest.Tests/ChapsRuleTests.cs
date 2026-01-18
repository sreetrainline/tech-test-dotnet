using ClearBank.DeveloperTest.Rules;
using ClearBank.DeveloperTest.Types;
using Xunit;

namespace ClearBank.DeveloperTest.Tests
{
    public class ChapsRuleTests
    {
        [Fact]
        public void IsValid_WhenAllowedAndAccountLive_ReturnsTrue()
        {
            var rule = new ChapsRule();

            var account = new Account
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps,
                Status = AccountStatus.Live
            };

            var request = new MakePaymentRequest
            {
                PaymentScheme = PaymentScheme.Chaps
            };

            var ok = rule.IsValid(account, request);

            Assert.True(ok);
        }

        [Fact]
        public void IsValid_WhenAccountNotLive_ReturnsFalse()
        {
            var rule = new ChapsRule();

            var account = new Account
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps,
                Status = AccountStatus.Disabled
            };

            var request = new MakePaymentRequest
            {
                PaymentScheme = PaymentScheme.Chaps
            };

            var ok = rule.IsValid(account, request);

            Assert.False(ok);
        }
    }
}