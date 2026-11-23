namespace SplitwiseClone.Domain;

public class ExpenseSplit
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid ExpenseId { get; set; }
    public Expense Expense { get; set; } = null!;

    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;

    public decimal PaidShare { get; set; }
    public decimal OwedShare { get; set; }
}
