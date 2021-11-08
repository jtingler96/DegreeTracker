using Microsoft.Data.Sqlite;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DegreeTracker
{
    class SqliteHelper
    {
        public static void InitialiseDB()
        {
            //Open a connection to the database using the value of ConnectionString. If Mode=ReadWriteCreate is used (the default) the file is created, if it doesn't already exist.
            string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                //Create a new command associated with the connection.
                var tableCmd = connection.CreateCommand();

                //Create a table
                tableCmd.CommandText =
                    $"create table classes (Id INTEGER PRIMARY KEY AUTOINCREMENT, Name STRING, Credits INTEGER, GPA INTEGER ) ";

                //Execute the CommandText against the database
                tableCmd.ExecuteNonQuery();

                //Close the connection to the database. Open transactions are rolled back.
                connection.Close();

                //Inform the user that a table was created
                Console.WriteLine("Table successfully created\n\n initializing database..\n\n");

                //Open the controller
                TrackerController.GetUserCommand();
            }
        }
    }
}
