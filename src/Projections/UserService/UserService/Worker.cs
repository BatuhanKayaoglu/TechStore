using EksiSozluk.Common;
using EksiSozluk.Common.Events.User;
using EksiSozluk.Common.Infrastructure;
using UserService.Services;

namespace UserService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration configuration;
        private readonly UserService.Services.UserService userService;
        private readonly EmailService emailService;


        public Worker(ILogger<Worker> logger, IConfiguration configuration, Services.UserService userService, EmailService emailService)
        {
            _logger = logger;
            this.configuration = configuration;
            this.userService = userService;
            this.emailService = emailService;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            QueueFactory.CreateBasicConsumer()
                           .EnsureExchange(SozlukConstants.UserExchangeName)
                           .EnsureQueue(SozlukConstants.UserEmailChangedQueueName, SozlukConstants.UserExchangeName)
                           .Receive<UserEmailChangedEvent>(async (user) =>
                           {
                               var confirmationId = userService.CreateEmailConfirmation(user).GetAwaiter().GetResult(); // add Db operation
                               var link = emailService.GenerateConfirmationLink(confirmationId); // send email url
                               await emailService.SendEmailAsync(user.NewEmailAdress, "Email Confirmation", link); // send email  

                               Console.WriteLine($"Email sent to {user.NewEmailAdress} with link {link}");
                           })
                           .StartingConsuming(SozlukConstants.UserEmailChangedQueueName);


            QueueFactory.CreateBasicConsumer()
               .EnsureExchange(SozlukConstants.UserExchangeName)
               .EnsureQueue(SozlukConstants.UserPasswordChangedQueueName, SozlukConstants.UserExchangeName)
               .Receive<UserPasswordChangedEvent>(async (user) =>
               {
                   userService.UpdateUserPasswordChanged(user).GetAwaiter().GetResult(); // add Db operation        
               })
               .StartingConsuming(SozlukConstants.UserPasswordChangedQueueName);



        }
    }
}
