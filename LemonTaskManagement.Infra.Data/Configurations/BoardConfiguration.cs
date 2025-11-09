using LemonTaskManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LemonTaskManagement.Infra.Data.Configurations;

public class BoardConfiguration : IEntityTypeConfiguration<Board>
{
    public void Configure(EntityTypeBuilder<Board> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Description)
            .HasMaxLength(1000);
        
        builder.HasMany(e => e.Columns)
            .WithOne(e => e.Board)
            .HasForeignKey(e => e.BoardId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(e => e.BoardUsers)
            .WithOne(e => e.Board)
            .HasForeignKey(e => e.BoardId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
