using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PersonalManager.Models;

namespace PersonalManager.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Budget> Budgets { get; set; }

    public virtual DbSet<Note> Notes { get; set; }

    public virtual DbSet<NoteAttachment> NoteAttachments { get; set; }

    public virtual DbSet<NoteCategory> NoteCategories { get; set; }

    public virtual DbSet<NoteCategoryAssignment> NoteCategoryAssignments { get; set; }

    public virtual DbSet<NoteTag> NoteTags { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<RecurringTransaction> RecurringTransactions { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<Session> Sessions { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<TaskComment> TaskComments { get; set; }

    public virtual DbSet<TaskSubtask> TaskSubtasks { get; set; }

    public virtual DbSet<TaskTag> TaskTags { get; set; }

    public virtual DbSet<TodoList> TodoLists { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<TransactionCategory> TransactionCategories { get; set; }

    public virtual DbSet<TransactionTag> TransactionTags { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserProfile> UserProfiles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

        => optionsBuilder.UseNpgsql("Host=localhost;Database=PersonalManager;Username=postgres;Password=123;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("accounts_pkey");

            entity.ToTable("accounts");

            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.AccountName)
                .HasMaxLength(100)
                .HasColumnName("account_name");
            entity.Property(e => e.AccountType)
                .HasMaxLength(20)
                .HasColumnName("account_type");
            entity.Property(e => e.Balance)
                .HasPrecision(15, 2)
                .HasColumnName("balance");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.Currency)
                .HasMaxLength(3)
                .HasDefaultValueSql("'USD'::character varying")
                .HasColumnName("currency");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("true")
                .HasColumnName("is_active");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("accounts_user_id_fkey");
        });

        modelBuilder.Entity<Budget>(entity =>
        {
            entity.HasKey(e => e.BudgetId).HasName("budgets_pkey");

            entity.ToTable("budgets");

            entity.Property(e => e.BudgetId).HasColumnName("budget_id");
            entity.Property(e => e.Amount)
                .HasPrecision(15, 2)
                .HasColumnName("amount");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CurrentSpent)
                .HasPrecision(15, 2)
                .HasDefaultValueSql("0")
                .HasColumnName("current_spent");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.Period)
                .HasMaxLength(10)
                .HasColumnName("period");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Category).WithMany(p => p.Budgets)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("budgets_category_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Budgets)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("budgets_user_id_fkey");
        });

        modelBuilder.Entity<Note>(entity =>
        {
            entity.HasKey(e => e.NoteId).HasName("notes_pkey");

            entity.ToTable("notes");

            entity.Property(e => e.NoteId).HasColumnName("note_id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.IsArchived)
                .HasDefaultValueSql("false")
                .HasColumnName("is_archived");
            entity.Property(e => e.IsPinned)
                .HasDefaultValueSql("false")
                .HasColumnName("is_pinned");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Notes)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("notes_user_id_fkey");
        });

        modelBuilder.Entity<NoteAttachment>(entity =>
        {
            entity.HasKey(e => e.AttachmentId).HasName("note_attachments_pkey");

            entity.ToTable("note_attachments");

            entity.Property(e => e.AttachmentId).HasColumnName("attachment_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.FileName)
                .HasMaxLength(100)
                .HasColumnName("file_name");
            entity.Property(e => e.FileSize).HasColumnName("file_size");
            entity.Property(e => e.FileType)
                .HasMaxLength(50)
                .HasColumnName("file_type");
            entity.Property(e => e.FileUrl)
                .HasMaxLength(255)
                .HasColumnName("file_url");
            entity.Property(e => e.NoteId).HasColumnName("note_id");

            entity.HasOne(d => d.Note).WithMany(p => p.NoteAttachments)
                .HasForeignKey(d => d.NoteId)
                .HasConstraintName("note_attachments_note_id_fkey");
        });

        modelBuilder.Entity<NoteCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("note_categories_pkey");

            entity.ToTable("note_categories");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Color)
                .HasMaxLength(20)
                .HasColumnName("color");
            entity.Property(e => e.Icon)
                .HasMaxLength(50)
                .HasColumnName("icon");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.NoteCategories)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("note_categories_user_id_fkey");
        });

        modelBuilder.Entity<NoteCategoryAssignment>(entity =>
        {
            entity.HasKey(e => e.AssignmentId).HasName("note_category_assignments_pkey");

            entity.ToTable("note_category_assignments");

            entity.HasIndex(e => new { e.NoteId, e.CategoryId }, "note_category_assignments_note_id_category_id_key").IsUnique();

            entity.Property(e => e.AssignmentId).HasColumnName("assignment_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.NoteId).HasColumnName("note_id");

            entity.HasOne(d => d.Category).WithMany(p => p.NoteCategoryAssignments)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("note_category_assignments_category_id_fkey");

            entity.HasOne(d => d.Note).WithMany(p => p.NoteCategoryAssignments)
                .HasForeignKey(d => d.NoteId)
                .HasConstraintName("note_category_assignments_note_id_fkey");
        });

        modelBuilder.Entity<NoteTag>(entity =>
        {
            entity.HasKey(e => e.NoteTagId).HasName("note_tags_pkey");

            entity.ToTable("note_tags");

            entity.HasIndex(e => new { e.NoteId, e.TagId }, "note_tags_note_id_tag_id_key").IsUnique();

            entity.Property(e => e.NoteTagId).HasColumnName("note_tag_id");
            entity.Property(e => e.NoteId).HasColumnName("note_id");
            entity.Property(e => e.TagId).HasColumnName("tag_id");

            entity.HasOne(d => d.Note).WithMany(p => p.NoteTags)
                .HasForeignKey(d => d.NoteId)
                .HasConstraintName("note_tags_note_id_fkey");

            entity.HasOne(d => d.Tag).WithMany(p => p.NoteTags)
                .HasForeignKey(d => d.TagId)
                .HasConstraintName("note_tags_tag_id_fkey");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("notifications_pkey");

            entity.ToTable("notifications");

            entity.Property(e => e.NotificationId).HasColumnName("notification_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.IsRead)
                .HasDefaultValueSql("false")
                .HasColumnName("is_read");
            entity.Property(e => e.Message).HasColumnName("message");
            entity.Property(e => e.RelatedEntityId).HasColumnName("related_entity_id");
            entity.Property(e => e.RelatedEntityType)
                .HasMaxLength(20)
                .HasColumnName("related_entity_type");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("notifications_user_id_fkey");
        });

        modelBuilder.Entity<RecurringTransaction>(entity =>
        {
            entity.HasKey(e => e.RecurringId).HasName("recurring_transactions_pkey");

            entity.ToTable("recurring_transactions");

            entity.Property(e => e.RecurringId).HasColumnName("recurring_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.Amount)
                .HasPrecision(15, 2)
                .HasColumnName("amount");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.Frequency)
                .HasMaxLength(10)
                .HasColumnName("frequency");
            entity.Property(e => e.NextDate).HasColumnName("next_date");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Account).WithMany(p => p.RecurringTransactions)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("recurring_transactions_account_id_fkey");

            entity.HasOne(d => d.Category).WithMany(p => p.RecurringTransactions)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("recurring_transactions_category_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.RecurringTransactions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("recurring_transactions_user_id_fkey");
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("reports_pkey");

            entity.ToTable("reports");

            entity.Property(e => e.ReportId).HasColumnName("report_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.LastViewed).HasColumnName("last_viewed");
            entity.Property(e => e.Parameters)
                .HasColumnType("jsonb")
                .HasColumnName("parameters");
            entity.Property(e => e.ReportType)
                .HasMaxLength(50)
                .HasColumnName("report_type");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Reports)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("reports_user_id_fkey");
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasKey(e => e.SessionId).HasName("sessions_pkey");

            entity.ToTable("sessions");

            entity.Property(e => e.SessionId).HasColumnName("session_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
            entity.Property(e => e.IpAddress)
                .HasMaxLength(45)
                .HasColumnName("ip_address");
            entity.Property(e => e.Token)
                .HasMaxLength(255)
                .HasColumnName("token");
            entity.Property(e => e.UserAgent).HasColumnName("user_agent");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Sessions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("sessions_user_id_fkey");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.TagId).HasName("tags_pkey");

            entity.ToTable("tags");

            entity.Property(e => e.TagId).HasColumnName("tag_id");
            entity.Property(e => e.Color)
                .HasMaxLength(20)
                .HasColumnName("color");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Tags)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("tags_user_id_fkey");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.TaskId).HasName("tasks_pkey");

            entity.ToTable("tasks");

            entity.Property(e => e.TaskId).HasColumnName("task_id");
            entity.Property(e => e.CompletedAt).HasColumnName("completed_at");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DueDate).HasColumnName("due_date");
            entity.Property(e => e.ListId).HasColumnName("list_id");
            entity.Property(e => e.Priority)
                .HasMaxLength(10)
                .HasColumnName("priority");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasColumnName("status");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.TodoList).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.ListId)
                .HasConstraintName("tasks_list_id_fkey");
        });

        modelBuilder.Entity<TaskComment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("task_comments_pkey");

            entity.ToTable("task_comments");

            entity.Property(e => e.CommentId).HasColumnName("comment_id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.TaskId).HasColumnName("task_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Task).WithMany(p => p.TaskComments)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("task_comments_task_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.TaskComments)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("task_comments_user_id_fkey");
        });

        modelBuilder.Entity<TaskSubtask>(entity =>
        {
            entity.HasKey(e => e.SubtaskId).HasName("task_subtasks_pkey");

            entity.ToTable("task_subtasks");

            entity.Property(e => e.SubtaskId).HasColumnName("subtask_id");
            entity.Property(e => e.CompletedAt).HasColumnName("completed_at");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.IsCompleted)
                .HasDefaultValueSql("false")
                .HasColumnName("is_completed");
            entity.Property(e => e.TaskId).HasColumnName("task_id");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");

            entity.HasOne(d => d.Task).WithMany(p => p.TaskSubtasks)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("task_subtasks_task_id_fkey");
        });

        modelBuilder.Entity<TaskTag>(entity =>
        {
            entity.HasKey(e => e.TaskTagId).HasName("task_tags_pkey");

            entity.ToTable("task_tags");

            entity.HasIndex(e => new { e.TaskId, e.TagId }, "task_tags_task_id_tag_id_key").IsUnique();

            entity.Property(e => e.TaskTagId).HasColumnName("task_tag_id");
            entity.Property(e => e.TagId).HasColumnName("tag_id");
            entity.Property(e => e.TaskId).HasColumnName("task_id");

            entity.HasOne(d => d.Tag).WithMany(p => p.TaskTags)
                .HasForeignKey(d => d.TagId)
                .HasConstraintName("task_tags_tag_id_fkey");

            entity.HasOne(d => d.Task).WithMany(p => p.TaskTags)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("task_tags_task_id_fkey");
        });

        modelBuilder.Entity<TodoList>(entity =>
        {
            entity.HasKey(e => e.ListId).HasName("todo_lists_pkey");

            entity.ToTable("todo_lists");

            entity.Property(e => e.ListId).HasColumnName("list_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IsArchived)
                .HasDefaultValueSql("false")
                .HasColumnName("is_archived");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.TodoLists)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("todo_lists_user_id_fkey");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("transactions_pkey");

            entity.ToTable("transactions");

            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.Amount)
                .HasPrecision(15, 2)
                .HasColumnName("amount");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IsRecurring)
                .HasDefaultValueSql("false")
                .HasColumnName("is_recurring");
            entity.Property(e => e.TransactionType)
                .HasMaxLength(10)
                .HasColumnName("transaction_type");

            entity.HasOne(d => d.Account).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("transactions_account_id_fkey");

            entity.HasOne(d => d.Category).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("transactions_category_id_fkey");
        });

        modelBuilder.Entity<TransactionCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("transaction_categories_pkey");

            entity.ToTable("transaction_categories");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Color)
                .HasMaxLength(20)
                .HasColumnName("color");
            entity.Property(e => e.Icon)
                .HasMaxLength(50)
                .HasColumnName("icon");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.ParentId).HasColumnName("parent_id");
            entity.Property(e => e.Type)
                .HasMaxLength(10)
                .HasColumnName("type");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("transaction_categories_parent_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.TransactionCategories)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("transaction_categories_user_id_fkey");
        });

        modelBuilder.Entity<TransactionTag>(entity =>
        {
            entity.HasKey(e => e.TransactionTagId).HasName("transaction_tags_pkey");

            entity.ToTable("transaction_tags");

            entity.HasIndex(e => new { e.TransactionId, e.TagId }, "transaction_tags_transaction_id_tag_id_key").IsUnique();

            entity.Property(e => e.TransactionTagId).HasColumnName("transaction_tag_id");
            entity.Property(e => e.TagId).HasColumnName("tag_id");
            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");

            entity.HasOne(d => d.Tag).WithMany(p => p.TransactionTags)
                .HasForeignKey(d => d.TagId)
                .HasConstraintName("transaction_tags_tag_id_fkey");

            entity.HasOne(d => d.Transaction).WithMany(p => p.TransactionTags)
                .HasForeignKey(d => d.TransactionId)
                .HasConstraintName("transaction_tags_transaction_id_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.HasIndex(e => e.Username, "users_username_key").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("true")
                .HasColumnName("is_active");
            entity.Property(e => e.LastLogin).HasColumnName("last_login");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
        });

        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.HasKey(e => e.ProfileId).HasName("user_profiles_pkey");

            entity.ToTable("user_profiles");

            entity.Property(e => e.ProfileId).HasColumnName("profile_id");
            entity.Property(e => e.AvatarUrl)
                .HasMaxLength(255)
                .HasColumnName("avatar_url");
            entity.Property(e => e.CurrencyPreference)
                .HasMaxLength(3)
                .HasDefaultValueSql("'USD'::character varying")
                .HasColumnName("currency_preference");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.Timezone)
                .HasMaxLength(50)
                .HasColumnName("timezone");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.UserProfiles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("user_profiles_user_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
