using System;

namespace HangmanGame
{
    // თამაშების ზოგადი სუპერ კლასი, რომლისგანაც სხვა თამაშები მემკვიდრეობას იღებენ
    public abstract class Game
    {
        public string Title { get; protected set; } // თამაშის სათაური

        public Game(string title)
        {
            Title = title; // თამაშის სათაურის ინიციალიზაცია
        }

        public abstract void Start(); // თამაშის დაწყების აბსტრაქტული მეთოდი
    }

    // Hangman კლასი, რომელიც მემკვიდრეობას იღებს Game კლასისგან
    public class Hangman : Game
    {
        private readonly string _wordToGuess; // სიტყვა, რომლის გამოცნობაც მოთამაშემ უნდა
        private readonly char[] _guessedWord; // სწორად გამოცნობილი ასოების სტატუსი
        private readonly int _maxAttempts; // მაქსიმალური მცდელობები
        private int _remainingAttempts; // დარჩენილი მცდელობები
        private string _wrongGuesses = ""; // არასწორად გამოცნობილი ასოები

        // კონსტრუქტორი, რომელიც შუალედურად თამაშს აგენერირებს
        public Hangman(string title, string wordToGuess, int maxAttempts) : base(title)
        {
            _wordToGuess = wordToGuess.ToUpper(); // სიტყვა, რომლის გამოცნობაც მოთამაშემ უნდა
            _guessedWord = new string('_', _wordToGuess.Length).ToCharArray(); // ინიციალიზაცია დახურული სიტყვით
            _maxAttempts = maxAttempts;
            _remainingAttempts = maxAttempts;
        }

        // თამაშის დაწყების ლოგიკა
        public override void Start()
        {
            Console.WriteLine($"Welcome to {Title}!");
            Console.WriteLine($"You have {_maxAttempts} attempts to guess the word.");

            while (_remainingAttempts > 0)
            {
                Console.WriteLine($"\nWord: {new string(_guessedWord)}"); // სიტყვის ამჟამინდელი მდგომარეობა
                Console.WriteLine($"Wrong guesses: {_wrongGuesses}");
                Console.WriteLine($"Remaining attempts: {_remainingAttempts}");

                Console.Write("Enter a letter: ");
                var input = Console.ReadLine();

                // ვამოწმებთ, რომ შეყვანილი ასო სწორია
                if (string.IsNullOrWhiteSpace(input) || input.Length != 1 || !char.IsLetter(input[0]))
                {
                    Console.WriteLine("Please enter a valid single letter.");
                    continue;
                }

                var guess = char.ToUpper(input[0]); // ასოს გარდაქმნა დიდ ასოდ

                // თუ სიტყვაში უკვე არის ეს ასო
                if (_guessedWord.Contains(guess) || _wrongGuesses.Contains(guess))
                {
                    Console.WriteLine("You already guessed that letter.");
                    continue;
                }

                // თუ მოთამაშემ სწორად გამოიცნო ასო
                if (_wordToGuess.Contains(guess))
                {
                    Console.WriteLine("Good guess!");

                    // შეცვლიან სწორად ნაპოვნი ასო
                    for (var i = 0; i < _wordToGuess.Length; i++)
                    {
                        if (_wordToGuess[i] == guess)
                        {
                            _guessedWord[i] = guess;
                        }
                    }

                    // თუ ყველა ასო სწორად გამოცნობილია
                    if (new string(_guessedWord) == _wordToGuess)
                    {
                        Console.WriteLine($"\nCongratulations! You guessed the word: {_wordToGuess}");
                        return;
                    }
                }
                else
                {
                    // არასწორი ასო
                    Console.WriteLine("Wrong guess!");
                    _wrongGuesses += guess + " ";
                    _remainingAttempts--;
                }
            }

            // თუ მცდელობები ამოიწურა
            Console.WriteLine($"\nGame over! The correct word was: {_wordToGuess}");
        }
    }

    // ძირითადი პროგრამის კლასი
    class Program
    {
        static void Main(string[] args)
        {
            // ახალი Hangman თამაშის შექმნა
            Hangman game = new Hangman("Hangman Game", "Programming", 6); // სიტყვა და მცდელობები

            game.Start(); // თამაშის დაწყება
        }
    }
}