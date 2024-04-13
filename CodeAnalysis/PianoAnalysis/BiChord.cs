using Acordes.CodeAnalysis.Chords;

namespace Acordes.CodeAnalysis.PianoAnalysis
{
    public class BiChord : Chord
    {
        public BiChord(NoteKind baseNote, NoteKind secondNote) 
            : base(baseNote, secondNote)
        {
        }

        protected override int CuantityNotes => 2;
    }
}