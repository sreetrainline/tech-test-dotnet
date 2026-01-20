# Payment Service – Refactoring Notes

First of all, thank you for taking the time to review my work. It was a really interesting
test and I thoroughly enjoyed it. What I have tried to do is to keep the logic as is and to Refactor the code with SOLID principles in mind.

## Points Noted with Original Payment Service

The original `PaymentService` worked functionally, but it had several structural and maintainability issues that would become problematic as the system grows:

1. **Hard-wired configuration**
    - `ConfigurationManager.AppSettings` was accessed directly inside the service.
    - This tightly coupled the service to configuration and made testing difficult.

2. **Dependencies created with `new`**
    - `AccountDataStore` and `BackupAccountDataStore` were instantiated directly.
    - This violated dependency inversion and made mocking or replacing implementations impossible.

3. **Repeated defensive checks**
    - `account == null` checks were repeated in every payment-scheme branch.
    - This led to duplicated logic and reduced readability.

4. **Large switch statement**
    - All payment scheme logic lived inside one method.
    - Adding a new payment type would require modifying existing logic, increasing the risk of regressions.

5. **Data store selection logic duplicated**
    - The logic to decide which datastore to use was repeated for both `GetAccount` and `UpdateAccount`.

6. **Difficult to test**
    - Because dependencies were created internally and logic was tightly coupled, meaningful unit tests were hard to write.

7. **No Error Handling or Logging**
    - There is no Error handling and returning meaningful information. No provision for logging

8. **Fields Missing or Additional Fields not used**
    - The Make PaymentResult could do with a reason when status is false, to indicate why something was unsucessful, Similarly, a lot of additional fields in classes whiuch are not used

8. **No Input validations on request values**
    - What if Amount is 0, or account number is empty etc etc..

---

## Changes Made During Refactoring

The refactor focused on **separation of concerns**, **testability**, and **clarity**:

1. **Injected configuration**
    - Configuration is now injected using `IOptions<T>`, making the service environment-agnostic and test-friendly.

2. **Dependency injection throughout**
    - All external dependencies are now injected via interfaces.
    - No more `new` inside business logic.

3. **Centralised validation**
    - Defensive checks like `account == null` are handled once, early, and consistently.
    - This avoids repeated checks and makes intent clearer.

4. **Rule-based payment validation**
    - Each payment scheme has its own rule class.
    - This removes the large switch statement and makes adding new schemes easier .

5. **Factory for data store selection**
    - A dedicated factory decides which datastore to use.
    - The service no longer knows or cares *how* the store is chosen.
    - A Confession : I think I made this implementation a bit complex using Keyed dependency.

6. **Tests for all components**
    - Unit tests for rules, validator, factory, and service.

## What I Would Do With More Time (Real-World Improvements)

If this were a production system with more time available, the next steps would be:

1. **Async support**
    - Make datastore access and payment processing asynchronous.

2. **Explicit exception handling**
    - Replace implicit failures with meaningful, domain-specific exceptions where appropriate.

3. **Clear failure reasons**
    - Return structured failure reasons instead of a simple success flag.

4. **Request validation**
    - Validate incoming requests early (amounts, account numbers, supported schemes, etc.).

5. **Tighter immutability**
    - Improve access levels and use `init` where possible to reduce accidental mutation.

6. **More integration coverage**
    - Add integration tests covering configuration, DI wiring, and failure scenarios.

7. **Logging**
    - Introduce structured logging for observability and easier debugging in production.

And more things for sure, which has escaped me.
