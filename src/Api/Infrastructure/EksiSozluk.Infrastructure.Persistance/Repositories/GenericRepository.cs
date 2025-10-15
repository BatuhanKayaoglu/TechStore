using EksiSozluk.Api.Application.Repositories;
using EksiSozluk.Infrastructure.Persistance.Context;
using EksİSozluk.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Infrastructure.Persistance.Repositories
{

    // Virtual kullanma sebebimiz ihtiyac halinde başka repositorylerde burdaki methodları override edebilelim yani değiştirebilelim diye.
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly DbContext dbContext;

        protected DbSet<TEntity> entity => dbContext.Set<TEntity>();

        public GenericRepository(DbContext dbContext) //EksiSozlukContext yerine DbContext veriyoruz ki ilerde baska db vs de kullanabilelim.
        {
            this.dbContext = dbContext ?? throw new ArgumentException(nameof(dbContext));
        }

        #region Insert Methods
        public virtual int Add(TEntity entity)
        {
            this.entity.Add(entity);
            return dbContext.SaveChanges();
        }

        public virtual int Add(IEnumerable<TEntity> entities)
        {
            if (entities != null && !entities.Any())
                return 0;

            entity.AddRange(entities); // bir koleksiyona birden fazla öğe eklemek istiyorsak AddRange performans avantajı sağlıyor.
            return dbContext.SaveChanges();
        }

        public virtual async Task<int> AddAsync(TEntity entity)
        {
            await this.entity.AddAsync(entity);
            return await dbContext.SaveChangesAsync();
        }

        public virtual async Task<int> AddAsync(IEnumerable<TEntity> entities)
        {
            if (entities != null && !entities.Any())
                return 0;

            await entity.AddRangeAsync(entities);
            return await dbContext.SaveChangesAsync();

        }

        #endregion



        #region Update Methods
        public virtual int Update(TEntity entity)
        {
            this.entity.Attach(entity); // varlık nesnesinin takip edilmesini sağlayarak, Entity Framework üzerinde değişikliklerin daha etkili bir şekilde yönetilmesine olanak tanır. 
            dbContext.Entry(entity).State = EntityState.Modified; // bunun ile sadece değişiklik yapılan yerlerin güncellenmesi saglanıyor.

            return dbContext.SaveChanges();
        }

        public virtual async Task<int> UpdateAsync(TEntity entity)
        {
            this.entity.Attach(entity);
            dbContext.Entry(entity).State = EntityState.Modified;

            return await dbContext.SaveChangesAsync();
        }

        #endregion




        #region Delete Methods
        public virtual int Delete(TEntity entity)
        {

            // Eğer bağlamdan kopuk bir varlık üzerinde değişiklik yapıldıktan sonra bu varlık veritabanından silinirse ve 'Attach' yapılmazsa,
            // SaveChanges çağrısı yapıldığında veritabanında ilgili kaydın silindiği yönünde bir hata alabilirsiniz.

            if (dbContext.Entry(entity).State == EntityState.Detached)
            {
                this.entity.Attach(entity);
            }

            this.entity.Remove(entity);
            return dbContext.SaveChanges();
        }

        public virtual async Task<int> DeleteAsync(TEntity entity)
        {
            if (dbContext.Entry(entity).State == EntityState.Detached)
            {
                this.entity.Attach(entity);
            }

            this.entity.Remove(entity);
            return await dbContext.SaveChangesAsync();
        }

        public virtual int Delete(Guid id)
        {
            var entity = this.entity.Find(id);
            return Delete(entity);
        }

        public virtual async Task<int> DeleteAsync(Guid id)
        {
            var entity = this.entity.Find(id);
            return await DeleteAsync(entity);
        }

        public virtual bool DeleteRange(Expression<Func<TEntity, bool>> predicate)
        {
            dbContext.RemoveRange(entity.Where(predicate));
            return dbContext.SaveChanges() > 0;
        }

        public virtual async Task<bool> DeleteRangeAsync(Expression<Func<TEntity, bool>> predicate)
        {
            dbContext.RemoveRange(predicate); // DeleteRange(product => product.Price > 100) şeklinde sorgular yazabilmek için predicate'e ihtiyacım var. Bu sayede deleteRange metodunu çağırırken istedğim kosulları belirleyebilirim.

            return await dbContext.SaveChangesAsync() > 0;
        }

        #endregion


        #region AddOrUpdate Methods

        public virtual int AddOrUpdate(TEntity entity)
        {
            if (!this.entity.Local.Any(i => EqualityComparer<Guid>.Default.Equals(i.Id, entity.Id)))
                dbContext.Update(entity);

            return dbContext.SaveChanges();
        }

        public virtual async Task<int> AddOrUpdateAsync(TEntity entity)
        {
            // chech the entity with the id already tracked
            if (!this.entity.Local.Any(i => EqualityComparer<Guid>.Default.Equals(i.Id, entity.Id)))
                dbContext.Update(entity);

            return await dbContext.SaveChangesAsync();
        }
        #endregion



        #region Get methods
        public IQueryable<TEntity> AsQueryable() => entity.AsQueryable();

        public virtual IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes)
        {

            /*  
             *  includes, sorgunun yürütülmesi sırasında ilgili nesnelerin (ilişkili nesnelerin) yüklenmesini sağlar. Bu, "eager loading" olarak bilinir ve performansı artırabilir.
             *  Birden fazla Expression<Func<TEntity, object>> ifadesi alabilir, bu sayede sorguya ilişkili tabloların (ilişkili nesnelerin) eklenmesini sağlar.
             *  Örneğin, includes parametresine entity => entity.Orders ifadesi ekleyerek, ana varlıkla ilişkilendirilmiş siparişleri yükleyebilirsiniz.

                Bir kullanıcının siparişleriyle birlikte bilgilerini getir
                var userWithOrders = Get(entity => entity.UserId == 1, includes: entity => entity.Orders);
             */

            var query = entity.AsQueryable();

            if (predicate != null)
                query = query.Where(predicate);

            query = ApplyIncludes(query, includes);

            if (noTracking)
                query = query.AsNoTracking();

            return query;
        }

        public virtual async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = entity.AsQueryable();

            if (predicate != null)
                query = query.Where(predicate);

            query = ApplyIncludes(query, includes);

            if (noTracking)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync();
        }


        public virtual async Task<List<TEntity>> GetList(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes)
        {

            /*
             var users = await GetList(
                predicate: entity => entity.Age > 18 && entity.City == "New York",
                orderBy: query => query.OrderBy(entity => entity.LastName),
                includes: entity => entity.Orders, entity => entity.Address);

             */

            IQueryable<TEntity> query = entity;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            foreach (Expression<Func<TEntity, object>> include in includes)
            {
                query = query.Include(include);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (noTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync();
        }


        public virtual async Task<List<TEntity>> GetAll(bool noTracking = true)
        {
            IQueryable<TEntity> query = entity;

            if (noTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync();
        }

        public virtual async Task<TEntity> GetByIdAsync(Guid id, bool noTracking = true, bool loadRelated = false, params Expression<Func<TEntity, object>>[] includes)
        {
            TEntity found = await entity.FindAsync(id);

            if (found == null)
                return null;

            if (noTracking)
                dbContext.Entry(found).State = EntityState.Detached;

            if (loadRelated)
            {
                foreach (Expression<Func<TEntity, object>> include in includes)
                {
                    dbContext.Entry(found).Reference(include).Load();
                }
            }

            return found;
        }



        public virtual async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = entity;

            if (predicate != null)
                query = query.Where(predicate);

            query = ApplyIncludes(query, includes);

            if (noTracking)
                query = query.AsNoTracking();

            return await query.SingleOrDefaultAsync();
        }

        #endregion



        #region Bulk Methods

        public virtual Task BulkDeleteById(IEnumerable<Guid> ids)
        {
            if (ids != null && !ids.Any())
                return Task.CompletedTask; // null ise veya içinde hiç öğe yoksa, işlem yapmadan tamamlanan bir Task döndürülüyor.

            dbContext.RemoveRange(entity.Where(i => ids.Contains(i.Id))); // belirli bir koşulu sağlayan öğeleri seçmek ve bu öğeleri ardından silmek için kullanıyoruz Where'li kısmı.
            return dbContext.SaveChangesAsync();
        }
        public virtual async Task BulkAdd(IEnumerable<TEntity> entities)
        {
            if (entities != null && !entities.Any())
                await Task.CompletedTask;

            //foreach(var entityItem in entities)
            //{
            //    entity.Add(entityItem);   
            //}

            await entity.AddRangeAsync(entities);

            await dbContext.SaveChangesAsync();
        }

        public virtual Task BulkDelete(Expression<Func<TEntity, bool>> predicate)
        {
            var entitiesToDelete = dbContext.Set<TEntity>().Where(predicate);
            dbContext.RemoveRange(entitiesToDelete);
            return dbContext.SaveChangesAsync();
        }

        public virtual async Task BulkDelete(IEnumerable<TEntity> entities)
        {
            if (entities != null && !entities.Any())
                await Task.CompletedTask;


            dbContext.RemoveRange(entities);
            await dbContext.SaveChangesAsync();
        }

        public virtual Task BulkUpdate(IEnumerable<TEntity> entities)
        {
            if (entities == null || !entities.Any())
                return Task.CompletedTask;

            foreach (var entity in entities)
            {
                // Attach the entity to the DbContext if not already attached
                if (!dbContext.Set<TEntity>().Local.Contains(entity))
                    dbContext.Set<TEntity>().Attach(entity);

                // Mark the entity as modified to ensure it gets updated
                dbContext.Entry(entity).State = EntityState.Modified;
            }

            return dbContext.SaveChangesAsync();
        }

        #endregion


        #region SaveChanges Methods
        public Task<int> SaveChangesAsync()
        {
            return dbContext.SaveChangesAsync();
        }

        public int SaveChanges()
        {
            return dbContext.SaveChanges();
        }
        #endregion

        private IQueryable<TEntity> ApplyIncludes(IQueryable<TEntity> query, params Expression<Func<TEntity, object>>[] includes)
        {
            #region Fonksiyonun kullanımı
            /*
             *  Bu metodun adı "ApplyIncludes" ve bu metot, bir IQueryable sorgusuna "eager loading" yapmak amacıyla ilişkili nesneleri eklemek için kullanılıyor. "Eager loading", ilişkili nesnelerin, ana nesneyle birlikte bir sorgu ile yüklenmesini ifade eder. Bu, daha sonra ilişkili nesnelere erişim sağlamak için ek bir sorgu yapma ihtiyacını ortadan kaldırabilir ve performans avantajı sağlayabilir.

                query (IQueryable<TEntity> query):

                Bu parametre, LINQ sorgusunun temelini oluşturan IQueryable nesnesini temsil eder. Bu genellikle bir veritabanı tablosundan veri çeken bir LINQ sorgusudur.
                includes (params Expression<Func<TEntity, object>>[] includes):

                includes parametresi, ilişkili nesnelerin (ilişkili tabloların) belirtilen property'lerini içeren bir dizi lambda ifadesini temsil eder.
                Bu lambda ifadeleri, sorguya eklenmiş olarak belirtilen property'lerin (ilişkili nesnelerin) yüklenmesini sağlar.

                Metodun İşleyişi:
                ApplyIncludes metodu, foreach döngüsü ile includes parametresinde belirtilen lambda ifadelerini alır.
                Her bir lambda ifadesi, query.Include(include) kullanılarak, sorguya belirtilen ilişkili nesnelerin yüklenmesini ekler.
                Bu işlem, LINQ sorgusunu genişleterek, belirtilen ilişkili nesnelerin sorgu sonucuna dahil edilmesini sağlar.
                Son olarak, genişletilmiş sorgu olan query nesnesi geri döndürülür.
                Bu metodun amacı, query üzerine belirtilen ilişkili nesnelerin yüklenmesini eklemektir. Bu sayede, sorgunun sonucu, ilişkili nesnelerle birlikte alınabilir ve daha sonra bu nesnelere erişim sağlamak için ek bir sorgu yapma ihtiyacı ortadan kalkar.
             */
            #endregion

            foreach (Expression<Func<TEntity, object>> include in includes)
            {
                query = query.Include(include);
            }

            return query;
        }



    }
}
