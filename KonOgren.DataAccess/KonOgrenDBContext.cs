using Microsoft.EntityFrameworkCore;
using System.Linq;

using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore.Design;
using System.IO;
using Microsoft.Extensions.Configuration;
using KonOgren.Domain.Model;
using System;

namespace KonOgren.DataAccess
{
    public class KonOgrenDBContext : DbContext 
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Question> Questions { get; set; }

        private IHttpContextAccessor _httpContextAccessor;
        public KonOgrenDBContext(DbContextOptions<KonOgrenDBContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
      
            _httpContextAccessor = httpContextAccessor;
            Database.EnsureCreated();
            this.ChangeTracker.LazyLoadingEnabled = false;
        }

        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            AddTimestamps();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void AddTimestamps()
        {
            
            var entities = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));
            foreach (var entity in entities)
            {

                Int64? userid = null;
                var utcNow = DateTime.UtcNow;
                try
                {
                    if (_httpContextAccessor.HttpContext != null &&_httpContextAccessor.HttpContext.User != null &&
                        _httpContextAccessor.HttpContext.User.Claims != null &&
                        _httpContextAccessor.HttpContext.User.Claims.First(t => t.Type == "UserId") != null)
                    {
                        userid = Int64.Parse(_httpContextAccessor.HttpContext.User.Claims.First(t => t.Type == "UserId").Value);
                    }
                   
                }
                catch (Exception)
                {

                  
                }
                
                if (entity.State == EntityState.Added)
                {
                    ((BaseEntity)entity.Entity).DateCreated = utcNow;
                    ((BaseEntity)entity.Entity).UserCreated = userid;
                    ((BaseEntity)entity.Entity).IsActive = true;
                    ((BaseEntity)entity.Entity).IsDeleted = false;
                }

                ((BaseEntity)entity.Entity).DateModified = utcNow;
                ((BaseEntity)entity.Entity).UserModified = userid;
            }
        }


       

    }

    
    public class KonOgrenDBFactory : IDesignTimeDbContextFactory<KonOgrenDBContext>
    {
        private IHttpContextAccessor _httpContextAccessor;
        public KonOgrenDBFactory(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public KonOgrenDBFactory()
        {

        }
        public KonOgrenDBContext CreateDbContext(string[] args)
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{environment}.json", optional: true)
                    .Build();
            var connectionString = configuration.GetConnectionString("DBContext");
            //Console.WriteLine(connectionString);
            var optionsBuilder = new DbContextOptionsBuilder<KonOgrenDBContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new KonOgrenDBContext(optionsBuilder.Options, _httpContextAccessor);
        }
    }
}





