using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

class Program
{
    static void Main()
    {
        List<string> wordList = LoadWordList(@"Data/wordlist.txt");
        //string programDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        //string dataFolderPath = Path.Combine(programDirectory, "Data");
        //string path = Path.Combine(dataFolderPath, "wordlist.txt");
        //string[] wordList = File.ReadAllLines(path);

        if (wordList.Count == 0)
        {
            Console.WriteLine("Error, no words.");
            return;
        }

        Console.WriteLine("Enter scrambled words:");

        List<string> scrambledWords = new List<string>();
        for (int i = 0; i < 10; i++)
        {
            Console.Write($"Word {i + 1}: ");
            string input = Console.ReadLine()?.Trim();
            scrambledWords.Add(input);
        }

        Console.WriteLine("\nScrambled Words:");
        for (int i = 0; i < scrambledWords.Count; i++)
        {
            Console.WriteLine($"Word {i + 1}: {scrambledWords[i]}");
        }

        Console.WriteLine("\nMatching scrambled words:\n");

        List<string> unscrambledWords = UnscrambleWords(scrambledWords, wordList);

        Console.WriteLine($"Answer: {string.Join(",", unscrambledWords)}");

        Console.ReadLine();
    }

    static List<string> LoadWordList(string filePath)
    {
        try
        {
            return File.ReadAllLines(filePath).ToList();
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine($"Error reading file: File not found - {filePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading file: {ex.Message}");
        }

        return new List<string>();
    }

    static List<string> GetScrambledWords(int count, List<string> wordList)
    {
        Random random = new Random();
        return Enumerable.Range(0, count)
                         .Select(_ => ScrambleWord(wordList[random.Next(wordList.Count)]))
                         .ToList();
    }

    static string ScrambleWord(string word)
    {
        char[] characters = word.ToCharArray();
        Random random = new Random();
        int length = word.Length;

        while (length > 1)
        {
            length--;
            int index = random.Next(length + 1);
            char temp = characters[index];
            characters[index] = characters[length];
            characters[length] = temp;
        }

        return new string(characters);
    }

    static List<string> UnscrambleWords(List<string> scrambledWords, List<string> wordList)
    {
        return scrambledWords.Select(sw => UnscrambleWord(sw, wordList)).ToList();
    }

    static string UnscrambleWord(string scrambledWord, List<string> wordList)
    {
        var permutations = GetPermutations(scrambledWord);

        foreach (var permutation in permutations)
        {
            if (wordList.Contains(permutation))
            {
                return permutation;
            }
        }

        return "No match found.";
    }

    static IEnumerable<string> GetPermutations(string word)
    {
        if (word.Length == 1)
        {
            yield return word;
        }
        else
        {
            foreach (var perm in GetPermutations(word.Substring(1)))
            {
                for (int i = 0; i < word.Length; i++)
                {
                    yield return perm.Substring(0, i) + word[0] + perm.Substring(i);
                }
            }
        }
    }
}
