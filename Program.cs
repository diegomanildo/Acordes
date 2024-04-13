using Acordes.CodeAnalysis.Syntax;
using Acordes.CodeAnalysis.Text;

namespace Acordes
{
	internal static class Program
	{
		private static void Main()
		{
			while (true)
			{
				Utils.Print("Enter chord: ", ConsoleColor.Green);
				var text = Console.ReadLine();

				if (string.IsNullOrEmpty(text))
				{
					break;
				}

				var parser = new Parser(text);
				var chordInfo = parser.Parse();

				if (parser.Diagnostics.Any())
				{
					parser.Diagnostics.Print(parser.SourceText);
					continue;
				}

				var evaluator = new Evaluator(chordInfo);
				var chord = evaluator.Evaluate();

				Console.WriteLine(chord);
			}
		}
	}
}