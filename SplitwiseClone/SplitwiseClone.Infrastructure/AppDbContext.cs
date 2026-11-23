using Microsoft.EntityFrameworkCore;
using SplitwiseClone.Domain;

namespace SplitwiseClone.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<ApplicationUser> Users => Set<ApplicationUser>();
    public DbSet<Group> Groups => Set<Group>();
    public DbSet<GroupMember> GroupMembers => Set<GroupMember>();
    public DbSet<Expense> Expenses => Set<Expense>();
    public DbSet<ExpenseSplit> ExpenseSplits => Set<ExpenseSplit>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<GroupMember>().HasKey(gm => new { gm.GroupId, gm.UserId });
        modelBuilder.Entity<GroupMember>().HasOne(gm => gm.Group).WithMany(g => g.Members).HasForeignKey(gm => gm.GroupId);
        modelBuilder.Entity<GroupMember>().HasOne(gm => gm.User).WithMany(u => u.GroupMemberships).HasForeignKey(gm => gm.UserId);
        modelBuilder.Entity<Expense>().HasOne(e => e.CreatedByUser).WithMany(u => u.CreatedExpenses).HasForeignKey(e => e.CreatedByUserId);
        modelBuilder.Entity<Expense>().HasOne(e => e.Group).WithMany(g => g.Expenses).HasForeignKey(e => e.GroupId);
        modelBuilder.Entity<ExpenseSplit>().HasOne(es => es.Expense).WithMany(e => e.Splits).HasForeignKey(es => es.ExpenseId);
        modelBuilder.Entity<ExpenseSplit>().HasOne(es => es.User).WithMany(u => u.ExpenseSplits).HasForeignKey(es => es.UserId);
    }
}
