using Microsoft.Data.Sqlite;
using System;
using System.Configuration;
using System.IO;

namespace DegreeTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\nWelcome to your Degree Tracker!\n");           
            Console.WriteLine("\nChecking for existing database...\n");

            //check for an existing database
            string databasePath = ConfigurationManager.AppSettings.Get("DatabasePath");
            bool dbPath = File.Exists(databasePath);

            //If the database does not exist, create one
            if (!dbPath)
            {
                Console.WriteLine("\n\nDatabase doesn't exist, creating one...\n\n");
                SqliteHelper.InitializeDB();
            }
            //If a database exists continue to the controller
            else
            {
                Console.WriteLine("\nInitializing database..\n");
                TrackerController.GetUserCommand();
            };
        }
    }
}
