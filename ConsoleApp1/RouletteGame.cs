namespace ConsoleApp1;

public class RouletteGame
{
    private Player player;
    private TriviaManager trivia;
    private Random rnd;

    public RouletteGame(Player player, TriviaManager trivia)
    {
        this.player = player;
        this.trivia = trivia;
        this.rnd = new Random();
    }

    public void Play()
    {
        AsciiArt.ShowRoulette();

        bool playing = true;

        while (playing)
        {
            if (player.IsTimeUp())
            {
                Console.WriteLine("Time is up! Returning to menu.");
                break;
            }

            if (player.Balance <= 0)
            {
                Console.WriteLine("You have no money left!");
                break;
            }

            Console.WriteLine("\nYour balance: $" + player.Balance);
            Console.WriteLine("\n--- ROULETTE ---");
            Console.WriteLine("Bet types:");
            Console.WriteLine("1. Single number (0-36) — pays 35x");
            Console.WriteLine("2. Red or Black — pays 2x");
            Console.WriteLine("3. Odd or Even — pays 2x");
            Console.WriteLine("4. Low (1-18) or High (19-36) — pays 2x");
            Console.Write("Choose a bet type: ");

            string betType = Console.ReadLine();

            // Get the bet amount
            Console.Write("How much would you like to bet? $");
            int bet = 0;
            bool validBet = int.TryParse(Console.ReadLine(), out bet);

            if (validBet == false || bet <= 0 || bet > player.Balance)
            {
                Console.WriteLine("Invalid bet amount.");
                continue;
            }

            // --- TRIVIA before rolling ---
            Console.WriteLine("\nAnswer a trivia question to roll!");
            bool correct = trivia.AskQuestion();

            if (correct == false)
            {
                Console.WriteLine("Wrong answer! You lose your bet of $" + bet);
                player.Balance = player.Balance - bet;
                playing = AskPlayAgain();
                continue;
            }

            // Spin the wheel (0 to 36)
            int result = rnd.Next(0, 37);

            // Determine color (0 is green, we treat it as a loss for red/black)
            // Standard roulette red numbers
            int[] redNumbers = { 1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36 };
            bool isRed = false;

            for (int i = 0; i < redNumbers.Length; i++)
            {
                if (redNumbers[i] == result)
                {
                    isRed = true;
                }
            }

            bool isBlack = (result != 0 && isRed == false);
            bool isOdd = (result != 0 && result % 2 != 0);
            bool isEven = (result != 0 && result % 2 == 0);
            bool isLow = (result >= 1 && result <= 18);
            bool isHigh = (result >= 19 && result <= 36);

            Console.WriteLine("\nThe wheel spins...");

            bool won = false;
            int winnings = 0;

            if (betType == "1")
            {
                Console.Write("Enter your number (0-36): ");
                int chosenNumber = 0;
                int.TryParse(Console.ReadLine(), out chosenNumber);

                if (chosenNumber == result)
                {
                    won = true;
                    winnings = bet * 35;
                }
            }
            else if (betType == "2")
            {
                Console.Write("Red or Black? (r/b): ");
                string colorChoice = Console.ReadLine();

                if (colorChoice == "r" && isRed)
                {
                    won = true;
                    winnings = bet * 2;
                }
                else if (colorChoice == "b" && isBlack)
                {
                    won = true;
                    winnings = bet * 2;
                }
            }
            else if (betType == "3")
            {
                Console.Write("Odd or Even? (o/e): ");
                string oeChoice = Console.ReadLine();

                if (oeChoice == "o" && isOdd)
                {
                    won = true;
                    winnings = bet * 2;
                }
                else if (oeChoice == "e" && isEven)
                {
                    won = true;
                    winnings = bet * 2;
                }
            }
            else if (betType == "4")
            {
                Console.Write("Low (1-18) or High (19-36)? (l/h): ");
                string lhChoice = Console.ReadLine();

                if (lhChoice == "l" && isLow)
                {
                    won = true;
                    winnings = bet * 2;
                }
                else if (lhChoice == "h" && isHigh)
                {
                    won = true;
                    winnings = bet * 2;
                }
            }
            else
            {
                Console.WriteLine("Invalid bet type.");
                continue;
            }
            Console.WriteLine("Result: " + result + " (" + GetColor(result, isRed) + ")");

            if (won)
            {
                Console.WriteLine("You win $" + winnings + "!");
                player.Balance = player.Balance + winnings;
            }
            else
            {
                Console.WriteLine("You lose $" + bet);
                player.Balance = player.Balance - bet;
            }

            Console.WriteLine("New balance: $" + player.Balance);
            playing = AskPlayAgain();
        }
    }

    private string GetColor(int number, bool isRed)
    {
        if (number == 0)
        {
            return "Green";
        }
        else if (isRed)
        {
            return "Red";
        }
        else
        {
            return "Black";
        }
    }

    private bool AskPlayAgain()
    {
        Console.Write("\nPlay again? (y/n): ");
        string answer = Console.ReadLine();
        return answer == "y" || answer == "Y";
    }
}