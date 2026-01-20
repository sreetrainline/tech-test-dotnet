using System;
using ClearBank.DeveloperTest.Rules;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using Moq;
using Xunit;

namespace ClearBank.DeveloperTest.Tests
{
    public class PaymentValidatorTests
    {
        private readonly Mock<IPaymentSchemeRule> _rule;
        private readonly Account _account;
        private MakePaymentRequest _request;

        public PaymentValidatorTests()
        {
            _rule = new Mock<IPaymentSchemeRule>();
            _account = new Account();
            _request = new MakePaymentRequest { PaymentScheme = PaymentScheme.Bacs };
        }

        [Fact]
        public void Validate_WhenAccount_IsNull_ReturnsFalse()
        {
            _rule.SetupGet(r => r.Scheme)
                  .Returns(PaymentScheme.Bacs);

            var sut = new PaymentValidator([_rule.Object]);

            Assert.False(sut.Validate(null, _request));
        }

        [Fact]
        public void Validate_WhenRule_Exists_Returns_RuleResult()
        {
            _rule.SetupGet(r => r.Scheme).Returns(PaymentScheme.Bacs);
            _rule.Setup(r => r.IsValid(_account, _request)).Returns(true);

            var sut = new PaymentValidator([_rule.Object]);

            Assert.True(sut.Validate(_account, _request));
        }

        [Fact]
        public void Validate_When_NoRule_ForScheme_Throws_Exception()
        {
            _request.PaymentScheme = PaymentScheme.Chaps;
            _rule.SetupGet(r => r.Scheme).Returns(PaymentScheme.Bacs);

            var sut = new PaymentValidator([_rule.Object]);

            Assert.Throws<NotSupportedException>(() => sut.Validate(_account, _request));
        }
    }
}