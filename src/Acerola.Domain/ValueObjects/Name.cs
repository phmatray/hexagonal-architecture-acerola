namespace Acerola.Domain.ValueObjects;

public sealed record Name
{
    public Name(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new NameShouldNotBeEmptyException();
        }

        Text = text;
    }

    public string Text { get; init; }

    public override string ToString()
        => Text;

    public static implicit operator Name(string text)
        => new(text);

    public static implicit operator string(Name name)
        => name.Text;
}