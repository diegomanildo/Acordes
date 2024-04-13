using Acordes.CodeAnalysis.Chords;

namespace Acordes.CodeAnalysis.PianoAnalysis
{
    public class HexaChord : Chord
    {
        public HexaChord(NoteKind baseNote, NoteKind secondNote, NoteKind thirdNote, NoteKind fourthNote, NoteKind fifthNote, NoteKind sixthNote)
            : base(baseNote, secondNote, thirdNote, fourthNote, fifthNote, sixthNote)
        {
        }
        
        public HexaChord(PentaChord pentaChord, NoteKind sixthNote)
            : this(pentaChord.BaseNote, pentaChord.SecondNote, pentaChord.ThirdNote, pentaChord.FourthNote, pentaChord.FifthNote, sixthNote)
        {
        }

        protected override int CuantityNotes => 6;
    }
}