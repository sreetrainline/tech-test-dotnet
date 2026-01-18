using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService(IDataStoreFactory dataStoreFactory,IPaymentValidator paymentValidator) : IPaymentService
    {
        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var makePaymentResult = new MakePaymentResult() { Success = false };

            if (request is not { Amount: > 0 })
                return makePaymentResult;

            var accountDataStore = dataStoreFactory.Create();
            var account = accountDataStore.GetAccount(request.DebtorAccountNumber);

            if (!paymentValidator.Validate(account, request))
                return makePaymentResult;

            account.Balance -= request.Amount;
            accountDataStore.UpdateAccount(account);
            makePaymentResult.Success = true;

            return makePaymentResult;
        }
    }
}
