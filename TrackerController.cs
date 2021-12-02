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
            Console.WriteLine("\nType 4 to remove a class");

            //assign user input to a string
            string userInput = inputInt();

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
                    viewProgress();
                    //Return to main menu
                    GetUserCommand();
                    break;
                case 2:
                    addClass();
                    //Return to main menu
                    GetUserCommand();
                    break;
                case 3:
                    //Placeholder
                    editClass();
                    //Return to main menu
                    GetUserCommand();
                    break;
                case 4:
                    removeClass();
                    //Return to main menu
                    GetUserCommand();
                    break;
                default:
                    Console.WriteLine("\n************   Please select an option from the menu   ************\n");
                    //Return to main menu
                    GetUserCommand();
                    break;
            }
        }

        internal static void viewProgress()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                SqliteCommand dBCommand = new SqliteCommand(
                 "SELECT id, name, credits, gpa FROM Classes;",
                 connection);
                connection.Open();

                List<UpdateClass> tableData = new List<UpdateClass>();
                SqliteDataReader reader = dBCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(
                        new UpdateClass
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Credits = reader.GetInt32(2),
                            GPA = reader.GetFloat(3)
                        });
                    }
                }
                else
                {
                    Console.WriteLine("No rows found");
                }
                reader.Close();
                Console.WriteLine("\n\n");

                ConsoleTableBuilder
                    .From(tableData)
                    .ExportAndWriteLine();
                Console.WriteLine("\n\n");
            }
        }

        internal static void addClass()
        {
            //Insert new record into the classes table

            Console.WriteLine("\nEnter the class name\n");
            string name = Console.ReadLine();

            Console.WriteLine("\nEnter the amount of credits earned\n");
            string credits = inputInt();

            Console.WriteLine("\nEnter the class gpa\n");
            string gpa = inputFloat();
            
            //open connection to the database
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                //use the connection here
                var dBCommand = connection.CreateCommand();
                dBCommand.CommandText = $"insert into classes (name, credits, gpa) values ('{name}','{ credits}','{gpa}')";
                dBCommand.ExecuteNonQuery();
                connection.Close();
            }

            Console.WriteLine($"\n\nYour class was submitted: {name} | {credits} credits | {gpa} gpa.\n\n");
        }

        internal static void removeClass()
        {
            Console.WriteLine("\n\nType the Id of the class you want to remove. Type 0 to return to main menu.\n\n");
       
            int Id = Int32.Parse(inputInt());

            if (Id == 0) GetUserCommand();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"DELETE from classes WHERE Id = '{Id}'";
                int rowCount = tableCmd.ExecuteNonQuery();
                while (rowCount == 0)
                {
                    Console.WriteLine($"\n\nClass with Id {Id} doesn't exist. Try Again or type 0 to return to main menu. \n\n");
                    Id = Int32.Parse(inputInt());

                    if (Id == 0) GetUserCommand();

                    if (rowCount != 0) break;
                }
                Console.WriteLine("\n\n\nclass was removed\n\n");
            }
        }

        internal static void editClass()
        {
            Console.WriteLine("\n\nType Id of the class would like to edit. Type 0 to return to main manu.\n\n");

            string userInput = inputInt();

            int Id = Convert.ToInt32(userInput);

            if (Id == 0) GetUserCommand();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM classes WHERE Id = {Id})";
                int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (checkQuery == 0)
                {
                    Console.WriteLine($"\n\nRecord with Id {Id} doesn't exist.\n\n");
                    editClass();
                }

                Console.WriteLine("\nEnter the class name\n");
                string name = Console.ReadLine();

                Console.WriteLine("\nEnter the amount of credits earned\n");
                string credits = inputInt();

                Console.WriteLine("\nEnter the class gpa\n");
                string gpa = inputFloat();

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"UPDATE classes SET name = '{name}', credits = '{credits}', gpa = '{gpa}' WHERE Id = {Id}";
                tableCmd.ExecuteNonQuery();

                Console.WriteLine($"\n\nChanges to your class were successful: {name} | {credits} credits | {gpa} gpa.\n\n");
                connection.Close();
            }
        }

        internal static string inputInt()
        {
            string userInput = Console.ReadLine();

            while (string.IsNullOrEmpty(userInput) || !int.TryParse(userInput, out _))
            {
                Console.WriteLine("\nInvalid input. Please type a numeric value");
                userInput = Console.ReadLine();
            }

            return userInput;

        }

        internal static string inputFloat()
        {
            string userInput = Console.ReadLine();

            while (string.IsNullOrEmpty(userInput) || !float.TryParse(userInput, out _))
            {
                Console.WriteLine("\nInvalid input. Please type a numeric value");
                userInput = Console.ReadLine();
            }
            return userInput;
        }
    }
}
