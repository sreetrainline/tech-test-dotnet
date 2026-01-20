using ClearBank.DeveloperTest.Rules;
using ClearBank.DeveloperTest.Types;
using Xunit;

namespace ClearBank.DeveloperTest.Tests
{
    public class ChapsRuleTests
    {
        private readonly ChapsRule _rule;
        private readonly MakePaymentRequest _request;

        public ChapsRuleTests()
        {
            _rule = new ChapsRule();
            _request = new MakePaymentRequest
            {
                PaymentScheme = PaymentScheme.Chaps
            };
        }

        [Theory]
        [InlineData(AllowedPaymentSchemes.Chaps)]
        [InlineData(AllowedPaymentSchemes.Chaps | AllowedPaymentSchemes.FasterPayments)]
        [InlineData(AllowedPaymentSchemes.Chaps | AllowedPaymentSchemes.Bacs)]
        public void IsValid_WhenChaps_IsAllowed_AndAccount_IsLive(AllowedPaymentSchemes allowedPaymentScheme)
        {
            var account = new Account
            {
                AllowedPaymentSchemes = allowedPaymentScheme,
                Status = AccountStatus.Live
            };

            Assert.True(_rule.IsValid(account, _request));
        }

        [Theory]
        [InlineData(AllowedPaymentSchemes.Chaps)]
        [InlineData(AllowedPaymentSchemes.Chaps | AllowedPaymentSchemes.FasterPayments)]
        [InlineData(AllowedPaymentSchemes.Chaps | AllowedPaymentSchemes.Bacs)]
        public void IsInValid_WhenChaps_IsAllowed_AndAccount_IsNotLive(AllowedPaymentSchemes allowedPaymentScheme)
        {
            var account = new Account
            {
                AllowedPaymentSchemes = allowedPaymentScheme,
                Status = AccountStatus.Disabled
            };

            Assert.False(_rule.IsValid(account, _request));
        }

        [Theory]
        [InlineData(AllowedPaymentSchemes.Bacs)]
        [InlineData(AllowedPaymentSchemes.FasterPayments)]
        [InlineData(AllowedPaymentSchemes.Bacs | AllowedPaymentSchemes.FasterPayments)]
        public void IsInValid_WhenAccount_IsLive_AndChaps_NotAllowed(AllowedPaymentSchemes allowedPaymentScheme)
        {
            var account = new Account
            {
                AllowedPaymentSchemes = allowedPaymentScheme,
                Status = AccountStatus.Live
            };

            Assert.False(_rule.IsValid(account, _request));
        }
    }

}