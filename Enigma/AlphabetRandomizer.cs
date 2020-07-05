using System;
using System.Linq;

namespace Enigma
{
    internal class AlphabetRandomizer
    {
        string[] alphabetCollection;

        public AlphabetRandomizer(string[] rotors = null)
        {
            alphabetCollection = rotors;
        }

        public int GetRotorsCount()
        {
            return alphabetCollection.Length;
        }

        internal string GetRandomizedAlphabet(int rotor)
        {
            if (rotor > (alphabetCollection.Length - 1))
                return null;
            else
                return alphabetCollection[rotor];
        }

        internal static string GenerateRandomizedAlphabet()
        {
            Random random = new Random();
            return new string(Program.Alphabet.ToCharArray().OrderBy(x => random.Next()).ToArray());
        }

        internal static string[] GenerateRandomizedAlphabetCollections(int rotorsCount = 5)
        {
            string[] generated = new string[rotorsCount];
            for (int i = 0; i < rotorsCount; i++)
            {
                generated[i] = GenerateRandomizedAlphabet();
            }
            return generated;
        }
    }
}