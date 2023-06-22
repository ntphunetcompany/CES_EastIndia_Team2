using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoutePlanning.Domain.BookingRequest;
using RoutePlanning.Domain.Locations;

namespace RoutePlanning.Infrastructure.Database.BookingRequests;

public sealed class BookingRequestConfiguration : IEntityTypeConfiguration<BookingRequest>
{
    public void Configure(EntityTypeBuilder<BookingRequest> builder)
    {

        builder.HasKey(br => br.Id); // Assuming 'Id' is the primary key

        builder.Property(br => br.Username)
            .IsRequired()
            .HasMaxLength(100); // Restricting 'Username' to a max length of 100 and it is required

        builder.Property(br => br.SourceLocationName)
            .IsRequired()
            .HasMaxLength(200); // 'SourceLocationName' is also required and has a max length of 200

        builder.Property(br => br.DestinationLocationName)
            .IsRequired()
            .HasMaxLength(200); // 'DestinationLocationName' is also required and has a max length of 200

        builder.Property(br => br.Price)
            .IsRequired();
        
        builder.OwnsOne(x => x.Distance);
    }
}

