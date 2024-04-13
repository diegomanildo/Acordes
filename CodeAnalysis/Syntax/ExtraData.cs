namespace Acordes.CodeAnalysis.Syntax
{
    public record struct ExtraData(SyntaxKind? Modification, int Grade)
    {
        public static readonly ExtraData Null = new(null, 0);

        public static implicit operator (SyntaxKind? Modification, int Grade)(ExtraData value)
        {
            return (value.Modification, value.Grade);
        }

        public static implicit operator ExtraData((SyntaxKind? Modification, int Grade) value)
        {
            return new ExtraData(value.Modification, value.Grade);
        }
    }
}