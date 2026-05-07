namespace ConsoleApp1;

public class Player
{
    public string Name { get; private set; }
    public Hand Hand { get; private set; }
    public int Balance { get; set; }

    private DateTime startTime;
    private int timeLimitSeconds = 500; 

    public Player(string name)
    {
        Name = name;
        Hand = new Hand();
        Balance = 0;
        startTime = DateTime.Now; 
    }

    public bool IsTimeUp()
    {
        double secondsPassed = (DateTime.Now - startTime).TotalSeconds;
        return secondsPassed >= timeLimitSeconds;
    }

    public int GetTimeLeft()
    {
        double secondsPassed = (DateTime.Now - startTime).TotalSeconds;
        int timeLeft = timeLimitSeconds - (int)secondsPassed;

        if (timeLeft < 0)
        {
            timeLeft = 0;
        }

        return timeLeft;
    }

    public void Hit(Deck deck)
    {
        Card card = deck.Deal();
        Hand.AddCard(card);
        Console.WriteLine(Name + " drew: " + card);
    }

    public void ShowHand()
    {
        Console.WriteLine(Name + "'s hand:");
        Hand.ShowHand();
    }

    public void ClearHand()
    {
        Hand.Clear();
    }
}