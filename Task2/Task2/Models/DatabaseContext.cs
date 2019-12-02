using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Task2.Models
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DatabaseContext() : base()
        {
            if (!Database.Exists())
                Database.Create();

            if (Users.ToList().Count == 0)
            {
                Users.Add(new User()
                {
                    Email = "admin",
                    Password = "admin",
                    Name = "Nikol",
                    SecondName = "Kolpakova",
                    Role = "Admin"
                });

                Products.Add(new Product()
                {
                    Name = "Arbuz",
                    Price = 99.99,
                    StorageCount = 10
                });

                Products.Add(new Product()
                {
                    Name = "kivi",
                    Price = 50.00,
                    StorageCount = 40
                });

                Products.Add(new Product()
                {
                    Name = "mandarin",
                    Price = 30.00,
                    StorageCount = 20
                });

                SaveChanges();
            }
        }
    }
}