using System;
using System.Threading;
using System.Collections.Generic;

//Implement the "Falling Rocks" game in the text console. A small dwarf stays at the
//bottom of the screen and can move left and right (by the arrows keys). 
//A number of rocks of different sizes and forms constantly fall down and you need
//to avoid a crash.Rocks are the symbols ^, @, *, &, +, %, $, #, !, ., ;, - 
//distributed with appropriate density. The dwarf is (O).
//Ensure a constant game speed by Thread.Sleep(150).
//
//It has been only a month toying with C# so the code is likely to be far from optimal


class FallingRocks
{

    // Game Stats ( !! Minnimum GameWidth=40 !!)
    const int GameWidth = 70;
    const int GameHeight = 40;
    static int Lives = 5;
    static int Score = 0;
    static int DifficultyOverScreenSize = GameWidth / 10;
    // used to slow the score
    static int gameCounter = 0;
    static int CurrentRows = 3;

    // Defines each object ( e.g. rocks , midget ) that is generated
    struct GameObject
    {
        // Coordinates of x and y axis
        public int x;
        public int y;

        // Symbol and color of the rock          
        public char c;
        public ConsoleColor color;
    }
    
    #region Rocks
    // An array that defines all the possible characters a rock can take ( only 1 character per rock though )
    static char[] Rocks = 
    {
       '^',
       '@',
       '*',
       '&',
       '%',
       '$',
       '#',
       '!',
       '.',
       ';',
       '+'
    };
    #endregion

    #region Colours
    // An array that defines all the possible colors a rock can take ( only 1 character per rock though )
    static ConsoleColor[] Colours = 
    {
        ConsoleColor.Gray,
        ConsoleColor.Cyan,
        ConsoleColor.Magenta,
        ConsoleColor.Yellow,
        ConsoleColor.Green                                    
    };
    #endregion

    // Generates an instance of the class Random so I can apply random
    static Random random = new Random();

    // This method is used to print different objects as a console output ( e.g. game boarders , rocks , the dwarf , etc.)
    static void Print(int row, int col, object data, ConsoleColor colour)
    {
        Console.SetCursorPosition(col, row);
        Console.Write(data);
        Console.ForegroundColor = colour;
    }    
    
    // This method prints the interface of the game ( boarders , score,lives, keys etc. )
    static void PrintGameBoardersScoreLivesAndKeys()
    {
        for (int colum = 0; colum < GameWidth; colum++)
        {
            Print(2, colum, "_", ConsoleColor.White);
            Print(GameHeight - 3, colum, "_", ConsoleColor.White);
        }

        // uses Print method that is described a few lines above
        Print(1, 0, "Score:" + Score, ConsoleColor.White);
        Print(1, GameWidth - 9, "Lives: " + Lives, ConsoleColor.White);
        Print(1, GameWidth / 2 - 10, "Use <- and -> arows!", ConsoleColor.White);
    }

    

    static void Main()
    {
        // Game Window Parameters:
        Console.Title = "Falling Rocks: Run for your madafakin' life!";
        // Sets the width of the console to the constant GameWidth ( If I want to change the width of the console I have to change the game parameter constant )
        Console.WindowWidth = GameWidth;
        Console.BufferWidth = GameWidth;
        // Sets the height of the console to the constant GameHeight ( If I want to change the height of the console I have to change the game parameter constant )
        Console.WindowHeight = GameHeight;
        Console.BufferHeight = GameHeight;
        // Hide that cursore so that it is not visible
        Console.CursorVisible = false;

        // Print Display 
        PrintGameBoardersScoreLivesAndKeys();

        // Create the midget at the middle of the current window
        GameObject Midget = new GameObject();
        Midget.x = GameWidth / 2;
        Midget.y = GameHeight - 3;
        Midget.c = 'O';
        Midget.color = ConsoleColor.White;

        // Creates a list of objects 
        // Each rock of every row will be stored in a list , so that it's coordinates are known and i can move it on each iteration
        List<GameObject> FirstRowOfRocks = new List<GameObject>();

        // I need an endless cycle in case the player is undefeatable 
        while (true)
        {
            bool DetectCollision = false;
            // Get a random value from [0, GameWidth) and store it in RockCOl
            int RockCol = random.Next(0, GameWidth);
            // Get a random value from [0, The length of the array that holds the possible symbols for a rock to have)
            char RockChar = Rocks[random.Next(0, Rocks.Length)];
            // Get a random value from [0 , the length of the array that holds the possible colors for a rock to have)
            ConsoleColor RockColor = Colours[random.Next(0, Colours.Length)];

            // This loop generates rocks and stores each in the of rocks
            // Minimum of 1 rock will fall on each iteration of the while loop
            // The maximum of rocks per row is a constant game parameter called  DifficultyOverScreenSize
            // Each row of rocks will have a random number of rocks , but not less than 1 and no more than the difficulty requires
            for (int i = 1; i < random.Next(1, DifficultyOverScreenSize); i++) //New Line of Rocks Production !!!
            {
                GameObject NewRock = new GameObject();
                // CurrentRows - 1 means that every rock will be generated 1 row above the last one that has been printed already
                // CurrentRows is set to three so that the first row of rocks is printed at 2 instead of 0 because of the game boarders
                NewRock.y = CurrentRows - 1;
                // Initialises the rock with the randoms that are set to the variables
                NewRock.x = RockCol;
                NewRock.c = RockChar;
                NewRock.color = RockColor;

                FirstRowOfRocks.Add(NewRock);
            }

            // Check if the key to move the midget is pressed already
            while (Console.KeyAvailable) //Moving the Midget
            {
                ConsoleKeyInfo PressedKey = Console.ReadKey(true);
                // Check which button is pressed and then move the midget accordingly
                if (PressedKey.Key == ConsoleKey.RightArrow)
                {
                    // Check if midget is already at the end of the screen ( + 2 because of the game boarders)
                    if (Midget.x + 2 < GameWidth)
                    {
                        Midget.x++;
                    }
                }

                else if (PressedKey.Key == ConsoleKey.LeftArrow)
                {
		    // Check if midget is already at the end of the screen ( + 2 because of the game boarders)
                    if (Midget.x - 2 >= 0)
                    {
                        Midget.x--;
                    }
                }
            }

            // Creates a new row of rocks , that has the same objects as FirstRowOfRocks , except y + 1 creates the illusion the first row moved downwards
            // Then it stores the current rocks in a new list called CurrentRocks
            List<GameObject> CurrentRocks = new List<GameObject>();
            for (int i = 0; i < FirstRowOfRocks.Count; i++)
            {
                GameObject OldRock = FirstRowOfRocks[i];
                GameObject CurrentRock = new GameObject();
                CurrentRock.x = OldRock.x;
                CurrentRock.y = OldRock.y + 1;
                CurrentRock.c = OldRock.c;
                CurrentRock.color = OldRock.color;
                // Check if the rock has to appear ( if CurrentRock.y > GameHeight - 2 it has to disappear , because it hits the ground )
                if (CurrentRock.y < GameHeight - 2)
                {
                    CurrentRocks.Add(CurrentRock);
                }
            }
            
            // It's a little subtle logic here and I intend to improve it in the future
            FirstRowOfRocks = CurrentRocks;

            // This loop checks if the midget and one of the rocks are at the same coordinate ( if a rock hit the midget )
            foreach (GameObject CurrentRock in CurrentRocks)
            {
                if (((CurrentRock.x == Midget.x - 1) || (CurrentRock.x == Midget.x) ||
                    (CurrentRock.x == Midget.x + 1)) && (CurrentRock.y == Midget.y))
                {
                    DetectCollision = true;
                }
            }
            
            // Clear all the output on the console
            Console.Clear();

            // If the midget is hit take a life from him !
            if (DetectCollision)
            {
                Lives--;
                // If the player is out of live it's Game Over
                if (Lives == 0)
                {
                    // Display GAME OVER screen
                    Print(GameHeight / 2 - 1, GameWidth / 2 - 5, "GAME OVER", ConsoleColor.White);
                    Print(GameHeight / 2 + 1, GameWidth / 2 - 5, "Score : " + Score, ConsoleColor.White);
                    Console.ReadLine();
                    return;
                }
                
                // Reset the position of the midget and start generating rocks all over from the first row
                else
                {                  
                    foreach (GameObject NewRock in FirstRowOfRocks)
                    {
                        Print(CurrentRows, NewRock.x, NewRock.c, NewRock.color);
                    }                   
                    PrintGameBoardersScoreLivesAndKeys();
                    Print(Midget.y, Midget.x - 1, '(', ConsoleColor.White);
                    Print(Midget.y, Midget.x, Midget.c, ConsoleColor.White);
                    Print(Midget.y, Midget.x + 1, ')', ConsoleColor.White);
                    FirstRowOfRocks.Clear();
                }
            }
            // If the midget is not hit then print all the data that is already stored in already
            // Subtle logic again
            else
            {
                foreach (GameObject NewRock in FirstRowOfRocks)
                    {
                        Print(NewRock.y, NewRock.x, NewRock.c, NewRock.color);
                    }                   
                    PrintGameBoardersScoreLivesAndKeys();
                    Print(Midget.y, Midget.x - 1, '(', ConsoleColor.White);
                    Print(Midget.y, Midget.x, Midget.c, ConsoleColor.White);
                    Print(Midget.y, Midget.x + 1, ')', ConsoleColor.White);
            }
            // Increase score by 1 on each 2 itterations of the while loop
            gameCounter++;
            if (gameCounter % 2 == 0)
                Score++;
            
            // Execute each iteration of the endless while loop with a pause of 150 milliseconds ,so that the player has time to react
            Thread.Sleep(150);
        }
    }
}



