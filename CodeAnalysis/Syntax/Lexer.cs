using System.Runtime.CompilerServices;
using Acordes.CodeAnalysis.Text;

namespace Acordes.CodeAnalysis.Syntax
{
    internal sealed class Lexer(string text)
	{
		private readonly string m_text = text;

		private static readonly Dictionary<string, SyntaxKind> s_keywords = new()
		{
			{"dim", SyntaxKind.DiminishedToken},
			{"aug", SyntaxKind.AugmentedToken},
			{"sus", SyntaxKind.SuspendedToken},
			// {"add", SyntaxKind.AddToken},
			
			{"Maj", SyntaxKind.MajToken},
		};

		private int m_position = 0;
		private SyntaxKind m_kind; 
		private int m_start = 0;

		private int Length => m_position - m_start;
		private string TokenText => m_text.Substring(m_start, Length);

		public DiagnosticBag Diagnostics { get; } = new();

		private char Peek(int offset)
		{
			var index = offset + m_position;
			return index >= m_text.Length ? '\0' : m_text[index];
		}

		private char Current => Peek(0);

        public SourceText SourceText { get; } = SourceText.From(text);

        public SyntaxToken[] Lex()
		{
			var tokens = new List<SyntaxToken>();
			SyntaxToken t;

			do
			{
				t = NextToken();

				if (t.Kind is not SyntaxKind.Whitespace)
				{
					tokens.Add(t);
				}
			}
			while (t.Kind is not SyntaxKind.EndOfFileToken);

			return [.. tokens];
		}

		private SyntaxToken NextToken()
		{
			m_start = m_position;
			m_kind = SyntaxKind.BadToken;

			switch (Current)
			{
				case '\0':
					m_kind = SyntaxKind.EndOfFileToken;
					break;
				case >= '0' and <= '9':
					ReadNumber();
					break;
				case ' ' or '\t':
					ReadWhitespace();
					break;
				case >= 'A' and <= 'G':
					m_kind = SyntaxKind.NoteToken;
					m_position++;
					break;
				case 'm' or '-':
					m_kind = SyntaxKind.MinorToken;
					m_position++;
					break;
				case 'b':
					m_kind = SyntaxKind.BemolToken;
					m_position++;
					break;
				case 'x':
					m_kind = SyntaxKind.DoubleSharpToken;
					m_position++;
					break;
				case '#':
					m_kind = SyntaxKind.SharpToken;
					m_position++;
					break;
				case '/':
					m_kind = SyntaxKind.WithBaseInToken;
					m_position++;
					break;
				case '(':
					m_kind = SyntaxKind.OpenParenthesisToken;
					m_position++;
					break;
				case ')':
					m_kind = SyntaxKind.CloseParenthesisToken;
					m_position++;
					break;
				case ',':
					m_kind = SyntaxKind.CommaToken;
					m_position++;
					break;
				default:
					ReadText();
					break;
			}

			if (m_kind is SyntaxKind.BadToken)
			{
				Diagnostics.ReportBadToken(m_start, TokenText);
			}

			return new SyntaxToken(m_kind, m_start, TokenText);
		}

        private void ReadText()
        {
			while (char.IsLetter(Current))
			{
				m_position++;
			}

			if (s_keywords.TryGetValue(TokenText, out var value))
			{
				m_kind = value;
			}
        }

        private void ReadNumber()
        {
			while (Current is >= '0' and <= '9')
			{
				m_position++;
			}

			m_kind = SyntaxKind.NumberToken;
        }

        private void ReadWhitespace()
        {
			while (Current is ' ' or '\t')
			{
				m_position++;
			}

			m_kind = SyntaxKind.Whitespace;
        }
	}
}