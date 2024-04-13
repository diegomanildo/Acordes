using Acordes.CodeAnalysis.Chords;

namespace Acordes.CodeAnalysis.Syntax
{
    public sealed class ChordInfo
    {
        public ChordInfo(NoteKind baseNote, ChordKind kind = ChordKind.Mayor)
            : this(baseNote, kind, Tensions.Null)
        {
            
        }
        
        public ChordInfo(NoteKind baseNote, ChordKind kind, Tensions tensions, int suspended = 0, NoteKind bassNote = NoteKind.None) : this(baseNote, kind, tensions, suspended, baseNote, new ExtraInfo())
        {

        }

        public ChordInfo(NoteKind baseNote, ChordKind kind, Tensions tensions, int suspended, NoteKind bassNote, ExtraInfo extraInfo)
        {
            BaseNote = baseNote;
            Kind = kind;
            Tensions = tensions;
            Suspended = suspended;
            BassNote = bassNote;
            ExtraInfo = extraInfo;
        }

        public NoteKind BaseNote { get; }
        public ChordKind Kind { get; }
        public Tensions Tensions { get; }
        public int Suspended { get; }
        public NoteKind BassNote { get; }
        public ExtraInfo ExtraInfo { get; }
    }
}