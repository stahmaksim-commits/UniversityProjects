namespace SplitwiseClone.Domain;

public class ApplicationUser
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<GroupMember> GroupMemberships { get; set; } = new();
    public List<Expense> CreatedExpenses { get; set; } = new();
    public List<ExpenseSplit> ExpenseSplits { get; set; } = new();
}
