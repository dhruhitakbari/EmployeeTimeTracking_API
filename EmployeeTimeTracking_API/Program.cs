using EmployeeTimeTracking_API.IService;
using EmployeeTimeTracking_API.Repository;
using EmployeeTimeTracking_API.Repository.Data;
using EmployeeTimeTracking_API.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace EmployeeTimeTracking_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ──────────────────────────────────────────
            // 1. READ JWT SETTINGS
            // ──────────────────────────────────────────
            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["Key"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];

            // ──────────────────────────────────────────
            // 2. AUTHENTICATION — JWT SETUP
            // ──────────────────────────────────────────
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                // ⭐ KEY LINE — stops .NET from renaming your claims
                options.MapInboundClaims = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,

                    ValidateAudience = true,
                    ValidAudience = audience,

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                                                   Encoding.UTF8.GetBytes(secretKey)),

                    // ⭐ KEY LINE — tells .NET which claim is the role
                    RoleClaimType = "role"
                };

                // Debug events — remove these after everything works
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        System.Diagnostics.Debug.WriteLine("❌ AUTH FAILED: " + context.Exception.Message);
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        System.Diagnostics.Debug.WriteLine("✅ TOKEN VALIDATED");
                        foreach (var claim in context.Principal.Claims)
                            System.Diagnostics.Debug.WriteLine($"   ➤ {claim.Type} : {claim.Value}");
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        System.Diagnostics.Debug.WriteLine("🚫 CHALLENGE (401) - " + context.ErrorDescription);
                        return Task.CompletedTask;
                    },
                    OnForbidden = context =>
                    {
                        System.Diagnostics.Debug.WriteLine("🚫 FORBIDDEN (403)");
                        return Task.CompletedTask;
                    }
                };
            });

            // ──────────────────────────────────────────
            // 3. AUTHORIZATION
            // ──────────────────────────────────────────
            builder.Services.AddAuthorization();

            // ──────────────────────────────────────────
            // 4. SWAGGER WITH JWT SUPPORT
            // ──────────────────────────────────────────
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Employee Tracker API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,   // ← Http (not ApiKey)
                    Scheme = "bearer",                  // ← lowercase
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your token here (without Bearer prefix)"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id   = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            // ──────────────────────────────────────────
            // 5. OTHER SERVICES
            // ──────────────────────────────────────────
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddRepository();
            builder.Services.AddServices();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngular", policy =>
                    policy.WithOrigins("http://localhost:4200")  // ← your Angular URL
                          .AllowAnyMethod()
                          .AllowAnyHeader());
            });

            // ──────────────────────────────────────────
            // 6. MIDDLEWARE PIPELINE (ORDER MATTERS!)
            // ──────────────────────────────────────────
            var app = builder.Build();

            app.UseCors("AllowAngular");   // ← must be FIRST

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();       // ← must be BEFORE Authorization
            app.UseAuthorization();        // ← must be AFTER Authentication

            app.MapControllers();
            app.Run();
        }
    }
}