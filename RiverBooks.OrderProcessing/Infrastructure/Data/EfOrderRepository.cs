﻿using Microsoft.EntityFrameworkCore;
using RiverBooks.OrderProcessing.Domain;
using RiverBooks.OrderProcessing.Interfaces;

namespace RiverBooks.OrderProcessing.Infrastructure.Data;

internal class EfOrderRepository(OrderProcessingDbContext dbContext) : IOrderRepository
{
  public async Task<List<Order>> ListAsync(CancellationToken cancellationToken = default)
  {
    return await dbContext.Orders
      .Include(o => o.OrderItems)
      .ToListAsync(cancellationToken);
  }

  public async Task AddAsync(Order order, CancellationToken cancellationToken = default)
  {
    await dbContext.Orders.AddAsync(order, cancellationToken);
  }

  public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    await dbContext.SaveChangesAsync(cancellationToken);
  }
}
