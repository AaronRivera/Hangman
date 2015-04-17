using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangman
{
    class Program
    {
        static string gamerName;
        static string clue;
        static int chances;
        static string letterG;
        static List<char> correctGuesses = new List<char>();
        static StringBuilder displayToPlayer;
        static char guess;
        static void Main(string[] args)
        {
            Hangman();
        }

        /// <summary>
        /// Fuction that set ups and plays the game
        /// </summary>
        public static void Hangman()
        {
            //initializes variables 
            //name of the gamer
            gamerName = "";
            //clue to help user guess the game
            clue = "";
            //number of chances
            chances = 0;
            //letters guessed
            letterG = "";
            //clears the list with guessed words
            correctGuesses.Clear();
            //clears the console
            Console.Clear();
            //calls Intruction function to display the instructions
            Instructions();

            //flag to control the user types in a valid user
            bool validateUser = false;


            //while the user keeps entering other than characters
            while (!validateUser)
            {
                Console.WriteLine("");
                //asks user for name
                Console.Write("Whats your name:? ");
                //gets the response and converts it to upper case
                string userName = Console.ReadLine().ToString().ToUpper();
                //if the string is correct
                if (Validate(userName))
                {
                    //changes the flag to true
                    validateUser = true;
                    //assigns the user name to global gamer name
                    gamerName = userName;
                }
                else
                { ShowError(); }
            }



            //Calls the game instructions and shows the instructions
            Instructions();
            //Calls the function to generate a random word
            GeneratingRandom();

            //create a list that separates the word to guess to a list
            List<string> wordClue = new List<string>();
            wordClue = WordToGuess().Split('|').ToList();
            string randomWord = wordClue[0];
            clue = wordClue[1];



            //creates the string that masks the word
            displayToPlayer = new StringBuilder(randomWord.Length);
            for (int i = 0; i < randomWord.Length; i++)
                displayToPlayer.Append('_');


            //creates a list to keep track of incorrect guesses
            List<char> incorrectGuesses = new List<char>();

            //number of guesses
            chances = 7;
            //flag to keep track if user wins
            bool win = false;
            //number of leters revealed
            int lettersRevealed = 0;
            //variable to get input from the user
            string input = "";

            //calls these functions to refresh the screen
            //displays the game intructions
            Instructions();
            //displays the vital stats
            DisplaysName();
            //displays the track of letter
            TrackLetters();


            //while the user hasnt won and the chances are
            while (!win && chances > 0)
            {
               //flag to validate the input
                bool validateOK = false;


                //while the input is not correct
                while (!validateOK)
                {
                    //refreshes the game stats 
                    Instructions();
                    DisplaysName();
                    TrackLetters();

                    //asks for user to guess a letter or word
                    Console.SetCursorPosition(5, 20);
                    Console.Write("What's your guess? ");
                    input = Console.ReadLine().ToString().ToUpper();

                    //validates if the input is OK
                    if (Validate(input))
                    {
                        //if OK then changes the flag to true
                        validateOK = true;
                    }
                    else
                        // shows error 
                    { ShowError(); }
                }


                //if input length is greater than 1 then its a word
                if (input.Length > 1)
                {

                    //checks if the word equals to the word to guess
                    if (input.ToUpper() == randomWord)
                    {
                        //if guessed then call the function win
                        ShowWin();
                        //ask if the user wants to play again
                        bool playGame2 = false;
                        //while the user inputs yes or no
                        while (!playGame2)
                        {
                            Console.WriteLine("");
                            //asks the user if wants to play another game
                            Console.Write("Would you like to play another game (Yes/No)? ");
                            string response = Console.ReadLine().ToString().ToUpper();
                            //if response equals yes or y
                            if (response == "YES" || response == "Y")
                            {
                               //calls the function to play another game
                                playGame2 = true;
                                Hangman();
                            }
                            //if response equals no or n
                            if (response == "NO" || response == "N")
                            {
                                //exits the game
                                playGame2 = true;
                                System.Environment.Exit(0);
                            }
                            else
                                //displays error
                            { Console.WriteLine("Type in Yes or No!!"); }


                        }
                    }
                    else
                    {
                        //if the word didnt match then chances +1
                        //shows message that the word didnt match the word to guess
                        chances--;
                        ShowNoWord(input);
                    }
                }

                else
                {
                    //if the input is one character then extracts that char and assign it to guess
                    guess = input[0];

                    //checks if the character is contained within the correct guesses list
                    if (correctGuesses.Contains(guess))
                    {
                        //shows message that it was a correct guess
                        ShowRepeated(guess.ToString().ToUpper(), "IT WAS CORRECT");
                        //jumps to the end of the loop
                        continue;
                    }
                        //check if the incorrect guess is contained withiin the incorerct guess
                    else if (incorrectGuesses.Contains(guess))
                    {
                        //shows incorrect message
                        ShowRepeated(guess.ToString().ToUpper(), "IT WAS INCORRECT");
                       //jumps to the end of the loop
                        continue;
                    }


                    //if the char to guess is contained within the random word 
                    if (randomWord.Contains(guess))
                    {
                        //if so add the guess to the correct guesses list
                        correctGuesses.Add(guess);
                        //adds it to the latterG to send it to display to game vitals
                        letterG = letterG + guess + " ";

                        //for loop to unmask the word to guess
                        for (int i = 0; i < randomWord.Length; i++)
                        {
                            //if that word equals to the guessed letter
                            if (randomWord[i] == guess)
                            {
                               //unmask the letter
                                displayToPlayer[i] = randomWord[i];
                                //increases the letters revealed 
                                lettersRevealed++;
                            }
                        }
                        //if the leteters revealed equals to the random word lenght 
                        if (lettersRevealed == randomWord.Length)
                            //then change the win flag to true
                        { win = true; }
                    }
                    else
                    {
                        //if the guess was incorrect then added to the incorrect guesses list
                        incorrectGuesses.Add(guess);
                        //decreases chances by one
                        chances--;
                        //show message that the guess was incorect
                        ShowNoInString(guess.ToString().ToUpper());
                    }


                    //refreshes the game 
                    //calls game intructions
                    Instructions();
                    //displays the gamer vitals
                    DisplaysName();
                    //displays the track of letters guessed and the word to guess
                    TrackLetters();




                }

            }
            //if the flag is true that means the user guessed the word
            if (win)
                //shows the message that the user won
                ShowWin();
            else
                //else the user lost. Show lost message
                ShowLost(randomWord);


            //asks if user wants to keep playing 
            bool playGame = false;
            //while the player hasnt chosen the right 
            while (!playGame)
            {
                Console.WriteLine("");
                //asks users if they want to keep playing
                Console.Write("Would you like to play another game (Yes/No)? ");
                //reads answer
                string response = Console.ReadLine().ToString().ToUpper();
                //if answer is yes
                if (response == "YES" || response == "Y")
                {
                    //calls the function hangman to play another game
                    playGame = true;
                    Hangman();
                }
                //if response is no
                if (response == "NO" || response == "N")
                {
                    //exits the game
                    playGame = true;
                    System.Environment.Exit(0);
                }
                else
                    //shows error 
                { Console.WriteLine("Type in Yes or No!!"); }


            }

            
        }




        /// <summary>
        /// Shows message when the word typed in is not the word to guess
        /// </summary>
        /// <param name="word"></param>
        public static void ShowNoWord(string word)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("        ..........................................................");
            Console.WriteLine("                " + word + " IS NOT THE WORD");
            Console.WriteLine("        ..........................................................");
            System.Threading.Thread.Sleep(1000);
            Console.ResetColor();
        }

        /// <summary>
        /// Message to show when the user didnt guess the random word
        /// </summary>
        /// <param name="word"></param>
        public static void ShowLost(string word)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("        ..........................................................");
            Console.WriteLine("                 YOU LOST THE WORD WAS:" + word);
            Console.WriteLine("        ..........................................................");
            System.Threading.Thread.Sleep(1000);
            Console.ResetColor();
        }

        /// <summary>
        /// When the user wins show you win message
        /// </summary>
        public static void ShowWin()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("        ..........................................................");
            Console.WriteLine("                             Y O U   W O N");
            Console.WriteLine("        ..........................................................");
            System.Threading.Thread.Sleep(1000);
            Console.ResetColor();
        }


        /// <summary>
        /// Message to show when the letter is not in the random word 
        /// </summary>
        /// <param name="character"></param>
        public static void ShowNoInString(string character)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("        ..........................................................");
            Console.WriteLine("                       THERE'S NO LETTER {0} IN IT ", character);
            Console.WriteLine("        ..........................................................");
            System.Threading.Thread.Sleep(1000);
            Console.ResetColor();
        }

        /// <summary>
        /// shows message when a word guessed its already repeated
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        public static void ShowRepeated(string character, string message)
        {
            //shows the message in a box
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("        ..........................................................");
            Console.WriteLine("                YOU ALREADY ENTERED " + character + " " + message );
            Console.WriteLine("        ..........................................................");
            //pauses for ten secs
            System.Threading.Thread.Sleep(1000);
            Console.ResetColor();
        }
        /// <summary>
        /// shows error when the user inputs other then character
        /// </summary>
        public static void ShowError()
        {
            //displays the message in a box
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("        ..........................................................");
            Console.WriteLine("                     O N L Y   C H A R A C T E R S ! !");
            Console.WriteLine("        ..........................................................");
            //pauses for 10 secs
            System.Threading.Thread.Sleep(1000);
            Console.ResetColor();
        }
        /// <summary>
        /// keeps track of letter guessed and the hidden letters to guess
        /// </summary>
        public static void TrackLetters() 
        {
            //Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
            Console.ForegroundColor = ConsoleColor.Cyan;
            //displays the letter guessed
            Console.WriteLine("                           | LETTERS GUESSED: " + letterG);
            Console.ResetColor();
            Console.WriteLine("                           +++++++++++++++++++++++++++++++++++++++++++++++");
            Console.WriteLine( "");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("                         W O R D  T O  G U E S S");
            Console.ResetColor();
            Console.WriteLine("        ..........................................................");
            //unmask the letter guessed
            Console.Write("              (" + displayToPlayer.Length + ") Letters ");
            Console.ForegroundColor = ConsoleColor.Green;
            //displays each character and gives it an space
            for (int i = 0; i < displayToPlayer.Length; i++ )
            {
                Console.Write("  "+ displayToPlayer[i]);
            }
            Console.ResetColor();
            Console.WriteLine("");
            Console.WriteLine("        ..........................................................");

           
        }

   


        /// <summary>
         /// Dispalys teh name, clue , and chances left
        /// </summary>
        public static void DisplaysName()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            // Dispalys teh name, clue , and chances left
            Console.WriteLine("|   GAMER: " + gamerName + "    | THE CLUE IS: " + clue  + "   | GUESSES LEFT: " + chances);
            Console.ResetColor();
            Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
            


        }

        /// <summary>
        /// Generating random number box
        /// </summary>
        public static void GeneratingRandom()
        {
           // banner
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("");
            Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            Console.WriteLine("|               G E N E R A T I N G   R A N D O M   W O R D              |");

            //Animates generating random number
            for (int i = 0; i < 60; i++)
            {
                Console.Write("^");
                System.Threading.Thread.Sleep(20);
            }
            Console.ResetColor();
            Console.WriteLine("");

        }

        /// <summary>
        /// Validates the input from the user
        /// </summary>
        /// <param name="inputS"></param>
        /// <returns></returns>
        public static bool Validate(string inputS)
        {
            //validates if the string is empty
            if (inputS != "")
                //checks if the string contains characters only
            { return inputS.All(Char.IsLetter); }
            else
            {
                //returns false if not characters
                return false;
            }
        }
        

        /// <summary>
        /// Displays the game instructions
        /// </summary>
        public static void Instructions()
        {
            //Instructions of the game
            Console.Clear();
            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("      Hangman is a brainteasing word game that uses two players");
            Console.WriteLine("   The idea is to have the computer pick a word and you guess letters");
            Console.WriteLine("      in an attempt to guess the computer chosen word. If the letter");
            Console.WriteLine("  is in the word. If the letter is in the word then you come that much ");
            Console.WriteLine("closer to winning. Each time you guess wrong the computer will add pieces");
            Console.WriteLine("    of a stickman to the gallows. You will guess the word correctly ");
            Console.WriteLine("                     or the stickman will get hung");
            Console.ResetColor();
            Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
        }


        /// <summary>
        /// Generate the random word
        /// </summary>
        /// <returns></returns>
        public static string WordToGuess()
        {
            //creates a randomnumber
            Random rnd = new Random();
            //list of words
            List<string> words = new List<string>() { "Minneapolis", "Orchid", "Pirate", "Mayflower", "WALKINGDEAD", "UNDERARMOUR", "Hydrocodon","Batman" };
            //list of clues
            List<String> clues = new List<string>() { "City", "Plant","People","Famous ship", "TV SHOW","FAMOUS BRAND","Prescription Drug", "Super hero"};
            //gets a random word
            int randWord = rnd.Next(words.Count());
            //returns the random word and the clue
            return words[randWord].ToUpper() + "|" + clues[randWord].ToUpper();
        }



    }
}
