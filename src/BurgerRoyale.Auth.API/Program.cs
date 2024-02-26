using BurgerRoyale.Auth.API.Middleware;
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

ConfigureDatabase.RunMigrations(app);

app.Run();
