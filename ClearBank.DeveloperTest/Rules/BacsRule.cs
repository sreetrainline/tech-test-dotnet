using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Rules;

public sealed class BacsRule : IPaymentSchemeRule
{
    public PaymentScheme Scheme => PaymentScheme.Bacs;

    public bool IsValid(Account account, MakePaymentRequest request)
        => (account.AllowedPaymentSchemes & AllowedPaymentSchemes.Bacs) != 0;
}