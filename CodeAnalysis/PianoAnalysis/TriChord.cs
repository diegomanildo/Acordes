using Acordes.CodeAnalysis.Chords;

namespace Acordes.CodeAnalysis.PianoAnalysis
{
    public class TriChord : Chord
    {
        protected override int CuantityNotes => 3;

        public TriChord(NoteKind baseNote, NoteKind secondNote, NoteKind thirdNote) : base(baseNote, secondNote, thirdNote)
        {
        }

        public TriChord(BiChord biChord, NoteKind thirdNote) : this(biChord.BaseNote, biChord.SecondNote, thirdNote)
        {
            
        }
    }
}