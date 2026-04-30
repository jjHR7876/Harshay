namespace ConsoleApp1;

public enum Suit
{
    Hearts,
    Diamonds,
    Clubs,
    Spades
}

public enum Rank
{
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5,
    Six = 6,
    Seven = 7,
    Eight = 8,
    Nine = 9,
    Ten = 10,
    Jack = 10,
    Queen = 10,
    King = 10,
    Ace = 11
}

public class Card
{
    public Suit Suit { get; private set; }
    public Rank Rank { get; private set; }

    public int BlackjackValue
    {
        get
        {
            return (int)Rank;
        }
    }

    public Card(Suit suit, Rank rank)
    {
        Suit = suit;
        Rank = rank;
    }

    public override string ToString()
    {
        return Rank + " of " + Suit;
    }
}

public class Deck
{
    private Queue<Card> _cards = new Queue<Card>();

    public int Count
    {
        get { return _cards.Count; }
    }

    public Deck(int numberOfDecks = 1)
    {
        for (int d = 0; d < numberOfDecks; d++)
        {
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Rank rank in Enum.GetValues(typeof(Rank)))
                {
                    Card card = new Card(suit, rank);
                    _cards.Enqueue(card);
                }
            }
        }
    }

}