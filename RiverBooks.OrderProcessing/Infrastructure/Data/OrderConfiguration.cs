using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RiverBooks.OrderProcessing.Domain;

namespace RiverBooks.OrderProcessing.Data;

internal class OrderConfiguration : IEntityTypeConfiguration<Order>
{
  void IEntityTypeConfiguration<Order>.Configure(EntityTypeBuilder<Order> builder)
  {
    builder
      .Property(x => x.Id)
      .ValueGeneratedNever();

    builder.ComplexProperty(o => o.ShippingAddress, address =>
    {
      address.Property(a => a.Street1)
      .HasMaxLength(DataSchemaConstants.STREET_MAXLENGTH);
      address.Property(a => a.Street2)
      .HasMaxLength(DataSchemaConstants.STREET_MAXLENGTH);
      address.Property(a => a.City)
      .HasMaxLength(DataSchemaConstants.CITY_MAXLENGTH);
      address.Property(a => a.State)
      .HasMaxLength(DataSchemaConstants.STATE_MAXLENGTH);
      address.Property(a => a.PostalCode)
      .HasMaxLength(DataSchemaConstants.POSTALCODE_MAXLENGTH);
      address.Property(a => a.Country)
      .HasMaxLength(DataSchemaConstants.COUNTRY_MAXLENGTH);
    });

    builder.ComplexProperty(o => o.BillingAddress, address =>
    {
      address.Property(a => a.Street1)
      .HasMaxLength(DataSchemaConstants.STREET_MAXLENGTH);
      address.Property(a => a.Street2)
      .HasMaxLength(DataSchemaConstants.STREET_MAXLENGTH);
      address.Property(a => a.City)
      .HasMaxLength(DataSchemaConstants.CITY_MAXLENGTH);
      address.Property(a => a.State)
      .HasMaxLength(DataSchemaConstants.STATE_MAXLENGTH);
      address.Property(a => a.PostalCode)
      .HasMaxLength(DataSchemaConstants.POSTALCODE_MAXLENGTH);
      address.Property(a => a.Country)
      .HasMaxLength(DataSchemaConstants.COUNTRY_MAXLENGTH);
    });
  }
}
