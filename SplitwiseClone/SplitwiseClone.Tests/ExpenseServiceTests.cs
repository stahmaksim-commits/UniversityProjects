using Moq;
using SplitwiseClone.Application;
using SplitwiseClone.Application.Services;
using SplitwiseClone.Domain;
using Xunit;

namespace SplitwiseClone.Tests;

public class ExpenseServiceTests
{
    [Fact]
    public async Task CreateEqualSplitExpenseAsync_100SplitThreeWays_SplitsCentsSoSumEqualsTotal()
    {
        var repoMock = new Mock<IRepository<Expense>>();
        var uowMock = new Mock<IUnitOfWork>();

        var service = new ExpenseService(repoMock.Object, uowMock.Object);
        var payer = Guid.NewGuid();
        var u2 = Guid.NewGuid();
        var u3 = Guid.NewGuid();

        var expense = await service.CreateEqualSplitExpenseAsync(
            "Dinner",
            100m,
            payer,
            new[] { payer, u2, u3 });

        Assert.NotNull(expense);
        Assert.Equal(100m, expense.TotalCost);
        Assert.Equal(3, expense.Splits.Count);
        Assert.Equal(100m, expense.Splits.Sum(s => s.PaidShare));
        Assert.Equal(100m, expense.Splits.Sum(s => s.OwedShare));
        Assert.Equal(33.34m, expense.Splits.First(s => s.UserId == payer).OwedShare);
        Assert.Equal(33.33m, expense.Splits.First(s => s.UserId == u2).OwedShare);
        Assert.Equal(33.33m, expense.Splits.First(s => s.UserId == u3).OwedShare);
    }

    [Fact]
    public async Task CreateEqualSplitExpenseAsync_EmptyDescription_ThrowsArgumentException()
    {
        var repoMock = new Mock<IRepository<Expense>>();
        var uowMock = new Mock<IUnitOfWork>();
        var service = new ExpenseService(repoMock.Object, uowMock.Object);

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.CreateEqualSplitExpenseAsync("   ", 50m, Guid.NewGuid(), new[] { Guid.NewGuid() }));
    }
}