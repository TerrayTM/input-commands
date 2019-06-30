using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;

namespace Input_Commands
{
    class Program
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, EntryPoint = "mouse_event")]
        public static extern void MouseEvent(uint eventFlags, uint x, uint y, uint buttons, uint info);

        private const int MOUSEEVENT_LEFTDOWN = 0x02;
        private const int MOUSEEVENT_LEFTUP = 0x04;
        private const int MOUSEEVENT_RIGHTDOWN = 0x08;
        private const int MOUSEEVENT_RIGHTUP = 0x10;

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(">  Input Commands Version 1.0 By Terry Zheng  <");
            Console.WriteLine(">  A simple input controller using commands.  <");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;

            while (true)
            {
                string command = Console.ReadLine();

                if (string.IsNullOrEmpty(command))
                {
                    PrintError("Invalid Command");
                    continue;
                }

                string[] parts = command.Split(' ');

                if (parts.Length == 1)
                {
                    switch (parts[0])
                    {
                        case "Exit":
                            Application.Exit();
                            break;
                        case "MouseLeftClick":
                            MouseLeftClick();
                            break;
                        case "MouseRightClick":
                            MouseRightClick();
                            break;
                        case "Help":
                            string help = "Commands:\n" +
                            "[MouseLeftClick]          - Performs a mouse left click.\n" +
                            "[MouseRightClick]         - Performs a mouse right click.\n" +
                            "[MouseLeftUp]             - Sets mouse left up at a position.\n" +
                            "[MouseLeftDown]           - Sets mouse left down at a position.\n" +
                            "[MouseRightUp]            - Sets mouse right up at a position.\n" +
                            "[MouseRightDown]          - Sets mouse right down at a position.\n" +
                            "[SetMousePosition]        - Sets mouse position on screen.\n" +
                            "[SendKeys]                - Sends the specified key strokes.\n" +
                            "[Help]                    - Displays a list of commands.\n" +
                            "[Exit]                    - Exits the program.";

                            Console.WriteLine();
                            Console.WriteLine(help);
                            Console.WriteLine();
                            break;
                        default:
                            PrintError("Invalid Command");
                            break;
                    }
                }
                else if (parts.Length > 1)
                {
                    switch (parts[0])
                    {
                        case "SetMousePosition":
                            CallWithPosition(parts[1], SetMousePosition);
                            break;
                        case "MouseLeftUp":
                            CallWithPosition(parts[1], MouseLeftUp);
                            break;
                        case "MouseLeftDown":
                            CallWithPosition(parts[1], MouseLeftDown);
                            break;
                        case "MouseRightUp":
                            CallWithPosition(parts[1], MouseRightUp);
                            break;
                        case "MouseRightDown":
                            CallWithPosition(parts[1], MouseRightDown);
                            break;
                        case "SendKeys":
                            string text = string.Join(" ", parts.Skip(1).ToArray());

                            SendKeys.SendWait(text);
                            break;
                        default:
                            PrintError("Invalid Command");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid Command");
                    continue;
                }
            }
        }

        private static void CallWithPosition(string position, Action<uint, uint> method)
        {
            string[] arguments = position.Split('|');

            if (arguments.Length != 2)
            {
                PrintError("Invalid Arguments");
            }
            else if (uint.TryParse(arguments[0], out uint x) && uint.TryParse(arguments[1], out uint y))
            {
                method.Invoke(x, y);
            }
            else
            {
                PrintError("Invalid Arguments");
            }
        }

        private static void PrintError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: {0}", message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void SetMousePosition(uint x, uint y)
        {
            Cursor.Position = new Point((int)x, (int)y);
        }

        private static void MouseLeftClick()
        {
            MouseEvent(MOUSEEVENT_LEFTDOWN | MOUSEEVENT_LEFTUP, 0, 0, 0, 0);
        }

        private static void MouseLeftDown(uint x, uint y)
        {
            SetMousePosition(x, y);
            MouseEvent(MOUSEEVENT_LEFTDOWN, 0, 0, 0, 0);
        }

        private static void MouseLeftUp(uint x, uint y)
        {
            SetMousePosition(x, y);
            MouseEvent(MOUSEEVENT_LEFTUP, 0, 0, 0, 0);
        }

        private static void MouseRightClick()
        {
            MouseEvent(MOUSEEVENT_RIGHTDOWN | MOUSEEVENT_RIGHTUP, 0, 0, 0, 0);
        }

        private static void MouseRightDown(uint x, uint y)
        {
            SetMousePosition(x, y);
            MouseEvent(MOUSEEVENT_RIGHTDOWN, 0, 0, 0, 0);
        }

        private static void MouseRightUp(uint x, uint y)
        {
            SetMousePosition(x, y);
            MouseEvent(MOUSEEVENT_RIGHTUP, 0, 0, 0, 0);
        }
    }
}