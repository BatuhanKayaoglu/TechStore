using UserService;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddTransient<UserService.Services.UserService>();      
builder.Services.AddTransient<UserService.Services.EmailService>();     

var host = builder.Build();
host.Run();
