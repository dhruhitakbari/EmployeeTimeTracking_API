
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

            // ✅ Swagger with JWT config
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

                // 🔑 Add JWT support
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your token.\n\nExample: \"Bearer 12345abcdef\""
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });


            // --- 1. Get JWT Settings from appsettings.json ---
            var jwtSettings = builder.Configuration.GetSection("JwtSettings");

            // --- 2. Add Authentication Services ---
            builder.Services.AddAuthentication(options =>
            {
                // This sets the default schemes.
                // When you use [Authorize], it will use this.
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine("❌ AUTH FAILED: " + context.Exception.Message);
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        Console.WriteLine("✅ TOKEN VALIDATED");
                        return Task.CompletedTask;
                    }
                };
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // --- These 5 settings are the most important ---

                    // 1. Validate the Issuer (who created the token)
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings["Issuer"],

                    // 2. Validate the Audience (who the token is for)
                    ValidateAudience = true,
                    ValidAudience = jwtSettings["Audience"],

                    // 3. Validate the Lifetime (check if it's expired)
                    ValidateLifetime = true,

                    // 4. Validate the Signing Key (the secret key)
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"])),

                    // ⭐ ADD THIS LINE (VERY IMPORTANT)
                    RoleClaimType = "role",

                    // 5. Clock Skew (optional, good for servers not in sync)
                    ClockSkew = TimeSpan.Zero
                };
            });
            builder.Services.AddAuthorization();

            // Add services to the container.

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            #region Service and Repository Registration
            builder.Services.AddRepository();
            builder.Services.AddServices();
            #endregion

            #region // Register AutoMapper
            // This scans your assemblies to find the "MappingProfile" class automatically
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            #endregion

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy => policy.AllowAnyOrigin()
                                    .AllowAnyMethod()
                                    .AllowAnyHeader());
            });

            var app = builder.Build();

            app.UseCors("AllowAll");

            // Configure the HTTP request pipeline.
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
        }
    }
}
