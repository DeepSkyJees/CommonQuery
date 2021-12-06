using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace CommonQuery.MVC5.Models
{
    public class MyContext : DbContext
    {
        //// Your context has been configured to use a 'Models' connection string from your application's
        //// configuration file (App.config or Web.config). By default, this connection string targets the
        //// 'CommonQuery.MVC.Example.Models.Models' database on your LocalDb instance.
        ////
        //// If you wish to target a different database and/or database provider, modify the 'Models'
        //// connection string in the application configuration file.
        ///// <summary>
        /////     Initializes a new instance of the <see cref="BimModelContext" /> class.
        ///// </summary>
        ///// <param name="options">The options.</param>
        //public Models(DbContextOptions<Models> options)
        //    : base(options)
        //{
        //}
        // Add a DbSet for each entity type that you want to include in your model. For more information
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<MyEntity> MyEntities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.HasCharSet(charSet: CharSet.Utf8Mb4);

            modelBuilder.Entity<MyEntity>().HasKey(e => e.Id);
          

            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = $"Server=192.168.1.228;Port=3306;database=mydatabase;uid=root;pwd=mysqladm;";
            //optionsBuilder.UseSqlServer(connectionString);
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    }

    public class MyEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}