using System.Collections;
using Acordes.CodeAnalysis.Syntax;

namespace Acordes.CodeAnalysis.Text
{
    public sealed class DiagnosticBag : IEnumerable<Diagnostic>
    {
        private readonly List<Diagnostic> m_diagnostics = [];

        public IEnumerator<Diagnostic> GetEnumerator() => m_diagnostics.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private void Report(TextSpan span, string text)
        {
            var diagnostic = new Diagnostic(span, text);
            m_diagnostics.Add(diagnostic);
        }

        public void ReportInvalidNumber(TextSpan span, string number)
        {
            var msg = $"The number '{number}' is not invalid";
            Report(span, msg);
        }

        public void ReportBadToken(int position, char c)
        {
            ReportBadToken(position, c.ToString());
        }

        public void ReportUnexpectedToken(TextSpan span, SyntaxKind current, SyntaxKind expected)
        {
            var msg = $"Unexpected token <{current}>, expected <{expected}>.";
			Report(span, msg);
        }

        public void ReportBadToken(int position, string text)
        {
            var msg = $"Bad input: '{text}'.";
            Report(new TextSpan(position, text.Length), msg);
        }

        public void ReportTokenNotFound(TextSpan span, SyntaxKind kind)
        {
            var msg = $"{kind} not found.";
            Report(span, msg);
        }
    }
}