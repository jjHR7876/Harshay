namespace ConsoleApp1;

public class BlackjackGame
{
    private Player player;
    private Dealer dealer;
    private Deck deck;
    private TriviaManager trivia;

    public BlackjackGame(Player player, TriviaManager trivia)
    {
        this.player = player;
        this.dealer = new Dealer();
        this.deck = new Deck(6); // 6-deck shoe like a real casino
        this.deck.shuffle();
        this.trivia = trivia;
    }

    public void Play()
    {
        AsciiArt.ShowBlackjack();

        bool playingRound = true;

        while (playingRound)
        {
            if (player.IsTimeUp())
            {
                Console.WriteLine("Time is up! Returning to menu.");
                break;
            }

            if (player.Balance <= 0)
            {
                Console.WriteLine("You have no money left! Returning to menu.");
                break;
            }

            // --- BETTING PHASE ---
            Console.WriteLine("\nYour balance: $" + player.Balance);
            Console.Write("How much would you like to bet? ");
            int bet = 0;
            string betInput = Console.ReadLine();

            // Make sure the bet is a valid number
            bool validBet = int.TryParse(betInput, out bet);

            if (validBet == false || bet <= 0 || bet > player.Balance)
            {
                Console.WriteLine("Invalid bet. Please enter a number between 1 and " + player.Balance);
                continue;
            }

            // --- TRIVIA PHASE (before dealing) ---
            // Player must answer a trivia question to earn their bet money
            Console.WriteLine("\nAnswer a trivia question to earn your bet!");
            bool answeredCorrectly = trivia.AskQuestion();

            if (answeredCorrectly == true)
            {
                Console.WriteLine("Correct! You earned $" + bet + " to bet with.");
            }
            else
            {
                Console.WriteLine("Wrong answer. You lose your bet of $" + bet);
                player.Balance = player.Balance - bet;
                continue;
            }

            // --- DEAL PHASE ---
            player.ClearHand();
            dealer.ClearHand();

            // Deal 2 cards to each
            player.Hit(deck);
            dealer.Hit(deck);
            player.Hit(deck);
            dealer.Hit(deck);

            // Show hands
            player.ShowHand();
            dealer.ShowFirstCard();

            // Check for blackjack right away
            if (player.Hand.HasBlackjack())
            {
                Console.WriteLine("BLACKJACK! You win $" + (int)(bet * 1.5));
                player.Balance = player.Balance + (int)(bet * 1.5);
                playingRound = AskPlayAgain();
                continue;
            }

            // --- PLAYER TURN ---
            bool playerTurnActive = true;

            while (playerTurnActive)
            {
                if (player.Hand.IsBust())
                {
                    Console.WriteLine("You busted!");
                    playerTurnActive = false;
                    break;
                }

                Thread.Sleep(5000); 
                Console.WriteLine("\nAnswer a trivia question to take your action!");
                bool triviaResult = trivia.AskQuestion();

                if (triviaResult == false)
                {
                    Console.WriteLine("Wrong! You must stand.");
                    playerTurnActive = false;
                    break;
                }

                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("1. Hit");
                Console.WriteLine("2. Stand");
                Console.Write("Choice: ");

                string action = Console.ReadLine();

                if (action == "1")
                {
                    player.Hit(deck);
                    player.ShowHand();
                }
                else if (action == "2")
                {
                    playerTurnActive = false;
                }
                else
                {
                    Console.WriteLine("Invalid choice.");
                }
            }

            if (player.Hand.IsBust() == false)
            {
                Console.WriteLine("\nDealer's turn:");
                dealer.ShowHand();
                dealer.DealerTurn(deck);
            }

            // --- RESULT ---
            Console.WriteLine("\n--- RESULT ---");
            player.ShowHand();
            dealer.ShowHand();

            int playerTotal = player.Hand.GetValue();
            int dealerTotal = dealer.Hand.GetValue();

            if (player.Hand.IsBust())
            {
                Console.WriteLine("You busted! You lose $" + bet);
                player.Balance = player.Balance - bet;
            }
            else if (dealer.Hand.IsBust())
            {
                Console.WriteLine("Dealer busted! You win $" + bet);
                player.Balance = player.Balance + bet;
            }
            else if (playerTotal > dealerTotal)
            {
                Console.WriteLine("You win $" + bet + "!");
                player.Balance = player.Balance + bet;
            }
            else if (dealerTotal > playerTotal)
            {
                Console.WriteLine("Dealer wins. You lose $" + bet);
                player.Balance = player.Balance - bet;
            }
            else
            {
                Console.WriteLine("Push! It's a tie. You get your bet back.");
            }

            playingRound = AskPlayAgain();
        }
    }

    private bool AskPlayAgain()
    {
        Console.Write("\nPlay another round? (y/n): ");
        string answer = Console.ReadLine();
        return answer == "y" || answer == "Y";
    }
}