namespace SplitwiseClone.Domain;

public class Expense
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Description { get; set; } = string.Empty;
    public decimal TotalCost { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;

    public Guid? GroupId { get; set; }
    public Group? Group { get; set; }

    public Guid CreatedByUserId { get; set; }
    public ApplicationUser CreatedByUser { get; set; } = null!;

    public bool IsPayment { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<ExpenseSplit> Splits { get; set; } = new();
}
