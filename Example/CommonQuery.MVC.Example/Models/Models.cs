using System.Data.Entity;

namespace CommonQuery.MVC.Example.Models
{
    public class Models : DbContext
    {
        // Your context has been configured to use a 'Models' connection string from your application's
        // configuration file (App.config or Web.config). By default, this connection string targets the
        // 'CommonQuery.MVC.Example.Models.Models' database on your LocalDb instance.
        //
        // If you wish to target a different database and/or database provider, modify the 'Models'
        // connection string in the application configuration file.
        public Models()
            : base("name=Models")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<MyEntity> MyEntities { get; set; }
    }

    public class MyEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}