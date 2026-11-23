using Moq;
using SplitwiseClone.Application;
using SplitwiseClone.Application.Services;
using SplitwiseClone.Domain;
using Xunit;

namespace SplitwiseClone.Tests;

public class BalanceServiceTests
{
    [Fact]
    public async Task GetGroupBalancesAsync_GroupNotFound_ThrowsKeyNotFoundException()
    {
        var groupRepoMock = new Mock<IRepository<Group>>();
        var splitRepoMock = new Mock<IRepository<ExpenseSplit>>();
        var service = new BalanceService(groupRepoMock.Object, splitRepoMock.Object);

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            service.GetGroupBalancesAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task GetGroupBalancesAsync_ValidGroupAndSplits_CalculatesPaidOwedAndNet()
    {
        var groupId = Guid.NewGuid();
        var expenseId = Guid.NewGuid();
        var userA = Guid.NewGuid();
        var userB = Guid.NewGuid();

        var group = new Group { Id = groupId, Name = "Trip" };
        group.Members.Add(new GroupMember { GroupId = groupId, UserId = userA });
        group.Members.Add(new GroupMember { GroupId = groupId, UserId = userB });
        group.Expenses.Add(new Expense { Id = expenseId, GroupId = groupId, Description = "Hotel", TotalCost = 100m });

        var splits = new List<ExpenseSplit>
        {
            new ExpenseSplit { ExpenseId = expenseId, UserId = userA, PaidShare = 100m, OwedShare = 50m },
            new ExpenseSplit { ExpenseId = expenseId, UserId = userB, PaidShare = 0m, OwedShare = 50m }
        };

        var groupRepoMock = new Mock<IRepository<Group>>();
        groupRepoMock.Setup(r => r.GetByIdAsync(groupId)).ReturnsAsync(group);

        var splitRepoMock = new Mock<IRepository<ExpenseSplit>>();
        splitRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(splits);

        var service = new BalanceService(groupRepoMock.Object, splitRepoMock.Object);

        var balances = await service.GetGroupBalancesAsync(groupId);

        var balA = balances.Single(b => b.UserId == userA);
        var balB = balances.Single(b => b.UserId == userB);

        Assert.Equal(100m, balA.PaidTotal);
        Assert.Equal(50m, balA.OwedTotal);
        Assert.Equal(50m, balA.NetBalance);

        Assert.Equal(0m, balB.PaidTotal);
        Assert.Equal(50m, balB.OwedTotal);
        Assert.Equal(-50m, balB.NetBalance);
    }
}