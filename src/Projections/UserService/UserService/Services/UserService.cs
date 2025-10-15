using Dapper;
using EksiSozluk.Common.Events.User;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Services
{
    public class UserService
    {
        private readonly IConfiguration configuration;
        private readonly string connectionString;

        public UserService(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.connectionString = configuration.GetConnectionString("SqlServer");
        }

        public async Task<Guid> CreateEmailConfirmation(UserEmailChangedEvent @event)
        {
            var guid = Guid.NewGuid();
            using var connection = new SqlConnection(connectionString);

            await connection.ExecuteAsync("INSERT INTO EmailConfirmation (Id,CreateDate,OldEmailAdress,NewEmailAdress) VALUES(@Id, GETDATE(),@OldEmailAdress, @NewEmailAdress)",
                               new
                               {
                                   Id = guid,
                                   OldEmailAdress = @event.OldEmailAdress,
                                   NewEmailAdress = @event.NewEmailAdress
                               });
            return guid;
        }

        public async Task UpdateUserPasswordChanged(UserPasswordChangedEvent @event)
        {
            using var connection = new SqlConnection(connectionString);

            await connection.ExecuteAsync("UPDATE [User] SET Password=@Password WHERE Id=@Id ",
                               new
                               {
                                   Id= @event.Id,   
                                   Password = @event.NewPassword
                               });
        }
    }
}
