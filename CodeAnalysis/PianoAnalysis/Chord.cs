using System.Text;
using Acordes.CodeAnalysis.Chords;

namespace Acordes.CodeAnalysis.PianoAnalysis
{
    public class Chord
    {
        protected virtual int CuantityNotes { get; } 
        public NoteKind[] Notes { get; private set; }

        public Chord(params NoteKind[] notes)
        {
            if (CuantityNotes <= 0)
            {
                throw new Exception($"{nameof(CuantityNotes)} has to be more than 0");
            }

            if (CuantityNotes != notes.Length)
            {
                throw new Exception($"Notes are more than {nameof(CuantityNotes)}");
            }

            Notes = notes;
        }

        public NoteKind BaseNote { get => Notes[0]; set => Notes[0] = value; }
        public NoteKind SecondNote { get => Notes[1]; set => Notes[1] = value; }
        public NoteKind ThirdNote { get => Notes[2]; set => Notes[2] = value; }
        public NoteKind FourthNote { get => Notes[3]; set => Notes[3] = value; }
        public NoteKind FifthNote { get => Notes[4]; set => Notes[4] = value; }
        public NoteKind SixthNote { get => Notes[5]; set => Notes[5] = value; }
        public NoteKind SeventhNote { get => Notes[6]; set => Notes[6] = value; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var note in Notes)
            {
                sb.Append(note);

                if (!note.Equals(Notes[^1]))
                {
                    sb.Append(" - ");
                }
            }

            return sb.ToString();
        }

        public void InvertInNote(NoteKind bassNote)
        {
            if (!Notes.Contains(bassNote))
            {
                Notes = [bassNote, .. Notes];
            }

            var index = Array.IndexOf(Notes, bassNote);
            var invertedNotes = Notes.Skip(index).Concat(Notes.Take(index)).ToArray();
            Array.Copy(invertedNotes, Notes, Notes.Length);
        }
    }
}