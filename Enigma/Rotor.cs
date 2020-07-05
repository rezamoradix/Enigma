using System;
using System.Collections.Generic;
using System.Text;

namespace Enigma
{
    class Rotor
    {
        private string randomizedAlphabet;

        public string RandomizedAlphabet { get { return randomizedAlphabet; } }

        public Rotor(string randAlpha)
        {
            randomizedAlphabet = randAlpha;
        }

        public char GetCipher(char inputCharacter)
        {
            return randomizedAlphabet[Program.Alphabet.IndexOf(inputCharacter)];
        }

        public char GetAlpha(char inputCharacter)
        {
            return Program.Alphabet[randomizedAlphabet.IndexOf(inputCharacter)];
        }


        public void Step()
        {
            randomizedAlphabet = randomizedAlphabet.Substring(1) + randomizedAlphabet[0];
        }
    }
}
