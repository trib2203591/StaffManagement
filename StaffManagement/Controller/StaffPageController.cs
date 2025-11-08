namespace StaffManagement.Controller;

using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using StaffManagement.Models;
using StaffManagement.Utils;

public class StaffPageController : ComponentBase
{
    [Inject]
    protected StaffDbContext dbContext { get; set; }

    [Inject]
    protected NavigationManager navManager { get; set; }
    
    protected List<Staff> allStaff;
    protected Staff selectedStaff;
    protected bool isModalVisible = false;
    
    protected override async Task OnInitializedAsync()
    {
        _ = LoadStaffDataAsync();
    }
        
    protected async Task LoadStaffDataAsync()
    {
        allStaff = await dbContext.Staff.ToListAsync();
        StateHasChanged(); 
    }

    protected void OpenDetailsModal(Staff staff)
    {
        selectedStaff = staff;
        isModalVisible = true;
    }

    protected void CloseDetailsModal()
    {
        isModalVisible = false;
        selectedStaff = null;
    }
}