using FluentValidation;

namespace SplitwiseClone.Application.Validators;

public record CreateEqualSplitExpenseRequest(
    string Description,
    decimal TotalCost,
    Guid PaidByUserId,
    IReadOnlyCollection<Guid> ParticipantUserIds,
    Guid? GroupId = null,
    bool IsPayment = false);

public class CreateEqualSplitExpenseRequestValidator : AbstractValidator<CreateEqualSplitExpenseRequest>
{
    public CreateEqualSplitExpenseRequestValidator()
    {
        RuleFor(x => x.Description).NotEmpty().MaximumLength(200);
        RuleFor(x => x.TotalCost).GreaterThan(0m);
        RuleFor(x => x.PaidByUserId).NotEqual(Guid.Empty);
        RuleFor(x => x.ParticipantUserIds).NotNull().Must(ids => ids.Count > 0).Must(ids => ids.All(id => id != Guid.Empty));
    }
}
