using ClearBank.DeveloperTest.Rules;
using ClearBank.DeveloperTest.Types;
using Xunit;

namespace ClearBank.DeveloperTest.Tests
{
    public class FasterPaymentsRuleTests
{
    private readonly FasterPaymentsRule _rule;
    private readonly MakePaymentRequest _request;

    public FasterPaymentsRuleTests()
    {
        _rule = new FasterPaymentsRule();
        _request = new MakePaymentRequest
        {
            PaymentScheme = PaymentScheme.FasterPayments,
            Amount = 50
        };
    }

    [Theory]
    [InlineData(AllowedPaymentSchemes.FasterPayments)]
    [InlineData(AllowedPaymentSchemes.FasterPayments | AllowedPaymentSchemes.Bacs)]
    [InlineData(AllowedPaymentSchemes.FasterPayments | AllowedPaymentSchemes.Chaps)]
    public void IsValid_WhenFasterPayments_IsAllowed_AndSufficientBalance(
        AllowedPaymentSchemes allowedPaymentScheme)
    {
        var account = new Account
        {
            AllowedPaymentSchemes = allowedPaymentScheme,
            Balance = 100
        };

        Assert.True(_rule.IsValid(account, _request));
    }

    [Theory]
    [InlineData(AllowedPaymentSchemes.FasterPayments)]
    [InlineData(AllowedPaymentSchemes.FasterPayments | AllowedPaymentSchemes.Bacs)]
    [InlineData(AllowedPaymentSchemes.FasterPayments | AllowedPaymentSchemes.Chaps)]
    public void IsInValid_WhenFasterPayments_IsAllowed_AndInsufficientBalance(
        AllowedPaymentSchemes allowedPaymentScheme)
    {
        var account = new Account
        {
            AllowedPaymentSchemes = allowedPaymentScheme,
            Balance = 10
        };

        Assert.False(_rule.IsValid(account, _request));
    }

    [Theory]
    [InlineData(AllowedPaymentSchemes.Bacs)]
    [InlineData(AllowedPaymentSchemes.Chaps)]
    [InlineData(AllowedPaymentSchemes.Bacs | AllowedPaymentSchemes.Chaps)]
    public void IsInValid_WhenFasterPayments_IsNotAllowed_AndSufficientBalance(
        AllowedPaymentSchemes allowedPaymentScheme)
    {
        var account = new Account
        {
            AllowedPaymentSchemes = allowedPaymentScheme,
            Balance = 100
        };

        Assert.False(_rule.IsValid(account, _request));
    }
}

}