using ConsultantsSalary.Domain.Entities;
using ConsultantsSalary.Infrastructure.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace ConsultantsSalary.Infrastructure.Data;
/// <summary>
/// I used GPT to generate this class to assit you in testing the application.
/// </summary>
public static class DataSeeder
{
    public static async Task SeedAllDataAsync(
        RoleManager<IdentityRole> roleManager,
        UserManager<ApplicationUser> userManager,
        AppDbContext dbContext)
    {
        Console.WriteLine("Starting data seeding...");

        await SeedRolesAsync(roleManager);
        await SeedUsersAsync(userManager, roleManager);
        await SeedRolesAndRatesAsync(dbContext);
        await SeedConsultantsAsync(dbContext, userManager);
        await SeedTasksAsync(dbContext);
        await SeedTaskAssignmentsAsync(dbContext);
        await SeedTimeEntriesAsync(dbContext);

        Console.WriteLine("Data seeding completed successfully!");
    }

    private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        var roles = new[] { "Manager", "Consultant" };

        foreach (var roleName in roles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                Console.WriteLine($"Creating role: {roleName}");
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }

    private static async System.Threading.Tasks.Task SeedUsersAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        var users = new[]
        {
            new { Email = "mustafaalamin.07@gmail.com", Password = "Mus90@live", Role = "Manager" },
            new { Email = "ahmed87s@gmail.com", Password = "Ahmed@123", Role = "Consultant" }

        };

        foreach (var userData in users)
        {
            var existingUser = await userManager.FindByEmailAsync(userData.Email);
            if (existingUser == null)
            {
                Console.WriteLine($"Creating user: {userData.Email}");
                var user = new ApplicationUser
                {
                    UserName = userData.Email,
                    Email = userData.Email,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, userData.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, userData.Role);
                }
            }
        }
    }

    private static async Task SeedRolesAndRatesAsync(AppDbContext dbContext)
    {
        if (!await dbContext.ConsultantRoles.AnyAsync())
        {
            Console.WriteLine("Creating consultant roles and rates");

            var roles = new[]
            {
                new { Name = "Consultant Level 1", InitialRate = 75m },
                new { Name = "Consultant Level 2", InitialRate = 120m }
            };

            var now = DateTime.UtcNow;
            var roleEntities = new List<Role>();

            foreach (var roleData in roles)
            {
                var role = new Role
                {
                    Id = Guid.NewGuid(),
                    Name = roleData.Name
                };
                roleEntities.Add(role);

                var rateHistory = new RoleRateHistory
                {
                    Id = Guid.NewGuid(),
                    RoleId = role.Id,
                    RatePerHour = roleData.InitialRate,
                    EffectiveDate = now.AddMonths(-6),
                    EndDate = null
                };
                dbContext.RoleRateHistories.Add(rateHistory);
            }

            dbContext.ConsultantRoles.AddRange(roleEntities);
            await dbContext.SaveChangesAsync();
        }
    }

    private static async Task SeedConsultantsAsync(AppDbContext dbContext, UserManager<ApplicationUser> userManager)
    {
        if (!await dbContext.Consultants.AnyAsync())
        {
            Console.WriteLine("Creating consultants");

            var levelOneRole = await dbContext.ConsultantRoles.FirstOrDefaultAsync(r => r.Name == "Consultant Level 1");
            var levelTwoRole = await dbContext.ConsultantRoles.FirstOrDefaultAsync(r => r.Name == "Consultant Level 2");

            var consultantUsers = new[]
            {
                new { Email = "ahmed87s@gmail.com", RoleId = levelOneRole?.Id ?? Guid.NewGuid(), FirstName = "Mustafa", LastName = "Ali" }
            };

            foreach (var consultantData in consultantUsers)
            {
                var user = await userManager.FindByEmailAsync(consultantData.Email);
                if (user != null)
                {
                    var consultant = new Consultant
                    {
                        Id = Guid.NewGuid(),
                        FirstName = consultantData.FirstName,
                        LastName = consultantData.LastName,
                        Email = consultantData.Email,
                        RoleId = consultantData.RoleId,
                        ProfileImage = GenerateDummyProfileImage()
                    };
                    dbContext.Consultants.Add(consultant);
                }
            }

            await dbContext.SaveChangesAsync();
        }
    }

    private static async Task SeedTasksAsync(AppDbContext dbContext)
    {
        if (!await dbContext.Tasks.AnyAsync())
        {
            Console.WriteLine("Creating tasks");

            var tasks = new[]
            {
                new { Name = "Database Design", Description = "Design and implement database schema for new client system" },
                new { Name = "API Development", Description = "Develop RESTful APIs for mobile application" },
                new { Name = "Frontend Development", Description = "Build responsive web interface using React" }
            };

            foreach (var taskData in tasks)
            {
                var task = new ConsultantsSalary.Domain.Entities.Task
                {
                    Id = Guid.NewGuid(),
                    Name = taskData.Name,
                    Description = taskData.Description
                };
                dbContext.Tasks.Add(task);
            }

            await dbContext.SaveChangesAsync();
        }
    }

    private static async Task SeedTaskAssignmentsAsync(AppDbContext dbContext)
    {
        if (!await dbContext.ConsultantTaskAssignments.AnyAsync())
        {
            Console.WriteLine("Creating task assignments");

            var consultants = await dbContext.Consultants.Include(c => c.TaskAssignments).ToListAsync();
            var tasks = await dbContext.Tasks.ToListAsync();

            var random = new Random();

            foreach (var consultant in consultants)
            {
                var taskCount = random.Next(2, 5);
                var assignedTasks = tasks.OrderBy(x => random.Next()).Take(taskCount).ToList();

                foreach (var task in assignedTasks)
                {
                    var assignment = new ConsultantTaskAssignment
                    {
                        Id = Guid.NewGuid(),
                        ConsultantId = consultant.Id,
                        TaskId = task.Id
                    };
                    dbContext.ConsultantTaskAssignments.Add(assignment);
                }
            }

            await dbContext.SaveChangesAsync();
        }
    }

    private static async Task SeedTimeEntriesAsync(AppDbContext dbContext)
    {
        if (!await dbContext.TimeEntries.AnyAsync())
        {
            Console.WriteLine("Creating time entries");

            var assignments = await dbContext.ConsultantTaskAssignments
                .Include(cta => cta.Consultant)
                .Include(cta => cta.Task)
                .ToListAsync();

            var random = new Random();
            var startDate = DateTime.UtcNow.AddDays(-30); // Last 30 days

            foreach (var assignment in assignments)
            {
                var entryCount = random.Next(5, 16);

                for (int i = 0; i < entryCount; i++)
                {
                    var workDate = startDate.AddDays(random.Next(0, 30));
                    var hoursWorked = random.Next(1, 9) + random.NextDouble(); // 1-8 hours with decimal

                    var existingHours = await dbContext.TimeEntries
                        .Where(te => te.ConsultantId == assignment.ConsultantId && te.DateWorked.Date == workDate.Date)
                        .SumAsync(te => te.HoursWorked);

                    if (existingHours + (decimal)hoursWorked <= 12)
                    {
                        var currentRate = await dbContext.RoleRateHistories
                            .Where(rh => rh.RoleId == assignment.Consultant.RoleId &&
                                       rh.EffectiveDate <= workDate &&
                                       (rh.EndDate == null || rh.EndDate > workDate))
                            .OrderByDescending(rh => rh.EffectiveDate)
                            .FirstOrDefaultAsync();

                        if (currentRate != null)
                        {
                            var timeEntry = new TimeEntry
                            {
                                Id = Guid.NewGuid(),
                                ConsultantId = assignment.ConsultantId,
                                TaskId = assignment.TaskId,
                                DateWorked = workDate,
                                HoursWorked = (decimal)hoursWorked,
                                RateSnapshotId = currentRate.Id
                            };
                            dbContext.TimeEntries.Add(timeEntry);
                        }
                    }
                }
            }

            await dbContext.SaveChangesAsync();
        }
    }

    private static byte[] GenerateDummyProfileImage()
    {
        return new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A, 0x00, 0x00, 0x00, 0x0D, 0x49, 0x48, 0x44, 0x52, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x08, 0x06, 0x00, 0x00, 0x00, 0x1F, 0x15, 0xC4, 0x89, 0x00, 0x00, 0x00, 0x0A, 0x49, 0x44, 0x41, 0x54, 0x78, 0x9C, 0x63, 0x00, 0x01, 0x00, 0x00, 0x05, 0x00, 0x01, 0x0D, 0x0A, 0x2D, 0xB4, 0x00, 0x00, 0x00, 0x00, 0x49, 0x45, 0x4E, 0x44, 0xAE, 0x42, 0x60, 0x82 };
    }
}
