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


    public void shuffle()
    {
        Card[] shuffle = new Card[_cards.Count];
        int[] randomizer = new int[_cards.Count];
        int x = 0;
        
        for (int i = 0; i < _cards.Count; i++)
        {
            while (randomizer.Contains(x))
            {
                x = rnd.Next(0, _cards.Count);
            }
            randomizer[i] = x;
            shuffle[x] = _cards.Dequeue();
        }
        for (int i = 0; i < _cards.Count; i++)
        {
       
            _cards.Enqueue(shuffle[i]);
        }
        
    }
    public Card Deal()
    {
     
            x =  _cards.Dequeue();
            _cards.Enqueue(x);
            return x;

    }

    public Card Peek()
    {
            return _cards.Peek();
    }
    
}


public class Hand
{
    private List<Card> cards = new List<Card>();

    public void AddCard(Card card)
    {
        cards.Add(card);
    }

    public int GetValue()
    {
        int total = 0;
        int aceCount = 0;

        foreach (Card card in cards)
        {
            total += card.BlackjackValue;

            if (card.Rank == Rank.Ace)
            {
                aceCount++;
            }
        }

        while (total > 21 && aceCount > 0)
        {
            total -= 10;
            aceCount--;
        }

        return total;
    }

    public bool IsBust()
    {
        return GetValue() > 21;
    }

    public bool HasBlackjack()
    {
        return cards.Count == 2 && GetValue() == 21;
    }

    public void ShowHand()
    {
        foreach (Card card in cards)
        {
            Console.WriteLine(card);
        }

        Console.WriteLine("Total: " + GetValue());
    }

    public void ShowDealerFirstCard()
    {
        if (cards.Count > 0)
        {
            Console.WriteLine(cards[0]);
            Console.WriteLine("Second card is hidden.");
        }
    }

    public void Clear()
    {
        cards.Clear();
    }
}

public class Player
{
    public string Name { get; private set; }
    public Hand Hand { get; private set; }

    public Player(string name)
    {
        Name = name;
        Hand = new Hand();
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

public class Dealer
{
    public Hand Hand { get; private set; }

    public Dealer()
    {
        Hand = new Hand();
    }

    public void Hit(Deck deck)
    {
        Card card = deck.Deal();
        Hand.AddCard(card);
        Console.WriteLine("Dealer drew: " + card);
    }

    public void DealerTurn(Deck deck)
    {
        while (Hand.GetValue() < 17)
        {
            Hit(deck);
        }
    }

    public void ShowHand()
    {
        Console.WriteLine("Dealer's hand:");
        Hand.ShowHand();
    }

    public void ShowFirstCard()
    {
        Console.WriteLine("Dealer's hand:");
        Hand.ShowDealerFirstCard();
    }

    public void ClearHand()
    {
        Hand.Clear();
    }
}

