using BurgerRoyale.Auth.API.Middleware;
using BurgerRoyale.Auth.API.Services;
using BurgerRoyale.Auth.Domain.Interface.Services;
using BurgerRoyale.Auth.IOC;
using BurgerRoyale.Auth.IOC.Configurations;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using System.Globalization;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("en-US");

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "BurgerRoyale.Auth",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using Bearer scheme."
    });

    options.AddSecurityRequirement
    (
        new OpenApiSecurityRequirement
        {            
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference{
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] { }
            }
        }
    );

    options.IncludeXmlComments
    (
        Path.Combine
        (
            AppContext.BaseDirectory,
            $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"
        )
    );

    options.EnableAnnotations();
});

builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

DependencyInjectionConfiguration.Register(builder.Services, builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuthenticatedUser, AuthenticatedUser>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

ConfigureDatabase.RunMigrations(app);

app.Run();
