using System;
using System.Collections.Generic;
using System.Linq;
using ClearBank.DeveloperTest.Rules;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services;

public class PaymentValidator(IEnumerable<IPaymentSchemeRule> rules) : IPaymentValidator
{
    private readonly IReadOnlyDictionary<PaymentScheme, IPaymentSchemeRule> _rules =
        rules.ToDictionary(r => r.Scheme);

    public bool Validate(Account? account, MakePaymentRequest request)
    {
        if (account is null) return false;

        /*Deviation from Original logic*/
        if (!_rules.TryGetValue(request.PaymentScheme, out var rule))
            throw new NotSupportedException($"Unsupported scheme {request.PaymentScheme}");

        return rule.IsValid(account, request);
    }
}