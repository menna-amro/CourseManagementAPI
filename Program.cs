using System.Text;
using CourseManagementAPI;
using CourseManagementAPI.Models;
using CourseManagementAPI.Services.Implementations;
using CourseManagementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


// ================= DATABASE =================

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);


// ================= SERVICES =================

// JWT
builder.Services.AddScoped<IJwtService, JwtService>();

// Course
builder.Services.AddScoped<ICourseService, CourseService>();

// Student
builder.Services.AddScoped<IStudentService, StudentService>();

// Instructor
builder.Services.AddScoped<IInstructorService, InstructorService>();

// Enrollment
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();


// ================= CONTROLLERS =================

builder.Services.AddControllers();


// ================= JWT AUTHENTICATION =================

var jwtSettings = builder.Configuration.GetSection("Jwt");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
        JwtBearerDefaults.AuthenticationScheme;

    options.DefaultChallengeScheme =
        JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters =
        new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],

            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings["Key"]!)
                )
        };
});


// ================= AUTHORIZATION =================

builder.Services.AddAuthorization();


// ================= SWAGGER =================

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter ONLY the token (Swagger adds Bearer automatically)",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",

        Reference = new OpenApiReference
        {
            Id = "Bearer",
            Type = ReferenceType.SecurityScheme
        }
    };

    options.AddSecurityDefinition("Bearer", securityScheme);

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            { securityScheme, new string[] { } }
        }
    );
});


// ================= BUILD APP =================

var app = builder.Build();


// ================= MIDDLEWARE =================

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();