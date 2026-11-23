using Microsoft.AspNetCore.Identity;

namespace SplitwiseClone.Domain;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<GroupMember> GroupMemberships { get; set; } = new();
    public List<Expense> CreatedExpenses { get; set; } = new();
    public List<ExpenseSplit> ExpenseSplits { get; set; } = new();
}
