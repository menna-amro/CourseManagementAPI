using System.Text;
using CourseManagementAPI;
using CourseManagementAPI.Models;
using CourseManagementAPI.Services.Implementations;
using CourseManagementAPI.Services.Interfaces;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
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


// ================= IDENTITY CONFIGURATION =================

builder.Services
.AddIdentity<User, IdentityRole>()
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();


// ================= SERVICES =================

builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddScoped<ICourseService, CourseService>();

builder.Services.AddScoped<IStudentService, StudentService>();

builder.Services.AddScoped<IInstructorService, InstructorService>();

builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();


// ================= CONTROLLERS =================

builder.Services.AddControllers();


// ================= JWT AUTHENTICATION =================

var jwtSettings = builder.Configuration.GetSection("Jwt");

builder.Services
.AddAuthentication(options =>
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


    // READ TOKEN FROM COOKIE
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var token =
                context.Request.Cookies["jwt"];

            if (!string.IsNullOrEmpty(token))
            {
                context.Token = token;
            }

            return Task.CompletedTask;
        }
    };
});


// ================= AUTHORIZATION =================

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy =
        options.DefaultPolicy;
});


// ================= SWAGGER =================

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    var securityScheme =
        new OpenApiSecurityScheme
        {
            Name = "jwt",

            Type = SecuritySchemeType.ApiKey,

            In = ParameterLocation.Cookie,

            Description =
                "JWT stored automatically in HttpOnly cookie after login",

            Reference =
                new OpenApiReference
                {
                    Id = "CookieAuth",

                    Type =
                        ReferenceType.SecurityScheme
                }
        };

    options.AddSecurityDefinition(
        "CookieAuth",
        securityScheme
    );

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                securityScheme,
                new string[] { }
            }
        }
    );
});


// ================= BUILD APP =================

var app = builder.Build();


// ================= MIDDLEWARE =================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();