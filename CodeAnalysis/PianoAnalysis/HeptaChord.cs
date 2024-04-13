using Acordes.CodeAnalysis.Chords;

namespace Acordes.CodeAnalysis.PianoAnalysis
{
    public class HeptaChord : Chord
    {
        protected override int CuantityNotes => 7;

        public HeptaChord(NoteKind baseNote, NoteKind secondNote, NoteKind thirdNote, NoteKind fourthNote, NoteKind fifthNote, NoteKind sixthNote, NoteKind seventhNote)
            : base(baseNote, secondNote, thirdNote, fourthNote, fifthNote, sixthNote, seventhNote)
        {
        }
        
        public HeptaChord(HexaChord hexaChord, NoteKind seventhNote)
            : this(hexaChord.BaseNote, hexaChord.SecondNote, hexaChord.ThirdNote, hexaChord.FourthNote, hexaChord.FifthNote, hexaChord.SixthNote, seventhNote)
        {
        }
    }
}