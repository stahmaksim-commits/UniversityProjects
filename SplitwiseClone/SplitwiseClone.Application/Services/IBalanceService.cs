namespace SplitwiseClone.Application.Services;

public record UserBalanceDto(
    Guid UserId,
    decimal PaidTotal,
    decimal OwedTotal,
    decimal NetBalance);

public interface IBalanceService
{
    Task<IReadOnlyList<UserBalanceDto>> GetGroupBalancesAsync(
        Guid groupId,
        CancellationToken cancellationToken = default);
}
