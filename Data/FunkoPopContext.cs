using Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Data
{
    public partial class FunkoPopContext : DbContext
    {
        public FunkoPopContext()
            : base("name=FunkoPopContext")
        {
        }

        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<Cart> Cart { get; set; }
        public virtual DbSet<Collection> Collection { get; set; }
        public virtual DbSet<Consultation_product> Consultation_product { get; set; }
        public virtual DbSet<Favorite> Favorite { get; set; }
        public virtual DbSet<Line_cart> Line_cart { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Province> Province { get; set; }
        public virtual DbSet<Sale> Sale { get; set; }
        public virtual DbSet<Sale_product> Sale_product { get; set; }
        public virtual DbSet<Shipment> Shipment { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>()
                .Property(e => e.Street_name)
                .IsUnicode(false);

            modelBuilder.Entity<Address>()
                .Property(e => e.Department)
                .IsUnicode(false);

            modelBuilder.Entity<Address>()
                .HasMany(e => e.Sale)
                .WithOptional(e => e.Address)
                .HasForeignKey(e => e.Address_id);

            modelBuilder.Entity<Cart>()
                .HasMany(e => e.Line_cart)
                .WithRequired(e => e.Cart)
                .HasForeignKey(e => e.Cart_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Collection>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Collection>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Collection>()
                .Property(e => e.Url_image)
                .IsUnicode(false);

            modelBuilder.Entity<Collection>()
                .Property(e => e.Ref_image)
                .IsUnicode(false);

            modelBuilder.Entity<Collection>()
                .HasMany(e => e.Product)
                .WithOptional(e => e.Collection)
                .HasForeignKey(e => e.Collection_id);

            modelBuilder.Entity<Consultation_product>()
                .Property(e => e.Message)
                .IsUnicode(false);

            modelBuilder.Entity<Consultation_product>()
                .Property(e => e.Response)
                .IsUnicode(false);

            modelBuilder.Entity<Location>()
                .Property(e => e.Postal_code)
                .IsUnicode(false);

            modelBuilder.Entity<Location>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.Address)
                .WithRequired(e => e.Location)
                .HasForeignKey(e => e.Location_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Url_image)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Ref_image)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Consultation_product)
                .WithRequired(e => e.Product)
                .HasForeignKey(e => e.Product_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Favorite)
                .WithRequired(e => e.Product)
                .HasForeignKey(e => e.Product_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Line_cart)
                .WithRequired(e => e.Product)
                .HasForeignKey(e => e.Product_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Sale_product)
                .WithRequired(e => e.Product)
                .HasForeignKey(e => e.Product_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Province>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Province>()
                .HasMany(e => e.Location)
                .WithRequired(e => e.Province)
                .HasForeignKey(e => e.Province_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sale>()
                .Property(e => e.Sale_type)
                .IsUnicode(false);

            modelBuilder.Entity<Sale>()
                .Property(e => e.Sale_status)
                .IsUnicode(false);

            modelBuilder.Entity<Sale>()
                .HasMany(e => e.Sale_product)
                .WithRequired(e => e.Sale)
                .HasForeignKey(e => e.Sale_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Shipment>()
                .HasMany(e => e.Sale)
                .WithOptional(e => e.Shipment)
                .HasForeignKey(e => e.Shipment_id);

            modelBuilder.Entity<User>()
                .Property(e => e.Uid)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Lastname)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Firstname)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Role)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Address)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.User_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Cart)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.User_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Consultation_product)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.User_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Favorite)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.User_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Sale)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.User_id)
                .WillCascadeOnDelete(false);
        }
    }
}
