using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Wasteland_Hacker {
    class Program {
        string correctWord;
        string signs = "<>!@#*:?%$";
        int attempts = 5;
        int maxLength = 0;
        int minLength = 0;
        Random rand = new Random();
        string txtData = Properties.Resources.epic;

        static void Main(string[] args) {
            Program pr = new Program();
            pr.Start();
        }

        void Start() {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Title = "Wasteland Hacker";
            string msg = "Booting up.........";
            for (int i = 0; i < msg.Length; i++) {
                Console.Write(msg[i]);
                System.Threading.Thread.Sleep(30);
            }
            System.Threading.Thread.Sleep(800);
            GetWords();
        }

        void Difficulty() {
            Console.Clear();
            string header = "____________WASTELAND HACKER____________";
            for (int i = 0; i < header.Length; i++) {
                Console.Write(header[i]);
                System.Threading.Thread.Sleep(20);
            }
            Console.WriteLine("\n\nChoose a difficulty (input a number):\n1. EASY, 2. NORMAL, 3. HARD\n");
            Console.Write(">> ");
            string difficulty = Console.ReadLine();

            switch (difficulty) {
                case "easy":
                case "e":
                case "1":
                    maxLength = 5;
                    minLength = 2;
                    break;
                case "normal":
                case "n":
                case "2":
                    maxLength = 7;
                    minLength = 5;
                    break;
                case "hard":
                case "h":
                case "3":
                    maxLength = 11;
                    minLength = 9;
                    break;
                default:
                    GetWords();
                    break;
            }
        }

        void GetWords() {
            string[] lines = txtData.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries); //reads every line from the dictionary

            Difficulty();
            string[] words = new string[12];

            Console.Clear();
            Console.WriteLine("____________WASTELAND HACKER____________\n\nType '!help' if you don't know how the guessing works.\n");

            int charNum = rand.Next(7, 12);
            for (int i = 0; i < words.Length; i++) { //gets x random words from the text file
                bool foundWord = false;
                string randWord = lines[rand.Next(lines.Length)];
                while (!foundWord) {
                    if (randWord.Length <= maxLength && randWord.Length >= minLength) { //check if the word is longer than the limit set by the difficulty
                        foundWord = true;
                    } else {
                        randWord = lines[rand.Next(lines.Length)];
                    }
                }
                words[i] = randWord;
                string randomChars = GenerateChars(charNum); //call a function which generates random characters. the string will have random number of chars
                Console.Write("0xF0" + rand.Next(10, 100) + "  " + randomChars + "_");
                Console.Write(words[i].ToUpper());
                randomChars = GenerateChars(charNum);
                Console.WriteLine("_" + randomChars);
                System.Threading.Thread.Sleep(50);
            }
            correctWord = words[rand.Next(words.Length)]; //randomly selects the correct answer
            Guess();
        }

        void Guess() {
            Console.Write("\n>> ");
            string answer = Console.ReadLine();
            if (answer == "!help") {
                Console.WriteLine("\nGuessing a character doesn't only mean it's a part of that word.\nIt also means that its position inside that word was guessed correctly.\nFor example:\n[a]ddictive\n[a]uto\nOnly 1 guessed character.\nBoth words have 'a' as the first letter.\nNote that both have letter 't', but only 1 was guessed correctly. This is because of their positions.");
                Guess();
            }
            if (answer == correctWord) {
                Console.WriteLine("Welcome, user.");
                Console.ReadKey();
            } else {
                GuessedRight(answer, correctWord);
                Attempts();
            }
        }

        string GenerateChars(int size) {
            char[] chars = new char[size]; //size is random
            for (int i = 0; i < size; i++) {
                chars[i] = signs[rand.Next(signs.Length)]; //selects random characters from signs[] and places it in chars[]
            }
            return new string(chars); //returns and converts chars[] into a string
        }

        void Attempts() {
            attempts -= 1;
            Console.WriteLine("ATTEMPTS LEFT: {0}", attempts);
            if (attempts == 0) {
                Console.WriteLine("/////////// LOCKDOWN INITIATED ////////////");
                Console.ReadKey();
            } else {
                Guess();
            }
        }

        void GuessedRight(string _answer, string _correctAnswer) { //checking if both a char and its position are the same as in the correct answer
            int count = 0;
            char[] correctChars = _correctAnswer.ToCharArray();
            char[] chars = _answer.ToCharArray();
            if (chars.Length > correctChars.Length) { //if the answer is longer than the correct one
                for (int i = 0; i < correctChars.Length; i++) {
                    if (chars[i] == correctChars[i]) {
                        count++;
                    }
                }
            } else {
                for (int i = 0; i < chars.Length; i++) {
                    if (correctChars[i] == chars[i]) {
                        count++;
                    }
                }
            }
            Console.WriteLine("{0}/{1} guessed correctly.", count, _answer.Length);
        }
    }
}