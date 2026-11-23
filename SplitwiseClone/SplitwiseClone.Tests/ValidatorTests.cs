using SplitwiseClone.Application.Validators;
using Xunit;

namespace SplitwiseClone.Tests;

public class ValidatorTests
{
    [Fact]
    public void Validator_RejectsEmptyDescription()
    {
        var validator = new CreateEqualSplitExpenseRequestValidator();
        var req = new CreateEqualSplitExpenseRequest("", 100m, Guid.NewGuid(), [Guid.NewGuid()]);
        Assert.False(validator.Validate(req).IsValid);
    }

    [Fact]
    public void Validator_AcceptsValidRequest()
    {
        var validator = new CreateEqualSplitExpenseRequestValidator();
        var user = Guid.NewGuid();
        var req = new CreateEqualSplitExpenseRequest("Lunch", 200m, user, [user]);
        Assert.True(validator.Validate(req).IsValid);
    }
}
