using System;
using System.Collections.Generic;

namespace PersonalManager.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? LastLogin { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Account> Accounts { get; } = new List<Account>();

    public virtual ICollection<Budget> Budgets { get; } = new List<Budget>();

    public virtual ICollection<NoteCategory> NoteCategories { get; } = new List<NoteCategory>();

    public virtual ICollection<Note> Notes { get; } = new List<Note>();

    public virtual ICollection<Notification> Notifications { get; } = new List<Notification>();

    public virtual ICollection<RecurringTransaction> RecurringTransactions { get; } = new List<RecurringTransaction>();

    public virtual ICollection<Report> Reports { get; } = new List<Report>();

    public virtual ICollection<Session> Sessions { get; } = new List<Session>();

    public virtual ICollection<Tag> Tags { get; } = new List<Tag>();

    public virtual ICollection<TaskComment> TaskComments { get; } = new List<TaskComment>();

    public virtual ICollection<TodoList> TodoLists { get; } = new List<TodoList>();

    public virtual ICollection<TransactionCategory> TransactionCategories { get; } = new List<TransactionCategory>();

    public virtual ICollection<UserProfile> UserProfiles { get; } = new List<UserProfile>();
}
