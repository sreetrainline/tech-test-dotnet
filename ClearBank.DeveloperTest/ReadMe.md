Refactoring Notes

1. Inject the configuration ( IOptions)
2. Stop newing up dependencies - create interfaces and inject them
3. Avoid repeating the same checks ( account = null) and use defensive coding
4. Create Rules for each Payment Type
5. Create Factory for data store
6. Tests for all classes



Things I would in a Real Life scenario - With More time

1. The method and the calls can be async
2. Exception handling
3. Add a Reason to Payment Failures - make it cleaner
4. Request Validation
5. access levels of fields - many can be init
6. Integration Tests
7. Logging