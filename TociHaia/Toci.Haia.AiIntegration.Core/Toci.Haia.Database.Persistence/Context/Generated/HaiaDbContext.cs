using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Toci.Haia.Database.Persistence.Entities;

namespace Toci.Haia.Database.Persistence.Context;

public partial class HaiaDbContext : DbContext
{
    public HaiaDbContext(DbContextOptions<HaiaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AccountAgeContext> AccountAgeContexts { get; set; }

    public virtual DbSet<AccountDocumentAcceptance> AccountDocumentAcceptances { get; set; }

    public virtual DbSet<AccountEmail> AccountEmails { get; set; }

    public virtual DbSet<AccountProfessionContext> AccountProfessionContexts { get; set; }

    public virtual DbSet<AccountRole> AccountRoles { get; set; }

    public virtual DbSet<AccountRoleAssignment> AccountRoleAssignments { get; set; }

    public virtual DbSet<ActivationChange> ActivationChanges { get; set; }

    public virtual DbSet<AgeEligibilityRecord> AgeEligibilityRecords { get; set; }

    public virtual DbSet<AgeRange> AgeRanges { get; set; }

    public virtual DbSet<AiGeneratedOutput> AiGeneratedOutputs { get; set; }

    public virtual DbSet<AiGenerationTarget> AiGenerationTargets { get; set; }

    public virtual DbSet<AiOperationExecution> AiOperationExecutions { get; set; }

    public virtual DbSet<AiOperationUsage> AiOperationUsages { get; set; }

    public virtual DbSet<AiPromptTemplate> AiPromptTemplates { get; set; }

    public virtual DbSet<AiPromptTemplateVersion> AiPromptTemplateVersions { get; set; }

    public virtual DbSet<AuditEvent> AuditEvents { get; set; }

    public virtual DbSet<CandidateActivation> CandidateActivations { get; set; }

    public virtual DbSet<CandidateClassification> CandidateClassifications { get; set; }

    public virtual DbSet<CandidateClassificationMeasure> CandidateClassificationMeasures { get; set; }

    public virtual DbSet<CandidateClassificationSensitivity> CandidateClassificationSensitivities { get; set; }

    public virtual DbSet<CandidateClassificationValue> CandidateClassificationValues { get; set; }

    public virtual DbSet<CandidateFeedback> CandidateFeedbacks { get; set; }

    public virtual DbSet<CandidatePerformanceSnapshot> CandidatePerformanceSnapshots { get; set; }

    public virtual DbSet<CandidatePresentation> CandidatePresentations { get; set; }

    public virtual DbSet<CandidateVersionMedium> CandidateVersionMedia { get; set; }

    public virtual DbSet<ClassificationAxis> ClassificationAxes { get; set; }

    public virtual DbSet<ClassificationMeasureProjectionRule> ClassificationMeasureProjectionRules { get; set; }

    public virtual DbSet<ClassificationModelAxis> ClassificationModelAxes { get; set; }

    public virtual DbSet<ClassificationModelValue> ClassificationModelValues { get; set; }

    public virtual DbSet<ClassificationModelVersion> ClassificationModelVersions { get; set; }

    public virtual DbSet<ClassificationProjectionModelVersion> ClassificationProjectionModelVersions { get; set; }

    public virtual DbSet<ClassificationValue> ClassificationValues { get; set; }

    public virtual DbSet<ClassificationValueProjectionRule> ClassificationValueProjectionRules { get; set; }

    public virtual DbSet<CohortCandidatePerformanceSnapshot> CohortCandidatePerformanceSnapshots { get; set; }

    public virtual DbSet<CohortDefinition> CohortDefinitions { get; set; }

    public virtual DbSet<CohortDefinitionVersion> CohortDefinitionVersions { get; set; }

    public virtual DbSet<DrynessRating> DrynessRatings { get; set; }

    public virtual DbSet<DrynessScale> DrynessScales { get; set; }

    public virtual DbSet<DrynessScaleLevel> DrynessScaleLevels { get; set; }

    public virtual DbSet<DrynessScaleVersion> DrynessScaleVersions { get; set; }

    public virtual DbSet<EditorialReview> EditorialReviews { get; set; }

    public virtual DbSet<ExternalLogin> ExternalLogins { get; set; }

    public virtual DbSet<HumorDimensionDefinition> HumorDimensionDefinitions { get; set; }

    public virtual DbSet<HumorDimensionModelMember> HumorDimensionModelMembers { get; set; }

    public virtual DbSet<HumorDimensionModelVersion> HumorDimensionModelVersions { get; set; }

    public virtual DbSet<HumorEvidence> HumorEvidences { get; set; }

    public virtual DbSet<HumorVerdict> HumorVerdicts { get; set; }

    public virtual DbSet<Industry> Industries { get; set; }

    public virtual DbSet<InitialHumorSnapshot> InitialHumorSnapshots { get; set; }

    public virtual DbSet<InitialHumorSnapshotDimension> InitialHumorSnapshotDimensions { get; set; }

    public virtual DbSet<LearningRecommendation> LearningRecommendations { get; set; }

    public virtual DbSet<LegalDocument> LegalDocuments { get; set; }

    public virtual DbSet<LegalDocumentVersion> LegalDocumentVersions { get; set; }

    public virtual DbSet<MediaAsset> MediaAssets { get; set; }

    public virtual DbSet<MemeCalibrationFeedback> MemeCalibrationFeedbacks { get; set; }

    public virtual DbSet<MemeCalibrationSession> MemeCalibrationSessions { get; set; }

    public virtual DbSet<MemeCalibrationVariant> MemeCalibrationVariants { get; set; }

    public virtual DbSet<ModerationDecision> ModerationDecisions { get; set; }

    public virtual DbSet<ModerationReview> ModerationReviews { get; set; }

    public virtual DbSet<OnboardingCandidate> OnboardingCandidates { get; set; }

    public virtual DbSet<OnboardingCandidateVersion> OnboardingCandidateVersions { get; set; }

    public virtual DbSet<OnboardingFlowVersion> OnboardingFlowVersions { get; set; }

    public virtual DbSet<OnboardingFunnelSnapshot> OnboardingFunnelSnapshots { get; set; }

    public virtual DbSet<OnboardingPriorHypothesis> OnboardingPriorHypotheses { get; set; }

    public virtual DbSet<OnboardingSession> OnboardingSessions { get; set; }

    public virtual DbSet<OnboardingStepState> OnboardingStepStates { get; set; }

    public virtual DbSet<OnboardingWorkingHumorDimension> OnboardingWorkingHumorDimensions { get; set; }

    public virtual DbSet<PasswordCredential> PasswordCredentials { get; set; }

    public virtual DbSet<PolicyDefinition> PolicyDefinitions { get; set; }

    public virtual DbSet<PolicyVersion> PolicyVersions { get; set; }

    public virtual DbSet<PredictionOutcome> PredictionOutcomes { get; set; }

    public virtual DbSet<Profession> Professions { get; set; }

    public virtual DbSet<PublicProfile> PublicProfiles { get; set; }

    public virtual DbSet<Reaction> Reactions { get; set; }

    public virtual DbSet<ReactionChoice> ReactionChoices { get; set; }

    public virtual DbSet<ReactionExposure> ReactionExposures { get; set; }

    public virtual DbSet<ReactionMechanism> ReactionMechanisms { get; set; }

    public virtual DbSet<ReactionPack> ReactionPacks { get; set; }

    public virtual DbSet<ReactionPackItem> ReactionPackItems { get; set; }

    public virtual DbSet<ReactionPackVersion> ReactionPackVersions { get; set; }

    public virtual DbSet<ReactionPerformanceSnapshot> ReactionPerformanceSnapshots { get; set; }

    public virtual DbSet<ReactionVersion> ReactionVersions { get; set; }

    public virtual DbSet<RecommendationDecision> RecommendationDecisions { get; set; }

    public virtual DbSet<RecoveryEvent> RecoveryEvents { get; set; }

    public virtual DbSet<RecoveryPerformanceSnapshot> RecoveryPerformanceSnapshots { get; set; }

    public virtual DbSet<SecondPunchlineFeedback> SecondPunchlineFeedbacks { get; set; }

    public virtual DbSet<SecondPunchlineView> SecondPunchlineViews { get; set; }

    public virtual DbSet<SecurityToken> SecurityTokens { get; set; }

    public virtual DbSet<SelectionDecision> SelectionDecisions { get; set; }

    public virtual DbSet<SelectionDecisionAlternative> SelectionDecisionAlternatives { get; set; }

    public virtual DbSet<SensitivityCategory> SensitivityCategories { get; set; }

    public virtual DbSet<StudioComment> StudioComments { get; set; }

    public virtual DbSet<UserHumorInspiration> UserHumorInspirations { get; set; }

    public virtual DbSet<UserHumorInspirationAnalysis> UserHumorInspirationAnalyses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresExtension("citext")
            .HasPostgresExtension("pgcrypto");

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("account_pkey");

            entity.ToTable("account", "identity", tb => tb.HasComment("Główna tożsamość konta HAIA. Status wielowartościowy zamiast pojedynczego is_active."));

            entity.Property(e => e.AccountId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("account_id");
            entity.Property(e => e.AccountStatus).HasColumnName("account_status");
            entity.Property(e => e.ActivatedAt).HasColumnName("activated_at");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletionReason).HasColumnName("deletion_reason");
            entity.Property(e => e.PendingExpiresAt).HasColumnName("pending_expires_at");
            entity.Property(e => e.RowVersion)
                .HasDefaultValue(1L)
                .HasColumnName("row_version");
            entity.Property(e => e.StatusChangedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("status_changed_at");
        });

        modelBuilder.Entity<AccountAgeContext>(entity =>
        {
            entity.HasKey(e => e.AccountAgeContextId).HasName("account_age_context_pkey");

            entity.ToTable("account_age_context", "onboarding", tb => tb.HasComment("Opcjonalny Age Context Seed do personalizacji; odseparowany od compliance."));

            entity.HasIndex(e => e.AccountId, "ux_account_age_context_one_active")
                .IsUnique()
                .HasFilter("((is_active = true) AND (removed_at IS NULL))");

            entity.Property(e => e.AccountAgeContextId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("account_age_context_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.AgeRangeId).HasColumnName("age_range_id");
            entity.Property(e => e.Confidence)
                .HasPrecision(5, 4)
                .HasDefaultValue(0.5000m)
                .HasColumnName("confidence");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.ProvidedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("provided_at");
            entity.Property(e => e.RemovedAt).HasColumnName("removed_at");
            entity.Property(e => e.Source)
                .HasDefaultValueSql("'user_declared'::text")
                .HasColumnName("source");
            entity.Property(e => e.StartWeight)
                .HasPrecision(5, 4)
                .HasDefaultValue(0.2000m)
                .HasColumnName("start_weight");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Account).WithOne(p => p.AccountAgeContext)
                .HasForeignKey<AccountAgeContext>(d => d.AccountId)
                .HasConstraintName("account_age_context_account_id_fkey");

            entity.HasOne(d => d.AgeRange).WithMany(p => p.AccountAgeContexts)
                .HasForeignKey(d => d.AgeRangeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("account_age_context_age_range_id_fkey");
        });

        modelBuilder.Entity<AccountDocumentAcceptance>(entity =>
        {
            entity.HasKey(e => e.AccountDocumentAcceptanceId).HasName("account_document_acceptance_pkey");

            entity.ToTable("account_document_acceptance", "identity");

            entity.HasIndex(e => new { e.AccountId, e.LegalDocumentVersionId, e.AcceptanceType }, "account_document_acceptance_account_id_legal_document_versi_key").IsUnique();

            entity.Property(e => e.AccountDocumentAcceptanceId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("account_document_acceptance_id");
            entity.Property(e => e.AcceptanceContext)
                .HasColumnType("jsonb")
                .HasColumnName("acceptance_context");
            entity.Property(e => e.AcceptanceType).HasColumnName("acceptance_type");
            entity.Property(e => e.AcceptedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("accepted_at");
            entity.Property(e => e.AcceptedVia).HasColumnName("accepted_via");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.LegalDocumentVersionId).HasColumnName("legal_document_version_id");
            entity.Property(e => e.SourceIp).HasColumnName("source_ip");
            entity.Property(e => e.UserAgent).HasColumnName("user_agent");

            entity.HasOne(d => d.Account).WithMany(p => p.AccountDocumentAcceptances)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("account_document_acceptance_account_id_fkey");

            entity.HasOne(d => d.LegalDocumentVersion).WithMany(p => p.AccountDocumentAcceptances)
                .HasForeignKey(d => d.LegalDocumentVersionId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("account_document_acceptance_legal_document_version_id_fkey");
        });

        modelBuilder.Entity<AccountEmail>(entity =>
        {
            entity.HasKey(e => e.AccountEmailId).HasName("account_email_pkey");

            entity.ToTable("account_email", "identity", tb => tb.HasComment("Adresy e-mail konta. Wspiera zmianę e-mail i case-insensitive uniqueness."));

            entity.HasIndex(e => e.EmailNormalized, "ux_account_email_normalized_active")
                .IsUnique()
                .HasFilter("(released_at IS NULL)");

            entity.HasIndex(e => e.AccountId, "ux_account_email_one_primary")
                .IsUnique()
                .HasFilter("((is_primary = true) AND (released_at IS NULL))");

            entity.Property(e => e.AccountEmailId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("account_email_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.EmailNormalized)
                .HasColumnType("citext")
                .HasColumnName("email_normalized");
            entity.Property(e => e.EmailOriginal).HasColumnName("email_original");
            entity.Property(e => e.IsPrimary).HasColumnName("is_primary");
            entity.Property(e => e.ReleasedAt).HasColumnName("released_at");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
            entity.Property(e => e.VerificationStatus)
                .HasDefaultValueSql("'pending'::text")
                .HasColumnName("verification_status");
            entity.Property(e => e.VerifiedAt).HasColumnName("verified_at");

            entity.HasOne(d => d.Account).WithOne(p => p.AccountEmail)
                .HasForeignKey<AccountEmail>(d => d.AccountId)
                .HasConstraintName("account_email_account_id_fkey");
        });

        modelBuilder.Entity<AccountProfessionContext>(entity =>
        {
            entity.HasKey(e => e.AccountProfessionContextId).HasName("account_profession_context_pkey");

            entity.ToTable("account_profession_context", "onboarding");

            entity.HasIndex(e => e.AccountId, "ix_account_profession_context_account_id");

            entity.HasIndex(e => e.AccountId, "ux_account_profession_context_one_active")
                .IsUnique()
                .HasFilter("((is_active = true) AND (removed_at IS NULL))");

            entity.Property(e => e.AccountProfessionContextId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("account_profession_context_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.Confidence)
                .HasPrecision(5, 4)
                .HasDefaultValue(0.5000m)
                .HasColumnName("confidence");
            entity.Property(e => e.FreeTextContext).HasColumnName("free_text_context");
            entity.Property(e => e.IndustryId).HasColumnName("industry_id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.ProfessionId).HasColumnName("profession_id");
            entity.Property(e => e.ProvidedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("provided_at");
            entity.Property(e => e.RemovedAt).HasColumnName("removed_at");
            entity.Property(e => e.Source)
                .HasDefaultValueSql("'user_declared'::text")
                .HasColumnName("source");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
            entity.Property(e => e.Visibility)
                .HasDefaultValueSql("'private'::text")
                .HasColumnName("visibility");

            entity.HasOne(d => d.Account).WithOne(p => p.AccountProfessionContext)
                .HasForeignKey<AccountProfessionContext>(d => d.AccountId)
                .HasConstraintName("account_profession_context_account_id_fkey");

            entity.HasOne(d => d.Industry).WithMany(p => p.AccountProfessionContexts)
                .HasForeignKey(d => d.IndustryId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("account_profession_context_industry_id_fkey");

            entity.HasOne(d => d.Profession).WithMany(p => p.AccountProfessionContexts)
                .HasForeignKey(d => d.ProfessionId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("account_profession_context_profession_id_fkey");
        });

        modelBuilder.Entity<AccountRole>(entity =>
        {
            entity.HasKey(e => e.AccountRoleId).HasName("account_role_pkey");

            entity.ToTable("account_role", "studio");

            entity.HasIndex(e => e.RoleKey, "account_role_role_key_key").IsUnique();

            entity.Property(e => e.AccountRoleId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("account_role_id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.RoleKey).HasColumnName("role_key");
            entity.Property(e => e.RoleName).HasColumnName("role_name");
            entity.Property(e => e.RoleScope).HasColumnName("role_scope");
        });

        modelBuilder.Entity<AccountRoleAssignment>(entity =>
        {
            entity.HasKey(e => e.AccountRoleAssignmentId).HasName("account_role_assignment_pkey");

            entity.ToTable("account_role_assignment", "studio");

            entity.HasIndex(e => new { e.AccountId, e.AccountRoleId, e.AssignedAt }, "account_role_assignment_account_id_account_role_id_assigned_key").IsUnique();

            entity.HasIndex(e => e.AccountId, "ix_role_assignment_active").HasFilter("(is_active = true)");

            entity.Property(e => e.AccountRoleAssignmentId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("account_role_assignment_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.AccountRoleId).HasColumnName("account_role_id");
            entity.Property(e => e.AssignedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("assigned_at");
            entity.Property(e => e.AssignedByAccountId).HasColumnName("assigned_by_account_id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.RevokedAt).HasColumnName("revoked_at");

            entity.HasOne(d => d.Account).WithMany(p => p.AccountRoleAssignmentAccounts)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("account_role_assignment_account_id_fkey");

            entity.HasOne(d => d.AccountRole).WithMany(p => p.AccountRoleAssignments)
                .HasForeignKey(d => d.AccountRoleId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("account_role_assignment_account_role_id_fkey");

            entity.HasOne(d => d.AssignedByAccount).WithMany(p => p.AccountRoleAssignmentAssignedByAccounts)
                .HasForeignKey(d => d.AssignedByAccountId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("account_role_assignment_assigned_by_account_id_fkey");
        });

        modelBuilder.Entity<ActivationChange>(entity =>
        {
            entity.HasKey(e => e.ActivationChangeId).HasName("activation_change_pkey");

            entity.ToTable("activation_change", "studio");

            entity.Property(e => e.ActivationChangeId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("activation_change_id");
            entity.Property(e => e.ActionType).HasColumnName("action_type");
            entity.Property(e => e.ChangedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("changed_at");
            entity.Property(e => e.ChangedByAccountId).HasColumnName("changed_by_account_id");
            entity.Property(e => e.NewState)
                .HasColumnType("jsonb")
                .HasColumnName("new_state");
            entity.Property(e => e.PreviousState)
                .HasColumnType("jsonb")
                .HasColumnName("previous_state");
            entity.Property(e => e.Reason).HasColumnName("reason");
            entity.Property(e => e.TargetId)
                .HasComment("Typed target pointer; intentionally loose UUID to support activation history across aggregates.")
                .HasColumnName("target_id");
            entity.Property(e => e.TargetType).HasColumnName("target_type");

            entity.HasOne(d => d.ChangedByAccount).WithMany(p => p.ActivationChanges)
                .HasForeignKey(d => d.ChangedByAccountId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("activation_change_changed_by_account_id_fkey");
        });

        modelBuilder.Entity<AgeEligibilityRecord>(entity =>
        {
            entity.HasKey(e => e.AgeEligibilityRecordId).HasName("age_eligibility_record_pkey");

            entity.ToTable("age_eligibility_record", "onboarding", tb => tb.HasComment("Warstwa compliance wieku. Nie służy do modelowania preferencji humoru."));

            entity.Property(e => e.AgeEligibilityRecordId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("age_eligibility_record_id");
            entity.Property(e => e.AccountId)
                .HasComment("Legal retention: ON DELETE RESTRICT, rekord compliance nie może znikać przez CASCADE konta.")
                .HasColumnName("account_id");
            entity.Property(e => e.BirthDate).HasColumnName("birth_date");
            entity.Property(e => e.DeterminationSource).HasColumnName("determination_source");
            entity.Property(e => e.DeterminedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("determined_at");
            entity.Property(e => e.EligibilityStatus).HasColumnName("eligibility_status");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.PolicyVersion).HasColumnName("policy_version");

            entity.HasOne(d => d.Account).WithMany(p => p.AgeEligibilityRecords)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("age_eligibility_record_account_id_fkey");
        });

        modelBuilder.Entity<AgeRange>(entity =>
        {
            entity.HasKey(e => e.AgeRangeId).HasName("age_range_pkey");

            entity.ToTable("age_range", "onboarding");

            entity.HasIndex(e => e.RangeKey, "age_range_range_key_key").IsUnique();

            entity.Property(e => e.AgeRangeId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("age_range_id");
            entity.Property(e => e.DisplayName).HasColumnName("display_name");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.MaxAge).HasColumnName("max_age");
            entity.Property(e => e.MinAge).HasColumnName("min_age");
            entity.Property(e => e.RangeKey).HasColumnName("range_key");
        });

        modelBuilder.Entity<AiGeneratedOutput>(entity =>
        {
            entity.HasKey(e => e.AiGeneratedOutputId).HasName("ai_generated_output_pkey");

            entity.ToTable("ai_generated_output", "ai");

            entity.HasIndex(e => new { e.AiOperationExecutionId, e.CreatedAt }, "ix_ai_generated_output_execution").IsDescending(false, true);

            entity.Property(e => e.AiGeneratedOutputId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("ai_generated_output_id");
            entity.Property(e => e.AiOperationExecutionId).HasColumnName("ai_operation_execution_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.MappedOutput)
                .HasColumnType("jsonb")
                .HasColumnName("mapped_output");
            entity.Property(e => e.OutputType).HasColumnName("output_type");
            entity.Property(e => e.RetentionUntil).HasColumnName("retention_until");
            entity.Property(e => e.StructuredOutput)
                .HasColumnType("jsonb")
                .HasColumnName("structured_output");

            entity.HasOne(d => d.AiOperationExecution).WithMany(p => p.AiGeneratedOutputs)
                .HasForeignKey(d => d.AiOperationExecutionId)
                .HasConstraintName("ai_generated_output_ai_operation_execution_id_fkey");
        });

        modelBuilder.Entity<AiGenerationTarget>(entity =>
        {
            entity.HasKey(e => e.AiGenerationTargetId).HasName("ai_generation_target_pkey");

            entity.ToTable("ai_generation_target", "ai");

            entity.Property(e => e.AiGenerationTargetId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("ai_generation_target_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.OnboardingCandidateVersionId).HasColumnName("onboarding_candidate_version_id");
            entity.Property(e => e.ReactionPackVersionId).HasColumnName("reaction_pack_version_id");
            entity.Property(e => e.TargetId)
                .HasComment("Intentionally polymorphic typed-id. target_type determines concrete aggregate; no generic FK by design.")
                .HasColumnName("target_id");
            entity.Property(e => e.TargetType).HasColumnName("target_type");

            entity.HasOne(d => d.OnboardingCandidateVersion).WithMany(p => p.AiGenerationTargets)
                .HasForeignKey(d => d.OnboardingCandidateVersionId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("ai_generation_target_onboarding_candidate_version_id_fkey");

            entity.HasOne(d => d.ReactionPackVersion).WithMany(p => p.AiGenerationTargets)
                .HasForeignKey(d => d.ReactionPackVersionId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("ai_generation_target_reaction_pack_version_id_fkey");
        });

        modelBuilder.Entity<AiOperationExecution>(entity =>
        {
            entity.HasKey(e => e.AiOperationExecutionId).HasName("ai_operation_execution_pkey");

            entity.ToTable("ai_operation_execution", "ai");

            entity.HasIndex(e => e.CorrelationId, "ix_ai_operation_execution_correlation");

            entity.HasIndex(e => new { e.OperationName, e.StartedAt }, "ix_ai_operation_execution_operation_time").IsDescending(false, true);

            entity.Property(e => e.AiOperationExecutionId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("ai_operation_execution_id");
            entity.Property(e => e.AiGenerationTargetId).HasColumnName("ai_generation_target_id");
            entity.Property(e => e.AiPromptTemplateVersionId).HasColumnName("ai_prompt_template_version_id");
            entity.Property(e => e.CorrelationId).HasColumnName("correlation_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DurationMs).HasColumnName("duration_ms");
            entity.Property(e => e.ErrorCode).HasColumnName("error_code");
            entity.Property(e => e.ExecutionStatus).HasColumnName("execution_status");
            entity.Property(e => e.FinishedAt).HasColumnName("finished_at");
            entity.Property(e => e.ImageSha256).HasColumnName("image_sha256");
            entity.Property(e => e.Model).HasColumnName("model");
            entity.Property(e => e.OperationName).HasColumnName("operation_name");
            entity.Property(e => e.Provider).HasColumnName("provider");
            entity.Property(e => e.ProviderResponseId).HasColumnName("provider_response_id");
            entity.Property(e => e.RepairCount).HasColumnName("repair_count");
            entity.Property(e => e.RetryCount).HasColumnName("retry_count");
            entity.Property(e => e.SourceMaterialRef).HasColumnName("source_material_ref");
            entity.Property(e => e.StartedAt).HasColumnName("started_at");

            entity.HasOne(d => d.AiGenerationTarget).WithMany(p => p.AiOperationExecutions)
                .HasForeignKey(d => d.AiGenerationTargetId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("ai_operation_execution_ai_generation_target_id_fkey");

            entity.HasOne(d => d.AiPromptTemplateVersion).WithMany(p => p.AiOperationExecutions)
                .HasForeignKey(d => d.AiPromptTemplateVersionId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("ai_operation_execution_ai_prompt_template_version_id_fkey");
        });

        modelBuilder.Entity<AiOperationUsage>(entity =>
        {
            entity.HasKey(e => e.AiOperationUsageId).HasName("ai_operation_usage_pkey");

            entity.ToTable("ai_operation_usage", "ai");

            entity.HasIndex(e => e.AiOperationExecutionId, "ai_operation_usage_ai_operation_execution_id_key").IsUnique();

            entity.Property(e => e.AiOperationUsageId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("ai_operation_usage_id");
            entity.Property(e => e.AiOperationExecutionId).HasColumnName("ai_operation_execution_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.InputTokens).HasColumnName("input_tokens");
            entity.Property(e => e.OutputTokens).HasColumnName("output_tokens");
            entity.Property(e => e.TotalTokens).HasColumnName("total_tokens");

            entity.HasOne(d => d.AiOperationExecution).WithOne(p => p.AiOperationUsage)
                .HasForeignKey<AiOperationUsage>(d => d.AiOperationExecutionId)
                .HasConstraintName("ai_operation_usage_ai_operation_execution_id_fkey");
        });

        modelBuilder.Entity<AiPromptTemplate>(entity =>
        {
            entity.HasKey(e => e.AiPromptTemplateId).HasName("ai_prompt_template_pkey");

            entity.ToTable("ai_prompt_template", "ai");

            entity.HasIndex(e => e.TemplateKey, "ai_prompt_template_template_key_key").IsUnique();

            entity.Property(e => e.AiPromptTemplateId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("ai_prompt_template_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.TemplateKey).HasColumnName("template_key");
        });

        modelBuilder.Entity<AiPromptTemplateVersion>(entity =>
        {
            entity.HasKey(e => e.AiPromptTemplateVersionId).HasName("ai_prompt_template_version_pkey");

            entity.ToTable("ai_prompt_template_version", "ai");

            entity.HasIndex(e => new { e.AiPromptTemplateId, e.VersionLabel }, "ai_prompt_template_version_ai_prompt_template_id_version_la_key").IsUnique();

            entity.HasIndex(e => e.AiPromptTemplateId, "ux_ai_prompt_template_one_active")
                .IsUnique()
                .HasFilter("(is_active = true)");

            entity.Property(e => e.AiPromptTemplateVersionId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("ai_prompt_template_version_id");
            entity.Property(e => e.AiPromptTemplateId).HasColumnName("ai_prompt_template_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedByAccountId).HasColumnName("created_by_account_id");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.PromptBody).HasColumnName("prompt_body");
            entity.Property(e => e.VersionLabel).HasColumnName("version_label");

            entity.HasOne(d => d.AiPromptTemplate).WithOne(p => p.AiPromptTemplateVersion)
                .HasForeignKey<AiPromptTemplateVersion>(d => d.AiPromptTemplateId)
                .HasConstraintName("ai_prompt_template_version_ai_prompt_template_id_fkey");

            entity.HasOne(d => d.CreatedByAccount).WithMany(p => p.AiPromptTemplateVersions)
                .HasForeignKey(d => d.CreatedByAccountId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("ai_prompt_template_version_created_by_account_id_fkey");
        });

        modelBuilder.Entity<AuditEvent>(entity =>
        {
            entity.HasKey(e => e.AuditEventId).HasName("audit_event_pkey");

            entity.ToTable("audit_event", "audit", tb => tb.HasComment("Append-oriented audit table. UPDATE/DELETE blocked by trigger. INSERT-only role grants are deployment/infrastructure concern."));

            entity.HasIndex(e => new { e.ActorAccountId, e.CreatedAt }, "ix_audit_event_actor").IsDescending(false, true);

            entity.HasIndex(e => new { e.TargetType, e.TargetId, e.CreatedAt }, "ix_audit_event_target").IsDescending(false, false, true);

            entity.Property(e => e.AuditEventId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("audit_event_id");
            entity.Property(e => e.ActionKey).HasColumnName("action_key");
            entity.Property(e => e.ActorAccountId).HasColumnName("actor_account_id");
            entity.Property(e => e.ActorRoleKey).HasColumnName("actor_role_key");
            entity.Property(e => e.CorrelationId).HasColumnName("correlation_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Metadata)
                .HasColumnType("jsonb")
                .HasColumnName("metadata");
            entity.Property(e => e.NewState)
                .HasColumnType("jsonb")
                .HasColumnName("new_state");
            entity.Property(e => e.PreviousState)
                .HasColumnType("jsonb")
                .HasColumnName("previous_state");
            entity.Property(e => e.Reason).HasColumnName("reason");
            entity.Property(e => e.SourceApplication).HasColumnName("source_application");
            entity.Property(e => e.TargetId)
                .HasComment("Audit target pointer; intentionally polymorphic and resolved via target_type.")
                .HasColumnName("target_id");
            entity.Property(e => e.TargetType).HasColumnName("target_type");

            entity.HasOne(d => d.ActorAccount).WithMany(p => p.AuditEvents)
                .HasForeignKey(d => d.ActorAccountId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("audit_event_actor_account_id_fkey");
        });

        modelBuilder.Entity<CandidateActivation>(entity =>
        {
            entity.HasKey(e => e.CandidateActivationId).HasName("candidate_activation_pkey");

            entity.ToTable("candidate_activation", "onboarding");

            entity.HasIndex(e => new { e.OnboardingCandidateVersionId, e.StartsAt }, "ix_candidate_activation_active").HasFilter("((is_enabled = true) AND (kill_switch = false))");

            entity.Property(e => e.CandidateActivationId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("candidate_activation_id");
            entity.Property(e => e.ActivationScope)
                .HasDefaultValueSql("'global'::text")
                .HasColumnName("activation_scope");
            entity.Property(e => e.CohortDefinitionVersionId).HasColumnName("cohort_definition_version_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedByAccountId).HasColumnName("created_by_account_id");
            entity.Property(e => e.EndsAt).HasColumnName("ends_at");
            entity.Property(e => e.ExperimentKey).HasColumnName("experiment_key");
            entity.Property(e => e.IsEnabled)
                .HasDefaultValue(true)
                .HasColumnName("is_enabled");
            entity.Property(e => e.KillSwitch).HasColumnName("kill_switch");
            entity.Property(e => e.MaxExposures).HasColumnName("max_exposures");
            entity.Property(e => e.MaxFrequencyPerUser).HasColumnName("max_frequency_per_user");
            entity.Property(e => e.OnboardingCandidateVersionId).HasColumnName("onboarding_candidate_version_id");
            entity.Property(e => e.StartsAt).HasColumnName("starts_at");

            entity.HasOne(d => d.CohortDefinitionVersion).WithMany(p => p.CandidateActivations)
                .HasForeignKey(d => d.CohortDefinitionVersionId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_candidate_activation_cohort");

            entity.HasOne(d => d.CreatedByAccount).WithMany(p => p.CandidateActivations)
                .HasForeignKey(d => d.CreatedByAccountId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("candidate_activation_created_by_account_id_fkey");

            entity.HasOne(d => d.OnboardingCandidateVersion).WithMany(p => p.CandidateActivations)
                .HasForeignKey(d => d.OnboardingCandidateVersionId)
                .HasConstraintName("candidate_activation_onboarding_candidate_version_id_fkey");
        });

        modelBuilder.Entity<CandidateClassification>(entity =>
        {
            entity.HasKey(e => e.CandidateClassificationId).HasName("candidate_classification_pkey");

            entity.ToTable("candidate_classification", "onboarding");

            entity.HasIndex(e => e.AiOperationExecutionId, "ix_candidate_classification_ai_execution").HasFilter("(ai_operation_execution_id IS NOT NULL)");

            entity.HasIndex(e => new { e.OnboardingCandidateVersionId, e.ClassificationStatus, e.RevisionNo }, "ix_candidate_classification_candidate_status").IsDescending(false, false, true);

            entity.HasIndex(e => new { e.ClassificationModelVersionId, e.ClassificationStatus, e.CreatedAt }, "ix_candidate_classification_model_status").IsDescending(false, false, true);

            entity.HasIndex(e => new { e.OnboardingCandidateVersionId, e.RevisionNo }, "ux_candidate_classification_candidate_revision").IsUnique();

            entity.HasIndex(e => new { e.CandidateClassificationId, e.ClassificationModelVersionId }, "ux_candidate_classification_id_model").IsUnique();

            entity.HasIndex(e => e.OnboardingCandidateVersionId, "ux_candidate_classification_one_published_per_candidate_version")
                .IsUnique()
                .HasFilter("(classification_status = 'published'::text)");

            entity.Property(e => e.CandidateClassificationId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("candidate_classification_id");
            entity.Property(e => e.AdditionalTags)
                .HasComment("Pozostaje dla rzadkich/eksperymentalnych metadanych (nie kanoniczna taksonomia).")
                .HasColumnType("jsonb")
                .HasColumnName("additional_tags");
            entity.Property(e => e.AgeHintStrength)
                .HasPrecision(6, 5)
                .HasComment("LEGACY: hint kontekstowy, nie jest observed evidence i nie powinien potwierdzać Humor DNA.")
                .HasColumnName("age_hint_strength");
            entity.Property(e => e.AiOperationExecutionId).HasColumnName("ai_operation_execution_id");
            entity.Property(e => e.ClassificationModelVersionId).HasColumnName("classification_model_version_id");
            entity.Property(e => e.ClassificationStatus).HasColumnName("classification_status");
            entity.Property(e => e.Complexity)
                .HasComment("LEGACY: zastępowane przez onboarding.candidate_classification_measure (axis=complexity).")
                .HasColumnName("complexity");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.CreatedByAccountId).HasColumnName("created_by_account_id");
            entity.Property(e => e.Dryness)
                .HasComment("LEGACY/DEPRECATED: zastępowane przez predicted_dryness_scale_level_id + predicted_dryness_confidence.")
                .HasColumnName("dryness");
            entity.Property(e => e.EditorialNote).HasColumnName("editorial_note");
            entity.Property(e => e.FormatKey).HasColumnName("format_key");
            entity.Property(e => e.Intensity)
                .HasComment("LEGACY: zastępowane przez onboarding.candidate_classification_measure (axis=intensity).")
                .HasColumnName("intensity");
            entity.Property(e => e.OnboardingCandidateVersionId).HasColumnName("onboarding_candidate_version_id");
            entity.Property(e => e.OverallConfidence)
                .HasPrecision(6, 5)
                .HasColumnName("overall_confidence");
            entity.Property(e => e.PredictedDrynessConfidence)
                .HasPrecision(6, 5)
                .HasColumnName("predicted_dryness_confidence");
            entity.Property(e => e.PredictedDrynessScaleLevelId)
                .HasComment("Predykcja poziomu Sucharka dla materiału (nie mylić z realną oceną usera w humor.dryness_rating).")
                .HasColumnName("predicted_dryness_scale_level_id");
            entity.Property(e => e.ProfessionHintStrength)
                .HasPrecision(6, 5)
                .HasComment("LEGACY: hint kontekstowy, nie jest observed evidence i nie powinien potwierdzać Humor DNA.")
                .HasColumnName("profession_hint_strength");
            entity.Property(e => e.PublishedAt).HasColumnName("published_at");
            entity.Property(e => e.ReactionRescuePotential)
                .HasPrecision(6, 5)
                .HasComment("LEGACY: zastępowane przez onboarding.candidate_classification_measure (axis=rescue_potential).")
                .HasColumnName("reaction_rescue_potential");
            entity.Property(e => e.ReviewedAt).HasColumnName("reviewed_at");
            entity.Property(e => e.ReviewedByAccountId).HasColumnName("reviewed_by_account_id");
            entity.Property(e => e.RevisionNo).HasColumnName("revision_no");
            entity.Property(e => e.SafetyFlags)
                .HasComment("LEGACY/raw AI payload. Kanoniczny safety model jest relacyjny w candidate_classification_sensitivity.")
                .HasColumnType("jsonb")
                .HasColumnName("safety_flags");
            entity.Property(e => e.SourceType).HasColumnName("source_type");
            entity.Property(e => e.SupersededAt).HasColumnName("superseded_at");
            entity.Property(e => e.SupersedesCandidateClassificationId).HasColumnName("supersedes_candidate_classification_id");
            entity.Property(e => e.UniversalityScore)
                .HasPrecision(6, 5)
                .HasComment("LEGACY: zastępowane przez onboarding.candidate_classification_measure (axis=universality).")
                .HasColumnName("universality_score");

            entity.HasOne(d => d.AiOperationExecution).WithMany(p => p.CandidateClassifications)
                .HasForeignKey(d => d.AiOperationExecutionId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_candidate_classification_ai_execution");

            entity.HasOne(d => d.ClassificationModelVersion).WithMany(p => p.CandidateClassifications)
                .HasForeignKey(d => d.ClassificationModelVersionId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_candidate_classification_model_version");

            entity.HasOne(d => d.CreatedByAccount).WithMany(p => p.CandidateClassificationCreatedByAccounts)
                .HasForeignKey(d => d.CreatedByAccountId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_candidate_classification_created_by");

            entity.HasOne(d => d.OnboardingCandidateVersion).WithOne(p => p.CandidateClassification)
                .HasForeignKey<CandidateClassification>(d => d.OnboardingCandidateVersionId)
                .HasConstraintName("candidate_classification_onboarding_candidate_version_id_fkey");

            entity.HasOne(d => d.PredictedDrynessScaleLevel).WithMany(p => p.CandidateClassifications)
                .HasForeignKey(d => d.PredictedDrynessScaleLevelId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_candidate_classification_predicted_dryness");

            entity.HasOne(d => d.ReviewedByAccount).WithMany(p => p.CandidateClassificationReviewedByAccounts)
                .HasForeignKey(d => d.ReviewedByAccountId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_candidate_classification_reviewed_by");

            entity.HasOne(d => d.SupersedesCandidateClassification).WithMany(p => p.InverseSupersedesCandidateClassification)
                .HasForeignKey(d => d.SupersedesCandidateClassificationId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_candidate_classification_supersedes");
        });

        modelBuilder.Entity<CandidateClassificationMeasure>(entity =>
        {
            entity.HasKey(e => e.CandidateClassificationMeasureId).HasName("candidate_classification_measure_pkey");

            entity.ToTable("candidate_classification_measure", "onboarding");

            entity.HasIndex(e => new { e.ClassificationModelAxisId, e.NormalizedValue }, "ix_candidate_classification_measure_axis_value");

            entity.HasIndex(e => new { e.CandidateClassificationId, e.ClassificationModelAxisId }, "ix_candidate_classification_measure_classification");

            entity.HasIndex(e => new { e.CandidateClassificationId, e.ClassificationModelAxisId }, "uq_candidate_classification_measure_axis").IsUnique();

            entity.Property(e => e.CandidateClassificationMeasureId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("candidate_classification_measure_id");
            entity.Property(e => e.CandidateClassificationId).HasColumnName("candidate_classification_id");
            entity.Property(e => e.ClassificationModelAxisId).HasColumnName("classification_model_axis_id");
            entity.Property(e => e.ClassificationModelVersionId).HasColumnName("classification_model_version_id");
            entity.Property(e => e.Confidence)
                .HasPrecision(6, 5)
                .HasColumnName("confidence");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.EditorialNote).HasColumnName("editorial_note");
            entity.Property(e => e.MeasurementSource).HasColumnName("measurement_source");
            entity.Property(e => e.NormalizedValue)
                .HasPrecision(6, 5)
                .HasColumnName("normalized_value");

            entity.HasOne(d => d.CandidateClassification).WithMany(p => p.CandidateClassificationMeasures)
                .HasPrincipalKey(p => new { p.CandidateClassificationId, p.ClassificationModelVersionId })
                .HasForeignKey(d => new { d.CandidateClassificationId, d.ClassificationModelVersionId })
                .HasConstraintName("fk_ccm_parent_classification_model");

            entity.HasOne(d => d.ClassificationModelAxis).WithMany(p => p.CandidateClassificationMeasures)
                .HasPrincipalKey(p => new { p.ClassificationModelAxisId, p.ClassificationModelVersionId })
                .HasForeignKey(d => new { d.ClassificationModelAxisId, d.ClassificationModelVersionId })
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_ccm_model_axis");
        });

        modelBuilder.Entity<CandidateClassificationSensitivity>(entity =>
        {
            entity.HasKey(e => e.CandidateClassificationSensitivityId).HasName("candidate_classification_sensitivity_pkey");

            entity.ToTable("candidate_classification_sensitivity", "onboarding");

            entity.HasIndex(e => new { e.CandidateClassificationId, e.SeverityLevel }, "ix_candidate_classification_sensitivity_classification").IsDescending(false, true);

            entity.HasIndex(e => new { e.SeverityLevel, e.ModerationRelevance }, "ix_candidate_classification_sensitivity_moderation")
                .IsDescending(true, false)
                .HasFilter("(moderation_relevance = true)");

            entity.HasIndex(e => new { e.CandidateClassificationId, e.SensitivityCategoryId }, "uq_candidate_classification_sensitivity").IsUnique();

            entity.Property(e => e.CandidateClassificationSensitivityId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("candidate_classification_sensitivity_id");
            entity.Property(e => e.AssignmentSource).HasColumnName("assignment_source");
            entity.Property(e => e.CandidateClassificationId).HasColumnName("candidate_classification_id");
            entity.Property(e => e.Confidence)
                .HasPrecision(6, 5)
                .HasColumnName("confidence");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.EditorialNote).HasColumnName("editorial_note");
            entity.Property(e => e.ModerationRelevance)
                .HasDefaultValue(true)
                .HasColumnName("moderation_relevance");
            entity.Property(e => e.SensitivityCategoryId).HasColumnName("sensitivity_category_id");
            entity.Property(e => e.SeverityLevel).HasColumnName("severity_level");

            entity.HasOne(d => d.CandidateClassification).WithMany(p => p.CandidateClassificationSensitivities)
                .HasForeignKey(d => d.CandidateClassificationId)
                .HasConstraintName("candidate_classification_sensi_candidate_classification_id_fkey");

            entity.HasOne(d => d.SensitivityCategory).WithMany(p => p.CandidateClassificationSensitivities)
                .HasForeignKey(d => d.SensitivityCategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("candidate_classification_sensitivi_sensitivity_category_id_fkey");
        });

        modelBuilder.Entity<CandidateClassificationValue>(entity =>
        {
            entity.HasKey(e => e.CandidateClassificationValueId).HasName("candidate_classification_value_pkey");

            entity.ToTable("candidate_classification_value", "onboarding");

            entity.HasIndex(e => new { e.CandidateClassificationId, e.ClassificationModelAxisId, e.RelevanceScore }, "ix_candidate_classification_value_classification").IsDescending(false, false, true);

            entity.HasIndex(e => new { e.ClassificationModelValueId, e.RelevanceScore, e.Confidence }, "ix_candidate_classification_value_model_value").IsDescending(false, true, true);

            entity.HasIndex(e => new { e.CandidateClassificationId, e.ClassificationModelValueId }, "uq_candidate_classification_value_value").IsUnique();

            entity.HasIndex(e => new { e.CandidateClassificationId, e.ClassificationModelAxisId }, "ux_candidate_classification_value_primary_per_axis")
                .IsUnique()
                .HasFilter("(is_primary = true)");

            entity.Property(e => e.CandidateClassificationValueId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("candidate_classification_value_id");
            entity.Property(e => e.AssignmentSource).HasColumnName("assignment_source");
            entity.Property(e => e.CandidateClassificationId).HasColumnName("candidate_classification_id");
            entity.Property(e => e.ClassificationModelAxisId).HasColumnName("classification_model_axis_id");
            entity.Property(e => e.ClassificationModelValueId).HasColumnName("classification_model_value_id");
            entity.Property(e => e.ClassificationModelVersionId).HasColumnName("classification_model_version_id");
            entity.Property(e => e.Confidence)
                .HasPrecision(6, 5)
                .HasColumnName("confidence");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.EditorialNote).HasColumnName("editorial_note");
            entity.Property(e => e.IsPrimary).HasColumnName("is_primary");
            entity.Property(e => e.RankNo).HasColumnName("rank_no");
            entity.Property(e => e.RelevanceScore)
                .HasPrecision(6, 5)
                .HasColumnName("relevance_score");

            entity.HasOne(d => d.CandidateClassification).WithMany(p => p.CandidateClassificationValues)
                .HasPrincipalKey(p => new { p.CandidateClassificationId, p.ClassificationModelVersionId })
                .HasForeignKey(d => new { d.CandidateClassificationId, d.ClassificationModelVersionId })
                .HasConstraintName("fk_ccv_parent_classification_model");

            entity.HasOne(d => d.ClassificationModelAxis).WithMany(p => p.CandidateClassificationValues)
                .HasPrincipalKey(p => new { p.ClassificationModelAxisId, p.ClassificationModelVersionId })
                .HasForeignKey(d => new { d.ClassificationModelAxisId, d.ClassificationModelVersionId })
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_ccv_model_axis");

            entity.HasOne(d => d.ClassificationModelValue).WithMany(p => p.CandidateClassificationValues)
                .HasPrincipalKey(p => new { p.ClassificationModelValueId, p.ClassificationModelVersionId, p.ClassificationModelAxisId })
                .HasForeignKey(d => new { d.ClassificationModelValueId, d.ClassificationModelVersionId, d.ClassificationModelAxisId })
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_ccv_model_value");
        });

        modelBuilder.Entity<CandidateFeedback>(entity =>
        {
            entity.HasKey(e => e.CandidateFeedbackId).HasName("candidate_feedback_pkey");

            entity.ToTable("candidate_feedback", "onboarding");

            entity.HasIndex(e => new { e.AccountId, e.SubmittedAt }, "ix_candidate_feedback_account_time").IsDescending(false, true);

            entity.HasIndex(e => new { e.CandidatePresentationId, e.SubmittedAt }, "ix_candidate_feedback_presentation").IsDescending(false, true);

            entity.HasIndex(e => e.CandidatePresentationId, "ux_candidate_feedback_current")
                .IsUnique()
                .HasFilter("(is_current = true)");

            entity.Property(e => e.CandidateFeedbackId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("candidate_feedback_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.CandidatePresentationId).HasColumnName("candidate_presentation_id");
            entity.Property(e => e.DryFlag).HasColumnName("dry_flag");
            entity.Property(e => e.ExplicitTextFeedback).HasColumnName("explicit_text_feedback");
            entity.Property(e => e.FeedbackReason).HasColumnName("feedback_reason");
            entity.Property(e => e.FeedbackRevisionNo)
                .HasDefaultValue(1)
                .HasColumnName("feedback_revision_no");
            entity.Property(e => e.IsCurrent)
                .HasDefaultValue(true)
                .HasColumnName("is_current");
            entity.Property(e => e.LaughScore).HasColumnName("laugh_score");
            entity.Property(e => e.NotMyStyleFlag).HasColumnName("not_my_style_flag");
            entity.Property(e => e.PredictableFlag).HasColumnName("predictable_flag");
            entity.Property(e => e.RatingValue).HasColumnName("rating_value");
            entity.Property(e => e.SkipFlag).HasColumnName("skip_flag");
            entity.Property(e => e.SubmittedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("submitted_at");
            entity.Property(e => e.TooIntenseFlag).HasColumnName("too_intense_flag");
            entity.Property(e => e.TooLongFlag).HasColumnName("too_long_flag");
            entity.Property(e => e.TooObviousFlag).HasColumnName("too_obvious_flag");

            entity.HasOne(d => d.Account).WithMany(p => p.CandidateFeedbacks)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("candidate_feedback_account_id_fkey");

            entity.HasOne(d => d.CandidatePresentation).WithOne(p => p.CandidateFeedback)
                .HasForeignKey<CandidateFeedback>(d => d.CandidatePresentationId)
                .HasConstraintName("candidate_feedback_candidate_presentation_id_fkey");
        });

        modelBuilder.Entity<CandidatePerformanceSnapshot>(entity =>
        {
            entity.HasKey(e => e.CandidatePerformanceSnapshotId).HasName("candidate_performance_snapshot_pkey");

            entity.ToTable("candidate_performance_snapshot", "learning");

            entity.HasIndex(e => new { e.OnboardingCandidateVersionId, e.PeriodStart }, "ix_candidate_perf_snapshot_candidate_time").IsDescending(false, true);

            entity.Property(e => e.CandidatePerformanceSnapshotId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("candidate_performance_snapshot_id");
            entity.Property(e => e.AbandonmentEffect)
                .HasPrecision(6, 5)
                .HasColumnName("abandonment_effect");
            entity.Property(e => e.AvgRating)
                .HasPrecision(6, 5)
                .HasColumnName("avg_rating");
            entity.Property(e => e.CompletionEffect)
                .HasPrecision(6, 5)
                .HasColumnName("completion_effect");
            entity.Property(e => e.Confidence)
                .HasPrecision(6, 5)
                .HasColumnName("confidence");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Exposures).HasColumnName("exposures");
            entity.Property(e => e.MedianRating)
                .HasPrecision(6, 5)
                .HasColumnName("median_rating");
            entity.Property(e => e.NotMyStyleRate)
                .HasPrecision(6, 5)
                .HasColumnName("not_my_style_rate");
            entity.Property(e => e.OnboardingCandidateVersionId).HasColumnName("onboarding_candidate_version_id");
            entity.Property(e => e.PeriodEnd).HasColumnName("period_end");
            entity.Property(e => e.PeriodStart).HasColumnName("period_start");
            entity.Property(e => e.PredictedVsActualError)
                .HasPrecision(6, 5)
                .HasColumnName("predicted_vs_actual_error");
            entity.Property(e => e.SampleSize).HasColumnName("sample_size");
            entity.Property(e => e.SkipRate)
                .HasPrecision(6, 5)
                .HasColumnName("skip_rate");
            entity.Property(e => e.UniqueUsers).HasColumnName("unique_users");

            entity.HasOne(d => d.OnboardingCandidateVersion).WithMany(p => p.CandidatePerformanceSnapshots)
                .HasForeignKey(d => d.OnboardingCandidateVersionId)
                .HasConstraintName("candidate_performance_snapsho_onboarding_candidate_version_fkey");
        });

        modelBuilder.Entity<CandidatePresentation>(entity =>
        {
            entity.HasKey(e => e.CandidatePresentationId).HasName("candidate_presentation_pkey");

            entity.ToTable("candidate_presentation", "onboarding");

            entity.HasIndex(e => new { e.OnboardingSessionId, e.PositionNo }, "candidate_presentation_onboarding_session_id_position_no_key").IsUnique();

            entity.HasIndex(e => e.OnboardingCandidateVersionId, "ix_candidate_presentation_candidate");

            entity.HasIndex(e => new { e.OnboardingSessionId, e.DisplayedAt }, "ix_candidate_presentation_session_time");

            entity.Property(e => e.CandidatePresentationId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("candidate_presentation_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.DisplayedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("displayed_at");
            entity.Property(e => e.FinishedAt).HasColumnName("finished_at");
            entity.Property(e => e.IsRecoveryPresentation).HasColumnName("is_recovery_presentation");
            entity.Property(e => e.OnboardingCandidateVersionId).HasColumnName("onboarding_candidate_version_id");
            entity.Property(e => e.OnboardingSessionId).HasColumnName("onboarding_session_id");
            entity.Property(e => e.PositionNo).HasColumnName("position_no");
            entity.Property(e => e.PredictedEnjoyment)
                .HasPrecision(6, 5)
                .HasColumnName("predicted_enjoyment");
            entity.Property(e => e.PredictedInformationGain)
                .HasPrecision(6, 5)
                .HasColumnName("predicted_information_gain");
            entity.Property(e => e.PriorSources)
                .HasColumnType("jsonb")
                .HasColumnName("prior_sources");
            entity.Property(e => e.RankingMode).HasColumnName("ranking_mode");
            entity.Property(e => e.ReactionPackVersionId).HasColumnName("reaction_pack_version_id");
            entity.Property(e => e.ScoringPolicyVersionId).HasColumnName("scoring_policy_version_id");
            entity.Property(e => e.SelectedByReason).HasColumnName("selected_by_reason");
            entity.Property(e => e.SelectionDecisionId).HasColumnName("selection_decision_id");

            entity.HasOne(d => d.Account).WithMany(p => p.CandidatePresentations)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("candidate_presentation_account_id_fkey");

            entity.HasOne(d => d.OnboardingCandidateVersion).WithMany(p => p.CandidatePresentations)
                .HasForeignKey(d => d.OnboardingCandidateVersionId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("candidate_presentation_onboarding_candidate_version_id_fkey");

            entity.HasOne(d => d.OnboardingSession).WithMany(p => p.CandidatePresentations)
                .HasForeignKey(d => d.OnboardingSessionId)
                .HasConstraintName("candidate_presentation_onboarding_session_id_fkey");

            entity.HasOne(d => d.ReactionPackVersion).WithMany(p => p.CandidatePresentations)
                .HasForeignKey(d => d.ReactionPackVersionId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("candidate_presentation_reaction_pack_version_id_fkey");

            entity.HasOne(d => d.ScoringPolicyVersion).WithMany(p => p.CandidatePresentations)
                .HasForeignKey(d => d.ScoringPolicyVersionId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_candidate_presentation_scoring_policy");

            entity.HasOne(d => d.SelectionDecision).WithMany(p => p.CandidatePresentations)
                .HasForeignKey(d => d.SelectionDecisionId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("candidate_presentation_selection_decision_id_fkey");
        });

        modelBuilder.Entity<CandidateVersionMedium>(entity =>
        {
            entity.HasKey(e => e.CandidateVersionMediaId).HasName("candidate_version_media_pkey");

            entity.ToTable("candidate_version_media", "onboarding");

            entity.HasIndex(e => new { e.OnboardingCandidateVersionId, e.MediaRole, e.DisplayOrder }, "candidate_version_media_onboarding_candidate_version_id_me_key1").IsUnique();

            entity.HasIndex(e => new { e.OnboardingCandidateVersionId, e.MediaAssetId, e.MediaRole }, "candidate_version_media_onboarding_candidate_version_id_med_key").IsUnique();

            entity.Property(e => e.CandidateVersionMediaId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("candidate_version_media_id");
            entity.Property(e => e.CaptionOverride).HasColumnName("caption_override");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DisplayOrder)
                .HasDefaultValue(1)
                .HasColumnName("display_order");
            entity.Property(e => e.MediaAssetId).HasColumnName("media_asset_id");
            entity.Property(e => e.MediaRole)
                .HasDefaultValueSql("'primary'::text")
                .HasColumnName("media_role");
            entity.Property(e => e.OnboardingCandidateVersionId).HasColumnName("onboarding_candidate_version_id");
            entity.Property(e => e.PresentationMetadata)
                .HasColumnType("jsonb")
                .HasColumnName("presentation_metadata");

            entity.HasOne(d => d.MediaAsset).WithMany(p => p.CandidateVersionMedia)
                .HasForeignKey(d => d.MediaAssetId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("candidate_version_media_media_asset_id_fkey");

            entity.HasOne(d => d.OnboardingCandidateVersion).WithMany(p => p.CandidateVersionMedia)
                .HasForeignKey(d => d.OnboardingCandidateVersionId)
                .HasConstraintName("candidate_version_media_onboarding_candidate_version_id_fkey");
        });

        modelBuilder.Entity<ClassificationAxis>(entity =>
        {
            entity.HasKey(e => e.ClassificationAxisId).HasName("classification_axis_pkey");

            entity.ToTable("classification_axis", "humor", tb => tb.HasComment("Słownik osi klasyfikacji humoru dla materiału (meme/joke)."));

            entity.HasIndex(e => e.AxisKey, "ix_classification_axis_key_active").HasFilter("(is_active = true)");

            entity.HasIndex(e => e.AxisKey, "uq_classification_axis_axis_key").IsUnique();

            entity.Property(e => e.ClassificationAxisId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("classification_axis_id");
            entity.Property(e => e.AxisKey)
                .HasComment("Stabilny klucz techniczny osi (EN), używany w integracjach i seedach.")
                .HasColumnName("axis_key");
            entity.Property(e => e.AxisRole)
                .HasComment("Rola osi w modelu: affinity/context/routing/gating/analytics.")
                .HasColumnName("axis_role");
            entity.Property(e => e.AxisType)
                .HasComment("Typ sygnału osi: categorical/ordinal/scalar.")
                .HasColumnName("axis_type");
            entity.Property(e => e.Cardinality)
                .HasComment("Dopuszczalna liczba przypisań: single lub multi.")
                .HasColumnName("cardinality");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DisplayName)
                .HasComment("Nazwa prezentacyjna osi (PL).")
                .HasColumnName("display_name");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.SortOrder)
                .HasDefaultValue(100)
                .HasColumnName("sort_order");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<ClassificationMeasureProjectionRule>(entity =>
        {
            entity.HasKey(e => e.ClassificationMeasureProjectionRuleId).HasName("classification_measure_projection_rule_pkey");

            entity.ToTable("classification_measure_projection_rule", "humor");

            entity.HasIndex(e => new { e.ClassificationProjectionModelVersionId, e.IsEnabled }, "ix_classification_measure_projection_enabled").HasFilter("(is_enabled = true)");

            entity.HasIndex(e => new { e.ClassificationModelAxisId, e.IsEnabled }, "ix_projection_rule_measure_lookup").HasFilter("(is_enabled = true)");

            entity.HasIndex(e => new { e.ClassificationProjectionModelVersionId, e.ClassificationModelAxisId, e.HumorDimensionModelMemberId }, "uq_classification_measure_projection_rule").IsUnique();

            entity.Property(e => e.ClassificationMeasureProjectionRuleId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("classification_measure_projection_rule_id");
            entity.Property(e => e.CenterValue)
                .HasPrecision(6, 5)
                .HasColumnName("center_value");
            entity.Property(e => e.ClassificationModelAxisId).HasColumnName("classification_model_axis_id");
            entity.Property(e => e.ClassificationModelVersionId).HasColumnName("classification_model_version_id");
            entity.Property(e => e.ClassificationProjectionModelVersionId).HasColumnName("classification_projection_model_version_id");
            entity.Property(e => e.Confidence)
                .HasPrecision(6, 5)
                .HasColumnName("confidence");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.EffectWeight)
                .HasPrecision(7, 6)
                .HasColumnName("effect_weight");
            entity.Property(e => e.HumorDimensionModelMemberId).HasColumnName("humor_dimension_model_member_id");
            entity.Property(e => e.HumorDimensionModelVersionId).HasColumnName("humor_dimension_model_version_id");
            entity.Property(e => e.IsEnabled)
                .HasDefaultValue(true)
                .HasColumnName("is_enabled");
            entity.Property(e => e.MappingMode).HasColumnName("mapping_mode");

            entity.HasOne(d => d.ClassificationModelAxis).WithMany(p => p.ClassificationMeasureProjectionRules)
                .HasPrincipalKey(p => new { p.ClassificationModelAxisId, p.ClassificationModelVersionId })
                .HasForeignKey(d => new { d.ClassificationModelAxisId, d.ClassificationModelVersionId })
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_classification_measure_projection_model_axis");

            entity.HasOne(d => d.HumorDimensionModelMember).WithMany(p => p.ClassificationMeasureProjectionRules)
                .HasPrincipalKey(p => new { p.HumorDimensionModelMemberId, p.HumorDimensionModelVersionId })
                .HasForeignKey(d => new { d.HumorDimensionModelMemberId, d.HumorDimensionModelVersionId })
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_classification_measure_projection_dimension_member");

            entity.HasOne(d => d.ClassificationProjectionModelVersion).WithMany(p => p.ClassificationMeasureProjectionRules)
                .HasPrincipalKey(p => new { p.ClassificationProjectionModelVersionId, p.ClassificationModelVersionId, p.HumorDimensionModelVersionId })
                .HasForeignKey(d => new { d.ClassificationProjectionModelVersionId, d.ClassificationModelVersionId, d.HumorDimensionModelVersionId })
                .HasConstraintName("fk_classification_measure_projection_model_triplet");
        });

        modelBuilder.Entity<ClassificationModelAxis>(entity =>
        {
            entity.HasKey(e => e.ClassificationModelAxisId).HasName("classification_model_axis_pkey");

            entity.ToTable("classification_model_axis", "humor", tb => tb.HasComment("Członkostwo osi w konkretnej wersji modelu wraz z wagą osi."));

            entity.HasIndex(e => new { e.ClassificationModelVersionId, e.MemberStatus, e.ClassificationAxisId }, "ix_classification_model_axis_model");

            entity.HasIndex(e => new { e.ClassificationModelVersionId, e.ClassificationAxisId }, "uq_classification_model_axis").IsUnique();

            entity.HasIndex(e => new { e.ClassificationModelAxisId, e.ClassificationModelVersionId }, "uq_classification_model_axis_id_model").IsUnique();

            entity.HasIndex(e => new { e.ClassificationModelAxisId, e.ClassificationModelVersionId, e.ClassificationAxisId }, "uq_classification_model_axis_triplet").IsUnique();

            entity.Property(e => e.ClassificationModelAxisId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("classification_model_axis_id");
            entity.Property(e => e.AxisWeight)
                .HasPrecision(8, 6)
                .HasDefaultValue(1.0m)
                .HasComment("Waga osi na poziomie wersji modelu (globalna ważność sygnału).")
                .HasColumnName("axis_weight");
            entity.Property(e => e.ClassificationAxisId).HasColumnName("classification_axis_id");
            entity.Property(e => e.ClassificationModelVersionId).HasColumnName("classification_model_version_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.IsRequired).HasColumnName("is_required");
            entity.Property(e => e.MaximumAssignments).HasColumnName("maximum_assignments");
            entity.Property(e => e.MemberStatus)
                .HasDefaultValueSql("'active'::text")
                .HasColumnName("member_status");
            entity.Property(e => e.MinimumAssignments).HasColumnName("minimum_assignments");
            entity.Property(e => e.NormalizationMethod)
                .HasDefaultValueSql("'none'::text")
                .HasComment("Metoda normalizacji agregacji przypisań osi.")
                .HasColumnName("normalization_method");

            entity.HasOne(d => d.ClassificationAxis).WithMany(p => p.ClassificationModelAxes)
                .HasForeignKey(d => d.ClassificationAxisId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("classification_model_axis_classification_axis_id_fkey");

            entity.HasOne(d => d.ClassificationModelVersion).WithMany(p => p.ClassificationModelAxes)
                .HasForeignKey(d => d.ClassificationModelVersionId)
                .HasConstraintName("classification_model_axis_classification_model_version_id_fkey");
        });

        modelBuilder.Entity<ClassificationModelValue>(entity =>
        {
            entity.HasKey(e => e.ClassificationModelValueId).HasName("classification_model_value_pkey");

            entity.ToTable("classification_model_value", "humor", tb => tb.HasComment("Aktywacja wartości słownikowej w wersji modelu wraz z wagą domyślną wartości."));

            entity.HasIndex(e => new { e.ClassificationModelValueId, e.ClassificationModelVersionId }, "ix_classification_model_value_lookup");

            entity.HasIndex(e => new { e.ClassificationModelVersionId, e.ClassificationModelAxisId }, "ix_classification_model_value_model_axis").HasFilter("(is_enabled = true)");

            entity.HasIndex(e => new { e.ClassificationModelValueId, e.ClassificationModelVersionId }, "uq_classification_model_value_id_model").IsUnique();

            entity.HasIndex(e => new { e.ClassificationModelValueId, e.ClassificationModelVersionId, e.ClassificationModelAxisId }, "uq_classification_model_value_id_model_axis").IsUnique();

            entity.HasIndex(e => new { e.ClassificationModelVersionId, e.ClassificationValueId }, "uq_classification_model_value_model_value").IsUnique();

            entity.Property(e => e.ClassificationModelValueId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("classification_model_value_id");
            entity.Property(e => e.ClassificationAxisId).HasColumnName("classification_axis_id");
            entity.Property(e => e.ClassificationModelAxisId).HasColumnName("classification_model_axis_id");
            entity.Property(e => e.ClassificationModelVersionId).HasColumnName("classification_model_version_id");
            entity.Property(e => e.ClassificationValueId).HasColumnName("classification_value_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DefaultWeight)
                .HasPrecision(8, 6)
                .HasDefaultValue(1.0m)
                .HasComment("Domyślna waga wartości (odrębna od wagi osi, relevance i confidence).")
                .HasColumnName("default_weight");
            entity.Property(e => e.IsEnabled)
                .HasDefaultValue(true)
                .HasColumnName("is_enabled");

            entity.HasOne(d => d.ClassificationValue).WithMany(p => p.ClassificationModelValues)
                .HasPrincipalKey(p => new { p.ClassificationValueId, p.ClassificationAxisId })
                .HasForeignKey(d => new { d.ClassificationValueId, d.ClassificationAxisId })
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_classification_model_value_axis_value");

            entity.HasOne(d => d.ClassificationModelAxis).WithMany(p => p.ClassificationModelValues)
                .HasPrincipalKey(p => new { p.ClassificationModelAxisId, p.ClassificationModelVersionId, p.ClassificationAxisId })
                .HasForeignKey(d => new { d.ClassificationModelAxisId, d.ClassificationModelVersionId, d.ClassificationAxisId })
                .HasConstraintName("fk_classification_model_value_model_axis_triplet");
        });

        modelBuilder.Entity<ClassificationModelVersion>(entity =>
        {
            entity.HasKey(e => e.ClassificationModelVersionId).HasName("classification_model_version_pkey");

            entity.ToTable("classification_model_version", "humor", tb => tb.HasComment("Wersjonowany model klasyfikacji humoru materiału."));

            entity.HasIndex(e => new { e.ModelKey, e.Status, e.VersionNo }, "ix_classification_model_key_status").IsDescending(false, false, true);

            entity.HasIndex(e => new { e.ModelKey, e.VersionNo }, "uq_classification_model_key_version").IsUnique();

            entity.HasIndex(e => e.ModelKey, "ux_classification_model_one_active_per_key")
                .IsUnique()
                .HasFilter("(status = 'active'::text)");

            entity.Property(e => e.ClassificationModelVersionId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("classification_model_version_id");
            entity.Property(e => e.ActivatedAt).HasColumnName("activated_at");
            entity.Property(e => e.Config)
                .HasDefaultValueSql("'{}'::jsonb")
                .HasComment("Konfiguracja niestabilna/eksperymentalna modelu (JSONB).")
                .HasColumnType("jsonb")
                .HasColumnName("config");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedByAccountId).HasColumnName("created_by_account_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.ModelKey).HasColumnName("model_key");
            entity.Property(e => e.RetiredAt).HasColumnName("retired_at");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.VersionLabel).HasColumnName("version_label");
            entity.Property(e => e.VersionNo).HasColumnName("version_no");

            entity.HasOne(d => d.CreatedByAccount).WithMany(p => p.ClassificationModelVersions)
                .HasForeignKey(d => d.CreatedByAccountId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("classification_model_version_created_by_account_id_fkey");
        });

        modelBuilder.Entity<ClassificationProjectionModelVersion>(entity =>
        {
            entity.HasKey(e => e.ClassificationProjectionModelVersionId).HasName("classification_projection_model_version_pkey");

            entity.ToTable("classification_projection_model_version", "humor");

            entity.HasIndex(e => new { e.ProjectionModelKey, e.Status, e.VersionNo }, "ix_classification_projection_lookup").IsDescending(false, false, true);

            entity.HasIndex(e => new { e.ProjectionModelKey, e.VersionNo }, "uq_classification_projection_model_key_version").IsUnique();

            entity.HasIndex(e => new { e.ClassificationProjectionModelVersionId, e.ClassificationModelVersionId, e.HumorDimensionModelVersionId }, "uq_classification_projection_model_version_triplet").IsUnique();

            entity.HasIndex(e => e.ProjectionModelKey, "ux_classification_projection_one_active_per_key")
                .IsUnique()
                .HasFilter("(status = 'active'::text)");

            entity.Property(e => e.ClassificationProjectionModelVersionId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("classification_projection_model_version_id");
            entity.Property(e => e.ActivatedAt).HasColumnName("activated_at");
            entity.Property(e => e.ClassificationModelVersionId).HasColumnName("classification_model_version_id");
            entity.Property(e => e.Config)
                .HasDefaultValueSql("'{}'::jsonb")
                .HasColumnType("jsonb")
                .HasColumnName("config");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.HumorDimensionModelVersionId).HasColumnName("humor_dimension_model_version_id");
            entity.Property(e => e.ProjectionModelKey).HasColumnName("projection_model_key");
            entity.Property(e => e.RetiredAt).HasColumnName("retired_at");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.VersionNo).HasColumnName("version_no");

            entity.HasOne(d => d.ClassificationModelVersion).WithMany(p => p.ClassificationProjectionModelVersions)
                .HasForeignKey(d => d.ClassificationModelVersionId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("classification_projection_mod_classification_model_version_fkey");

            entity.HasOne(d => d.HumorDimensionModelVersion).WithMany(p => p.ClassificationProjectionModelVersions)
                .HasForeignKey(d => d.HumorDimensionModelVersionId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("classification_projection_mod_humor_dimension_model_versio_fkey");
        });

        modelBuilder.Entity<ClassificationValue>(entity =>
        {
            entity.HasKey(e => e.ClassificationValueId).HasName("classification_value_pkey");

            entity.ToTable("classification_value", "humor", tb => tb.HasComment("Słownik wartości dla osi klasyfikacji humoru."));

            entity.HasIndex(e => new { e.ClassificationAxisId, e.SortOrder, e.ValueKey }, "ix_classification_value_axis_sort").HasFilter("(is_active = true)");

            entity.HasIndex(e => e.ValueKey, "ix_classification_value_key");

            entity.HasIndex(e => new { e.ClassificationAxisId, e.ValueKey }, "uq_classification_value_axis_value_key").IsUnique();

            entity.HasIndex(e => new { e.ClassificationValueId, e.ClassificationAxisId }, "uq_classification_value_id_axis").IsUnique();

            entity.Property(e => e.ClassificationValueId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("classification_value_id");
            entity.Property(e => e.AiDescription)
                .HasComment("Krótka wskazówka semantyczna dla pipeline AI.")
                .HasColumnName("ai_description");
            entity.Property(e => e.ClassificationAxisId).HasColumnName("classification_axis_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DisplayName).HasColumnName("display_name");
            entity.Property(e => e.EditorialGuidance)
                .HasComment("Krótka wskazówka redakcyjna do review klasyfikacji.")
                .HasColumnName("editorial_guidance");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.ParentClassificationValueId)
                .HasComment("Opcjonalna hierarchia wartości (parent-child).")
                .HasColumnName("parent_classification_value_id");
            entity.Property(e => e.SortOrder)
                .HasDefaultValue(100)
                .HasColumnName("sort_order");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
            entity.Property(e => e.ValueKey)
                .HasComment("Stabilny klucz techniczny wartości (EN).")
                .HasColumnName("value_key");

            entity.HasOne(d => d.ClassificationAxis).WithMany(p => p.ClassificationValues)
                .HasForeignKey(d => d.ClassificationAxisId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("classification_value_classification_axis_id_fkey");

            entity.HasOne(d => d.ClassificationValueNavigation).WithMany(p => p.InverseClassificationValueNavigation)
                .HasPrincipalKey(p => new { p.ClassificationValueId, p.ClassificationAxisId })
                .HasForeignKey(d => new { d.ParentClassificationValueId, d.ClassificationAxisId })
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_classification_value_parent_same_axis");
        });

        modelBuilder.Entity<ClassificationValueProjectionRule>(entity =>
        {
            entity.HasKey(e => e.ClassificationValueProjectionRuleId).HasName("classification_value_projection_rule_pkey");

            entity.ToTable("classification_value_projection_rule", "humor");

            entity.HasIndex(e => new { e.ClassificationProjectionModelVersionId, e.IsEnabled }, "ix_classification_value_projection_enabled").HasFilter("(is_enabled = true)");

            entity.HasIndex(e => new { e.ClassificationModelValueId, e.IsEnabled }, "ix_projection_rule_value_lookup").HasFilter("(is_enabled = true)");

            entity.HasIndex(e => new { e.ClassificationProjectionModelVersionId, e.ClassificationModelValueId, e.HumorDimensionModelMemberId }, "uq_classification_value_projection_rule").IsUnique();

            entity.Property(e => e.ClassificationValueProjectionRuleId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("classification_value_projection_rule_id");
            entity.Property(e => e.ClassificationModelValueId).HasColumnName("classification_model_value_id");
            entity.Property(e => e.ClassificationModelVersionId).HasColumnName("classification_model_version_id");
            entity.Property(e => e.ClassificationProjectionModelVersionId).HasColumnName("classification_projection_model_version_id");
            entity.Property(e => e.Confidence)
                .HasPrecision(6, 5)
                .HasColumnName("confidence");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.EffectWeight)
                .HasPrecision(7, 6)
                .HasColumnName("effect_weight");
            entity.Property(e => e.HumorDimensionModelMemberId).HasColumnName("humor_dimension_model_member_id");
            entity.Property(e => e.HumorDimensionModelVersionId).HasColumnName("humor_dimension_model_version_id");
            entity.Property(e => e.IsEnabled)
                .HasDefaultValue(true)
                .HasColumnName("is_enabled");

            entity.HasOne(d => d.ClassificationModelValue).WithMany(p => p.ClassificationValueProjectionRules)
                .HasPrincipalKey(p => new { p.ClassificationModelValueId, p.ClassificationModelVersionId })
                .HasForeignKey(d => new { d.ClassificationModelValueId, d.ClassificationModelVersionId })
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_classification_value_projection_model_value");

            entity.HasOne(d => d.HumorDimensionModelMember).WithMany(p => p.ClassificationValueProjectionRules)
                .HasPrincipalKey(p => new { p.HumorDimensionModelMemberId, p.HumorDimensionModelVersionId })
                .HasForeignKey(d => new { d.HumorDimensionModelMemberId, d.HumorDimensionModelVersionId })
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_classification_value_projection_dimension_member");

            entity.HasOne(d => d.ClassificationProjectionModelVersion).WithMany(p => p.ClassificationValueProjectionRules)
                .HasPrincipalKey(p => new { p.ClassificationProjectionModelVersionId, p.ClassificationModelVersionId, p.HumorDimensionModelVersionId })
                .HasForeignKey(d => new { d.ClassificationProjectionModelVersionId, d.ClassificationModelVersionId, d.HumorDimensionModelVersionId })
                .HasConstraintName("fk_classification_value_projection_model_triplet");
        });

        modelBuilder.Entity<CohortCandidatePerformanceSnapshot>(entity =>
        {
            entity.HasKey(e => e.CohortCandidatePerformanceSnapshotId).HasName("cohort_candidate_performance_snapshot_pkey");

            entity.ToTable("cohort_candidate_performance_snapshot", "learning");

            entity.Property(e => e.CohortCandidatePerformanceSnapshotId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("cohort_candidate_performance_snapshot_id");
            entity.Property(e => e.AvgRating)
                .HasPrecision(6, 5)
                .HasColumnName("avg_rating");
            entity.Property(e => e.CohortDefinitionVersionId).HasColumnName("cohort_definition_version_id");
            entity.Property(e => e.CompletionRate)
                .HasPrecision(6, 5)
                .HasColumnName("completion_rate");
            entity.Property(e => e.Confidence)
                .HasPrecision(6, 5)
                .HasColumnName("confidence");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Exposures).HasColumnName("exposures");
            entity.Property(e => e.OnboardingCandidateVersionId).HasColumnName("onboarding_candidate_version_id");
            entity.Property(e => e.PeriodEnd).HasColumnName("period_end");
            entity.Property(e => e.PeriodStart).HasColumnName("period_start");
            entity.Property(e => e.SampleSize).HasColumnName("sample_size");
            entity.Property(e => e.SkipRate)
                .HasPrecision(6, 5)
                .HasColumnName("skip_rate");

            entity.HasOne(d => d.CohortDefinitionVersion).WithMany(p => p.CohortCandidatePerformanceSnapshots)
                .HasForeignKey(d => d.CohortDefinitionVersionId)
                .HasConstraintName("cohort_candidate_performance__cohort_definition_version_id_fkey");

            entity.HasOne(d => d.OnboardingCandidateVersion).WithMany(p => p.CohortCandidatePerformanceSnapshots)
                .HasForeignKey(d => d.OnboardingCandidateVersionId)
                .HasConstraintName("cohort_candidate_performance__onboarding_candidate_version_fkey");
        });

        modelBuilder.Entity<CohortDefinition>(entity =>
        {
            entity.HasKey(e => e.CohortDefinitionId).HasName("cohort_definition_pkey");

            entity.ToTable("cohort_definition", "learning");

            entity.HasIndex(e => e.CohortKey, "cohort_definition_cohort_key_key").IsUnique();

            entity.Property(e => e.CohortDefinitionId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("cohort_definition_id");
            entity.Property(e => e.CohortKey).HasColumnName("cohort_key");
            entity.Property(e => e.CohortType).HasColumnName("cohort_type");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
        });

        modelBuilder.Entity<CohortDefinitionVersion>(entity =>
        {
            entity.HasKey(e => e.CohortDefinitionVersionId).HasName("cohort_definition_version_pkey");

            entity.ToTable("cohort_definition_version", "learning");

            entity.HasIndex(e => new { e.CohortDefinitionId, e.VersionLabel }, "cohort_definition_version_cohort_definition_id_version_labe_key").IsUnique();

            entity.Property(e => e.CohortDefinitionVersionId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("cohort_definition_version_id");
            entity.Property(e => e.ActiveFrom).HasColumnName("active_from");
            entity.Property(e => e.ActiveTo).HasColumnName("active_to");
            entity.Property(e => e.CohortDefinitionId).HasColumnName("cohort_definition_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DefinitionRules)
                .HasColumnType("jsonb")
                .HasColumnName("definition_rules");
            entity.Property(e => e.MinimumSampleSize).HasColumnName("minimum_sample_size");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'draft'::text")
                .HasColumnName("status");
            entity.Property(e => e.VersionLabel).HasColumnName("version_label");

            entity.HasOne(d => d.CohortDefinition).WithMany(p => p.CohortDefinitionVersions)
                .HasForeignKey(d => d.CohortDefinitionId)
                .HasConstraintName("cohort_definition_version_cohort_definition_id_fkey");
        });

        modelBuilder.Entity<DrynessRating>(entity =>
        {
            entity.HasKey(e => e.DrynessRatingId).HasName("dryness_rating_pkey");

            entity.ToTable("dryness_rating", "humor");

            entity.HasIndex(e => new { e.CandidatePresentationId, e.AccountId, e.IdempotencyKey }, "dryness_rating_candidate_presentation_id_account_id_idempot_key").IsUnique();

            entity.HasIndex(e => new { e.CandidatePresentationId, e.SelectedAt }, "ix_dryness_rating_presentation_time");

            entity.HasIndex(e => new { e.CandidatePresentationId, e.AccountId }, "ux_dryness_rating_active")
                .IsUnique()
                .HasFilter("(unselected_at IS NULL)");

            entity.Property(e => e.DrynessRatingId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("dryness_rating_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.CandidatePresentationId).HasColumnName("candidate_presentation_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DrynessScaleLevelId).HasColumnName("dryness_scale_level_id");
            entity.Property(e => e.IdempotencyKey).HasColumnName("idempotency_key");
            entity.Property(e => e.SelectedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("selected_at");
            entity.Property(e => e.UnselectedAt).HasColumnName("unselected_at");

            entity.HasOne(d => d.Account).WithMany(p => p.DrynessRatings)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("dryness_rating_account_id_fkey");

            entity.HasOne(d => d.CandidatePresentation).WithMany(p => p.DrynessRatings)
                .HasForeignKey(d => d.CandidatePresentationId)
                .HasConstraintName("dryness_rating_candidate_presentation_id_fkey");

            entity.HasOne(d => d.DrynessScaleLevel).WithMany(p => p.DrynessRatings)
                .HasForeignKey(d => d.DrynessScaleLevelId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("dryness_rating_dryness_scale_level_id_fkey");
        });

        modelBuilder.Entity<DrynessScale>(entity =>
        {
            entity.HasKey(e => e.DrynessScaleId).HasName("dryness_scale_pkey");

            entity.ToTable("dryness_scale", "humor");

            entity.HasIndex(e => e.ScaleKey, "dryness_scale_scale_key_key").IsUnique();

            entity.Property(e => e.DrynessScaleId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("dryness_scale_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.ScaleKey).HasColumnName("scale_key");
        });

        modelBuilder.Entity<DrynessScaleLevel>(entity =>
        {
            entity.HasKey(e => e.DrynessScaleLevelId).HasName("dryness_scale_level_pkey");

            entity.ToTable("dryness_scale_level", "humor");

            entity.HasIndex(e => new { e.DrynessScaleVersionId, e.LevelNo }, "dryness_scale_level_dryness_scale_version_id_level_no_key").IsUnique();

            entity.Property(e => e.DrynessScaleLevelId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("dryness_scale_level_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DrynessScaleVersionId).HasColumnName("dryness_scale_version_id");
            entity.Property(e => e.Label).HasColumnName("label");
            entity.Property(e => e.LevelNo).HasColumnName("level_no");

            entity.HasOne(d => d.DrynessScaleVersion).WithMany(p => p.DrynessScaleLevels)
                .HasForeignKey(d => d.DrynessScaleVersionId)
                .HasConstraintName("dryness_scale_level_dryness_scale_version_id_fkey");
        });

        modelBuilder.Entity<DrynessScaleVersion>(entity =>
        {
            entity.HasKey(e => e.DrynessScaleVersionId).HasName("dryness_scale_version_pkey");

            entity.ToTable("dryness_scale_version", "humor");

            entity.HasIndex(e => new { e.DrynessScaleId, e.VersionNo }, "dryness_scale_version_dryness_scale_id_version_no_key").IsUnique();

            entity.Property(e => e.DrynessScaleVersionId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("dryness_scale_version_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DrynessScaleId).HasColumnName("dryness_scale_id");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'active'::text")
                .HasColumnName("status");
            entity.Property(e => e.VersionNo).HasColumnName("version_no");

            entity.HasOne(d => d.DrynessScale).WithMany(p => p.DrynessScaleVersions)
                .HasForeignKey(d => d.DrynessScaleId)
                .HasConstraintName("dryness_scale_version_dryness_scale_id_fkey");
        });

        modelBuilder.Entity<EditorialReview>(entity =>
        {
            entity.HasKey(e => e.EditorialReviewId).HasName("editorial_review_pkey");

            entity.ToTable("editorial_review", "studio");

            entity.Property(e => e.EditorialReviewId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("editorial_review_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.ReviewStatus).HasColumnName("review_status");
            entity.Property(e => e.ReviewerAccountId).HasColumnName("reviewer_account_id");
            entity.Property(e => e.Summary).HasColumnName("summary");
            entity.Property(e => e.TargetId)
                .HasComment("Typed target pointer; enforced by target_type and application workflow, no single relational FK available.")
                .HasColumnName("target_id");
            entity.Property(e => e.TargetType).HasColumnName("target_type");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.ReviewerAccount).WithMany(p => p.EditorialReviews)
                .HasForeignKey(d => d.ReviewerAccountId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("editorial_review_reviewer_account_id_fkey");
        });

        modelBuilder.Entity<ExternalLogin>(entity =>
        {
            entity.HasKey(e => e.ExternalLoginId).HasName("external_login_pkey");

            entity.ToTable("external_login", "identity", tb => tb.HasComment("Powiązanie konta z providerami social login. Brak tokenów dostępowych."));

            entity.HasIndex(e => e.AccountId, "ix_external_login_account_id");

            entity.HasIndex(e => new { e.Provider, e.ProviderSubject }, "ux_external_login_provider_subject").IsUnique();

            entity.Property(e => e.ExternalLoginId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("external_login_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.LastUsedAt).HasColumnName("last_used_at");
            entity.Property(e => e.LinkedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("linked_at");
            entity.Property(e => e.Metadata)
                .HasColumnType("jsonb")
                .HasColumnName("metadata");
            entity.Property(e => e.Provider).HasColumnName("provider");
            entity.Property(e => e.ProviderEmailHint)
                .HasColumnType("citext")
                .HasColumnName("provider_email_hint");
            entity.Property(e => e.ProviderEmailVerified).HasColumnName("provider_email_verified");
            entity.Property(e => e.ProviderSubject).HasColumnName("provider_subject");

            entity.HasOne(d => d.Account).WithMany(p => p.ExternalLogins)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("external_login_account_id_fkey");
        });

        modelBuilder.Entity<HumorDimensionDefinition>(entity =>
        {
            entity.HasKey(e => e.HumorDimensionDefinitionId).HasName("humor_dimension_definition_pkey");

            entity.ToTable("humor_dimension_definition", "humor");

            entity.HasIndex(e => e.DimensionKey, "humor_dimension_definition_dimension_key_key").IsUnique();

            entity.Property(e => e.HumorDimensionDefinitionId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("humor_dimension_definition_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DimensionGroup).HasColumnName("dimension_group");
            entity.Property(e => e.DimensionKey).HasColumnName("dimension_key");
            entity.Property(e => e.DisplayName).HasColumnName("display_name");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
        });

        modelBuilder.Entity<HumorDimensionModelMember>(entity =>
        {
            entity.HasKey(e => e.HumorDimensionModelMemberId).HasName("humor_dimension_model_member_pkey");

            entity.ToTable("humor_dimension_model_member", "humor");

            entity.HasIndex(e => new { e.HumorDimensionModelMemberId, e.HumorDimensionModelVersionId }, "humor_dimension_model_member_humor_dimension_model_member_i_key").IsUnique();

            entity.HasIndex(e => new { e.HumorDimensionModelVersionId, e.HumorDimensionDefinitionId }, "humor_dimension_model_member_humor_dimension_model_version__key").IsUnique();

            entity.Property(e => e.HumorDimensionModelMemberId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("humor_dimension_model_member_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.HumorDimensionDefinitionId).HasColumnName("humor_dimension_definition_id");
            entity.Property(e => e.HumorDimensionModelVersionId).HasColumnName("humor_dimension_model_version_id");
            entity.Property(e => e.MaxSupportedValue)
                .HasPrecision(6, 5)
                .HasDefaultValue(1m)
                .HasColumnName("max_supported_value");
            entity.Property(e => e.MemberStatus)
                .HasDefaultValueSql("'active'::text")
                .HasColumnName("member_status");
            entity.Property(e => e.MinSupportedValue)
                .HasPrecision(6, 5)
                .HasColumnName("min_supported_value");
            entity.Property(e => e.Weight)
                .HasPrecision(8, 6)
                .HasDefaultValue(1.0m)
                .HasColumnName("weight");

            entity.HasOne(d => d.HumorDimensionDefinition).WithMany(p => p.HumorDimensionModelMembers)
                .HasForeignKey(d => d.HumorDimensionDefinitionId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("humor_dimension_model_member_humor_dimension_definition_id_fkey");

            entity.HasOne(d => d.HumorDimensionModelVersion).WithMany(p => p.HumorDimensionModelMembers)
                .HasForeignKey(d => d.HumorDimensionModelVersionId)
                .HasConstraintName("humor_dimension_model_member_humor_dimension_model_version_fkey");
        });

        modelBuilder.Entity<HumorDimensionModelVersion>(entity =>
        {
            entity.HasKey(e => e.HumorDimensionModelVersionId).HasName("humor_dimension_model_version_pkey");

            entity.ToTable("humor_dimension_model_version", "humor");

            entity.HasIndex(e => new { e.ModelKey, e.VersionLabel }, "humor_dimension_model_version_model_key_version_label_key").IsUnique();

            entity.Property(e => e.HumorDimensionModelVersionId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("humor_dimension_model_version_id");
            entity.Property(e => e.Config)
                .HasDefaultValueSql("'{}'::jsonb")
                .HasColumnType("jsonb")
                .HasColumnName("config");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.ModelKey).HasColumnName("model_key");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'draft'::text")
                .HasColumnName("status");
            entity.Property(e => e.VersionLabel).HasColumnName("version_label");
        });

        modelBuilder.Entity<HumorEvidence>(entity =>
        {
            entity.HasKey(e => e.HumorEvidenceId).HasName("humor_evidence_pkey");

            entity.ToTable("humor_evidence", "humor");

            entity.HasIndex(e => new { e.OnboardingSessionId, e.CreatedAt }, "ix_humor_evidence_session");

            entity.Property(e => e.HumorEvidenceId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("humor_evidence_id");
            entity.Property(e => e.CandidateFeedbackId).HasColumnName("candidate_feedback_id");
            entity.Property(e => e.CandidatePresentationId).HasColumnName("candidate_presentation_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DrynessRatingId).HasColumnName("dryness_rating_id");
            entity.Property(e => e.EvidenceDirection).HasColumnName("evidence_direction");
            entity.Property(e => e.EvidenceType).HasColumnName("evidence_type");
            entity.Property(e => e.EvidenceWeight)
                .HasPrecision(6, 5)
                .HasColumnName("evidence_weight");
            entity.Property(e => e.Explanation).HasColumnName("explanation");
            entity.Property(e => e.HumorDimensionModelMemberId).HasColumnName("humor_dimension_model_member_id");
            entity.Property(e => e.HumorDimensionModelVersionId).HasColumnName("humor_dimension_model_version_id");
            entity.Property(e => e.OnboardingSessionId).HasColumnName("onboarding_session_id");
            entity.Property(e => e.ReactionChoiceId).HasColumnName("reaction_choice_id");
            entity.Property(e => e.SecondPunchlineFeedbackId).HasColumnName("second_punchline_feedback_id");

            entity.HasOne(d => d.CandidateFeedback).WithMany(p => p.HumorEvidences)
                .HasForeignKey(d => d.CandidateFeedbackId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("humor_evidence_candidate_feedback_id_fkey");

            entity.HasOne(d => d.CandidatePresentation).WithMany(p => p.HumorEvidences)
                .HasForeignKey(d => d.CandidatePresentationId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("humor_evidence_candidate_presentation_id_fkey");

            entity.HasOne(d => d.DrynessRating).WithMany(p => p.HumorEvidences)
                .HasForeignKey(d => d.DrynessRatingId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("humor_evidence_dryness_rating_id_fkey");

            entity.HasOne(d => d.OnboardingSession).WithMany(p => p.HumorEvidenceOnboardingSessions)
                .HasForeignKey(d => d.OnboardingSessionId)
                .HasConstraintName("humor_evidence_onboarding_session_id_fkey");

            entity.HasOne(d => d.ReactionChoice).WithMany(p => p.HumorEvidences)
                .HasForeignKey(d => d.ReactionChoiceId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("humor_evidence_reaction_choice_id_fkey");

            entity.HasOne(d => d.SecondPunchlineFeedback).WithMany(p => p.HumorEvidences)
                .HasForeignKey(d => d.SecondPunchlineFeedbackId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("humor_evidence_second_punchline_feedback_id_fkey");

            entity.HasOne(d => d.HumorDimensionModelMember).WithMany(p => p.HumorEvidences)
                .HasPrincipalKey(p => new { p.HumorDimensionModelMemberId, p.HumorDimensionModelVersionId })
                .HasForeignKey(d => new { d.HumorDimensionModelMemberId, d.HumorDimensionModelVersionId })
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_humor_evidence_member");

            entity.HasOne(d => d.OnboardingSessionNavigation).WithMany(p => p.HumorEvidenceOnboardingSessionNavigations)
                .HasPrincipalKey(p => new { p.OnboardingSessionId, p.HumorDimensionModelVersionId })
                .HasForeignKey(d => new { d.OnboardingSessionId, d.HumorDimensionModelVersionId })
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_humor_evidence_session_model");
        });

        modelBuilder.Entity<HumorVerdict>(entity =>
        {
            entity.HasKey(e => e.HumorVerdictId).HasName("humor_verdict_pkey");

            entity.ToTable("humor_verdict", "humor");

            entity.HasIndex(e => e.InitialHumorSnapshotId, "humor_verdict_initial_humor_snapshot_id_key").IsUnique();

            entity.Property(e => e.HumorVerdictId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("humor_verdict_id");
            entity.Property(e => e.AiOperationExecutionId).HasColumnName("ai_operation_execution_id");
            entity.Property(e => e.Confidence)
                .HasPrecision(6, 5)
                .HasColumnName("confidence");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.GeneratedBy)
                .HasDefaultValueSql("'system'::text")
                .HasColumnName("generated_by");
            entity.Property(e => e.InitialHumorSnapshotId).HasColumnName("initial_humor_snapshot_id");
            entity.Property(e => e.LanguageCode)
                .HasDefaultValueSql("'pl-PL'::text")
                .HasColumnName("language_code");
            entity.Property(e => e.VerdictText).HasColumnName("verdict_text");

            entity.HasOne(d => d.AiOperationExecution).WithMany(p => p.HumorVerdicts)
                .HasForeignKey(d => d.AiOperationExecutionId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_humor_verdict_ai_execution");

            entity.HasOne(d => d.InitialHumorSnapshot).WithOne(p => p.HumorVerdict)
                .HasForeignKey<HumorVerdict>(d => d.InitialHumorSnapshotId)
                .HasConstraintName("humor_verdict_initial_humor_snapshot_id_fkey");
        });

        modelBuilder.Entity<Industry>(entity =>
        {
            entity.HasKey(e => e.IndustryId).HasName("industry_pkey");

            entity.ToTable("industry", "onboarding");

            entity.HasIndex(e => e.IndustryKey, "industry_industry_key_key").IsUnique();

            entity.Property(e => e.IndustryId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("industry_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DisplayName).HasColumnName("display_name");
            entity.Property(e => e.IndustryKey).HasColumnName("industry_key");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
        });

        modelBuilder.Entity<InitialHumorSnapshot>(entity =>
        {
            entity.HasKey(e => e.InitialHumorSnapshotId).HasName("initial_humor_snapshot_pkey");

            entity.ToTable("initial_humor_snapshot", "humor");

            entity.HasIndex(e => new { e.InitialHumorSnapshotId, e.HumorDimensionModelVersionId }, "initial_humor_snapshot_initial_humor_snapshot_id_humor_dime_key").IsUnique();

            entity.HasIndex(e => new { e.OnboardingSessionId, e.SnapshotVersion }, "initial_humor_snapshot_onboarding_session_id_snapshot_versi_key").IsUnique();

            entity.HasIndex(e => e.OnboardingSessionId, "ux_initial_snapshot_one_published")
                .IsUnique()
                .HasFilter("(is_published = true)");

            entity.Property(e => e.InitialHumorSnapshotId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("initial_humor_snapshot_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.ConfirmedTraitsSummary).HasColumnName("confirmed_traits_summary");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.HumorDimensionModelVersionId).HasColumnName("humor_dimension_model_version_id");
            entity.Property(e => e.IsPublished).HasColumnName("is_published");
            entity.Property(e => e.OnboardingSessionId).HasColumnName("onboarding_session_id");
            entity.Property(e => e.OverallConfidence)
                .HasPrecision(6, 5)
                .HasColumnName("overall_confidence");
            entity.Property(e => e.PresentationJson)
                .HasColumnType("jsonb")
                .HasColumnName("presentation_json");
            entity.Property(e => e.PublishedAt).HasColumnName("published_at");
            entity.Property(e => e.SnapshotVersion)
                .HasDefaultValue(1)
                .HasColumnName("snapshot_version");
            entity.Property(e => e.SupersedesSnapshotId).HasColumnName("supersedes_snapshot_id");
            entity.Property(e => e.UnexploredAreasSummary).HasColumnName("unexplored_areas_summary");
            entity.Property(e => e.WeakHypothesisSummary).HasColumnName("weak_hypothesis_summary");

            entity.HasOne(d => d.Account).WithMany(p => p.InitialHumorSnapshots)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("initial_humor_snapshot_account_id_fkey");

            entity.HasOne(d => d.HumorDimensionModelVersion).WithMany(p => p.InitialHumorSnapshots)
                .HasForeignKey(d => d.HumorDimensionModelVersionId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("initial_humor_snapshot_humor_dimension_model_version_id_fkey");

            entity.HasOne(d => d.OnboardingSession).WithOne(p => p.InitialHumorSnapshot)
                .HasForeignKey<InitialHumorSnapshot>(d => d.OnboardingSessionId)
                .HasConstraintName("initial_humor_snapshot_onboarding_session_id_fkey");

            entity.HasOne(d => d.SupersedesSnapshot).WithMany(p => p.InverseSupersedesSnapshot)
                .HasForeignKey(d => d.SupersedesSnapshotId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("initial_humor_snapshot_supersedes_snapshot_id_fkey");
        });

        modelBuilder.Entity<InitialHumorSnapshotDimension>(entity =>
        {
            entity.HasKey(e => e.InitialHumorSnapshotDimensionId).HasName("initial_humor_snapshot_dimension_pkey");

            entity.ToTable("initial_humor_snapshot_dimension", "humor");

            entity.HasIndex(e => new { e.InitialHumorSnapshotId, e.HumorDimensionModelMemberId }, "initial_humor_snapshot_dimens_initial_humor_snapshot_id_hum_key").IsUnique();

            entity.Property(e => e.InitialHumorSnapshotDimensionId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("initial_humor_snapshot_dimension_id");
            entity.Property(e => e.Confidence)
                .HasPrecision(6, 5)
                .HasColumnName("confidence");
            entity.Property(e => e.ConfirmationLevel).HasColumnName("confirmation_level");
            entity.Property(e => e.EstimatedValue)
                .HasPrecision(6, 5)
                .HasColumnName("estimated_value");
            entity.Property(e => e.EvidenceCount).HasColumnName("evidence_count");
            entity.Property(e => e.HumorDimensionModelMemberId).HasColumnName("humor_dimension_model_member_id");
            entity.Property(e => e.HumorDimensionModelVersionId).HasColumnName("humor_dimension_model_version_id");
            entity.Property(e => e.InitialHumorSnapshotId).HasColumnName("initial_humor_snapshot_id");

            entity.HasOne(d => d.InitialHumorSnapshot).WithMany(p => p.InitialHumorSnapshotDimensionInitialHumorSnapshots)
                .HasForeignKey(d => d.InitialHumorSnapshotId)
                .HasConstraintName("initial_humor_snapshot_dimension_initial_humor_snapshot_id_fkey");

            entity.HasOne(d => d.HumorDimensionModelMember).WithMany(p => p.InitialHumorSnapshotDimensions)
                .HasPrincipalKey(p => new { p.HumorDimensionModelMemberId, p.HumorDimensionModelVersionId })
                .HasForeignKey(d => new { d.HumorDimensionModelMemberId, d.HumorDimensionModelVersionId })
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_snapshot_dimension_member");

            entity.HasOne(d => d.InitialHumorSnapshotNavigation).WithMany(p => p.InitialHumorSnapshotDimensionInitialHumorSnapshotNavigations)
                .HasPrincipalKey(p => new { p.InitialHumorSnapshotId, p.HumorDimensionModelVersionId })
                .HasForeignKey(d => new { d.InitialHumorSnapshotId, d.HumorDimensionModelVersionId })
                .HasConstraintName("fk_snapshot_dimension_snapshot_model");
        });

        modelBuilder.Entity<LearningRecommendation>(entity =>
        {
            entity.HasKey(e => e.LearningRecommendationId).HasName("learning_recommendation_pkey");

            entity.ToTable("learning_recommendation", "studio");

            entity.Property(e => e.LearningRecommendationId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("learning_recommendation_id");
            entity.Property(e => e.Confidence)
                .HasPrecision(6, 5)
                .HasColumnName("confidence");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.EvidenceRangeEnd).HasColumnName("evidence_range_end");
            entity.Property(e => e.EvidenceRangeStart).HasColumnName("evidence_range_start");
            entity.Property(e => e.PredictedImpact)
                .HasPrecision(6, 5)
                .HasColumnName("predicted_impact");
            entity.Property(e => e.RecommendationPayload)
                .HasColumnType("jsonb")
                .HasColumnName("recommendation_payload");
            entity.Property(e => e.RecommendationStatus)
                .HasDefaultValueSql("'proposed'::text")
                .HasColumnName("recommendation_status");
            entity.Property(e => e.RecommendationType).HasColumnName("recommendation_type");
            entity.Property(e => e.RiskLevel).HasColumnName("risk_level");
            entity.Property(e => e.SampleSize).HasColumnName("sample_size");
            entity.Property(e => e.TargetId)
                .HasComment("Typed target pointer for recommendation scope; generic FK intentionally deferred.")
                .HasColumnName("target_id");
            entity.Property(e => e.TargetType).HasColumnName("target_type");
        });

        modelBuilder.Entity<LegalDocument>(entity =>
        {
            entity.HasKey(e => e.LegalDocumentId).HasName("legal_document_pkey");

            entity.ToTable("legal_document", "identity");

            entity.HasIndex(e => e.DocumentKey, "legal_document_document_key_key").IsUnique();

            entity.Property(e => e.LegalDocumentId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("legal_document_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DocumentKey).HasColumnName("document_key");
            entity.Property(e => e.DocumentType).HasColumnName("document_type");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsRequired)
                .HasDefaultValue(true)
                .HasColumnName("is_required");
        });

        modelBuilder.Entity<LegalDocumentVersion>(entity =>
        {
            entity.HasKey(e => e.LegalDocumentVersionId).HasName("legal_document_version_pkey");

            entity.ToTable("legal_document_version", "identity");

            entity.HasIndex(e => new { e.LegalDocumentId, e.VersionLabel }, "legal_document_version_legal_document_id_version_label_key").IsUnique();

            entity.Property(e => e.LegalDocumentVersionId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("legal_document_version_id");
            entity.Property(e => e.ContentHash).HasColumnName("content_hash");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.LanguageCode).HasColumnName("language_code");
            entity.Property(e => e.LegalDocumentId).HasColumnName("legal_document_id");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'active'::text")
                .HasColumnName("status");
            entity.Property(e => e.ValidFrom).HasColumnName("valid_from");
            entity.Property(e => e.ValidTo).HasColumnName("valid_to");
            entity.Property(e => e.VersionLabel).HasColumnName("version_label");

            entity.HasOne(d => d.LegalDocument).WithMany(p => p.LegalDocumentVersions)
                .HasForeignKey(d => d.LegalDocumentId)
                .HasConstraintName("legal_document_version_legal_document_id_fkey");
        });

        modelBuilder.Entity<MediaAsset>(entity =>
        {
            entity.HasKey(e => e.MediaAssetId).HasName("media_asset_pkey");

            entity.ToTable("media_asset", "onboarding");

            entity.HasIndex(e => e.Sha256Hash, "ux_media_asset_sha256").IsUnique();

            entity.HasIndex(e => e.StorageKey, "ux_media_asset_storage_key").IsUnique();

            entity.Property(e => e.MediaAssetId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("media_asset_id");
            entity.Property(e => e.AltText).HasColumnName("alt_text");
            entity.Property(e => e.ByteSize).HasColumnName("byte_size");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.HeightPx).HasColumnName("height_px");
            entity.Property(e => e.MimeType).HasColumnName("mime_type");
            entity.Property(e => e.ModerationStatus)
                .HasDefaultValueSql("'pending'::text")
                .HasColumnName("moderation_status");
            entity.Property(e => e.ProcessingStatus)
                .HasDefaultValueSql("'ready'::text")
                .HasColumnName("processing_status");
            entity.Property(e => e.RightsStatus)
                .HasDefaultValueSql("'unknown'::text")
                .HasColumnName("rights_status");
            entity.Property(e => e.Sha256Hash).HasColumnName("sha256_hash");
            entity.Property(e => e.StorageKey).HasColumnName("storage_key");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
            entity.Property(e => e.WidthPx).HasColumnName("width_px");
        });

        modelBuilder.Entity<MemeCalibrationFeedback>(entity =>
        {
            entity.HasKey(e => e.MemeCalibrationFeedbackId).HasName("meme_calibration_feedback_pkey");

            entity.ToTable("meme_calibration_feedback", "humor");

            entity.Property(e => e.MemeCalibrationFeedbackId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("meme_calibration_feedback_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.ImpactWeight)
                .HasPrecision(6, 5)
                .HasColumnName("impact_weight");
            entity.Property(e => e.MemeCalibrationVariantId).HasColumnName("meme_calibration_variant_id");
            entity.Property(e => e.RatingValue).HasColumnName("rating_value");
            entity.Property(e => e.SelectedFinal).HasColumnName("selected_final");

            entity.HasOne(d => d.Account).WithMany(p => p.MemeCalibrationFeedbacks)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("meme_calibration_feedback_account_id_fkey");

            entity.HasOne(d => d.MemeCalibrationVariant).WithMany(p => p.MemeCalibrationFeedbacks)
                .HasForeignKey(d => d.MemeCalibrationVariantId)
                .HasConstraintName("meme_calibration_feedback_meme_calibration_variant_id_fkey");
        });

        modelBuilder.Entity<MemeCalibrationSession>(entity =>
        {
            entity.HasKey(e => e.MemeCalibrationSessionId).HasName("meme_calibration_session_pkey");

            entity.ToTable("meme_calibration_session", "humor");

            entity.Property(e => e.MemeCalibrationSessionId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("meme_calibration_session_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.MechanismAnalysis)
                .HasColumnType("jsonb")
                .HasColumnName("mechanism_analysis");
            entity.Property(e => e.MediaAssetId).HasColumnName("media_asset_id");
            entity.Property(e => e.ModerationStatus)
                .HasDefaultValueSql("'pending'::text")
                .HasColumnName("moderation_status");
            entity.Property(e => e.OnboardingSessionId).HasColumnName("onboarding_session_id");
            entity.Property(e => e.PrivacyStatus)
                .HasDefaultValueSql("'private'::text")
                .HasColumnName("privacy_status");
            entity.Property(e => e.UserDeclaredFunny).HasColumnName("user_declared_funny");

            entity.HasOne(d => d.Account).WithMany(p => p.MemeCalibrationSessions)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("meme_calibration_session_account_id_fkey");

            entity.HasOne(d => d.MediaAsset).WithMany(p => p.MemeCalibrationSessions)
                .HasForeignKey(d => d.MediaAssetId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("meme_calibration_session_media_asset_id_fkey");

            entity.HasOne(d => d.OnboardingSession).WithMany(p => p.MemeCalibrationSessions)
                .HasForeignKey(d => d.OnboardingSessionId)
                .HasConstraintName("meme_calibration_session_onboarding_session_id_fkey");
        });

        modelBuilder.Entity<MemeCalibrationVariant>(entity =>
        {
            entity.HasKey(e => e.MemeCalibrationVariantId).HasName("meme_calibration_variant_pkey");

            entity.ToTable("meme_calibration_variant", "humor");

            entity.HasIndex(e => new { e.MemeCalibrationSessionId, e.VariantNo }, "meme_calibration_variant_meme_calibration_session_id_varian_key").IsUnique();

            entity.Property(e => e.MemeCalibrationVariantId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("meme_calibration_variant_id");
            entity.Property(e => e.AiOperationExecutionId).HasColumnName("ai_operation_execution_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.MemeCalibrationSessionId).HasColumnName("meme_calibration_session_id");
            entity.Property(e => e.VariantNo).HasColumnName("variant_no");
            entity.Property(e => e.VariantStyle).HasColumnName("variant_style");
            entity.Property(e => e.VariantText).HasColumnName("variant_text");

            entity.HasOne(d => d.AiOperationExecution).WithMany(p => p.MemeCalibrationVariants)
                .HasForeignKey(d => d.AiOperationExecutionId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("meme_calibration_variant_ai_operation_execution_id_fkey");

            entity.HasOne(d => d.MemeCalibrationSession).WithMany(p => p.MemeCalibrationVariants)
                .HasForeignKey(d => d.MemeCalibrationSessionId)
                .HasConstraintName("meme_calibration_variant_meme_calibration_session_id_fkey");
        });

        modelBuilder.Entity<ModerationDecision>(entity =>
        {
            entity.HasKey(e => e.ModerationDecisionId).HasName("moderation_decision_pkey");

            entity.ToTable("moderation_decision", "studio");

            entity.Property(e => e.ModerationDecisionId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("moderation_decision_id");
            entity.Property(e => e.DecidedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("decided_at");
            entity.Property(e => e.DecisionReason).HasColumnName("decision_reason");
            entity.Property(e => e.DecisionType).HasColumnName("decision_type");
            entity.Property(e => e.ModerationReviewId).HasColumnName("moderation_review_id");
            entity.Property(e => e.ModeratorAccountId).HasColumnName("moderator_account_id");

            entity.HasOne(d => d.ModerationReview).WithMany(p => p.ModerationDecisions)
                .HasForeignKey(d => d.ModerationReviewId)
                .HasConstraintName("moderation_decision_moderation_review_id_fkey");

            entity.HasOne(d => d.ModeratorAccount).WithMany(p => p.ModerationDecisions)
                .HasForeignKey(d => d.ModeratorAccountId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("moderation_decision_moderator_account_id_fkey");
        });

        modelBuilder.Entity<ModerationReview>(entity =>
        {
            entity.HasKey(e => e.ModerationReviewId).HasName("moderation_review_pkey");

            entity.ToTable("moderation_review", "studio");

            entity.Property(e => e.ModerationReviewId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("moderation_review_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.ModerationStatus).HasColumnName("moderation_status");
            entity.Property(e => e.ModeratorAccountId).HasColumnName("moderator_account_id");
            entity.Property(e => e.RiskLevel).HasColumnName("risk_level");
            entity.Property(e => e.TargetId)
                .HasComment("Typed target pointer; enforced by target_type and application workflow, no single relational FK available.")
                .HasColumnName("target_id");
            entity.Property(e => e.TargetType).HasColumnName("target_type");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.ModeratorAccount).WithMany(p => p.ModerationReviews)
                .HasForeignKey(d => d.ModeratorAccountId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("moderation_review_moderator_account_id_fkey");
        });

        modelBuilder.Entity<OnboardingCandidate>(entity =>
        {
            entity.HasKey(e => e.OnboardingCandidateId).HasName("onboarding_candidate_pkey");

            entity.ToTable("onboarding_candidate", "onboarding");

            entity.HasIndex(e => e.CandidateKey, "onboarding_candidate_candidate_key_key").IsUnique();

            entity.Property(e => e.OnboardingCandidateId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("onboarding_candidate_id");
            entity.Property(e => e.ApprovedVersionId).HasColumnName("approved_version_id");
            entity.Property(e => e.CandidateKey).HasColumnName("candidate_key");
            entity.Property(e => e.CandidateStatus)
                .HasDefaultValueSql("'draft'::text")
                .HasColumnName("candidate_status");
            entity.Property(e => e.ContentFormat).HasColumnName("content_format");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedByAccountId).HasColumnName("created_by_account_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.ApprovedVersion).WithMany(p => p.OnboardingCandidates)
                .HasForeignKey(d => d.ApprovedVersionId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_onboarding_candidate_approved_version");

            entity.HasOne(d => d.CreatedByAccount).WithMany(p => p.OnboardingCandidates)
                .HasForeignKey(d => d.CreatedByAccountId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("onboarding_candidate_created_by_account_id_fkey");
        });

        modelBuilder.Entity<OnboardingCandidateVersion>(entity =>
        {
            entity.HasKey(e => e.OnboardingCandidateVersionId).HasName("onboarding_candidate_version_pkey");

            entity.ToTable("onboarding_candidate_version", "onboarding");

            entity.HasIndex(e => new { e.OnboardingCandidateId, e.VersionNo }, "ix_candidate_version_candidate").IsDescending(false, true);

            entity.HasIndex(e => new { e.OnboardingCandidateId, e.VersionNo }, "onboarding_candidate_version_onboarding_candidate_id_versio_key").IsUnique();

            entity.Property(e => e.OnboardingCandidateVersionId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("onboarding_candidate_version_id");
            entity.Property(e => e.BodyText).HasColumnName("body_text");
            entity.Property(e => e.CaptionText).HasColumnName("caption_text");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedByAccountId).HasColumnName("created_by_account_id");
            entity.Property(e => e.EditorialStatus)
                .HasDefaultValueSql("'draft'::text")
                .HasColumnName("editorial_status");
            entity.Property(e => e.IsSignificantChange)
                .HasDefaultValue(true)
                .HasColumnName("is_significant_change");
            entity.Property(e => e.LanguageCode)
                .HasDefaultValueSql("'pl-PL'::text")
                .HasColumnName("language_code");
            entity.Property(e => e.ModerationStatus)
                .HasDefaultValueSql("'pending'::text")
                .HasColumnName("moderation_status");
            entity.Property(e => e.OnboardingCandidateId).HasColumnName("onboarding_candidate_id");
            entity.Property(e => e.RightsStatus)
                .HasDefaultValueSql("'pending_review'::text")
                .HasColumnName("rights_status");
            entity.Property(e => e.RoleInFlow)
                .HasDefaultValueSql("'exploration'::text")
                .HasColumnName("role_in_flow");
            entity.Property(e => e.SafetyClassification)
                .HasDefaultValueSql("'unknown'::text")
                .HasColumnName("safety_classification");
            entity.Property(e => e.SituationDescription).HasColumnName("situation_description");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.ValidFrom).HasColumnName("valid_from");
            entity.Property(e => e.ValidTo).HasColumnName("valid_to");
            entity.Property(e => e.VersionNo).HasColumnName("version_no");

            entity.HasOne(d => d.CreatedByAccount).WithMany(p => p.OnboardingCandidateVersions)
                .HasForeignKey(d => d.CreatedByAccountId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("onboarding_candidate_version_created_by_account_id_fkey");

            entity.HasOne(d => d.OnboardingCandidate).WithMany(p => p.OnboardingCandidateVersions)
                .HasForeignKey(d => d.OnboardingCandidateId)
                .HasConstraintName("onboarding_candidate_version_onboarding_candidate_id_fkey");
        });

        modelBuilder.Entity<OnboardingFlowVersion>(entity =>
        {
            entity.HasKey(e => e.OnboardingFlowVersionId).HasName("onboarding_flow_version_pkey");

            entity.ToTable("onboarding_flow_version", "onboarding");

            entity.HasIndex(e => new { e.FlowKey, e.VersionLabel }, "onboarding_flow_version_flow_key_version_label_key").IsUnique();

            entity.HasIndex(e => e.FlowKey, "ux_onboarding_flow_one_active")
                .IsUnique()
                .HasFilter("(status = 'active'::text)");

            entity.Property(e => e.OnboardingFlowVersionId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("onboarding_flow_version_id");
            entity.Property(e => e.ActivatedAt).HasColumnName("activated_at");
            entity.Property(e => e.Config)
                .HasDefaultValueSql("'{}'::jsonb")
                .HasColumnType("jsonb")
                .HasColumnName("config");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedByAccountId).HasColumnName("created_by_account_id");
            entity.Property(e => e.FlowKey).HasColumnName("flow_key");
            entity.Property(e => e.RetiredAt).HasColumnName("retired_at");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'draft'::text")
                .HasColumnName("status");
            entity.Property(e => e.VersionLabel).HasColumnName("version_label");

            entity.HasOne(d => d.CreatedByAccount).WithMany(p => p.OnboardingFlowVersions)
                .HasForeignKey(d => d.CreatedByAccountId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("onboarding_flow_version_created_by_account_id_fkey");
        });

        modelBuilder.Entity<OnboardingFunnelSnapshot>(entity =>
        {
            entity.HasKey(e => e.OnboardingFunnelSnapshotId).HasName("onboarding_funnel_snapshot_pkey");

            entity.ToTable("onboarding_funnel_snapshot", "learning");

            entity.Property(e => e.OnboardingFunnelSnapshotId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("onboarding_funnel_snapshot_id");
            entity.Property(e => e.AbandonedCount).HasColumnName("abandoned_count");
            entity.Property(e => e.AvgTimeToCompleteSec)
                .HasPrecision(10, 2)
                .HasColumnName("avg_time_to_complete_sec");
            entity.Property(e => e.CompletedCount).HasColumnName("completed_count");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.PeriodEnd).HasColumnName("period_end");
            entity.Property(e => e.PeriodStart).HasColumnName("period_start");
            entity.Property(e => e.PolicyVersionId).HasColumnName("policy_version_id");
            entity.Property(e => e.SourceRegistration).HasColumnName("source_registration");
            entity.Property(e => e.StartedCount).HasColumnName("started_count");

            entity.HasOne(d => d.PolicyVersion).WithMany(p => p.OnboardingFunnelSnapshots)
                .HasForeignKey(d => d.PolicyVersionId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("onboarding_funnel_snapshot_policy_version_id_fkey");
        });

        modelBuilder.Entity<OnboardingPriorHypothesis>(entity =>
        {
            entity.HasKey(e => e.OnboardingPriorHypothesisId).HasName("onboarding_prior_hypothesis_pkey");

            entity.ToTable("onboarding_prior_hypothesis", "humor");

            entity.Property(e => e.OnboardingPriorHypothesisId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("onboarding_prior_hypothesis_id");
            entity.Property(e => e.AccountAgeContextId).HasColumnName("account_age_context_id");
            entity.Property(e => e.AccountProfessionContextId).HasColumnName("account_profession_context_id");
            entity.Property(e => e.AppliedWeight)
                .HasPrecision(6, 5)
                .HasColumnName("applied_weight");
            entity.Property(e => e.CohortDefinitionVersionId).HasColumnName("cohort_definition_version_id");
            entity.Property(e => e.Confidence)
                .HasPrecision(6, 5)
                .HasColumnName("confidence");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.EstimatedValue)
                .HasPrecision(6, 5)
                .HasColumnName("estimated_value");
            entity.Property(e => e.HumorDimensionModelMemberId).HasColumnName("humor_dimension_model_member_id");
            entity.Property(e => e.HumorDimensionModelVersionId).HasColumnName("humor_dimension_model_version_id");
            entity.Property(e => e.OnboardingSessionId).HasColumnName("onboarding_session_id");
            entity.Property(e => e.PolicyVersionId).HasColumnName("policy_version_id");
            entity.Property(e => e.SourceType).HasColumnName("source_type");

            entity.HasOne(d => d.AccountAgeContext).WithMany(p => p.OnboardingPriorHypotheses)
                .HasForeignKey(d => d.AccountAgeContextId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("onboarding_prior_hypothesis_account_age_context_id_fkey");

            entity.HasOne(d => d.AccountProfessionContext).WithMany(p => p.OnboardingPriorHypotheses)
                .HasForeignKey(d => d.AccountProfessionContextId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("onboarding_prior_hypothesis_account_profession_context_id_fkey");

            entity.HasOne(d => d.CohortDefinitionVersion).WithMany(p => p.OnboardingPriorHypotheses)
                .HasForeignKey(d => d.CohortDefinitionVersionId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_prior_hypothesis_cohort_definition_version");

            entity.HasOne(d => d.OnboardingSession).WithMany(p => p.OnboardingPriorHypothesisOnboardingSessions)
                .HasForeignKey(d => d.OnboardingSessionId)
                .HasConstraintName("onboarding_prior_hypothesis_onboarding_session_id_fkey");

            entity.HasOne(d => d.PolicyVersion).WithMany(p => p.OnboardingPriorHypotheses)
                .HasForeignKey(d => d.PolicyVersionId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_prior_hypothesis_policy_version");

            entity.HasOne(d => d.HumorDimensionModelMember).WithMany(p => p.OnboardingPriorHypotheses)
                .HasPrincipalKey(p => new { p.HumorDimensionModelMemberId, p.HumorDimensionModelVersionId })
                .HasForeignKey(d => new { d.HumorDimensionModelMemberId, d.HumorDimensionModelVersionId })
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_prior_member");

            entity.HasOne(d => d.OnboardingSessionNavigation).WithMany(p => p.OnboardingPriorHypothesisOnboardingSessionNavigations)
                .HasPrincipalKey(p => new { p.OnboardingSessionId, p.HumorDimensionModelVersionId })
                .HasForeignKey(d => new { d.OnboardingSessionId, d.HumorDimensionModelVersionId })
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_prior_session_model");
        });

        modelBuilder.Entity<OnboardingSession>(entity =>
        {
            entity.HasKey(e => e.OnboardingSessionId).HasName("onboarding_session_pkey");

            entity.ToTable("onboarding_session", "onboarding");

            entity.HasIndex(e => new { e.AccountId, e.StartedAt }, "ix_onboarding_session_account_id").IsDescending(false, true);

            entity.HasIndex(e => new { e.SessionStatus, e.LastActivityAt }, "ix_onboarding_session_status").IsDescending(false, true);

            entity.HasIndex(e => new { e.OnboardingSessionId, e.HumorDimensionModelVersionId }, "onboarding_session_onboarding_session_id_humor_dimension_mo_key").IsUnique();

            entity.Property(e => e.OnboardingSessionId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("onboarding_session_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.CompletedAt).HasColumnName("completed_at");
            entity.Property(e => e.CurrentStepKey).HasColumnName("current_step_key");
            entity.Property(e => e.HumorDimensionModelVersionId).HasColumnName("humor_dimension_model_version_id");
            entity.Property(e => e.HumorInspirationSkipped).HasColumnName("humor_inspiration_skipped");
            entity.Property(e => e.InterruptedAt).HasColumnName("interrupted_at");
            entity.Property(e => e.LastActivityAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("last_activity_at");
            entity.Property(e => e.LastCompletedPosition).HasColumnName("last_completed_position");
            entity.Property(e => e.MemeCalibrationSkipped).HasColumnName("meme_calibration_skipped");
            entity.Property(e => e.OnboardingFlowVersionId).HasColumnName("onboarding_flow_version_id");
            entity.Property(e => e.OptionalContextSkipped).HasColumnName("optional_context_skipped");
            entity.Property(e => e.ResumeTokenHash).HasColumnName("resume_token_hash");
            entity.Property(e => e.ResumedAt).HasColumnName("resumed_at");
            entity.Property(e => e.RowVersion)
                .HasDefaultValue(1L)
                .HasColumnName("row_version");
            entity.Property(e => e.ScoringPolicyVersionId).HasColumnName("scoring_policy_version_id");
            entity.Property(e => e.SessionStatus)
                .HasDefaultValueSql("'started'::text")
                .HasColumnName("session_status");
            entity.Property(e => e.SourceRegistration).HasColumnName("source_registration");
            entity.Property(e => e.StartedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("started_at");

            entity.HasOne(d => d.Account).WithMany(p => p.OnboardingSessions)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("onboarding_session_account_id_fkey");

            entity.HasOne(d => d.HumorDimensionModelVersion).WithMany(p => p.OnboardingSessions)
                .HasForeignKey(d => d.HumorDimensionModelVersionId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_onboarding_session_humor_dimension_model_version");

            entity.HasOne(d => d.OnboardingFlowVersion).WithMany(p => p.OnboardingSessions)
                .HasForeignKey(d => d.OnboardingFlowVersionId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("onboarding_session_onboarding_flow_version_id_fkey");

            entity.HasOne(d => d.ScoringPolicyVersion).WithMany(p => p.OnboardingSessions)
                .HasForeignKey(d => d.ScoringPolicyVersionId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_onboarding_session_scoring_policy");
        });

        modelBuilder.Entity<OnboardingStepState>(entity =>
        {
            entity.HasKey(e => e.OnboardingStepStateId).HasName("onboarding_step_state_pkey");

            entity.ToTable("onboarding_step_state", "onboarding");

            entity.HasIndex(e => new { e.OnboardingSessionId, e.StepKey }, "onboarding_step_state_onboarding_session_id_step_key_key").IsUnique();

            entity.Property(e => e.OnboardingStepStateId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("onboarding_step_state_id");
            entity.Property(e => e.CompletedAt).HasColumnName("completed_at");
            entity.Property(e => e.EnteredAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("entered_at");
            entity.Property(e => e.OnboardingSessionId).HasColumnName("onboarding_session_id");
            entity.Property(e => e.SkippedAt).HasColumnName("skipped_at");
            entity.Property(e => e.StepKey).HasColumnName("step_key");
            entity.Property(e => e.StepPayload)
                .HasColumnType("jsonb")
                .HasColumnName("step_payload");
            entity.Property(e => e.StepStatus).HasColumnName("step_status");

            entity.HasOne(d => d.OnboardingSession).WithMany(p => p.OnboardingStepStates)
                .HasForeignKey(d => d.OnboardingSessionId)
                .HasConstraintName("onboarding_step_state_onboarding_session_id_fkey");
        });

        modelBuilder.Entity<OnboardingWorkingHumorDimension>(entity =>
        {
            entity.HasKey(e => e.OnboardingWorkingHumorDimensionId).HasName("onboarding_working_humor_dimension_pkey");

            entity.ToTable("onboarding_working_humor_dimension", "humor");

            entity.HasIndex(e => new { e.OnboardingSessionId, e.HumorDimensionModelMemberId }, "onboarding_working_humor_dime_onboarding_session_id_humor_d_key").IsUnique();

            entity.Property(e => e.OnboardingWorkingHumorDimensionId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("onboarding_working_humor_dimension_id");
            entity.Property(e => e.Confidence)
                .HasPrecision(6, 5)
                .HasDefaultValue(0.10000m)
                .HasColumnName("confidence");
            entity.Property(e => e.EstimatedValue)
                .HasPrecision(6, 5)
                .HasDefaultValue(0.50000m)
                .HasColumnName("estimated_value");
            entity.Property(e => e.HumorDimensionModelMemberId).HasColumnName("humor_dimension_model_member_id");
            entity.Property(e => e.HumorDimensionModelVersionId).HasColumnName("humor_dimension_model_version_id");
            entity.Property(e => e.LastUpdateSource).HasColumnName("last_update_source");
            entity.Property(e => e.NegativeEvidenceCount).HasColumnName("negative_evidence_count");
            entity.Property(e => e.OnboardingSessionId).HasColumnName("onboarding_session_id");
            entity.Property(e => e.PositiveEvidenceCount).HasColumnName("positive_evidence_count");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.OnboardingSession).WithMany(p => p.OnboardingWorkingHumorDimensionOnboardingSessions)
                .HasForeignKey(d => d.OnboardingSessionId)
                .HasConstraintName("onboarding_working_humor_dimension_onboarding_session_id_fkey");

            entity.HasOne(d => d.HumorDimensionModelMember).WithMany(p => p.OnboardingWorkingHumorDimensions)
                .HasPrincipalKey(p => new { p.HumorDimensionModelMemberId, p.HumorDimensionModelVersionId })
                .HasForeignKey(d => new { d.HumorDimensionModelMemberId, d.HumorDimensionModelVersionId })
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_working_dimension_model_member");

            entity.HasOne(d => d.OnboardingSessionNavigation).WithMany(p => p.OnboardingWorkingHumorDimensionOnboardingSessionNavigations)
                .HasPrincipalKey(p => new { p.OnboardingSessionId, p.HumorDimensionModelVersionId })
                .HasForeignKey(d => new { d.OnboardingSessionId, d.HumorDimensionModelVersionId })
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_working_dimension_session_model");
        });

        modelBuilder.Entity<PasswordCredential>(entity =>
        {
            entity.HasKey(e => e.PasswordCredentialId).HasName("password_credential_pkey");

            entity.ToTable("password_credential", "identity", tb => tb.HasComment("Wyłącznie hash hasła; brak jawnych sekretów."));

            entity.HasIndex(e => e.AccountId, "ux_password_credential_one_active")
                .IsUnique()
                .HasFilter("(is_active = true)");

            entity.Property(e => e.PasswordCredentialId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("password_credential_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.ChangedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("changed_at");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.ForceChangeRequired).HasColumnName("force_change_required");
            entity.Property(e => e.HashAlgorithm).HasColumnName("hash_algorithm");
            entity.Property(e => e.HashFormatVersion).HasColumnName("hash_format_version");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash");

            entity.HasOne(d => d.Account).WithOne(p => p.PasswordCredential)
                .HasForeignKey<PasswordCredential>(d => d.AccountId)
                .HasConstraintName("password_credential_account_id_fkey");
        });

        modelBuilder.Entity<PolicyDefinition>(entity =>
        {
            entity.HasKey(e => e.PolicyDefinitionId).HasName("policy_definition_pkey");

            entity.ToTable("policy_definition", "learning");

            entity.HasIndex(e => e.PolicyKey, "policy_definition_policy_key_key").IsUnique();

            entity.Property(e => e.PolicyDefinitionId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("policy_definition_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.PolicyArea).HasColumnName("policy_area");
            entity.Property(e => e.PolicyKey).HasColumnName("policy_key");
        });

        modelBuilder.Entity<PolicyVersion>(entity =>
        {
            entity.HasKey(e => e.PolicyVersionId).HasName("policy_version_pkey");

            entity.ToTable("policy_version", "learning");

            entity.HasIndex(e => new { e.PolicyDefinitionId, e.VersionLabel }, "policy_version_policy_definition_id_version_label_key").IsUnique();

            entity.HasIndex(e => e.PolicyDefinitionId, "ux_policy_version_one_active")
                .IsUnique()
                .HasFilter("(status = 'active'::text)");

            entity.Property(e => e.PolicyVersionId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("policy_version_id");
            entity.Property(e => e.ActivatedAt).HasColumnName("activated_at");
            entity.Property(e => e.Config)
                .HasDefaultValueSql("'{}'::jsonb")
                .HasColumnType("jsonb")
                .HasColumnName("config");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedByAccountId).HasColumnName("created_by_account_id");
            entity.Property(e => e.PolicyDefinitionId).HasColumnName("policy_definition_id");
            entity.Property(e => e.RetiredAt).HasColumnName("retired_at");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'draft'::text")
                .HasColumnName("status");
            entity.Property(e => e.VersionLabel).HasColumnName("version_label");

            entity.HasOne(d => d.CreatedByAccount).WithMany(p => p.PolicyVersions)
                .HasForeignKey(d => d.CreatedByAccountId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("policy_version_created_by_account_id_fkey");

            entity.HasOne(d => d.PolicyDefinition).WithOne(p => p.PolicyVersion)
                .HasForeignKey<PolicyVersion>(d => d.PolicyDefinitionId)
                .HasConstraintName("policy_version_policy_definition_id_fkey");
        });

        modelBuilder.Entity<PredictionOutcome>(entity =>
        {
            entity.HasKey(e => e.PredictionOutcomeId).HasName("prediction_outcome_pkey");

            entity.ToTable("prediction_outcome", "learning");

            entity.Property(e => e.PredictionOutcomeId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("prediction_outcome_id");
            entity.Property(e => e.ActualEnjoyment)
                .HasPrecision(6, 5)
                .HasColumnName("actual_enjoyment");
            entity.Property(e => e.ActualInformationGain)
                .HasPrecision(6, 5)
                .HasColumnName("actual_information_gain");
            entity.Property(e => e.CompletedFlow).HasColumnName("completed_flow");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.MismatchFlag).HasColumnName("mismatch_flag");
            entity.Property(e => e.PredictedEnjoyment)
                .HasPrecision(6, 5)
                .HasColumnName("predicted_enjoyment");
            entity.Property(e => e.PredictedInformationGain)
                .HasPrecision(6, 5)
                .HasColumnName("predicted_information_gain");
            entity.Property(e => e.SelectionDecisionId).HasColumnName("selection_decision_id");

            entity.HasOne(d => d.SelectionDecision).WithMany(p => p.PredictionOutcomes)
                .HasForeignKey(d => d.SelectionDecisionId)
                .HasConstraintName("prediction_outcome_selection_decision_id_fkey");
        });

        modelBuilder.Entity<Profession>(entity =>
        {
            entity.HasKey(e => e.ProfessionId).HasName("profession_pkey");

            entity.ToTable("profession", "onboarding");

            entity.HasIndex(e => e.ProfessionKey, "profession_profession_key_key").IsUnique();

            entity.Property(e => e.ProfessionId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("profession_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DisplayName).HasColumnName("display_name");
            entity.Property(e => e.IndustryId).HasColumnName("industry_id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.ProfessionKey).HasColumnName("profession_key");

            entity.HasOne(d => d.Industry).WithMany(p => p.Professions)
                .HasForeignKey(d => d.IndustryId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("profession_industry_id_fkey");
        });

        modelBuilder.Entity<PublicProfile>(entity =>
        {
            entity.HasKey(e => e.ProfileId).HasName("public_profile_pkey");

            entity.ToTable("public_profile", "identity", tb => tb.HasComment("Publiczna tożsamość użytkownika. Pseudonim rezerwowany podczas rejestracji."));

            entity.HasIndex(e => e.AccountId, "public_profile_account_id_key").IsUnique();

            entity.HasIndex(e => e.NicknameNormalized, "ux_public_profile_nickname_active")
                .IsUnique()
                .HasFilter("(released_at IS NULL)");

            entity.Property(e => e.ProfileId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("profile_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.AvatarAssetRef).HasColumnName("avatar_asset_ref");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Nickname).HasColumnName("nickname");
            entity.Property(e => e.NicknameNormalized)
                .HasColumnType("citext")
                .HasColumnName("nickname_normalized");
            entity.Property(e => e.ProfileVisibility)
                .HasDefaultValueSql("'draft_private'::text")
                .HasColumnName("profile_visibility");
            entity.Property(e => e.ReleasedAt).HasColumnName("released_at");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Account).WithOne(p => p.PublicProfile)
                .HasForeignKey<PublicProfile>(d => d.AccountId)
                .HasConstraintName("public_profile_account_id_fkey");
        });

        modelBuilder.Entity<Reaction>(entity =>
        {
            entity.HasKey(e => e.ReactionId).HasName("reaction_pkey");

            entity.ToTable("reaction", "humor");

            entity.HasIndex(e => e.ReactionKey, "reaction_reaction_key_key").IsUnique();

            entity.Property(e => e.ReactionId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("reaction_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.ReactionKey).HasColumnName("reaction_key");
        });

        modelBuilder.Entity<ReactionChoice>(entity =>
        {
            entity.HasKey(e => e.ReactionChoiceId).HasName("reaction_choice_pkey");

            entity.ToTable("reaction_choice", "humor");

            entity.HasIndex(e => new { e.AccountId, e.SelectedAt }, "ix_reaction_choice_account_time").IsDescending(false, true);

            entity.HasIndex(e => new { e.CandidatePresentationId, e.SelectedAt }, "ix_reaction_choice_presentation");

            entity.HasIndex(e => new { e.CandidatePresentationId, e.AccountId, e.IdempotencyKey }, "reaction_choice_candidate_presentation_id_account_id_idempo_key").IsUnique();

            entity.HasIndex(e => new { e.CandidatePresentationId, e.AccountId, e.ReactionPackItemId }, "ux_reaction_choice_active_per_item")
                .IsUnique()
                .HasFilter("(unselected_at IS NULL)");

            entity.Property(e => e.ReactionChoiceId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("reaction_choice_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.CandidatePresentationId).HasColumnName("candidate_presentation_id");
            entity.Property(e => e.IdempotencyKey).HasColumnName("idempotency_key");
            entity.Property(e => e.ReactionPackItemId).HasColumnName("reaction_pack_item_id");
            entity.Property(e => e.SelectedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("selected_at");
            entity.Property(e => e.SelectionContext).HasColumnName("selection_context");
            entity.Property(e => e.SelectionOrderNo)
                .HasDefaultValue(1)
                .HasColumnName("selection_order_no");
            entity.Property(e => e.SourceReactionExposureId).HasColumnName("source_reaction_exposure_id");
            entity.Property(e => e.UnselectedAt).HasColumnName("unselected_at");
            entity.Property(e => e.UnselectedReason).HasColumnName("unselected_reason");

            entity.HasOne(d => d.Account).WithMany(p => p.ReactionChoices)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("reaction_choice_account_id_fkey");

            entity.HasOne(d => d.CandidatePresentation).WithMany(p => p.ReactionChoices)
                .HasForeignKey(d => d.CandidatePresentationId)
                .HasConstraintName("reaction_choice_candidate_presentation_id_fkey");

            entity.HasOne(d => d.ReactionPackItem).WithMany(p => p.ReactionChoices)
                .HasForeignKey(d => d.ReactionPackItemId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("reaction_choice_reaction_pack_item_id_fkey");

            entity.HasOne(d => d.ReactionExposure).WithMany(p => p.ReactionChoices)
                .HasPrincipalKey(p => new { p.ReactionExposureId, p.CandidatePresentationId, p.ReactionPackItemId, p.AccountId })
                .HasForeignKey(d => new { d.SourceReactionExposureId, d.CandidatePresentationId, d.ReactionPackItemId, d.AccountId })
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_reaction_choice_source_exposure");
        });

        modelBuilder.Entity<ReactionExposure>(entity =>
        {
            entity.HasKey(e => e.ReactionExposureId).HasName("reaction_exposure_pkey");

            entity.ToTable("reaction_exposure", "humor");

            entity.HasIndex(e => new { e.AccountId, e.VisibleFrom }, "ix_reaction_exposure_account_time");

            entity.HasIndex(e => new { e.ReactionPackItemId, e.VisibleFrom }, "ix_reaction_exposure_item_time");

            entity.HasIndex(e => new { e.CandidatePresentationId, e.VisibleFrom }, "ix_reaction_exposure_presentation_time");

            entity.HasIndex(e => new { e.CandidatePresentationId, e.ExposureSequenceNo }, "reaction_exposure_candidate_presentation_id_exposure_sequen_key").IsUnique();

            entity.HasIndex(e => new { e.CandidatePresentationId, e.IdempotencyKey }, "reaction_exposure_candidate_presentation_id_idempotency_key_key").IsUnique();

            entity.HasIndex(e => new { e.ReactionExposureId, e.CandidatePresentationId, e.ReactionPackItemId, e.AccountId }, "reaction_exposure_reaction_exposure_id_candidate_presentati_key").IsUnique();

            entity.Property(e => e.ReactionExposureId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("reaction_exposure_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.CandidatePresentationId).HasColumnName("candidate_presentation_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.ExposureSequenceNo).HasColumnName("exposure_sequence_no");
            entity.Property(e => e.ExposureSource).HasColumnName("exposure_source");
            entity.Property(e => e.IdempotencyKey).HasColumnName("idempotency_key");
            entity.Property(e => e.ReactionPackItemId).HasColumnName("reaction_pack_item_id");
            entity.Property(e => e.SlotNo).HasColumnName("slot_no");
            entity.Property(e => e.VisibleFrom)
                .HasDefaultValueSql("now()")
                .HasColumnName("visible_from");
            entity.Property(e => e.VisibleTo).HasColumnName("visible_to");

            entity.HasOne(d => d.Account).WithMany(p => p.ReactionExposures)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("reaction_exposure_account_id_fkey");

            entity.HasOne(d => d.CandidatePresentation).WithMany(p => p.ReactionExposures)
                .HasForeignKey(d => d.CandidatePresentationId)
                .HasConstraintName("reaction_exposure_candidate_presentation_id_fkey");

            entity.HasOne(d => d.ReactionPackItem).WithMany(p => p.ReactionExposures)
                .HasForeignKey(d => d.ReactionPackItemId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("reaction_exposure_reaction_pack_item_id_fkey");
        });

        modelBuilder.Entity<ReactionMechanism>(entity =>
        {
            entity.HasKey(e => e.ReactionMechanismId).HasName("reaction_mechanism_pkey");

            entity.ToTable("reaction_mechanism", "humor");

            entity.HasIndex(e => e.MechanismKey, "reaction_mechanism_mechanism_key_key").IsUnique();

            entity.Property(e => e.ReactionMechanismId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("reaction_mechanism_id");
            entity.Property(e => e.DisplayName).HasColumnName("display_name");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.MechanismKey).HasColumnName("mechanism_key");
        });

        modelBuilder.Entity<ReactionPack>(entity =>
        {
            entity.HasKey(e => e.ReactionPackId).HasName("reaction_pack_pkey");

            entity.ToTable("reaction_pack", "humor");

            entity.HasIndex(e => new { e.OnboardingCandidateVersionId, e.PackKey, e.LanguageCode }, "reaction_pack_onboarding_candidate_version_id_pack_key_lang_key").IsUnique();

            entity.Property(e => e.ReactionPackId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("reaction_pack_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.LanguageCode)
                .HasDefaultValueSql("'pl-PL'::text")
                .HasColumnName("language_code");
            entity.Property(e => e.OnboardingCandidateVersionId).HasColumnName("onboarding_candidate_version_id");
            entity.Property(e => e.PackKey).HasColumnName("pack_key");
            entity.Property(e => e.SourceType).HasColumnName("source_type");

            entity.HasOne(d => d.OnboardingCandidateVersion).WithMany(p => p.ReactionPacks)
                .HasForeignKey(d => d.OnboardingCandidateVersionId)
                .HasConstraintName("reaction_pack_onboarding_candidate_version_id_fkey");
        });

        modelBuilder.Entity<ReactionPackItem>(entity =>
        {
            entity.HasKey(e => e.ReactionPackItemId).HasName("reaction_pack_item_pkey");

            entity.ToTable("reaction_pack_item", "humor");

            entity.HasIndex(e => new { e.ReactionPackVersionId, e.PoolOrderNo }, "reaction_pack_item_reaction_pack_version_id_pool_order_no_key").IsUnique();

            entity.HasIndex(e => new { e.ReactionPackVersionId, e.ReactionVersionId }, "reaction_pack_item_reaction_pack_version_id_reaction_versio_key").IsUnique();

            entity.HasIndex(e => new { e.ReactionPackVersionId, e.InitialSlotNo }, "ux_reaction_pack_item_initial_slot")
                .IsUnique()
                .HasFilter("(initial_slot_no IS NOT NULL)");

            entity.Property(e => e.ReactionPackItemId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("reaction_pack_item_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.InitialSlotNo).HasColumnName("initial_slot_no");
            entity.Property(e => e.IsAvailableInMore)
                .HasDefaultValue(true)
                .HasColumnName("is_available_in_more");
            entity.Property(e => e.PoolOrderNo).HasColumnName("pool_order_no");
            entity.Property(e => e.ReactionPackVersionId).HasColumnName("reaction_pack_version_id");
            entity.Property(e => e.ReactionVersionId).HasColumnName("reaction_version_id");
            entity.Property(e => e.ReplacementPriority).HasColumnName("replacement_priority");

            entity.HasOne(d => d.ReactionPackVersion).WithMany(p => p.ReactionPackItems)
                .HasForeignKey(d => d.ReactionPackVersionId)
                .HasConstraintName("reaction_pack_item_reaction_pack_version_id_fkey");

            entity.HasOne(d => d.ReactionVersion).WithMany(p => p.ReactionPackItems)
                .HasForeignKey(d => d.ReactionVersionId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("reaction_pack_item_reaction_version_id_fkey");
        });

        modelBuilder.Entity<ReactionPackVersion>(entity =>
        {
            entity.HasKey(e => e.ReactionPackVersionId).HasName("reaction_pack_version_pkey");

            entity.ToTable("reaction_pack_version", "humor");

            entity.HasIndex(e => new { e.ReactionPackId, e.VersionNo }, "reaction_pack_version_reaction_pack_id_version_no_key").IsUnique();

            entity.HasIndex(e => e.ReactionPackId, "ux_reaction_pack_version_one_active")
                .IsUnique()
                .HasFilter("(status = 'active'::text)");

            entity.Property(e => e.ReactionPackVersionId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("reaction_pack_version_id");
            entity.Property(e => e.ActivatedAt).HasColumnName("activated_at");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedByAccountId).HasColumnName("created_by_account_id");
            entity.Property(e => e.DrynessInteractionMode)
                .HasDefaultValueSql("'exclusive'::text")
                .HasColumnName("dryness_interaction_mode");
            entity.Property(e => e.EditorialStatus)
                .HasDefaultValueSql("'awaiting_editorial_review'::text")
                .HasColumnName("editorial_status");
            entity.Property(e => e.InitialVisibleCount)
                .HasDefaultValue(6)
                .HasColumnName("initial_visible_count");
            entity.Property(e => e.MaxActiveSelections)
                .HasDefaultValue(12)
                .HasColumnName("max_active_selections");
            entity.Property(e => e.ModerationStatus)
                .HasDefaultValueSql("'pending'::text")
                .HasColumnName("moderation_status");
            entity.Property(e => e.PromptTemplateVersionId).HasColumnName("prompt_template_version_id");
            entity.Property(e => e.ReactionPackId).HasColumnName("reaction_pack_id");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'generated'::text")
                .HasColumnName("status");
            entity.Property(e => e.TargetPoolSize)
                .HasDefaultValue(12)
                .HasColumnName("target_pool_size");
            entity.Property(e => e.VersionNo).HasColumnName("version_no");
            entity.Property(e => e.WithdrawnAt).HasColumnName("withdrawn_at");

            entity.HasOne(d => d.CreatedByAccount).WithMany(p => p.ReactionPackVersions)
                .HasForeignKey(d => d.CreatedByAccountId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("reaction_pack_version_created_by_account_id_fkey");

            entity.HasOne(d => d.PromptTemplateVersion).WithMany(p => p.ReactionPackVersions)
                .HasForeignKey(d => d.PromptTemplateVersionId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_reaction_pack_version_prompt");

            entity.HasOne(d => d.ReactionPack).WithOne(p => p.ReactionPackVersion)
                .HasForeignKey<ReactionPackVersion>(d => d.ReactionPackId)
                .HasConstraintName("reaction_pack_version_reaction_pack_id_fkey");
        });

        modelBuilder.Entity<ReactionPerformanceSnapshot>(entity =>
        {
            entity.HasKey(e => e.ReactionPerformanceSnapshotId).HasName("reaction_performance_snapshot_pkey");

            entity.ToTable("reaction_performance_snapshot", "learning");

            entity.Property(e => e.ReactionPerformanceSnapshotId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("reaction_performance_snapshot_id");
            entity.Property(e => e.Clicks).HasColumnName("clicks");
            entity.Property(e => e.Confidence)
                .HasPrecision(6, 5)
                .HasColumnName("confidence");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Ctr)
                .HasPrecision(6, 5)
                .HasColumnName("ctr");
            entity.Property(e => e.ExplicitPositiveFeedback).HasColumnName("explicit_positive_feedback");
            entity.Property(e => e.Exposures).HasColumnName("exposures");
            entity.Property(e => e.FatigueIndex)
                .HasPrecision(6, 5)
                .HasColumnName("fatigue_index");
            entity.Property(e => e.PeriodEnd).HasColumnName("period_end");
            entity.Property(e => e.PeriodStart).HasColumnName("period_start");
            entity.Property(e => e.ReactionRescueCases).HasColumnName("reaction_rescue_cases");
            entity.Property(e => e.ReactionRescueRate)
                .HasPrecision(6, 5)
                .HasColumnName("reaction_rescue_rate");
            entity.Property(e => e.ReactionVersionId).HasColumnName("reaction_version_id");
            entity.Property(e => e.SampleSize).HasColumnName("sample_size");
            entity.Property(e => e.SecondPunchlineViews).HasColumnName("second_punchline_views");

            entity.HasOne(d => d.ReactionVersion).WithMany(p => p.ReactionPerformanceSnapshots)
                .HasForeignKey(d => d.ReactionVersionId)
                .HasConstraintName("reaction_performance_snapshot_reaction_version_id_fkey");
        });

        modelBuilder.Entity<ReactionVersion>(entity =>
        {
            entity.HasKey(e => e.ReactionVersionId).HasName("reaction_version_pkey");

            entity.ToTable("reaction_version", "humor");

            entity.HasIndex(e => new { e.ReactionId, e.VersionNo }, "reaction_version_reaction_id_version_no_key").IsUnique();

            entity.Property(e => e.ReactionVersionId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("reaction_version_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.EditorialNote).HasColumnName("editorial_note");
            entity.Property(e => e.Emoji).HasColumnName("emoji");
            entity.Property(e => e.Intensity).HasColumnName("intensity");
            entity.Property(e => e.IsReactionRescue).HasColumnName("is_reaction_rescue");
            entity.Property(e => e.ReactionId).HasColumnName("reaction_id");
            entity.Property(e => e.ReactionLabel).HasColumnName("reaction_label");
            entity.Property(e => e.SafetyFlags)
                .HasColumnType("jsonb")
                .HasColumnName("safety_flags");
            entity.Property(e => e.SecondPunchlineText).HasColumnName("second_punchline_text");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'generated'::text")
                .HasColumnName("status");
            entity.Property(e => e.StyleKey).HasColumnName("style_key");
            entity.Property(e => e.VersionNo).HasColumnName("version_no");

            entity.HasOne(d => d.Reaction).WithMany(p => p.ReactionVersions)
                .HasForeignKey(d => d.ReactionId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("reaction_version_reaction_id_fkey");

            entity.HasMany(d => d.ReactionMechanisms).WithMany(p => p.ReactionVersions)
                .UsingEntity<Dictionary<string, object>>(
                    "ReactionVersionMechanism",
                    r => r.HasOne<ReactionMechanism>().WithMany()
                        .HasForeignKey("ReactionMechanismId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("reaction_version_mechanism_reaction_mechanism_id_fkey"),
                    l => l.HasOne<ReactionVersion>().WithMany()
                        .HasForeignKey("ReactionVersionId")
                        .HasConstraintName("reaction_version_mechanism_reaction_version_id_fkey"),
                    j =>
                    {
                        j.HasKey("ReactionVersionId", "ReactionMechanismId").HasName("reaction_version_mechanism_pkey");
                        j.ToTable("reaction_version_mechanism", "humor");
                        j.IndexerProperty<Guid>("ReactionVersionId").HasColumnName("reaction_version_id");
                        j.IndexerProperty<Guid>("ReactionMechanismId").HasColumnName("reaction_mechanism_id");
                    });
        });

        modelBuilder.Entity<RecommendationDecision>(entity =>
        {
            entity.HasKey(e => e.RecommendationDecisionId).HasName("recommendation_decision_pkey");

            entity.ToTable("recommendation_decision", "studio");

            entity.Property(e => e.RecommendationDecisionId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("recommendation_decision_id");
            entity.Property(e => e.DecidedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("decided_at");
            entity.Property(e => e.DecidedByAccountId).HasColumnName("decided_by_account_id");
            entity.Property(e => e.DecisionReason).HasColumnName("decision_reason");
            entity.Property(e => e.DecisionStatus).HasColumnName("decision_status");
            entity.Property(e => e.ExecutedAction).HasColumnName("executed_action");
            entity.Property(e => e.LearningRecommendationId).HasColumnName("learning_recommendation_id");

            entity.HasOne(d => d.DecidedByAccount).WithMany(p => p.RecommendationDecisions)
                .HasForeignKey(d => d.DecidedByAccountId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("recommendation_decision_decided_by_account_id_fkey");

            entity.HasOne(d => d.LearningRecommendation).WithMany(p => p.RecommendationDecisions)
                .HasForeignKey(d => d.LearningRecommendationId)
                .HasConstraintName("recommendation_decision_learning_recommendation_id_fkey");
        });

        modelBuilder.Entity<RecoveryEvent>(entity =>
        {
            entity.HasKey(e => e.RecoveryEventId).HasName("recovery_event_pkey");

            entity.ToTable("recovery_event", "onboarding");

            entity.Property(e => e.RecoveryEventId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("recovery_event_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.ImprovementDetected).HasColumnName("improvement_detected");
            entity.Property(e => e.OnboardingSessionId).HasColumnName("onboarding_session_id");
            entity.Property(e => e.RecoveryPolicyVersionId).HasColumnName("recovery_policy_version_id");
            entity.Property(e => e.RecoveryStrategy).HasColumnName("recovery_strategy");
            entity.Property(e => e.ResultType).HasColumnName("result_type");
            entity.Property(e => e.RetainedInFlow).HasColumnName("retained_in_flow");
            entity.Property(e => e.SelectionDecisionId).HasColumnName("selection_decision_id");
            entity.Property(e => e.TriggerDetails)
                .HasColumnType("jsonb")
                .HasColumnName("trigger_details");
            entity.Property(e => e.TriggerType).HasColumnName("trigger_type");

            entity.HasOne(d => d.OnboardingSession).WithMany(p => p.RecoveryEvents)
                .HasForeignKey(d => d.OnboardingSessionId)
                .HasConstraintName("recovery_event_onboarding_session_id_fkey");

            entity.HasOne(d => d.RecoveryPolicyVersion).WithMany(p => p.RecoveryEvents)
                .HasForeignKey(d => d.RecoveryPolicyVersionId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_recovery_event_policy_version");

            entity.HasOne(d => d.SelectionDecision).WithMany(p => p.RecoveryEvents)
                .HasForeignKey(d => d.SelectionDecisionId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("recovery_event_selection_decision_id_fkey");
        });

        modelBuilder.Entity<RecoveryPerformanceSnapshot>(entity =>
        {
            entity.HasKey(e => e.RecoveryPerformanceSnapshotId).HasName("recovery_performance_snapshot_pkey");

            entity.ToTable("recovery_performance_snapshot", "learning");

            entity.Property(e => e.RecoveryPerformanceSnapshotId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("recovery_performance_snapshot_id");
            entity.Property(e => e.Confidence)
                .HasPrecision(6, 5)
                .HasColumnName("confidence");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.ImprovementDetectedCount).HasColumnName("improvement_detected_count");
            entity.Property(e => e.PeriodEnd).HasColumnName("period_end");
            entity.Property(e => e.PeriodStart).HasColumnName("period_start");
            entity.Property(e => e.RecoveryStrategy).HasColumnName("recovery_strategy");
            entity.Property(e => e.RetainedInFlowCount).HasColumnName("retained_in_flow_count");
            entity.Property(e => e.SampleSize).HasColumnName("sample_size");
            entity.Property(e => e.SuccessRate)
                .HasPrecision(6, 5)
                .HasColumnName("success_rate");
            entity.Property(e => e.TriggerCount).HasColumnName("trigger_count");
        });

        modelBuilder.Entity<SecondPunchlineFeedback>(entity =>
        {
            entity.HasKey(e => e.SecondPunchlineFeedbackId).HasName("second_punchline_feedback_pkey");

            entity.ToTable("second_punchline_feedback", "humor");

            entity.Property(e => e.SecondPunchlineFeedbackId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("second_punchline_feedback_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.FeedbackText).HasColumnName("feedback_text");
            entity.Property(e => e.FeedbackType).HasColumnName("feedback_type");
            entity.Property(e => e.FeedbackValue).HasColumnName("feedback_value");
            entity.Property(e => e.SecondPunchlineViewId).HasColumnName("second_punchline_view_id");

            entity.HasOne(d => d.SecondPunchlineView).WithMany(p => p.SecondPunchlineFeedbacks)
                .HasForeignKey(d => d.SecondPunchlineViewId)
                .HasConstraintName("second_punchline_feedback_second_punchline_view_id_fkey");
        });

        modelBuilder.Entity<SecondPunchlineView>(entity =>
        {
            entity.HasKey(e => e.SecondPunchlineViewId).HasName("second_punchline_view_pkey");

            entity.ToTable("second_punchline_view", "humor");

            entity.HasIndex(e => e.ReactionChoiceId, "ix_second_punchline_view_choice");

            entity.Property(e => e.SecondPunchlineViewId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("second_punchline_view_id");
            entity.Property(e => e.DwellMs).HasColumnName("dwell_ms");
            entity.Property(e => e.ReactionChoiceId).HasColumnName("reaction_choice_id");
            entity.Property(e => e.SourceAction)
                .HasDefaultValueSql("'reaction_click'::text")
                .HasColumnName("source_action");
            entity.Property(e => e.ViewedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("viewed_at");

            entity.HasOne(d => d.ReactionChoice).WithMany(p => p.SecondPunchlineViews)
                .HasForeignKey(d => d.ReactionChoiceId)
                .HasConstraintName("second_punchline_view_reaction_choice_id_fkey");
        });

        modelBuilder.Entity<SecurityToken>(entity =>
        {
            entity.HasKey(e => e.SecurityTokenId).HasName("security_token_pkey");

            entity.ToTable("security_token", "identity", tb => tb.HasComment("Tokeny bezpieczeństwa przechowywane jako hash; wspiera idempotencję aktywacji i resend."));

            entity.HasIndex(e => new { e.AccountId, e.TokenPurpose, e.ExpiresAt }, "ix_security_token_active_lookup").HasFilter("((used_at IS NULL) AND (invalidated_at IS NULL))");

            entity.HasIndex(e => e.TokenHash, "ux_security_token_hash").IsUnique();

            entity.Property(e => e.SecurityTokenId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("security_token_id");
            entity.Property(e => e.AccountEmailId).HasColumnName("account_email_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
            entity.Property(e => e.InvalidatedAt).HasColumnName("invalidated_at");
            entity.Property(e => e.RequestCorrelationId).HasColumnName("request_correlation_id");
            entity.Property(e => e.ResendSequence).HasColumnName("resend_sequence");
            entity.Property(e => e.TokenHash).HasColumnName("token_hash");
            entity.Property(e => e.TokenPurpose).HasColumnName("token_purpose");
            entity.Property(e => e.UsedAt).HasColumnName("used_at");

            entity.HasOne(d => d.AccountEmail).WithMany(p => p.SecurityTokens)
                .HasForeignKey(d => d.AccountEmailId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("security_token_account_email_id_fkey");

            entity.HasOne(d => d.Account).WithMany(p => p.SecurityTokens)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("security_token_account_id_fkey");
        });

        modelBuilder.Entity<SelectionDecision>(entity =>
        {
            entity.HasKey(e => e.SelectionDecisionId).HasName("selection_decision_pkey");

            entity.ToTable("selection_decision", "onboarding");

            entity.HasIndex(e => new { e.OnboardingSessionId, e.DecisionPositionNo }, "selection_decision_onboarding_session_id_decision_position__key").IsUnique();

            entity.Property(e => e.SelectionDecisionId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("selection_decision_id");
            entity.Property(e => e.Confidence)
                .HasPrecision(6, 5)
                .HasColumnName("confidence");
            entity.Property(e => e.CorrelationId).HasColumnName("correlation_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DecisionPositionNo).HasColumnName("decision_position_no");
            entity.Property(e => e.DecisionReason).HasColumnName("decision_reason");
            entity.Property(e => e.OnboardingSessionId).HasColumnName("onboarding_session_id");
            entity.Property(e => e.PredictedEnjoyment)
                .HasPrecision(6, 5)
                .HasColumnName("predicted_enjoyment");
            entity.Property(e => e.PredictedInformationGain)
                .HasPrecision(6, 5)
                .HasColumnName("predicted_information_gain");
            entity.Property(e => e.PriorSources)
                .HasColumnType("jsonb")
                .HasColumnName("prior_sources");
            entity.Property(e => e.RankingMode).HasColumnName("ranking_mode");
            entity.Property(e => e.RecoveryModeEnabled).HasColumnName("recovery_mode_enabled");
            entity.Property(e => e.ScoreComponents)
                .HasColumnType("jsonb")
                .HasColumnName("score_components");
            entity.Property(e => e.ScoringPolicyVersionId).HasColumnName("scoring_policy_version_id");
            entity.Property(e => e.SelectedOnboardingCandidateVersionId).HasColumnName("selected_onboarding_candidate_version_id");
            entity.Property(e => e.SelectedReactionPackVersionId).HasColumnName("selected_reaction_pack_version_id");
            entity.Property(e => e.TopNCount).HasColumnName("top_n_count");

            entity.HasOne(d => d.OnboardingSession).WithMany(p => p.SelectionDecisions)
                .HasForeignKey(d => d.OnboardingSessionId)
                .HasConstraintName("selection_decision_onboarding_session_id_fkey");

            entity.HasOne(d => d.ScoringPolicyVersion).WithMany(p => p.SelectionDecisions)
                .HasForeignKey(d => d.ScoringPolicyVersionId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_selection_decision_scoring_policy");

            entity.HasOne(d => d.SelectedOnboardingCandidateVersion).WithMany(p => p.SelectionDecisions)
                .HasForeignKey(d => d.SelectedOnboardingCandidateVersionId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("selection_decision_selected_onboarding_candidate_version_i_fkey");

            entity.HasOne(d => d.SelectedReactionPackVersion).WithMany(p => p.SelectionDecisions)
                .HasForeignKey(d => d.SelectedReactionPackVersionId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("selection_decision_selected_reaction_pack_version_id_fkey");
        });

        modelBuilder.Entity<SelectionDecisionAlternative>(entity =>
        {
            entity.HasKey(e => e.SelectionDecisionAlternativeId).HasName("selection_decision_alternative_pkey");

            entity.ToTable("selection_decision_alternative", "onboarding");

            entity.HasIndex(e => new { e.SelectionDecisionId, e.RankNo }, "selection_decision_alternativ_selection_decision_id_rank_no_key").IsUnique();

            entity.Property(e => e.SelectionDecisionAlternativeId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("selection_decision_alternative_id");
            entity.Property(e => e.Explanation)
                .HasColumnType("jsonb")
                .HasColumnName("explanation");
            entity.Property(e => e.OnboardingCandidateVersionId).HasColumnName("onboarding_candidate_version_id");
            entity.Property(e => e.RankNo).HasColumnName("rank_no");
            entity.Property(e => e.ReactionPackVersionId).HasColumnName("reaction_pack_version_id");
            entity.Property(e => e.SelectionDecisionId).HasColumnName("selection_decision_id");
            entity.Property(e => e.TotalScore)
                .HasPrecision(10, 6)
                .HasColumnName("total_score");

            entity.HasOne(d => d.OnboardingCandidateVersion).WithMany(p => p.SelectionDecisionAlternatives)
                .HasForeignKey(d => d.OnboardingCandidateVersionId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("selection_decision_alternativ_onboarding_candidate_version_fkey");

            entity.HasOne(d => d.ReactionPackVersion).WithMany(p => p.SelectionDecisionAlternatives)
                .HasForeignKey(d => d.ReactionPackVersionId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("selection_decision_alternative_reaction_pack_version_id_fkey");

            entity.HasOne(d => d.SelectionDecision).WithMany(p => p.SelectionDecisionAlternatives)
                .HasForeignKey(d => d.SelectionDecisionId)
                .HasConstraintName("selection_decision_alternative_selection_decision_id_fkey");
        });

        modelBuilder.Entity<SensitivityCategory>(entity =>
        {
            entity.HasKey(e => e.SensitivityCategoryId).HasName("sensitivity_category_pkey");

            entity.ToTable("sensitivity_category", "humor", tb => tb.HasComment("Słownik kategorii wrażliwości/safety dla materiału."));

            entity.HasIndex(e => e.CategoryKey, "ix_sensitivity_category_active").HasFilter("(is_active = true)");

            entity.HasIndex(e => e.CategoryKey, "sensitivity_category_category_key_key").IsUnique();

            entity.Property(e => e.SensitivityCategoryId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("sensitivity_category_id");
            entity.Property(e => e.CategoryKey)
                .HasComment("Stabilny klucz kategorii safety (EN).")
                .HasColumnName("category_key");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DisplayName).HasColumnName("display_name");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.SortOrder)
                .HasDefaultValue(100)
                .HasColumnName("sort_order");
        });

        modelBuilder.Entity<StudioComment>(entity =>
        {
            entity.HasKey(e => e.StudioCommentId).HasName("studio_comment_pkey");

            entity.ToTable("studio_comment", "studio");

            entity.Property(e => e.StudioCommentId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("studio_comment_id");
            entity.Property(e => e.AuthorAccountId).HasColumnName("author_account_id");
            entity.Property(e => e.CommentText).HasColumnName("comment_text");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.IsInternal)
                .HasDefaultValue(true)
                .HasColumnName("is_internal");
            entity.Property(e => e.TargetId)
                .HasComment("Typed target pointer; intentionally loose UUID to support multiple reviewed aggregates.")
                .HasColumnName("target_id");
            entity.Property(e => e.TargetType).HasColumnName("target_type");

            entity.HasOne(d => d.AuthorAccount).WithMany(p => p.StudioComments)
                .HasForeignKey(d => d.AuthorAccountId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("studio_comment_author_account_id_fkey");
        });

        modelBuilder.Entity<UserHumorInspiration>(entity =>
        {
            entity.HasKey(e => e.UserHumorInspirationId).HasName("user_humor_inspiration_pkey");

            entity.ToTable("user_humor_inspiration", "humor");

            entity.Property(e => e.UserHumorInspirationId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("user_humor_inspiration_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.ContentText).HasColumnName("content_text");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.InspirationType).HasColumnName("inspiration_type");
            entity.Property(e => e.LanguageCode)
                .HasDefaultValueSql("'pl-PL'::text")
                .HasColumnName("language_code");
            entity.Property(e => e.OnboardingSessionId).HasColumnName("onboarding_session_id");
            entity.Property(e => e.PrivacyStatus)
                .HasDefaultValueSql("'private'::text")
                .HasColumnName("privacy_status");
            entity.Property(e => e.RemovedAt).HasColumnName("removed_at");

            entity.HasOne(d => d.Account).WithMany(p => p.UserHumorInspirations)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("user_humor_inspiration_account_id_fkey");

            entity.HasOne(d => d.OnboardingSession).WithMany(p => p.UserHumorInspirations)
                .HasForeignKey(d => d.OnboardingSessionId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("user_humor_inspiration_onboarding_session_id_fkey");
        });

        modelBuilder.Entity<UserHumorInspirationAnalysis>(entity =>
        {
            entity.HasKey(e => e.UserHumorInspirationAnalysisId).HasName("user_humor_inspiration_analysis_pkey");

            entity.ToTable("user_humor_inspiration_analysis", "humor");

            entity.Property(e => e.UserHumorInspirationAnalysisId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("user_humor_inspiration_analysis_id");
            entity.Property(e => e.AiOperationExecutionId).HasColumnName("ai_operation_execution_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.HumorDimensionImpact)
                .HasColumnType("jsonb")
                .HasColumnName("humor_dimension_impact");
            entity.Property(e => e.StyleAnalysis)
                .HasColumnType("jsonb")
                .HasColumnName("style_analysis");
            entity.Property(e => e.UserHumorInspirationId).HasColumnName("user_humor_inspiration_id");

            entity.HasOne(d => d.AiOperationExecution).WithMany(p => p.UserHumorInspirationAnalyses)
                .HasForeignKey(d => d.AiOperationExecutionId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("user_humor_inspiration_analysis_ai_operation_execution_id_fkey");

            entity.HasOne(d => d.UserHumorInspiration).WithMany(p => p.UserHumorInspirationAnalyses)
                .HasForeignKey(d => d.UserHumorInspirationId)
                .HasConstraintName("user_humor_inspiration_analysis_user_humor_inspiration_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
