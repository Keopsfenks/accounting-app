using Application;
using DefaultAllowCorsPolicyNugetPackage;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Persistance;
using Persistance.Services;
using Persistance.Services.Interface;
using WebApi.Middlewares;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);


// My dependency injection extension method
builder.Services.AddDefaultCors();
builder.Services.AddHttpContextAccessor();

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddPersistance(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup => {
	var jwtSecuritySheme = new OpenApiSecurityScheme {
		BearerFormat = "JWT",
		Name         = "JWT Authentication",
		In           = ParameterLocation.Header,
		Type         = SecuritySchemeType.Http,
		Scheme       = JwtBearerDefaults.AuthenticationScheme,
		Description  = "Put **_ONLY_** yourt JWT Bearer token on textbox below!",

		Reference = new OpenApiReference {
			Id   = JwtBearerDefaults.AuthenticationScheme,
			Type = ReferenceType.SecurityScheme
		}
	};
	setup.AddSecurityDefinition(jwtSecuritySheme.Reference.Id, jwtSecuritySheme);
	setup.AddSecurityRequirement(new OpenApiSecurityRequirement {
		{ jwtSecuritySheme, Array.Empty<string>() }
	});
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
	app.UseSwagger();
	app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseCors();

app.UseAuthorization();


app.MapControllers();

ExtensionsMiddleware.CreateFirstUser(app);

app.Run();