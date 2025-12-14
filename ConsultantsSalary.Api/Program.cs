using ConsultantsSalary.Infrastructure;
using ConsultantsSalary.Infrastructure.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ConsultantsSalary.Application.Interfaces;
using ConsultantsSalary.Infrastructure.Services;
using ConsultantsSalary.Infrastructure.Repositories;
using Microsoft.OpenApi.Models;
using MediatR;
using Mapster;
using ConsultantsSalary.Application.Mapping;
using ConsultantsSalary.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Consultants Salary API", Version = "v1" });

    var jwtScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer {your JWT}'",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    c.AddSecurityDefinition("Bearer", jwtScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtScheme, Array.Empty<string>() }
    });
});

// EF Core - SQL Server
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// Application services
builder.Services.AddScoped<IRateHistoryService, RateHistoryService>();
builder.Services.AddScoped<ITimeEntryService, TimeEntryService>();
builder.Services.AddScoped<IConsultantRepository, ConsultantRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<ITimeEntryRepository, TimeEntryRepository>();

// MediatR & Mapster
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<AssemblyMarker>());
builder.Services.AddSingleton(TypeAdapterConfig.GlobalSettings);

// Identity Core
builder.Services
    .AddIdentityCore<ApplicationUser>(options =>
    {
        options.Password.RequiredLength = 6;
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// JWT Auth
var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSection.GetValue<string>("Key")!;
var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSection["Issuer"],
        ValidAudience = jwtSection["Audience"],
        IssuerSigningKey = signingKey,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure Mapster mappings
ConsultantMappingConfig.Configure();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Consultants Salary API v1");
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Global exception handling (ProblemDetails)
app.UseMiddleware<ConsultantsSalary.Api.Shared.ExceptionHandlingMiddleware>();

app.MapControllers();

// Seed Manager role and user
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var db = services.GetRequiredService<AppDbContext>();

    const string managerRole = "Manager";
    if (!await roleManager.RoleExistsAsync(managerRole))
    {
        await roleManager.CreateAsync(new IdentityRole(managerRole));
    }

    var managerEmail = "manager@local";
    var existing = await userManager.FindByEmailAsync(managerEmail);
    if (existing is null)
    {
        var user = new ApplicationUser
        {
            UserName = managerEmail,
            Email = managerEmail,
            EmailConfirmed = true
        };
        var create = await userManager.CreateAsync(user, "Pass123$!");
        if (create.Succeeded)
        {
            await userManager.AddToRoleAsync(user, managerRole);
        }
    }

    // Seed baseline domain roles and initial rate history if empty
    if (!db.ConsultantRoles.Any())
    {
        var level1 = new ConsultantsSalary.Domain.Entities.Role { Id = Guid.NewGuid(), Name = "Consultant Level 1" };
        var level2 = new ConsultantsSalary.Domain.Entities.Role { Id = Guid.NewGuid(), Name = "Consultant Level 2" };
        db.ConsultantRoles.AddRange(level1, level2);

        var now = DateTime.UtcNow;
        db.RoleRateHistories.AddRange(
            new ConsultantsSalary.Domain.Entities.RoleRateHistory { Id = Guid.NewGuid(), RoleId = level1.Id, RatePerHour = 100, EffectiveDate = now },
            new ConsultantsSalary.Domain.Entities.RoleRateHistory { Id = Guid.NewGuid(), RoleId = level2.Id, RatePerHour = 150, EffectiveDate = now }
        );

        await db.SaveChangesAsync();
    }
}

app.Run();
