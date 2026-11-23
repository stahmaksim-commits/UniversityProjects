namespace SplitwiseClone.Domain;

public class Group
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<GroupMember> Members { get; set; } = new();
    public List<Expense> Expenses { get; set; } = new();
}
