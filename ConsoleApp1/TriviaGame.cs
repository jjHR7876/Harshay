namespace ConsoleApp1;

public class TriviaGame
{
    private TriviaManager trivia;
    private Player player;
    private int rewardPerQuestion = 50;

    public TriviaGame(TriviaManager trivia, Player player)
    {
        this.trivia = trivia;
        this.player = player;
    }

    public void Play()
    {
        Console.WriteLine("\n--- TRIVIA MODE ---");
        Console.WriteLine("Answer questions to earn $" + rewardPerQuestion + " each.");
        Console.WriteLine("Press Enter to start or type 'quit' to go back.");

        string input = Console.ReadLine();

        if (input == "quit")
        {
            return;
        }

        bool playing = true;

        while (playing)
        {
            if (player.IsTimeUp())
            {
                Console.WriteLine("Time is up!");
                break;
            }

            bool correct = trivia.AskQuestion();

            if (correct)
            {
                player.Balance = player.Balance + rewardPerQuestion;
                Console.WriteLine("Balance: $" + player.Balance);
            }

            Console.Write("\nAnswer another question? (y/n): ");
            string again = Console.ReadLine();

            if (again != "y" && again != "Y")
            {
                playing = false;
            }
        }
    }
}