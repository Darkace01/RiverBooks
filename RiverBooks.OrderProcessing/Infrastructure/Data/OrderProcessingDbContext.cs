﻿using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RiverBooks.OrderProcessing.Domain;

namespace RiverBooks.OrderProcessing.Infrastructure.Data;

public class OrderProcessingDbContext(DbContextOptions<OrderProcessingDbContext> options) : DbContext(options)
{
  internal DbSet<Order> Orders { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.HasDefaultSchema("OrderProcessing");
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

    base.OnModelCreating(modelBuilder);
  }

  protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
  {
    configurationBuilder.Properties<decimal>()
       .HavePrecision(18, 6);
  }
}
