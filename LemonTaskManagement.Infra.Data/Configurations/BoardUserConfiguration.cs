using LemonTaskManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LemonTaskManagement.Infra.Data.Configurations;

public class BoardUserConfiguration : IEntityTypeConfiguration<BoardUser>
{
    public void Configure(EntityTypeBuilder<BoardUser> builder)
    {
        builder.HasKey(e => new { e.UserId, e.BoardId });

        builder.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Board)
            .WithMany()
            .HasForeignKey(e => e.BoardId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
