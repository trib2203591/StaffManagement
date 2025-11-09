using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StaffManagement.Models;
using StaffManagement.Utils;
using StaffManagement.Views.Pages;

namespace StaffManagementChecker;

public class StaffListPageTests : TestContext
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
    public void RendersEmptyMessage_WhenNoStaffInDb()
    {
        // Arrange
        var db = GetDbContext();
        Services.AddSingleton(db);
        Services.AddSingleton<NavigationManager>(new MockNavManager());

        // Act
        var component = RenderComponent<StaffList>(); 

        // Assert
        component.Markup.Contains("<em>Empty</em>");
        Assert.Empty(component.FindAll(".staff-card"));
    }
    
    [Fact]
    public void RendersStaffCards_WhenStaffExistInDb()
    {
        // Arrange
        var db = GetDbContext();
        db.Staff.Add(new Staff { StaffName = "John Doe", PhoneNumber = "0931028825", Email = "john@example.com", Photo = "john.jpg" });
        db.Staff.Add(new Staff { StaffName = "Jane Doe", PhoneNumber = "0931028825", Email = "jane@example.com", Photo = "jane.jpg" });
        db.SaveChanges();
        
        Services.AddSingleton(db);
        Services.AddSingleton<NavigationManager>(new MockNavManager());

        // Act
        var component = RenderComponent<StaffList>();

        // Assert
        Assert.DoesNotContain("<em>Empty</em>", component.Markup);
        
        var cards = component.FindAll(".staff-card");
        Assert.Equal(2, cards.Count);
        
        Assert.Contains("John Doe", cards[0].TextContent);
        Assert.Contains("john.jpg", cards[0].InnerHtml);
    }
    
    [Fact]
    public void OpensModalWithCorrectData_WhenViewDetailsIsClicked()
    {
        // Arrange
        var db = GetDbContext();
        var staff1 = new Staff 
        { 
            StaffID = 1, 
            StaffName = "John Doe", 
            Email = "john@example.com", 
            PhoneNumber = "0931028825",
            StartingDate = new DateTime(2025, 1, 1),
            Photo = "john.jpg" 
        };
        db.Staff.Add(staff1);
        db.SaveChanges();
        
        Services.AddSingleton(db);
        Services.AddSingleton<NavigationManager>(new MockNavManager());
    
        // Act
        var component = RenderComponent<StaffList>();
        
        Assert.Equal(0, component.FindAll(".modal-content").Count);
        
        component.Find(".staff-card button").Click();
        
        var modal = component.Find(".modal-content");
        Assert.NotNull(modal);

        // Assert
        Assert.Contains("John Doe", modal.TextContent);
        Assert.Contains("john@example.com", modal.TextContent);
        Assert.Contains("0931028825", modal.TextContent);
        Assert.Contains(staff1.StartingDate.ToShortDateString(), modal.TextContent);
    }
    
    [Fact]
    public void ClosesModal_WhenCloseButtonIsClicked()
    {
        // Arrange
        var db = GetDbContext();
        db.Staff.Add(new Staff { StaffName = "John Doe", PhoneNumber = "0931028825", Email = "john@example.com", Photo = "john.jpg" });
        db.SaveChanges();
        
        Services.AddSingleton(db);
        Services.AddSingleton<NavigationManager>(new MockNavManager());

        var component = RenderComponent<StaffList>();

        // Act 
        component.Find(".staff-card button").Click();

        // Assert 
        Assert.NotNull(component.Find(".modal-content"));

        // Act
        component.Find(".modal-close-btn").Click();

        // Assert 
        Assert.Equal(0, component.FindAll(".modal-content").Count);
    }
}