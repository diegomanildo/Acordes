using Acordes.CodeAnalysis.Chords;

namespace Acordes.CodeAnalysis.PianoAnalysis
{
    public class TetraChord : Chord
    {
        public TetraChord(NoteKind baseNote, NoteKind secondNote, NoteKind thirdNote, NoteKind fourthNote) 
            : base(baseNote, secondNote, thirdNote, fourthNote)
        {
        }

        public TetraChord(TriChord chord, NoteKind fourthNote) 
            : this(chord.BaseNote, chord.SecondNote, chord.ThirdNote, fourthNote)
        {
        }

        protected override int CuantityNotes => 4;
    }
}