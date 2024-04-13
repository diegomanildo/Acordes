using Acordes.CodeAnalysis.Text;

namespace Acordes.CodeAnalysis
{
    public sealed record Diagnostic(TextSpan Span, string Text)
    {
        public override string ToString() => Text;
    }
}