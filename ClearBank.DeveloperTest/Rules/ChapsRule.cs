using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Rules;

public sealed class ChapsRule : IPaymentSchemeRule
{
    public PaymentScheme Scheme => PaymentScheme.Chaps;

    public bool IsValid(Account account, MakePaymentRequest request)
        => (account.AllowedPaymentSchemes & AllowedPaymentSchemes.Chaps) != 0
           && account.Status == AccountStatus.Live;
}