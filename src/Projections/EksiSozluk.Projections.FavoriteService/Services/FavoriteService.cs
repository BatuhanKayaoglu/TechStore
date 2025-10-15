using EksiSozluk.Common.Events.Entry;
using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Dapper;
using EksiSozluk.Common.Events.EntryComment;

namespace EksiSozluk.Projections.FavoriteService.Services
{
    public class FavoriteService
    {
        private readonly string connectionString;

        public FavoriteService(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public async Task CreateEntryFav(CreateEntryFavEvent @event)
        {
            using var connection = new SqlConnection(connectionString);

            await connection.ExecuteAsync("INSERT INTO EntryFavorite (Id,EntryId, CreatedById, CreateDate) VALUES(@Id, @EntryId, @CreatedById, GETDATE())",
                new
                {
                    Id = Guid.NewGuid(),
                    EntryId = @event.EntryId,
                    CreatedById = @event.CreatedBy
                    //CreatedById = "3fa85f64-5717-4562-b3fc-2c963f66afa6"
                });
        }   

        public async Task DeleteEntryFav(DeleteEntryFavEvent @event)
        {
            using var connection = new SqlConnection(connectionString);

            await connection.ExecuteAsync("DELETE FROM EntryFavorite WHERE EntryId = @EntryId AND CreatedBy=@CreatedBy",
                new
                {
                    Id = @event.EntryId,      
                    CreatedBy = @event.CreatedBy        
                });
        }

        public async Task CreateEntryCommentFav(CreateEntryCommentFavEvent @event)
        {
            using var connection = new SqlConnection(connectionString);

            await connection.ExecuteAsync("INSERT INTO EntryCommentFavorite (Id,EntryCommentId, CreatedById, CreateDate) VALUES(@Id, @EntryCommentId, @CreatedById, GETDATE())",
                               new
                               {
                    Id = Guid.NewGuid(),
                    EntryCommentId = @event.EntryCommentId,
                    CreatedById = @event.CreatedBy
                    //CreatedById = "3fa85f64-5717-4562-b3fc-2c963f66afa6"
                });
        }

        public async Task DeleteEntryCommentFav(DeleteEntryCommentFavEvent @event)
        {
            using var connection = new SqlConnection(connectionString);

            await connection.ExecuteAsync("DELETE FROM EntryCommentFavorite WHERE EntryId = @EntryId AND CreatedBy=@CreatedBy",
                new
                {
                    Id = @event.EntryCommentId,
                    CreatedBy = @event.CreatedBy
                });
        }

    }
}
