using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Enigma
{
    class Program
    {
        // ~ is space
        // if you want to add double quotation, d-quotations in input should be \"
        // see this: https://weblogs.asp.net/jongalloway/_5B002E00_NET-Gotcha_5D00_-Commandline-args-ending-in-_5C002200_-are-subject-to-CommandLineToArgvW-whackiness

        public static string Alphabet = "abcdefghijklmnopqrstuvwxyz~0123456789.,;:?!@#$%^&*()[]{}<>";
        public static int rotorsCount = 5;
        static string rotorsPath = Path.Combine(Directory.GetCurrentDirectory(), "rotors.json");
        static AlphabetRandomizer randomizer;

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Enigma version 0.2; author: https://github.com/rezamoradix \n");
                Console.WriteLine($"Alphabet: {Alphabet}\n");
                Console.WriteLine("Usage:");
                Console.WriteLine("     enigma <TEXT_HERE>");
                Console.WriteLine("     enigma -- space <TEXT_HERE>                                   showing spaces (for decrypting)");
                Console.WriteLine("     enigma -- <R1_STEP>:<R2_STEP>:<R3_STEP>:... <TEXT_HERE>       apply stepping settings for rotors");
                Console.WriteLine("     enigma -- generate                                            generate new randomized alphabet collection for rotors");
                Console.WriteLine("     enigma -- rotors <ROTORS_COUNT>                               choose number of rotors and regenerate alphabet");
                Console.WriteLine("\nInput is empty.");
                return;
            }
            string[] collection = ReadGeneratedAlphabetCollection();
            if (collection != null)
            {
                randomizer = new AlphabetRandomizer(collection);
                rotorsCount = randomizer.GetRotorsCount();
            }
            else
            {
                randomizer = new AlphabetRandomizer(GenerateRandomizedAlphabetCollections());
            }

            List<Rotor> rotors = new List<Rotor>();

            for (int i = 0; i < rotorsCount; i++)
            {
                rotors.Add(new Rotor(randomizer.GetRandomizedAlphabet(i)));
            }

            bool space = false;

            if (args.Length >= 2 && args[0] == "--")
            {
                switch (args[1])
                {
                    case "space":
                        space = true;
                        if (args.Length < 2)
                        {
                            Console.WriteLine("Input is empty.");
                            return;
                        }

                        args[0] = args[2];
                        break;


                    case "generate":
                        GenerateRandomizedAlphabetCollections();
                        Console.WriteLine("Randomized alphabet collections generated.");
                        return;

                    case "rotors":
                        if (args.Length < 2 || !int.TryParse(args[2], out _))
                        {
                            Console.WriteLine("Rotors count is not acceptable.");
                            return;
                        }
                        rotorsCount = Int32.Parse(args[2]);
                        GenerateRandomizedAlphabetCollections();
                        Console.WriteLine("Rotors count changed. Randomized alphabet collection regenerated.");
                        return;
                }
                
                if (args[1].Contains(':'))
                {
                    int[] inputSetting = args[1].Split(':').Select(x => Int32.Parse(x)).ToArray();
                    if (inputSetting.Length > rotorsCount)
                    {
                        Console.WriteLine("Stepping settings count is greater than rotors count.");
                        return;
                    }

                    int[] rotation = Enumerable.Repeat(0, rotorsCount).ToArray();
                    inputSetting.CopyTo(rotation, 0);

                    for (int i = 0; i < inputSetting.Length; i++)
                    {
                        for (int j = 0; j < inputSetting[i]; j++)
                        {
                            rotors[i].Step();
                        }
                    }

                    // swaping last arg with the first one
                    args[0] = args[2];
                }
            }

            string inputText = string.Join("", args[0].ToLower().Replace(' ', '~').Where(x => Alphabet.Contains(x)).ToArray());

            string cipher = "";
            int counter = 0;
            char c = '.'; 
            foreach (var character in inputText)
            {
                counter++;
                for (int i = 0; i < rotorsCount; i++)
                {
                    if (counter % (Math.Pow(Alphabet.Length, i)) == 0)
                    {
                        rotors[i].Step();
                    }

                    c = rotors[i].GetCipher(i == 0 ? character : c);
                }

                // Reflect
                c = Reflector.Reflect(c);
                for (int i = rotorsCount - 1; i >= 0; i--)
                {
                    c = rotors[i].GetAlpha(c);
                }

                cipher += c;
            }

            Console.WriteLine(space ? cipher.Replace('~', ' ') : cipher);
        }

        private static string[] ReadGeneratedAlphabetCollection()
        {
            return !File.Exists(rotorsPath)
                ? null
                : JsonSerializer.Deserialize(File.ReadAllText(rotorsPath), typeof(string[])) as string[];
        }

        private static string[] GenerateRandomizedAlphabetCollections()
        {
            string[] collection = AlphabetRandomizer.GenerateRandomizedAlphabetCollections(rotorsCount);
            File.WriteAllText(rotorsPath, JsonSerializer.Serialize(collection, typeof(string[])));
            return collection;
        }
    }
}
