using LemonTaskManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LemonTaskManagement.Infra.Data.Configurations;

public class CardConfiguration : IEntityTypeConfiguration<Card>
{
    public void Configure(EntityTypeBuilder<Card> builder)
    {
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Description)
            .IsRequired()
            .HasMaxLength(400);
        
        builder.Property(e => e.Order)
            .IsRequired();
        
        builder.HasOne(e => e.BoardColumn)
            .WithMany(e => e.Cards)
            .HasForeignKey(e => e.BoardColumnId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(e => new { e.BoardColumnId, e.Order });
    }
}
