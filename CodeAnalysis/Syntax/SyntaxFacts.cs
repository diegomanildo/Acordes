
using System.Collections;
using Acordes.CodeAnalysis.Chords;
using Acordes.CodeAnalysis.PianoAnalysis;
using Acordes.CodeAnalysis.Text;

namespace Acordes.CodeAnalysis.Syntax
{
    internal static class SyntaxFacts
	{
		public static string GetTextFromKind(SyntaxKind kind)
		{
			return kind switch
            {
				SyntaxKind.EndOfFileToken => "\0",
                _ => throw new Exception($"Invalid kind {kind}"),
            };
		}

		private sealed class TextSpanComparer : IComparer<TextSpan>
        {
            public int Compare(TextSpan x, TextSpan y)
            {
                var cmp = x.Start - y.Start;
                if (cmp is 0)
                {
                    cmp = x.Length - y.Length;
                }

                return cmp;
            }
        }

		public static void Print(this DiagnosticBag diagnostics, SourceText text)
		{
			foreach (var diagnostic in diagnostics.OrderBy(diag => diag.Span, new TextSpanComparer()))
            {
                var lineIndex = text.GetLineIndex(diagnostic.Span.Start);
                var line = text.Lines[lineIndex];
                var lineNumber = lineIndex + 1;
                var character = diagnostic.Span.Start - line.Start + 1;

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"({lineNumber}, {character}): ");
                Console.WriteLine(diagnostic);
                Console.ResetColor();

                var prefixSpan = TextSpan.FromBounds(line.Start, diagnostic.Span.Start);
                var suffixSpan = TextSpan.FromBounds(diagnostic.Span.End, line.End);

                var prefix = text.ToString(prefixSpan);
                var error = text.ToString(diagnostic.Span);
                var suffix = text.ToString(suffixSpan);

                Console.Write("    ");
                Console.Write(prefix);

                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(error);
                Console.ResetColor();

                Console.Write(suffix);
                Console.WriteLine();
            }

            Console.WriteLine();
		}

        private static int NormalizeNote(NoteKind note)
        {
            return NormalizeNote((int)note);
        }

        private static int NormalizeNote(int note)
        {
            const int Notes = 12;

            while (note <= 0)
            {
                note += Notes;
            }

            while (note > Notes)
            {
                note -= Notes;
            }

            return note;
        }

        public static NoteKind Add(this NoteKind note, int number)
        {
            return (NoteKind)NormalizeNote(note + number);
        }

        public static NoteKind Sub(this NoteKind note, int number)
        {
            return (NoteKind)NormalizeNote(note - number);
        }

        public static NoteKind ConvertToNote(string note)
        {
            var initNote = note[0] - 'A';

            initNote = initNote switch
            {
                0 => 1,
                1 => 3,
                2 => 4,
                3 => 6,
                4 => 8,
                5 => 9,
                6 => 11,
                _ => throw new Exception($"There was an error"),
            };

            for (var i = 1; i < note.Length; i++)
            {
                switch (note[i])
                {
                    case '#':
                        initNote++;
                        break;
                    case 'b':
                        initNote--; 
                        break;
                    case 'x':
                        initNote+=2;
                        break;
                    default:
                        throw new Exception($"Invalid modificator '{note[i]}'");
                }
            }

            initNote = NormalizeNote(initNote);

            return (NoteKind)initNote;
        }
    }
}