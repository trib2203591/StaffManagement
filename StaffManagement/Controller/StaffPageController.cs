namespace StaffManagement.Controller;

using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using StaffManagement.Models;
using StaffManagement.Utils;

public class StaffPageController : ComponentBase
{
    [Inject]
    public StaffDbContext dbContext { get; set; }

    [Inject]
    public NavigationManager navManager { get; set; }
    
    public List<Staff> allStaff;
    public Staff selectedStaff;
    public bool isModalVisible = false;
    
    protected override async Task OnInitializedAsync()
    {
        _ = LoadStaffDataAsync();
    }
        
    public async Task LoadStaffDataAsync()
    {
        allStaff = await dbContext.Staff.ToListAsync();
        StateHasChanged(); 
    }

    public void OpenDetailsModal(Staff staff)
    {
        selectedStaff = staff;
        isModalVisible = true;
    }

    public void CloseDetailsModal()
    {
        isModalVisible = false;
        selectedStaff = null;
    }
}