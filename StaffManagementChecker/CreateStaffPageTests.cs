using AngleSharp.Dom;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using StaffManagement.Utils;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using StaffManagement.Views.Pages;

namespace StaffManagementChecker;

public class CreateStaffPageTests : TestContext
{
    private StaffDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<StaffDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) 
            .Options;

        return new StaffDbContext(options);
    }
    
    private class MockNavManager : NavigationManager
    {
        public string? NavigatedTo { get; private set; }

        public MockNavManager() => Initialize("http://localhost/", "http://localhost/");

        protected override void NavigateToCore(string uri, bool forceLoad)
        {
            NavigatedTo = uri;
        }
    }
    
    [Fact]
    public void RendersForm_WithAllInputFields()
    {
        // Arrange
        var db = GetDbContext();
        Services.AddSingleton(db);
        Services.AddSingleton<NavigationManager>(new MockNavManager());

        // Act
        var component = RenderComponent<CreateStaff>();

        // Assert
        component.Markup.Contains("Staff Name:");
        component.Markup.Contains("Email:");
        component.Markup.Contains("Phone Number:");
        component.Markup.Contains("Starting Date:");
        component.Markup.Contains("Photo URL:");
    }

    [Fact]
    public void ShowsError_WhenEmailIsInvalid()
    {
        // Arrange
        var db = GetDbContext();
        Services.AddSingleton(db);
        var nav = new MockNavManager();
        Services.AddSingleton<NavigationManager>(nav);

        var component = RenderComponent<CreateStaff>();

        // Fill form inputs
        component.Find("#staff-name").Change("John Doe");
        component.Find("#staff-email").Change("invalidemail");
        component.Find("#staff-phone").Change("0123456789");
        //component.Find("#staff-date").Change("0123456789");
        //component.Find("#staff-photo").Change("example.com");

        // Act
        component.Find("form").Submit();

        // Assert: UI should show email error
        component.Markup.Contains("The email format is invalid.");
        Assert.Null(nav.NavigatedTo);
    }
    
    [Fact]
    public void SavesToDbAndNavigates_WhenFormIsValid()
    {
        // Arrange
        var db = GetDbContext();
        Services.AddSingleton(db);
        var nav = new MockNavManager();
        Services.AddSingleton<NavigationManager>(nav);

        var component = RenderComponent<CreateStaff>();
        
        var validDate = new DateTime(2025, 1, 1);
        
        component.Find("#staff-name").Change("Jane Doe");
        component.Find("#staff-email").Change("jane.doe@example.com");
        component.Find("#staff-phone").Change("0987654321");
        component.Find("#staff-date").Change(validDate);
        component.Find("#staff-photo").Change("http://example.com/photo.jpg");

        // Act
        component.Find("form").Submit();

        // Assert: Check for navigation
        Assert.NotNull(nav.NavigatedTo); 
        Assert.Contains("/staff", nav.NavigatedTo); 

        // Assert: Check database
        Assert.Equal(1, db.Staff.Count());
        Assert.Equal("Jane Doe", db.Staff.First().StaffName);
        Assert.Equal("jane.doe@example.com", db.Staff.First().Email);
    }
    
    [Fact]
    public void ShowsError_WhenNameIsEmpty()
    {
        // Arrange
        var db = GetDbContext();
        Services.AddSingleton(db);
        var nav = new MockNavManager();
        Services.AddSingleton<NavigationManager>(nav);

        var component = RenderComponent<CreateStaff>();
        
        component.Find("#staff-name").Change(""); 
        component.Find("#staff-email").Change("test@example.com");
        component.Find("#staff-phone").Change("0123456789");

        // Act
        component.Find("form").Submit();

        // Assert: UI should show name error
        var validationMessage = component.Find(".validation-message");
        Assert.NotNull(validationMessage);
        Assert.False(string.IsNullOrEmpty(validationMessage.TextContent));
        
        // Assert: No navigation or DB change
        Assert.Null(nav.NavigatedTo);
        Assert.Equal(0, db.Staff.Count());
    }
    
    [Fact]
    public void ShowsMultipleErrors_WhenFormIsEmpty()
    {
        // Arrange
        var db = GetDbContext();
        Services.AddSingleton(db);
        var nav = new MockNavManager();
        Services.AddSingleton<NavigationManager>(nav);

        var component = RenderComponent<CreateStaff>();

        // Act
        component.Find("form").Submit();

        // Assert: UI should show multiple errors
        var validationMessages = component.FindAll(".validation-message");
        
        Assert.True(validationMessages.Count >= 3); 

        // Assert: No navigation or DB change
        Assert.Null(nav.NavigatedTo);
        Assert.Equal(0, db.Staff.Count());
    }
}