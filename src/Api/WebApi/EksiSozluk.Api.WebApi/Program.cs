using EksiSozluk.Infrastructure.Persistance.Extensions;
using EksiSozluk.Api.Application.Extensions;
using FluentValidation.AspNetCore;
using EksiSozluk.Api.WebApi.Infrastructure.Extensions;
using EksiSozluk.Common.Infrastructure;
using EksiSozluk.Api.WebApi.Middlewares.Filter.Validation;
using EksiSozluk.Api.WebApi.Middlewares.Filter.GlobalExceptionHandler;
using EksiSozluk.Api.WebApi.Extensions;

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
