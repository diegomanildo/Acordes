
using Acordes.CodeAnalysis.Text;

namespace Acordes.CodeAnalysis.Syntax
{
    internal sealed class SyntaxToken
	{
		public SyntaxToken(SyntaxKind kind, int position, string text)
		{
            Kind = kind;
            Position = position;
            Text = text;
        }

        public SyntaxKind Kind { get; }
        public int Position { get; }
        public string Text { get; }
        public TextSpan Span => new TextSpan(Position, Text.Length);

        public override string ToString()
        {
            return $"[{Position}] -> \"{Text}\": {Kind}";
        }
    }
}