using System;
using System.Collections.Generic;
using System.Text;

namespace Enigma
{
    public static class Reflector
    {
        public static char Reflect(char character)
        {
            return Program.Alphabet[(Program.Alphabet.Length - 1) - Program.Alphabet.IndexOf(character)];
        }
    }
}
