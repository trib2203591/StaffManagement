namespace StaffManagement.Models;

using System;
using StaffManagement.Utils;

public class Staff
{
    public int StaffID { get; set; }
    public string StaffName { get; set; }
    [CheckAttribute("^\\S+@\\S+\\.\\S+$")]
    public string Email { get; set; }
    [CheckAttribute("^\\+?\\d{1,3}?[- .]?\\d{2,4}[- .]?\\d{3,4}[- .]?\\d{3,4}$")]
    public string PhoneNumber { get; set; }
    public DateTime StartingDate { get; set; }
    public string Photo { get; set; }
    
    public Staff() { }

    public Staff (int staffID, string staffName, string email, string phoneNumber, DateTime startingDate, string photo)
    {
        StaffID = staffID;
        StaffName = staffName;
        Email = email;
        PhoneNumber = phoneNumber;
        StartingDate = startingDate;
        Photo = photo;
    }
}