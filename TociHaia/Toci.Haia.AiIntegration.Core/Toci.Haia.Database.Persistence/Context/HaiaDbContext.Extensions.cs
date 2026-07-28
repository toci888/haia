using Microsoft.EntityFrameworkCore;
using Toci.Haia.Database.Persistence.Entities;

namespace Toci.Haia.Database.Persistence.Context;

public partial class HaiaDbContext
{
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OnboardingCandidate>(entity =>
        {
            entity.ToTable(tb => tb.HasCheckConstraint(
                "onboarding_candidate_content_format_check",
                "content_format IN ('image/jpeg', 'image/png', 'image/webp')"));
        });
    }
}
