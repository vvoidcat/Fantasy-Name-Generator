using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAMEGEN.Core {
    public class Letter {
        public int index { get; private set; }
        public string uppercase { get; private set; }
        public string lowercase { get; private set; }
        public bool isVowel { get; private set; }
        public bool isConsonant { get; private set; }

        public Letter(string upper, string lower, bool vowel, bool consonant) {
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
            return (a.isConsonant == b.isConsonant && a.isVowel == b.isVowel
                        && String.Equals(a.uppercase, b.uppercase)
                            && String.Equals(a.lowercase, b.lowercase) && a.index == b.index) ? true : false;
        }

        public static bool operator !=(Letter a, Letter b) {
            return (a.isConsonant != b.isConsonant || a.isVowel != b.isVowel
                        || !String.Equals(a.uppercase, b.uppercase)
                            || !String.Equals(a.lowercase, b.lowercase) || a.index != b.index) ? true : false;
        }
    }
}
