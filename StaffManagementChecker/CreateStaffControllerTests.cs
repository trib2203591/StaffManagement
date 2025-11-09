using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using StaffManagement.Controller;
using StaffManagement.Utils;

namespace StaffManagementChecker;

using StaffManagement.Models;

public class CreateStaffControllerTests
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
    
    [Theory]
    [InlineData("abc@gmail.com", true, null)]
    [InlineData("@gmail.com", false, "The email format is invalid.")]
    [InlineData("@@gmail.com", false, "The email format is invalid.")]
    [InlineData("abc@", false, "The email format is invalid.")]
    [InlineData("noatsign.com", false, "The email format is invalid.")]
    [InlineData("", false, "The email is empty.")]
    [InlineData(null, false, "The email is empty.")]
    public async Task checkEmailValidation(String email, bool expected, String errorMessage)
    {
        var db = GetDbContext();
        var nav = new MockNavManager();
        var controller = new CreateStaffPageController
        {
            dbContext = db,
            navManager = nav
        };

        controller.staff = new Staff
        {
            StaffID = 1,
            StaffName = "John",
            Email = email,
            PhoneNumber = "0123456789",
            StartingDate = DateTime.Today,
            Photo = ""
        };
        
        await controller.HandleSubmit();
        
        if (expected)
        {
            var savedStaff = await db.Staff.FirstOrDefaultAsync();
            Assert.NotNull(savedStaff);
            Assert.Equal("/staff", nav.NavigatedTo);
        }
        else
        {
            Assert.Equal(errorMessage, controller.emailError);
            Assert.Equal(0, await db.Staff.CountAsync());
        }
    }
    
    [Theory]
    [InlineData("0123456789", true, null)]         
    [InlineData("0987 654 321", true, null)]
    [InlineData("0987-654-321", true, null)]          
    [InlineData("+84123456789", true, null)]        
    [InlineData("84901234567", true, null)]          
    [InlineData("123456789", true, null)]  
    [InlineData("+84 abc456789", false, "The phone number format is invalid.")]       
    [InlineData("+08490123#5234567", false, "The phone number format is invalid.")]       
    [InlineData("+9999999999999999", false, "The phone number format is invalid.")]  
    [InlineData("", false, "The phone number is empty.")]
    [InlineData(null, false, "The phone number is empty.")]
    public async Task checkPhoneNumberValidation(String phoneNumber, bool expected, String errorMessage)
    {
        var db = GetDbContext();
        var nav = new MockNavManager();
        var controller = new CreateStaffPageController
        {
            dbContext = db,
            navManager = nav
        };

        controller.staff = new Staff
        {
            StaffID = 1,
            StaffName = "John",
            Email = "example@gmail.com",
            PhoneNumber = phoneNumber,
            StartingDate = DateTime.Today,
            Photo = ""
        };
        
        await controller.HandleSubmit();
        
        if (expected)
        {
            var savedStaff = await db.Staff.FirstOrDefaultAsync();
            Assert.NotNull(savedStaff);
            Assert.Equal("/staff", nav.NavigatedTo);
        }
        else
        {
            Assert.Equal(errorMessage, controller.phoneError);
            Assert.Equal(0, await db.Staff.CountAsync());
        }
    }
}