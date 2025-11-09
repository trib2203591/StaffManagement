using Microsoft.AspNetCore.Components;
using StaffManagement.Models;
using StaffManagement.Utils;

namespace StaffManagement.Controller;

public class CreateStaffPageController : ComponentBase
{
    [Inject]
    public StaffDbContext dbContext { get; set; }

    [Inject]
    public NavigationManager navManager { get; set; }
    
    public Staff staff = new Staff();
    
    public string nameError = "";
    public string emailError = "";
    public string phoneError = "";
    public string dateError = "";
    public string photoError = "";
    
    protected override void OnInitialized()
    {
        staff.StartingDate = DateTime.Today;
    }
    
    public async Task HandleSubmit()
    {
        nameError = emailError = phoneError = dateError = photoError = "";
        bool isValid = true;
        
        if (string.IsNullOrWhiteSpace(staff.Email))
        {
            emailError = "The email is empty.";
            isValid = false;
        }
        else if (!ValidateField.CheckEmail(staff))
        {
            emailError = "The email format is invalid.";
            isValid = false;
        }
        
        if (string.IsNullOrWhiteSpace(staff.PhoneNumber))
        {
            phoneError = "The phone number is empty."; 
            isValid = false;
        }
        else if (!ValidateField.CheckPhone(staff))
        {
            phoneError = "The phone number format is invalid.";
            isValid = false;
        }

        if (string.IsNullOrWhiteSpace(staff.StaffName))
        {
            nameError = "Staff Name is required.";
            isValid = false;
        }
        
        if (string.IsNullOrWhiteSpace(staff.Photo))
        {
            staff.Photo = "Empty";
        }
        
        
        if (isValid)
        {
            try
            {
                dbContext.Staff.Add(staff);
                await dbContext.SaveChangesAsync();
                
                navManager.NavigateTo("/staff");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving staff: {ex.Message}");
                nameError = $"Save failed: {ex.Message}";
            }
        }
    }
}