using System;
using System.Collections.Generic;
using System.Text;

namespace Enigma
{
    class Rotor
    {
        public string RandomizedAlphabet { get; private set; }

        public Rotor(string randAlpha)
        {
            RandomizedAlphabet = randAlpha;
        }

        public char GetCipher(char inputCharacter)
        {
            return RandomizedAlphabet[Program.Alphabet.IndexOf(inputCharacter)];
        }

        public char GetAlpha(char inputCharacter)
        {
            return Program.Alphabet[RandomizedAlphabet.IndexOf(inputCharacter)];
        }

        public void Step()
        {
            RandomizedAlphabet = RandomizedAlphabet.Substring(1) + RandomizedAlphabet[0];
        }
    }
}
