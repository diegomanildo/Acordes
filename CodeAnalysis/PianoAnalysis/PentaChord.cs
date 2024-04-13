using Acordes.CodeAnalysis.Chords;

namespace Acordes.CodeAnalysis.PianoAnalysis
{
    public class PentaChord : Chord
    {
        protected override int CuantityNotes => 5;

        public PentaChord(NoteKind baseNote, NoteKind secondNote, NoteKind thirdNote, NoteKind fourthNote, NoteKind fifthNote)
            : base(baseNote, secondNote, thirdNote, fourthNote, fifthNote)
        {
        }
        
        public PentaChord(TetraChord tetraChord, NoteKind fifthNote)
            : this(tetraChord.BaseNote, tetraChord.SecondNote, tetraChord.ThirdNote, tetraChord.FourthNote, fifthNote)
        {
        }
    }
}