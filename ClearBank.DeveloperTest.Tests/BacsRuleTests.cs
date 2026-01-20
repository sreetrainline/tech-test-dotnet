using ClearBank.DeveloperTest.Rules;
using ClearBank.DeveloperTest.Types;
using Xunit;

namespace ClearBank.DeveloperTest.Tests
{
    public class BacsRuleTests
    {
        private BacsRule _rule;
        private MakePaymentRequest _request;

        public BacsRuleTests()
        {
            _rule = new BacsRule();
            _request = new MakePaymentRequest
            {
                PaymentScheme = PaymentScheme.Bacs
            };
        }

        [Theory]
        [InlineData(AllowedPaymentSchemes.Bacs)]
        [InlineData(AllowedPaymentSchemes.Bacs | AllowedPaymentSchemes.FasterPayments)]
        [InlineData(AllowedPaymentSchemes.Bacs | AllowedPaymentSchemes.Chaps)]
        public void IsValid_WhenBacsIsAllowed(AllowedPaymentSchemes allowedPaymentScheme)
        {
            var account = new Account
            {
                AllowedPaymentSchemes = allowedPaymentScheme
            };

            Assert.True(_rule.IsValid(account, _request));
        }

        [Theory]
        [InlineData(AllowedPaymentSchemes.FasterPayments)]
        [InlineData(AllowedPaymentSchemes.Chaps)]
        [InlineData(AllowedPaymentSchemes.Chaps | AllowedPaymentSchemes.FasterPayments)]
        public void IsInValid_WhenBacsIsNotAllowed(AllowedPaymentSchemes allowedPaymentScheme)
        {
            var account = new Account
            {
                AllowedPaymentSchemes = allowedPaymentScheme
            };

            Assert.False(_rule.IsValid(account, _request));
        }
    }
}