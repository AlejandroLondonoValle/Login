using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LoginSistem.Models;
using System.Security.Cryptography;

namespace LoginSistem.Data;

public class AppDBContext : DbContext
{

public AppDBContext(DbContextOptions<AppDBContext>options) : base(options)
{
    
}


public DbSet<User> Users { get; set; }

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<User>(tb=>
    {
        tb.HasKey(col => col.IdUser);
        tb.Property(col => col.IdUser)
        .UseMySqlIdentityColumn()
        .ValueGeneratedOnAdd();

        tb.Property(col => col.FullName).HasMaxLength(100);
        tb.Property(col => col.Email).HasMaxLength(100);
        tb.Property(col => col.Password).HasMaxLength(100);
    });

 //No poner esto y solo dejar el DataAnnotation que hay en el modelo de User   
 modelBuilder.Entity<User>().ToTable("User");
}
}
