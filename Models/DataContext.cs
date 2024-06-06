using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace PTUDTMDT.Models
{
    public partial class DataContext : DbContext
    {
        public DataContext()
            : base("name=DataContext")
        {
        }

        public virtual DbSet<cart> carts { get; set; }
        public virtual DbSet<category> categories { get; set; }
        public virtual DbSet<customer> customers { get; set; }
        public virtual DbSet<order_> order_ { get; set; }
        public virtual DbSet<order_item> order_item { get; set; }
        public virtual DbSet<payment> payments { get; set; }
        public virtual DbSet<product> products { get; set; }
        public virtual DbSet<shipment> shipments { get; set; }
        public virtual DbSet<wishlist> wishlists { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<order_>()
                .Property(e => e.order_price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<order_item>()
                .Property(e => e.price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<payment>()
                .Property(e => e.amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<product>()
                .Property(e => e.price)
                .HasPrecision(10, 2);
        }
    }
}
