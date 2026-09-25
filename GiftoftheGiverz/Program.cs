using GiftoftheGiverz.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ============================================================
// Database
// ============================================================

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();


// ============================================================
// IDENTITY
// ============================================================

builder.Services.AddDefaultIdentity<IdentityUser>(
    options =>
    {
        // Prototype accounts are confirmed automatically.
        options.SignIn.RequireConfirmedAccount = true;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();


// ============================================================
// RAZOR PAGES
// ============================================================

builder.Services.AddRazorPages();

builder.Services.AddHttpClient();

var app = builder.Build();


// ============================================================
// CREATE ROLES AND PROTOTYPE ACCOUNTS
// ============================================================

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var roleManager =
            services.GetRequiredService<RoleManager<IdentityRole>>();

        var userManager =
            services.GetRequiredService<UserManager<IdentityUser>>();


        // --------------------------------------------------------
        // CREATE ROLES
        // --------------------------------------------------------

        string[] roles =
        {
            "Donor",
            "Employee"
        };

        foreach (var roleName in roles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var roleResult =
                    await roleManager.CreateAsync(
                        new IdentityRole(roleName));

                if (!roleResult.Succeeded)
                {
                    foreach (var error in roleResult.Errors)
                    {
                        Console.WriteLine(
                            $"Role creation error: {error.Description}");
                    }
                }
            }
        }


        // --------------------------------------------------------
        // CREATE PROTOTYPE EMPLOYEE
        // --------------------------------------------------------

        var employeeEmail =
            "employee@giftofthegiverz.co.za";

        var employeePassword =
            "Employee@123";

        var employee =
            await userManager.FindByEmailAsync(employeeEmail);

        if (employee == null)
        {
            employee = new IdentityUser
            {
                UserName = employeeEmail,
                Email = employeeEmail,
                EmailConfirmed = true
            };

            var result =
                await userManager.CreateAsync(
                    employee,
                    employeePassword);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(
                    employee,
                    "Employee");

                Console.WriteLine(
                    "Prototype Employee account created.");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine(
                        $"Employee account error: {error.Description}");
                }
            }
        }
        else
        {
            if (!await userManager.IsInRoleAsync(
                    employee,
                    "Employee"))
            {
                await userManager.AddToRoleAsync(
                    employee,
                    "Employee");
            }
        }


        // --------------------------------------------------------
        // CREATE PROTOTYPE DONOR
        // --------------------------------------------------------

        var donorEmail =
            "donor@giftofthegiverz.co.za";

        var donorPassword =
            "Donor@123";

        var donor =
            await userManager.FindByEmailAsync(donorEmail);

        if (donor == null)
        {
            donor = new IdentityUser
            {
                UserName = donorEmail,
                Email = donorEmail,
                EmailConfirmed = true
            };

            var result =
                await userManager.CreateAsync(
                    donor,
                    donorPassword);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(
                    donor,
                    "Donor");

                Console.WriteLine(
                    "Prototype Donor account created.");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine(
                        $"Donor account error: {error.Description}");
                }
            }
        }
        else
        {
            if (!await userManager.IsInRoleAsync(
                    donor,
                    "Donor"))
            {
                await userManager.AddToRoleAsync(
                    donor,
                    "Donor");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(
            $"An error occurred creating roles/users: {ex.Message}");
    }
}


// ============================================================
// HTTP REQUEST PIPELINE
// ============================================================

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();


// Authentication must come before Authorization.
app.UseAuthentication();

app.UseAuthorization();


// Razor Pages
app.MapRazorPages();

app.Run();