using System;
using System.IO;

namespace Terminal
{
    class Program
    {
        public static string currDir = @"C:\Users\Chris\source\repos\CodeBreaker\CodeBreaker\player\Root";
        public static string clipboard = "";
        public static string clipboardFileName = "";
        static void Main(string[] args)
        {
            startConsole();
        }

        private static void startConsole()
        {
            while (true)
            {
                Console.Write(currDir + "> ");
                string command = Console.ReadLine();
                string[] commandParts = command.Split();
                try
                {
                    switch (commandParts[0])
                    {
                        //help
                        case "help":
                            Console.WriteLine();
                            break;

                        //cd [directory name]
                        //"cd .." to go back
                        case "cd":
                            try
                            {
                                if (commandParts[1] == "..")
                                {
                                    if(Directory.GetParent(currDir) != null)
                                    {
                                        currDir = Directory.GetParent(currDir).ToString();
                                    }
                                }
                                else if (DirectoryExistsCaseSensitive(currDir + "\\" + commandParts[1]))
                                {
                                    currDir = currDir + "\\" + commandParts[1];
                                }
                                else
                                {
                                    Console.WriteLine("That directory does not exist.\n");
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Something went wrong.");
                            }
                            break;

                        //edit [filename]
                        //if it doesnt exist, creates it and then opens it in the editor
                        case "edit":


                        //read [filename]
                        case "read":
                            if (FileExistsCaseSensitive(currDir + @"\" + commandParts[1]))
                            {
                                try
                                {
                                    // Read each line of the file into a string array. Each element
                                    // of the array is one line of the file.
                                    string[] lines = File.ReadAllLines(currDir + "\\" + commandParts[1]);

                                    // Display the file contents by using a foreach loop.
                                    Console.WriteLine("Contents of " + commandParts[1] + ": ");
                                    foreach (string line in lines)
                                    {
                                        // Use a tab to indent each line of the file.
                                        Console.WriteLine("\t" + line);
                                    }

                                    // Keep the console window open in debug mode.
                                    Console.WriteLine("Press any key to exit.\n");
                                    System.Console.ReadKey();
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("There is nothing in that file.\n");
                                }
                            }
                            else
                            {
                                Console.WriteLine("That file does not exist.\n");
                            }
                            break;

                        case "run":
                            try
                            {
                                System.Diagnostics.Process.Start(commandParts[1]);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Something went wrong.\n");
                            }
                            break;

                        //copy [filename]
                        case "copy":
                            try
                            {
                                if (FileExistsCaseSensitive(currDir + @"\" + commandParts[1]))
                                {
                                    clipboard = currDir + "\\" + commandParts[1];
                                    clipboardFileName = commandParts[1];
                                    Console.WriteLine("");
                                }
                                else if (DirectoryExistsCaseSensitive(currDir + @"\" + commandParts[1]))
                                {
                                    Console.WriteLine("You may not copy directories.");
                                }
                                else
                                {
                                    Console.WriteLine("That file does not exist.\n");
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Something went wrong.\n");
                            }
                            break;

                        //paste
                        case "paste":
                            if (clipboard == "")
                            {
                                Console.WriteLine("Your clipboard is empty.\n");
                            }
                            else
                            {
                                try
                                {
                                    File.Copy(clipboard, currDir + "\\" + clipboardFileName);
                                    Console.WriteLine("File successfully copied.\n");
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("something went wrong");
                                }
                            }
                            break;

                        //rename [file or directory name] [new name]
                        case "rename":
                            try
                            {
                                if (FileExistsCaseSensitive(currDir + "\\" + commandParts[1]))
                                {
                                    File.Move(currDir + "\\" + commandParts[1], currDir + "\\" + commandParts[2]);
                                }
                                else if (DirectoryExistsCaseSensitive(currDir + "\\" + commandParts[1]))
                                {
                                    Directory.Move(currDir + "\\" + commandParts[1], currDir + "\\" + commandParts[2]);
                                }
                                else
                                {
                                    Console.WriteLine("That file or directory does not exist.");
                                }
                            }
                            catch(Exception e)
                            {
                                Console.WriteLine("Something went wrong.");
                            }
                            break;

                        //list
                        case "list":
                            Console.WriteLine("");
                            string[] filePaths = Directory.GetFiles(currDir);
                            string[] dirPaths = Directory.GetDirectories(currDir);
                            foreach (string item in dirPaths)
                            {
                                Console.WriteLine(item.Remove(0, currDir.Length));
                            }
                            foreach (string item in filePaths)
                            {
                                Console.WriteLine(Path.GetFileName(item));
                            }
                            Console.WriteLine("");
                            break;

                        //cls
                        case "cls":
                            Console.Clear();
                            break;

                        //delete [file or directory name]
                        case "delete":
                            if (DirectoryExistsCaseSensitive(currDir + @"\" + commandParts[1]) || FileExistsCaseSensitive(currDir + @"\" + commandParts[1]))
                            {
                                Console.WriteLine("Are you sure you want to delete " + commandParts[1] + "?(Default no) ");
                                string ans = Console.ReadLine();
                                if (ans == "yes" || ans == "y")
                                {
                                    try
                                    {
                                        File.Delete(currDir + @"\" + commandParts[1]);
                                        Console.WriteLine("");
                                    }

                                    catch (Exception e)
                                    {

                                    }
                                    try
                                    {
                                        Directory.Delete(currDir + @"\" + commandParts[1]);
                                        Console.WriteLine("");
                                    }
                                    catch (Exception e)
                                    {

                                    }
                                }
                                else
                                {
                                    Console.WriteLine("");
                                }
                            }
                            else
                            {
                                Console.WriteLine("What you are trying to delete does not exist.\n");
                            }
                            break;

                        //new [file/directory] [name]
                        case "new":
                            try
                            {
                                if (commandParts[1] == "dir")
                                {
                                    Directory.CreateDirectory(currDir + @"\" + commandParts[2]);
                                    Console.WriteLine("");
                                }
                                if (commandParts[1] == "file")
                                {
                                    File.Create(currDir + @"\" + commandParts[2]);
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Something went wrong.\n");
                            }
                            break;

                        case "exit":
                            closeProgram();
                            break;

                        case "quit":
                            closeProgram();
                            break;

                        default:
                            Console.WriteLine("\"" + command + "\" is an unknown command. Please type \"help\" for a list of commands.\n");
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Something went very wrong.");
                }
            }
        }

        public static void closeProgram()
        {
            Environment.Exit(0);
        }

        public static bool FileExistsCaseSensitive(string filename)
        {
            string name = Path.GetDirectoryName(filename);

            return name != null
                   && Array.Exists(Directory.GetFiles(name), s => s == Path.GetFullPath(filename));
        }
        public static bool DirectoryExistsCaseSensitive(string dirname)
        {
            string name = Path.GetDirectoryName(dirname);

            return name != null
                   && Array.Exists(Directory.GetDirectories(name), s => s == Path.GetFullPath(dirname));
        }
    }
}
