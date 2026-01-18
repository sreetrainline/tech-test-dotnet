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
        [Fact]
        public void Validate_WhenAccountIsNull_ReturnsFalse()
        {
            var rule = new Mock<IPaymentSchemeRule>();
            rule.SetupGet(r => r.Scheme).Returns(PaymentScheme.Bacs);

            var sut = new PaymentValidator(new[] { rule.Object });

            var request = new MakePaymentRequest { PaymentScheme = PaymentScheme.Bacs };

            var ok = sut.Validate(null, request);

            Assert.False(ok);
        }

        [Fact]
        public void Validate_WhenRuleExists_ReturnsRuleResult()
        {
            var account = new Account();
            var request = new MakePaymentRequest { PaymentScheme = PaymentScheme.Bacs };

            var rule = new Mock<IPaymentSchemeRule>();
            rule.SetupGet(r => r.Scheme).Returns(PaymentScheme.Bacs);
            rule.Setup(r => r.IsValid(account, request)).Returns(true);

            var sut = new PaymentValidator(new[] { rule.Object });

            var ok = sut.Validate(account, request);

            Assert.True(ok);
        }

        [Fact]
        public void Validate_WhenNoRuleForScheme_Throws()
        {
            var account = new Account();
            var request = new MakePaymentRequest { PaymentScheme = PaymentScheme.Chaps };

            var rule = new Mock<IPaymentSchemeRule>();
            rule.SetupGet(r => r.Scheme).Returns(PaymentScheme.Bacs); // different scheme

            var sut = new PaymentValidator(new[] { rule.Object });

            Assert.Throws<NotSupportedException>(() => sut.Validate(account, request));
        }
    }
}