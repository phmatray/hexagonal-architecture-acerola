namespace Acerola.Domain.ValueObjects;

public sealed partial record SSN
{
    public SSN(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new SSNShouldNotBeEmptyException();
        }

        Regex regex = SSNValidationRegex();
        Match match = regex.Match(text);

        if (!match.Success)
        {
            throw new InvalidSSNException();
        }

        Text = text;
    }

    public string Text { get; private set; }

    public override string ToString() => Text;

    public static implicit operator SSN(string text) => new(text);

    public static implicit operator string(SSN ssn) => ssn.Text;

    [GeneratedRegex(@"^\d{6,8}[-|(\s)]{0,1}\d{4}$")]
    private static partial Regex SSNValidationRegex();
}