using LemonTaskManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LemonTaskManagement.Infra.Data.Configurations;

public class BoardColumnConfiguration : IEntityTypeConfiguration<BoardColumn>
{
    public void Configure(EntityTypeBuilder<BoardColumn> builder)
    {
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(10);
        
        builder.Property(e => e.Order)
            .IsRequired();
        
        builder.HasOne(e => e.Board)
            .WithMany(e => e.Columns)
            .HasForeignKey(e => e.BoardId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(e => e.Cards)
            .WithOne(e => e.BoardColumn)
            .HasForeignKey(e => e.BoardColumnId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(e => new { e.BoardId, e.Order });
    }
}
