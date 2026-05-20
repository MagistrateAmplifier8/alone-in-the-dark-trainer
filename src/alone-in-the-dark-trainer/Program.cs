using System;
using System.Threading;

namespace AloneInTheDarkTrainer
{
    /// <summary>
    /// Main entry point for the trainer. Listens for hotkeys to toggle cheats.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Alone in the Dark Trainer v1.0");
            Console.WriteLine("Make sure the game process 'ALONE' is running.");
            Console.WriteLine("Hotkeys: [F1] Health, [F2] Sanity, [F3] Ammo, [F4] Item, [F5] Speed, [F6] Teleport");
            Console.WriteLine("Press [Esc] to exit.");

            try
            {
                using var engine = new CheatEngine("ALONE");
                engine.StartLoop();

                bool running = true;
                while (running)
                {
                    if (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(true).Key;
                        switch (key)
                        {
                            case ConsoleKey.F1:
                                engine.SetHealthUnlimited(true);
                                Console.WriteLine("Health: Unlimited ON");
                                break;
                            case ConsoleKey.F2:
                                engine.SetSanityUnlimited(true);
                                Console.WriteLine("Sanity: Unlimited ON");
                                break;
                            case ConsoleKey.F3:
                                engine.SetAmmoUnlimited(true);
                                Console.WriteLine("Ammo: Infinite ON");
                                break;
                            case ConsoleKey.F4:
                                engine.SetInventoryItem(2); // Gun
                                Console.WriteLine("Item: Gun added");
                                break;
                            case ConsoleKey.F5:
                                engine.SetGameSpeed(2.0f);
                                Console.WriteLine("Speed: 2x");
                                break;
                            case ConsoleKey.F6:
                                engine.TeleportToCheckpoint(150.0f, 200.0f);
                                Console.WriteLine("Teleported to checkpoint");
                                break;
                            case ConsoleKey.Escape:
                                running = false;
                                break;
                        }
                    }
                    Thread.Sleep(50);
                }

                engine.StopLoop();
                Console.WriteLine("Trainer stopped.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
