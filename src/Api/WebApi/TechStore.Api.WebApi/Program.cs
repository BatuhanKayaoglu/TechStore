using TechStore.Infrastructure.Persistance.Extensions;
using TechStore.Api.Application.Extensions;
using FluentValidation.AspNetCore;
using TechStore.Api.WebApi.Infrastructure.Extensions;
using TechStore.Common.Infrastructure;
using TechStore.Api.WebApi.Middlewares.Filter.Validation;
using TechStore.Api.WebApi.Middlewares.Filter.GlobalExceptionHandler;
using TechStore.Api.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructureRegistration(builder.Configuration);
builder.Services.AddApplicationRegistration();
builder.Services.ConfigureAuth(builder.Configuration);
builder.Services.AddWebApiRegistration(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.ConfigureExceptionHandlingMiddleware();
app.ConfigureExceptionHandling(app.Environment.IsDevelopment());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
