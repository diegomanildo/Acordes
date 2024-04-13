namespace Acordes.CodeAnalysis.Text
{
    public readonly record struct TextSpan(int Start, int Length)
    {
        public int End => Start + Length;

        public static TextSpan FromBounds(int start, int end)
        {
            var length = end - start;
            return new TextSpan(start, length);
        }
    }
}