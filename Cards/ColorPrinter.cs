namespace Cards;

using System;

public class ColorPrinter {

    public static void Write(object text, ConsoleColor color) {
        Console.ForegroundColor = color;
        Console.Write(text);
        Console.ResetColor();
    }
    public static void WriteLine(object text, ConsoleColor color) {
        Write(text, color);
        Console.WriteLine();
    }
}