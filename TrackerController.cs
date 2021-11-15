using ConsoleTableExt;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DegreeTracker
{
    internal static class TrackerController
    {
        //Open a connection to the Database
        static readonly string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");

        //Give the user some instructions and display an options menu
        internal static void GetUserCommand()
        {
            Console.WriteLine("\nMain menu");
            Console.WriteLine("\nInstructions:\nAfter entering a number, press enter to execute function\n");
            Console.WriteLine("\nType 0 to exit the program");
            Console.WriteLine("\nType 1 to view your degree progress");
            Console.WriteLine("\nType 2 to add a completed class");
            Console.WriteLine("\nType 3 to edit a class");
            Console.WriteLine("\nType 4 to delete a class");

            //assign user input to a string
            string userInput = Console.ReadLine();

            //If user presses enter without typing a number, returns statement informing user to choose an option
            if (string.IsNullOrEmpty(userInput))
            {
                Console.WriteLine("\n************   Please select an option from the menu   ************\n");
                GetUserCommand();
            }

            //convert user string input to an integer
            int command = Convert.ToInt32(userInput);

            //The switch statement selects a statement list to execute based on a pattern match with a match expression
            switch (command)
            {
                case 0:
                    Environment.Exit(0);
                    break;
                case 1:
                    //ViewRecords();
                    using (var connection = new SqliteConnection(connectionString))
                    {
                        connection.Open();
                        var dBCommand = connection.CreateCommand();
                        dBCommand.CommandText = "SELECT * FROM classes";

                        List<UpdateClass> tableData = new List<UpdateClass>();

                        connection.Close();
                    }

                    GetUserCommand();
                    break;
                case 2:
                    //Insert new record into the classes table

                    Console.WriteLine("\nEnter the class name\n");
                    string className = Console.ReadLine();

                    Console.WriteLine("\nEnter the amount of credits earned\n");
                    int creditsEarned = int.Parse(Console.ReadLine());

                    Console.WriteLine("\nEnter the class gpa\n");
                    int classGpa = Int32.Parse(Console.ReadLine());

                    //open connection to the database
                    using (var connection = new SqliteConnection(connectionString))
                    {
                        connection.Open();
                        //use the connection here
                        var dBCommand = connection.CreateCommand();
                        dBCommand.CommandText = $"insert into classes (name, credits, gpa) values ('{className}','{ creditsEarned}','{classGpa}')";
                        dBCommand.ExecuteNonQuery();
                        connection.Close();
                    }

                    Console.WriteLine("\nYour class was submitted!\n");
                    GetUserCommand();
                    break;
                case 3:
                    //Edit();
                    Console.WriteLine("\n************   Edit function not yet available   ***********");
                    GetUserCommand();
                    break;
                case 4:
                    //Delete();
                    Console.WriteLine("\n************   Delete function not yet available   ***********");
                    GetUserCommand();
                    break;
                default:
                    Console.WriteLine("\n************   Please select an option from the menu   ************\n");
                    GetUserCommand();
                    break;
            }
        }
    }
}
