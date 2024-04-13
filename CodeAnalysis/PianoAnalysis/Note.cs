using Acordes.CodeAnalysis.Chords;

namespace Acordes.CodeAnalysis.PianoAnalysis
{
    public abstract class Note(NoteKind baseNote)
    {
        public NoteKind BaseNote { get; } = baseNote;

        public override string ToString()
		{
			return BaseNote.ToString();
		}
	}
}