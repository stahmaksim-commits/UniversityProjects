using SplitwiseClone.Domain;

namespace SplitwiseClone.Application.Services;

public class ExpenseService : IExpenseService
{
    private readonly IRepository<Expense> _expenseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ExpenseService(IRepository<Expense> expenseRepository, IUnitOfWork unitOfWork)
    {
        _expenseRepository = expenseRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Expense> CreateEqualSplitExpenseAsync(
        string description,
        decimal totalCost,
        Guid paidByUserId,
        IReadOnlyCollection<Guid> participantUserIds,
        Guid? groupId = null,
        bool isPayment = false,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description is required.", nameof(description));

        if (totalCost <= 0)
            throw new ArgumentException("Total cost must be greater than zero.", nameof(totalCost));

        var participants = participantUserIds
            .Where(id => id != Guid.Empty)
            .Distinct()
            .ToList();

        if (participants.Count == 0)
            throw new ArgumentException("At least one participant is required.", nameof(participantUserIds));

        var expenseId = Guid.NewGuid();
        var now = DateTime.UtcNow;
        var roundedTotal = Math.Round(totalCost, 2, MidpointRounding.AwayFromZero);
        var baseShare = Math.Truncate((roundedTotal / participants.Count) * 100m) / 100m;
        var extraCents = (int)Math.Round(
            (roundedTotal - baseShare * participants.Count) * 100m,
            MidpointRounding.AwayFromZero);

        var splits = new List<ExpenseSplit>();
        for (var i = 0; i < participants.Count; i++)
        {
            var owed = baseShare + (i < extraCents ? 0.01m : 0m);
            splits.Add(new ExpenseSplit
            {
                Id = Guid.NewGuid(),
                ExpenseId = expenseId,
                UserId = participants[i],
                PaidShare = participants[i] == paidByUserId ? roundedTotal : 0m,
                OwedShare = Math.Round(owed, 2, MidpointRounding.AwayFromZero)
            });
        }

        var expense = new Expense
        {
            Id = expenseId,
            Description = description.Trim(),
            TotalCost = roundedTotal,
            Date = now,
            CreatedAt = now,
            CreatedByUserId = paidByUserId,
            GroupId = groupId,
            IsPayment = isPayment,
            Splits = splits
        };

        await _expenseRepository.AddAsync(expense);
        await _unitOfWork.SaveChangesAsync();

        return expense;
    }
}