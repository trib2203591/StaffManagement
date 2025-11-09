This is the checker for StaffManagement

Technology:
- use xUnit to test the controller business logic
    - use EF core in memory database to test data persistence
- use bUnit to test the blazor interface
- use Moq to mock dependencies

CreateStaffControllerTests.cs
- test the Email and Phone validation logic

CreateStaffPageTests.cs
- Test the interface of Create Staff function
    - Test if all the element display correctly
    - All the invalid input data warning display.

StaffListPageTests.cs
- Test the interface of View staff list and detail function
    - Test if all staff card display correctly
    - Test if staff info panel display correctly
    - Test if the UI display correctly

To run test do `dotnet test`