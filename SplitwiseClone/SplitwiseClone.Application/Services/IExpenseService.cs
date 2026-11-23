using SplitwiseClone.Domain;

namespace SplitwiseClone.Application.Services;

public interface IExpenseService
{
    Task<Expense> CreateEqualSplitExpenseAsync(
        string description,
        decimal totalCost,
        Guid paidByUserId,
        IReadOnlyCollection<Guid> participantUserIds,
        Guid? groupId = null,
        bool isPayment = false,
        CancellationToken cancellationToken = default);
}
