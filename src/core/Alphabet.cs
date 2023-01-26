using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAMEGEN.Core {
    public class Alphabet {
        public List<Letter> letters { get; private set; }

        public Alphabet() {
            letters = new List<Letter>();
            InitENG();
            AssignIndexToLetters();
        }

        private void InitENG() {
            letters.Add(new Letter('A', 'a', true, false));
            letters.Add(new Letter('B', 'b', false, true));
            letters.Add(new Letter('C', 'c', false, true));
            letters.Add(new Letter('D', 'd', false, true));
            letters.Add(new Letter('E', 'e', true, false));
            letters.Add(new Letter('F', 'f', false, true));
            letters.Add(new Letter('G', 'g', false, true));
            letters.Add(new Letter('H', 'h', false, true));
            letters.Add(new Letter('I', 'i', true, false));
            letters.Add(new Letter('J', 'j', false, true));
            letters.Add(new Letter('K', 'k', false, true));
            letters.Add(new Letter('L', 'l', false, true));
            letters.Add(new Letter('M', 'm', false, true));
            letters.Add(new Letter('N', 'n', false, true));
            letters.Add(new Letter('O', 'o', true, false));
            letters.Add(new Letter('P', 'p', false, true));
            letters.Add(new Letter('Q', 'q', false, true));
            letters.Add(new Letter('R', 'r', false, true));
            letters.Add(new Letter('S', 's', false, true));
            letters.Add(new Letter('T', 't', false, true));
            letters.Add(new Letter('U', 'u', true, false));
            letters.Add(new Letter('V', 'v', false, true));
            letters.Add(new Letter('W', 'w', false, true));
            letters.Add(new Letter('X', 'x', false, true));
            letters.Add(new Letter('Y', 'y', true, true));
            letters.Add(new Letter('Z', 'z', false, true));
        }

        private void AssignIndexToLetters() {
            for (int i = 0; i < letters.Count; i++) {
                letters[i].SetLetterIndex(i);
            }
        }
    }
}
