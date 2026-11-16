namespace SplitwiseClone.Domain;

public class Group
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public List<User> Members { get; set; } = new();
    public List<Expense> Expenses { get; set; } = new();
}