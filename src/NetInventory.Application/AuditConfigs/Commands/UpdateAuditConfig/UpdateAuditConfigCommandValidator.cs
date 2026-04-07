using FluentValidation;
using NetInventory.Resources;

namespace NetInventory.Application.AuditConfigs.Commands.UpdateAuditConfig;

public sealed class UpdateAuditConfigCommandValidator : AbstractValidator<UpdateAuditConfigCommand>
{
    private static readonly string[] AllowedMethods = ["GET", "POST", "PUT", "DELETE", "PATCH", "*"];

    public UpdateAuditConfigCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage(Messages.Val_AuditConfig_IdRequired);

        RuleFor(x => x.Method)
            .NotEmpty().WithMessage(Messages.Val_AuditConfig_HttpMethodRequired)
            .Must(m => AllowedMethods.Contains(m.ToUpperInvariant()))
            .WithMessage(Messages.Val_AuditConfig_HttpMethodInvalid);

        RuleFor(x => x.UrlPattern)
            .NotEmpty().WithMessage(Messages.Val_AuditConfig_UrlPatternRequired)
            .MaximumLength(500).WithMessage(Messages.Val_AuditConfig_UrlPatternMaxLength)
            .Must(u => u.StartsWith('/'))
            .WithMessage(Messages.Val_AuditConfig_UrlPatternStartsWithSlash);

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage(Messages.Val_AuditConfig_DescriptionRequired)
            .MaximumLength(200).WithMessage(Messages.Val_AuditConfig_DescriptionMaxLength);
    }
}
