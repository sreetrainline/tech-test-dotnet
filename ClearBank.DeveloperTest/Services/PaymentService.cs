using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Types;
using System.Configuration;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService(IDataStoreFactory dataStoreFactory,IPaymentValidator paymentValidator) : IPaymentService
    {
        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            if (request is not { Amount: > 0 })
                return new MakePaymentResult() { Success = false };

            var accountDataStore = dataStoreFactory.Create();
            var account = accountDataStore.GetAccount(request.DebtorAccountNumber);

            if (!paymentValidator.Validate(account, request))
                return new MakePaymentResult { Success = false };

            account.Balance -= request.Amount;
            accountDataStore.UpdateAccount(account);

            return new MakePaymentResult { Success = true };
        }
    }
}
