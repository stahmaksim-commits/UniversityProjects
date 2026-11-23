using Microsoft.AspNetCore.Identity;

namespace SplitwiseClone.Infrastructure.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    public string? DisplayName { get; set; }
}
