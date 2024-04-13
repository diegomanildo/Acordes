using Acordes.CodeAnalysis.Chords;
using Acordes.CodeAnalysis.Text;

namespace Acordes.CodeAnalysis.Syntax
{
    public sealed class Parser
	{
        private readonly SyntaxToken[] m_tokens;
		private int m_position = 0;
		
        public Parser(string text)
        {
			var lexer = new Lexer(text);
			m_tokens = lexer.Lex();

			// foreach (var t in m_tokens)
			// {
			// 	Console.WriteLine(t);
			// }

			SourceText = lexer.SourceText;
			Diagnostics = lexer.Diagnostics;
        }

        public SourceText SourceText { get; }
		public DiagnosticBag Diagnostics { get; }

		private SyntaxToken Peek(int offset)
		{
			var index = offset + m_position;
			return m_tokens[index >= m_tokens.Length ? ^1 : index];
		}

		private SyntaxToken Current => Peek(0);
		private SyntaxToken Lookahead => Peek(1);

		private SyntaxToken NextToken()
		{
			var c = Current;
			m_position++;
			return c;
		}

		private SyntaxToken MatchToken(SyntaxKind expected)
		{
			if (Current.Kind == expected)
			{
				return NextToken();
			}
			else
			{
				if (!Diagnostics.Any())
                {
                    Diagnostics.ReportUnexpectedToken(Current.Span, Current.Kind, expected);
                }

                return new SyntaxToken(expected, Current.Position, Current.Text);
			}
		}
		
		public ChordInfo Parse()
		{
			if (Diagnostics.Any())
			{
				return new ChordInfo(NoteKind.None);
			}

			var note = ParseNote();
			var tensions = ParseTensions();
			var chordKind = ParseChordKind();
			var suspended = ParseSuspended(chordKind);
			tensions = tensions == Tensions.Null ? ParseTensions() : tensions;
			var extraInfo = ParseExtraInfo();
			var bassNote = ParseBassNote();

			var endOfFile = MatchToken(SyntaxKind.EndOfFileToken);

			return new ChordInfo(note, chordKind, tensions, suspended, bassNote, extraInfo);
		}

        private NoteKind ParseNote()
        {
			var noteToken = MatchToken(SyntaxKind.NoteToken);
			var note = noteToken.Text;

			while (Current.Kind is SyntaxKind.SharpToken or SyntaxKind.DoubleSharpToken or SyntaxKind.BemolToken)
			{
				var modification = NextToken();
				note += modification.Text;
			}

			return SyntaxFacts.ConvertToNote(note);
        }

        private ChordKind ParseChordKind()
        {
            var kind = Current.Kind switch
			{
				SyntaxKind.AugmentedToken => ChordKind.Augmented,
				SyntaxKind.DiminishedToken => ChordKind.Diminished,
				SyntaxKind.SuspendedToken => ChordKind.Suspended,
				SyntaxKind.MinorToken => ChordKind.Minor,
				SyntaxKind.MajorToken or _ => ChordKind.Mayor,
			};

			if (kind is not ChordKind.Mayor)
			{
				m_position++;
			}

			return kind;
        }

        private int ParseSuspended(ChordKind kind)
        {
			if (kind is not ChordKind.Suspended)
			{
				return 0;
			}

			var token = MatchToken(SyntaxKind.NumberToken);
			
			if (token.Text is not "4" and not "2")
			{
				Diagnostics.ReportInvalidNumber(token.Span, token.Text);
			}

			return Diagnostics.Any() ? 0 : Convert.ToInt32(token.Text);
        }

        private Tensions ParseTensions()
        {
			bool isMaj;
			if (Current.Kind is SyntaxKind.MajToken)
			{
				isMaj = true;
				m_position++;				
			}
			else
			{
				isMaj = false;
			}

			if (Current.Kind is not SyntaxKind.NumberToken)
			{
				return Tensions.Null;
			}

			var tensionToken = MatchToken(SyntaxKind.NumberToken);
			var tension = tensionToken.Text switch
			{
				"5" => Tension.Fifth,
				"6" => Tension.Sixth,
				"7" => Tension.Seventh,
				"9" => Tension.Ninth,
				"11" => Tension.Eleventh,
				"13" => Tension.Thirteenth,
				_ => throw new Exception($"Invalid tension {Current.Text}")
			};

			return new Tensions(tension, isMaj);
        }

        private NoteKind ParseBassNote()
        {
            if (Current.Kind is not SyntaxKind.WithBaseInToken)
            {
                return NoteKind.None;
            }

            m_position++;
            return ParseNote();
        }

        private ExtraInfo ParseExtraInfo()
        {
            if (Current.Kind is not SyntaxKind.OpenParenthesisToken)
            {
				return ExtraInfo.Null;
            }

			var extraInfo = new ExtraInfo();

			m_position++;

			while (true)
			{
				SyntaxKind? modification = Current.Kind switch
				{
					SyntaxKind.SharpToken => MatchToken(SyntaxKind.SharpToken).Kind,
					SyntaxKind.BemolToken => MatchToken(SyntaxKind.BemolToken).Kind,
					_ => null,
				};

				var gradeText = MatchToken(SyntaxKind.NumberToken).Text;

				if (Diagnostics.Any())
				{
					return ExtraInfo.Null;
				}
				
				var grade = Convert.ToInt32(gradeText);
				var current = (modification, grade);
				extraInfo.Add(current);

				if (Current.Kind is SyntaxKind.CommaToken)
				{
					m_position++;
				}
				else
				{
					MatchToken(SyntaxKind.CloseParenthesisToken);
					if (Diagnostics.Any())
					{
						break;
					}
				}
			}

			m_position++;

			return extraInfo;
        }
    }
}