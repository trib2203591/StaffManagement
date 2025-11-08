# Project overview
Created using blazor and organized as MVC pattern

using EFcore and Sqlite to persist data

folder structure
```
.
├── Controller/        // controller classes to handle business logics
│   ├── CreateStaffPageController.cs  
│   └── StaffPageController.cs        
├── Migrations/        // Database migration
├── Models/            
│   └── Staff.cs       // Staff model with regex Attributes for data validation
├── Utils
│   ├── CheckAttribute.cs    // util class to store a field regex Attribute
│   ├── StaffDBContext.cs    // EF core config for Staff model
│   └── ValidateField.cs     // util class to read  regex Attribute and validate data
├── Views/             // contains front-end UI blazor pages
│   ├── Layouts/       // page layouts and navigations
│   ├── Pages/
│   │   ├── StaffList.razor    // staff list and detail page
│   │   ├── CreateStaff.razor  // new staff page
├── staff.db           // the sqlite database file
└── Program.cs         // the app main entry point
```

to do database migration do `dotnet ef database update`


# To run to project
if you don't have staff.db file do `dotnet ef database update`

after that just `dotnet run`

# Side notes

*for linux using rider and installed .net using rider*
to use dotnet tools in rider
- `export DOTNET_ROOT=/home/tris/.dotnet`
- `export PATH="$DOTNET_ROOT:$PATH:$DOTNET_ROOT/tools"`
