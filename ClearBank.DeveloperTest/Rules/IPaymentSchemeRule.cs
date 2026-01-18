using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Rules;

public interface IPaymentSchemeRule
{
    PaymentScheme Scheme { get; }
    bool IsValid(Account account, MakePaymentRequest request);

}