using Acordes.CodeAnalysis.Chords;
using Acordes.CodeAnalysis.PianoAnalysis;
using Acordes.CodeAnalysis.Syntax;

namespace Acordes.CodeAnalysis.Text
{
    public sealed class Evaluator(ChordInfo chord)
    {
        private readonly ChordInfo m_chord = chord;

        private const int Sus2 = 2;
        private const int Sus4 = 5;

        private const int MinorThird = 3;
        private const int MayorThird = 4;

        private const int DiminishedFifth = 6;
        private const int PerfectFifth = 7;
        private const int AumentedFifth = 8;

        private const int PerfectSixth = 9;

        private const int DiminishedSeventh = 9;
        private const int MinorSeventh = 10;
        private const int MayorSeventh = 11;
        private const int Ninth = 14;

        public Chord Evaluate()
        {
            var chord = (Chord)EvaluateBasicChords();
            chord = EvaluateTensions((TriChord)chord);
            chord = EvaluateInversions(chord);
            chord = EvaluateExtras(chord);
            return chord;
        }

        private TriChord EvaluateBasicChords()
        {
            return m_chord.Kind switch
            {
                ChordKind.Mayor => EvaluateMayorChord(),
                ChordKind.Minor => EvaluateMinorChord(),
                ChordKind.Diminished => EvaluateDiminishedChord(),
                ChordKind.Augmented => EvaluateAugmentedChord(),
                ChordKind.Suspended => EvaluateSuspendedChord(),
                _ => throw new Exception($"Invalid chord kind {m_chord.Kind}"),
            };
        }

        private TriChord EvaluateMayorChord()
        {
            var secondNote = m_chord.BaseNote.Add(MayorThird);
            var thirdNote = m_chord.BaseNote.Add(PerfectFifth);
            return new TriChord(m_chord.BaseNote, secondNote, thirdNote);
        }

        private TriChord EvaluateMinorChord()
        {
            var secondNote = m_chord.BaseNote.Add(MinorThird);
            var thirdNote = m_chord.BaseNote.Add(PerfectFifth);
            return new TriChord(m_chord.BaseNote, secondNote, thirdNote);
        }

        private TriChord EvaluateDiminishedChord()
        {
            var secondNote = m_chord.BaseNote.Add(MinorThird);
            var thirdNote = m_chord.BaseNote.Add(DiminishedFifth);
            return new TriChord(m_chord.BaseNote, secondNote, thirdNote);
        }

        private TriChord EvaluateAugmentedChord()
        {
            var secondNote = m_chord.BaseNote.Add(MayorThird);
            var thirdNote = m_chord.BaseNote.Add(AumentedFifth);
            return new TriChord(m_chord.BaseNote, secondNote, thirdNote);
        }

        private TriChord EvaluateSuspendedChord()
        {
            var move = m_chord.Suspended is 4 ? Sus4 : Sus2; 
            var secondNote = m_chord.BaseNote.Add(move);
            var thirdNote = m_chord.BaseNote.Add(PerfectFifth);
            return new TriChord(m_chord.BaseNote, secondNote, thirdNote);
        }

        private Chord EvaluateTensions(TriChord chord)
        {
            return m_chord.Tensions.Tension switch
            {
                Tension.None => chord,
                Tension.Fifth => EvaluateFifthChord(chord),
                Tension.Sixth => EvaluateSixthChord(chord),
                Tension.Seventh => EvaluateSeventhChord(chord),
                Tension.Ninth => EvaluateNinthChord(chord),
                Tension.Eleventh => EvaluateEleventhChord(chord),
                Tension.Thirteenth => EvaluateThirteenthChord(chord),
                _ => throw new Exception($"Invalid tension {m_chord.Tensions.Tension}"),
            };
        }

        private static BiChord EvaluateFifthChord(TriChord chord)
        {
            return new BiChord(chord.BaseNote, chord.ThirdNote);
        }

        private static TetraChord EvaluateSixthChord(TriChord chord)
        {
            var fourthNote = chord.BaseNote.Add(PerfectSixth);
            return new TetraChord(chord, fourthNote);
        }

        private TetraChord EvaluateSeventhChord(TriChord chord)
        {
            var move = m_chord.Tensions.IsMaj ? MayorSeventh : MinorSeventh;
            var fourthNote = chord.BaseNote.Add(move);
            return new TetraChord(chord, fourthNote);
        }

        private PentaChord EvaluateNinthChord(TriChord chord)
        {
            var seventhChord = EvaluateSeventhChord(chord);
            var fifthNote = chord.BaseNote.Add(Ninth);
            return new PentaChord(seventhChord, fifthNote);
        }

        private HexaChord EvaluateEleventhChord(TriChord chord)
        {
            var ninthChord = EvaluateNinthChord(chord);
            var sixthNote = chord.BaseNote.Add(17);
            return new HexaChord(ninthChord, sixthNote);
        }

        private HeptaChord EvaluateThirteenthChord(TriChord chord)
        {
            var eleventhChord = EvaluateEleventhChord(chord);
            var seventhNote = chord.BaseNote.Add(21);
            return new HeptaChord(eleventhChord, seventhNote);
        }

        private Chord EvaluateInversions(Chord chord)
        {
            if (m_chord.BassNote is not NoteKind.None)
            {
                chord.InvertInNote(m_chord.BassNote);
            }
            return chord;
        }

        private Chord EvaluateExtras(Chord chord)
        {
            var extras = m_chord.ExtraInfo.Modifications;

            foreach (var extra in extras)
            {
                var numberOfChord = extra.Grade switch
                {
                    1 => 1,
                    3 => 2,
                    5 => 3,
                    6 => 4,
                    7 => 4,
                    9 => 5,
                    11 => 6,
                    13 => 7,
                    _ => throw new Exception($"Invalid grade {extra.Grade}"),
                };

                chord = MoveChord(chord, numberOfChord, extra.Modification);
            }

            return chord;
        }

        private static Chord MoveChord(Chord chord, int numberOfChord, SyntaxKind? modification)
        {
            var move = modification switch
            {
                SyntaxKind.SharpToken => 1,
                SyntaxKind.BemolToken => -1,
                null => 0,
                _ => throw new Exception($"Invalid modification \"{modification}\""),
            };

            chord.Notes[numberOfChord-1] = chord.Notes[numberOfChord-1].Add(move);
            return chord;
        }
    }
}