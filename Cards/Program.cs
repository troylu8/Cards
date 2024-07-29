namespace Cards;

using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class Program {

    static readonly Random random = new Random();

    public static void Main(string[] args) {
        Console.OutputEncoding = UTF8Encoding.Default;

        while (true) {
            StartGame();

            Thread.Sleep(1000);
            Console.WriteLine("\n");
            CountDown();
            PromptAnyKey("play again?");
            Console.Clear();
        }
    }
    
    public static void StartGame() {

        PromptAnyKey("the deck is deftly shuffled. begin?");

        int[] sets = [4, 5, 6, 7, 8, 9, 12];
        int completed = 0;
        int round = 0;
        
        for (; round < sets.Length; round++) {
            bool won = StartSet(sets[round], 1000 - round * 100);
            
            if (!won) {
                string[] tips = [
                    "* hit a key as soon as the odd card appears!",
                    "* if a card has passed, you cannot select it",
                ];

                ColorPrinter.WriteLine(tips[random.Next(tips.Length)], ConsoleColor.Blue);
                break;
            } 
            
            completed += sets[round];

            PromptAnyKey("\nchallenge next set?");
            Console.Clear();
        }
        
        ConsoleColor resultsColor;
        if      (round < 2)   resultsColor = ConsoleColor.DarkGray;
        else if (round < 4)   resultsColor = ConsoleColor.DarkYellow;
        else if (round < 7)   resultsColor = ConsoleColor.DarkMagenta;
        else  /*(completed)*/ resultsColor = ConsoleColor.Cyan;

        ColorPrinter.WriteLine($"\ncompleted {completed}/52 cards\t({round} round{((round == 1)? "" : "s")})", resultsColor);
    }

    public static void PromptAnyKey(string prompt) {
        Console.Write(prompt);
        ColorPrinter.WriteLine("\t(press any key)", ConsoleColor.DarkGray);
        WaitForInput();
    }

    public static ConsoleKeyInfo WaitForInput() {
        while (Console.KeyAvailable) Console.ReadKey(true);
        return Console.ReadKey(true);
    }

    public static void CountDown() {
        string cd = "...!";

        Thread.Sleep(500);
        for (int i = 0; i < cd.Length; i++) {
            Console.Write(cd[i]);
            Thread.Sleep(300);
        }
        Console.WriteLine();
    }

    public static bool StartSet(int amt, int delay) {
        
        Console.WriteLine("pay attention!");
        CountDown();
        Console.WriteLine();

        Card[] drawn = new Card[amt];

        for (int i = 0; i < amt; i++) {
            Card c = Deck.DrawCard();
            c.PrintLine();
            drawn[i] = c;

            Thread.Sleep(delay);
        }
        
        Console.Clear();
        PromptAnyKey("now, which wasn't there before?");
        CountDown();
        Console.WriteLine();

        Card odd = Deck.GetRandomUndrawnCard();
        int oddIndex = random.Next(amt);
        
        Card curr = (0 == oddIndex)? odd : drawn[0];

        var cTokenSrc = new CancellationTokenSource();
        CancellationToken cToken = cTokenSrc.Token;

        Task printCards = Task.Run(() => {
            curr.PrintLine();
            Thread.Sleep(delay);

            for (int i = 1; i < amt && !cToken.IsCancellationRequested; i++) {
                curr = (i == oddIndex)? odd : drawn[i];
                curr.PrintLine();

                Thread.Sleep(delay);
            }
        });

        WaitForInput();
        cTokenSrc.Cancel();

        Console.Write("\nselected: ");
        curr.Print();
        Console.Write("\tanswer: ");
        odd.PrintLine();

        bool won = odd.Equals(curr);
        Console.WriteLine( won? "victory." : "a loss." );

        return won;
    }

    
}