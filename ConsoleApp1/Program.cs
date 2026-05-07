namespace ConsoleApp1;

class Program
{
    static void Main(string[] args)
    {
        // Create the player and ask for their name
        Console.WriteLine("Welcome to the Casino!");
        Console.Write("Enter your name: ");
        string playerName = Console.ReadLine();

        Player player = new Player(playerName);
        player.Balance = 500; // starting balance

        // Load the trivia questions from a file
        TriviaManager trivia = new TriviaManager();
        trivia.LoadQuestions("questions.txt");

        // Load the leaderboard
        Leaderboard leaderboard = new Leaderboard("leaderboard.txt");

        bool running = true;

        while (running)
        {
            // Check if the 5 minute timer has expired
            if (player.IsTimeUp())
            {
                Console.WriteLine("\nTime is up! Your session has ended.");
                break;
            }

            Console.WriteLine("\n=============================");
            Console.WriteLine("        MAIN MENU");
            Console.WriteLine("=============================");
            Console.WriteLine("Balance: $" + player.Balance);
            Console.WriteLine("Time left: " + player.GetTimeLeft() + " seconds");
            Console.WriteLine("1. Answer a trivia question (earn bet money)");
            Console.WriteLine("2. Play Blackjack");
            Console.WriteLine("3. Play Roulette");
            Console.WriteLine("4. View Leaderboard");
            Console.WriteLine("5. Quit");
            Console.Write("Choose an option: ");

            string choice = Console.ReadLine();

            if (choice == "1")
            {
                TriviaGame triviaGame = new TriviaGame(trivia, player);
                triviaGame.Play();
            }
            else if (choice == "2")
            {
                BlackjackGame blackjack = new BlackjackGame(player, trivia);
                blackjack.Play();
            }
            else if (choice == "3")
            {
                RouletteGame roulette = new RouletteGame(player, trivia);
                roulette.Play();
            }
            else if (choice == "4")
            {
                leaderboard.Display();
            }
            else if (choice == "5")
            {
                running = false;
            }
            else
            {
                Console.WriteLine("Invalid choice. Please try again.");
            }
        }

        leaderboard.AddEntry(player.Name, player.Balance);
        leaderboard.Save();

        Console.WriteLine("\nThanks for playing, " + player.Name + "!");
        Console.WriteLine("Final balance: $" + player.Balance);
        trivia.ShowMissedQuestions();
    }
}