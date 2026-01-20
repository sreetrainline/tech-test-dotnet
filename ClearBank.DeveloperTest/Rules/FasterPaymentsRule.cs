using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Rules;

public sealed class FasterPaymentsRule : IPaymentSchemeRule
{
    public PaymentScheme Scheme => PaymentScheme.FasterPayments;

    public bool IsValid(Account account, MakePaymentRequest request)
        => (account.AllowedPaymentSchemes & AllowedPaymentSchemes.FasterPayments) != 0
           && account.Balance >= request.Amount;
}