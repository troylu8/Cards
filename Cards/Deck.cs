namespace Cards;

using System;

public class Deck {
    static readonly Card[] deck = new Card[52];
    static int size = 0;

    static Deck() {
        ResetDeck();
    }

    public static void ResetDeck() {

        size = 0;
        
        string[] values = ["A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K"];
        char[] suits = ['♠', '♥', '♣', '♦'];

        foreach (var value in values) {
            foreach (var suit in suits) {
                deck[size++] = new Card(value, suit);
            }
        }
    }

    static readonly Random random = new();

    public static Card DrawCard() {
        if (size == 0) throw new Exception("out of cards! deck size is 0");
        
        int i = random.Next(size);
        (deck[i], deck[size-1]) = (deck[size-1], deck[i]);
        return deck[--size];
    }

    public static Card GetRandomUndrawnCard() {
        if (size == 0) throw new Exception("out of cards! deck size is 0");

        return deck[random.Next(size)];
    }
}

public class Card {
    
    public string value;
    public char suit;

    public Card(string value, char suit) {
        this.value = value;
        this.suit = suit;
    }

    public void Print() {        
        ColorPrinter.Write(this, (suit == '♥' || suit == '♦')? ConsoleColor.DarkRed : ConsoleColor.DarkGray);
    }
    public void PrintLine() {
        Print();
        Console.WriteLine();
    }

    public override string ToString() {
        string spacing;

        if      (suit == '♠')   spacing = " ";
        else if (suit == '♥')   spacing = "  ";
        else if (suit == '♣')   spacing = "   ";
        else  /*(suit == '♦')*/ spacing = "    ";
        
        return $"{value,2}{spacing}{suit}";
    }

    public override bool Equals(object? obj) {
        if (obj == null || obj is not Card) return false;
        Card other = (Card) obj;
        return other.suit == this.suit && other.value == this.value;
    }

    public override int GetHashCode() {
        return HashCode.Combine(value, suit);
    }
}