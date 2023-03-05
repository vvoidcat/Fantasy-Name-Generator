using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAMEGEN.Core {
    public class Letter {
        public int index { get; private set; }
        public char uppercase { get; private set; }
        public char lowercase { get; private set; }
        public bool isVowel { get; private set; }
        public bool isConsonant { get; private set; }

        public Letter(char upper, char lower, bool vowel, bool consonant) {
            uppercase = upper;
            lowercase = lower;
            isVowel = vowel;
            isConsonant = consonant;
            index = 0;
        }

        public void SetLetterIndex(int newIndex) {
            index = newIndex;
        }

        public static bool operator ==(Letter a, Letter b) {
            return (a is not null && b is not null && a.isConsonant == b.isConsonant && a.isVowel == b.isVowel
                   && a.uppercase == b.uppercase && a.lowercase == b.lowercase && a.index == b.index) ? true : false;
        }

        public static bool operator !=(Letter a, Letter b) {
            return !Equals(a, b);
        }

        public override bool Equals(object? obj) {
            return (obj is not null && obj is Letter && (Letter)obj == this) ? true : false;
        }
    }
}
