using SplitwiseClone.Domain;

namespace SplitwiseClone.Application.Services;

public class BalanceService : IBalanceService
{
    private readonly IRepository<Group> _groupRepository;
    private readonly IRepository<ExpenseSplit> _splitRepository;

    public BalanceService(
        IRepository<Group> groupRepository,
        IRepository<ExpenseSplit> splitRepository)
    {
        _groupRepository = groupRepository;
        _splitRepository = splitRepository;
    }

    public async Task<IReadOnlyList<UserBalanceDto>> GetGroupBalancesAsync(
        Guid groupId,
        CancellationToken cancellationToken = default)
    {
        var group = await _groupRepository.GetByIdAsync(groupId);
        if (group is null)
            throw new KeyNotFoundException($"Group {groupId} not found.");

        var allSplits = await _splitRepository.GetAllAsync();
        var expenseIds = group.Expenses.Select(e => e.Id).ToHashSet();
        var memberIds = group.Members.Select(m => m.UserId).Distinct().ToList();
        var splits = allSplits.Where(s => expenseIds.Contains(s.ExpenseId)).ToList();
        var userIds = memberIds.Union(splits.Select(s => s.UserId)).Distinct();

        var result = new List<UserBalanceDto>();
        foreach (var userId in userIds)
        {
            var userSplits = splits.Where(s => s.UserId == userId).ToList();
            var paid = Math.Round(userSplits.Sum(s => s.PaidShare), 2, MidpointRounding.AwayFromZero);
            var owed = Math.Round(userSplits.Sum(s => s.OwedShare), 2, MidpointRounding.AwayFromZero);
            var net = Math.Round(paid - owed, 2, MidpointRounding.AwayFromZero);
            result.Add(new UserBalanceDto(userId, paid, owed, net));
        }

        return result;
    }
}