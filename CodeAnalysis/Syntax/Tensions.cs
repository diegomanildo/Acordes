namespace Acordes.CodeAnalysis.Syntax
{
    public sealed record Tensions(Tension Tension = Tension.None, bool IsMaj = false)
    {
        public static readonly Tensions Null = new();
    }
}