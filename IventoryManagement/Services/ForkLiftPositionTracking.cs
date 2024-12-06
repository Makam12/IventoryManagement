using System;
using System.Collections.Generic;



namespace IventoryManagement.Services
{
    public class ForkLiftPositionTracking
    {

            private static readonly string[] Directions = { "North", "East", "South", "West" };
            private static readonly HashSet<string> Obstacles = new HashSet<string> { "3,3", "4,5", "7,8" };

            public int X { get; set; } = 0;
            public int Y { get; set; } = 0;
            public string Direction { get; set; } = "North";

            // Process commands and return position and log of actions
            public (string message, int x, int y, string direction, List<string> log) ProcessCommands(string commands)
            {
                var log = new List<string>();
                int i = 0;
                while (i < commands.Length)
                {
                    char cmd = commands[i];
                    if (cmd == 'F' || cmd == 'B') // Move commands
                    {
                        i++;
                        int distance = int.Parse(commands[i].ToString());
                        for (int j = 0; j < distance; j++)
                        {
                            if (cmd == 'F')
                            {
                                MoveForward();
                                log.Add($"Move Forward by 1 meter.");
                            }
                            else
                            {
                                MoveBackward();
                                log.Add($"Move Backward by 1 meter.");
                            }

                            string currentPos = $"{X},{Y}";
                            if (Obstacles.Contains(currentPos))
                            {
                                return ($"Collision detected at {currentPos}", X, Y, Direction, log);
                            }
                        }
                    }
                    else if (cmd == 'L') // Turn Left
                    {
                        TurnLeft();
                        log.Add("Turn Left by 90 degrees.");
                    }
                    else if (cmd == 'R') // Turn Right
                    {
                        TurnRight();
                        log.Add("Turn Right by 90 degrees.");
                    }
                    i++;
                }
                return ($"Final position: ({X}, {Y}), Facing: {Direction}", X, Y, Direction, log);
            }

            private void MoveForward()
            {
                switch (Direction)
                {
                    case "North": Y++; break;
                    case "East": X++; break;
                    case "South": Y--; break;
                    case "West": X--; break;
                }
            }

            private void MoveBackward()
            {
                switch (Direction)
                {
                    case "North": Y--; break;
                    case "East": X--; break;
                    case "South": Y++; break;
                    case "West": X++; break;
                }
            }

            private void TurnLeft()
            {
                int currentIndex = Array.IndexOf(Directions, Direction);
                Direction = Directions[(currentIndex - 1 + 4) % 4]; // Rotate counterclockwise
            }

            private void TurnRight()
            {
                int currentIndex = Array.IndexOf(Directions, Direction);
                Direction = Directions[(currentIndex + 1) % 4]; // Rotate clockwise
            }
        }
    }



