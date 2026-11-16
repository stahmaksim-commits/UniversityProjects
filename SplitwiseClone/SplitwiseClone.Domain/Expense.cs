namespace SplitwiseClone.Domain;

public class Expense
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public Guid PaidById { get; set; }
    public User PaidBy { get; set; } = null!;
    public Guid GroupId { get; set; }
    public Group Group { get; set; } = null!;
}