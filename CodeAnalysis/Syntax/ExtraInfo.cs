namespace Acordes.CodeAnalysis.Syntax
{
    public sealed class ExtraInfo(params ExtraData[] data)
    {
        public static readonly ExtraInfo Null = new();

        public void Add(ExtraData value)
        {
            Modifications.Add(value);
        }

        public List<ExtraData> Modifications = [.. data];
    }
}