using DocumentLibrary.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseAttendeesCheck.Infrastructure.DataAccess.Mappings
{
    public class DocumentTypeConfiguration : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.FileName)
                .HasMaxLength(200);

            builder.Property(e => e.ContentType)
                .HasMaxLength(100);
        }
    }
}
