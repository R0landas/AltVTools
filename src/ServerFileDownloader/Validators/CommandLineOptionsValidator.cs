using FluentValidation;

namespace ServerFileDownloader.Validators;

public class CommandLineOptionsValidator : AbstractValidator<CommandLineOptions>
{
    private static readonly string[] AvailableOperatingSystems = ["x64_linux", "x64_win32"];
    private static readonly string[] AvailableChannels = ["dev", "rc", "release"];

    public CommandLineOptionsValidator()
    {
        RuleFor(x => x.Platform)
            .Must(value => AvailableOperatingSystems.Contains(value))
            .WithMessage("Parameter os must have a value of 'x64_win32' or 'x64_linux'");
        
        RuleFor(x => x.Channel)
            .Must(value => AvailableChannels.Contains(value))
            .WithMessage("Parameter channel must be either 'dev', 'rc', 'release'");
    }
}