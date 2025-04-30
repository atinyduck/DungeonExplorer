namespace DungeonExplorer;

public static class UI
{
    #region Strings

    public static string Title => FileManager.ReadFileAsync("TitleText.txt").Result;

    public static string Intro => FileManager.ReadFileAsync("IntroText.txt").Result;

    public static string WelcomeText => FileManager.ReadFileAsync("WelcomeText.txt").Result;

    public static string MainMenuText => FileManager.ReadFileAsync("MainMenuText.txt").Result;

    #endregion

    #region Output Control

    /// <summary>
    /// Displays the message.
    /// </summary>
    /// <param name="message">The message.</param>
    public static void Message(string message, ConsoleColor colour = ConsoleColor.White,  bool clear = false, bool wait = false)
    {
        if (clear) Console.Clear();

        Console.WriteLine(message, colour);

        if (wait) WaitForInput();
    }

    #endregion

    #region Input Control

    /// <summary>
    /// Gets the input.
    /// </summary>
    /// <param name="valid_inputs">The valid inputs.</param>
    /// <param name="output">The output.</param>
    /// <returns>string: The user's input.</returns>
    public static string GetInput(List<string> inputs, string output, 
        bool validation = true, bool clear = false, bool wait = false)
    {
        while (true)
        {
            // Output message and get user input
            Message(output, clear: clear, wait: wait);
            Console.Write("\n :: ");
            string input = Console.ReadLine()?.Trim();

            if (!validation && !string.IsNullOrEmpty(input)) return input;
            

            if (!inputs.Contains(input, StringComparer.OrdinalIgnoreCase))
            {
                Message($"{input} is not a valid option.");
            }
            else return input;
            
        }
    }

    /// <summary>
    /// Confirm if the user wants to quit.
    /// </summary>
    /// <returns>bool: If the input is 'Y'</returns>
    public static bool Confirm(string message = null)
    {

        if (string.IsNullOrWhiteSpace(message))
            message = "Are you sure you want to quit?\nUnsaved progress will be deleted.";

        List<string> inputs = new List<string>() { "Y", "N" };
        string input = GetInput(inputs, message + "\n\n Y :: N");

        return string.Equals(input, "Y", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Waits for input from the user.
    /// </summary>
    public static void WaitForInput()
    {
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    #endregion
}