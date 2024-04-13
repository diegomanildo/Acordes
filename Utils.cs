namespace Acordes
{
    public static class Utils
	{
		public static void PrintLn(object toPrint, ConsoleColor color = ConsoleColor.Gray, string endl = "\n")
		{
			Print(toPrint, color);
			Print(endl);
		}

		public static void Print(object toPrint, ConsoleColor color = ConsoleColor.Gray)
		{
			Console.ForegroundColor = color;
			Console.Write(toPrint);
			Console.ResetColor();
		}
	}
}