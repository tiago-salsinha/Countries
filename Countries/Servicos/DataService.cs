namespace Countries.Servicos
{
    using Countries.Modelos;
    using System;
    using System.Collections.Generic;
    using System.Data.SQLite;
    using System.IO;

    /// <summary>
    /// Class to insert and update data from the Api to the sqlite file
    /// </summary>
    public class DataService
    {
        
        private SQLiteConnection connectionCountries;

        private SQLiteCommand command;

        private DialogService dialogService;


        /// <summary>
        /// Constructor to create a Data folder and Create Table on the Sqlite file 
        /// </summary>
        public DataService()
        {
            dialogService = new DialogService();

            if (!Directory.Exists("Data"))
            {
                Directory.CreateDirectory("Data");
            }

            var path = @"Data\Countries.sqlite";


            //Create Table if it doesn´t exists
            try
            {
                connectionCountries = new SQLiteConnection("Data Source=" + path);
                connectionCountries.Open();

                
                string sqlcommand =
                    "CREATE TABLE IF NOT EXISTS Countries(Name nvarchar(250)," +
                        "Capital nvarchar(30)," +
                        "Region nvarchar(30)," +
                        "Subregion nvarchar(30)," +
                        "Population int," +
                        "Gini nvarchar(30)," +
                        "Flag nvarchar(300))";

                command = new SQLiteCommand(sqlcommand, connectionCountries);

                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                dialogService.ShowMessage("Erro", e.Message);
            }
        }


        /// <summary>
        /// Method to insert data into sqlite file
        /// </summary>
        public void SaveCountriesData(List<Country> Countries)
        {            
            try
            {
                foreach (var country in Countries)
                {                    
                    string sql =
                    string.Format("INSERT INTO Countries(Name, Capital, Region, Subregion, Population, Gini, Flag) values ('{0}','{1}','{2}','{3}',{4},'{5}','{6}')",
                        country.Name.Replace("'", " "), country.Capital.Replace("'", " "), country.Region.Replace("'", " "), country.Subregion.Replace("'", " "), country.Population, country.Gini, country.Flag.Replace("'", " "));

                    command = new SQLiteCommand(sql, connectionCountries);

                    command.ExecuteNonQuery();
                }

                connectionCountries.Close();
            }
            catch (Exception e)
            {
                dialogService.ShowMessage("Erro", e.Message);
            }
        }


        /// <summary>      
        /// Method to load the data from the Sqlite file if the application does not have internet connection
        /// </summary>
        public List<Country> GetCountriesData()
        {
            List<Country> Countries = new List<Country>();

            try
            {
                string sql = "SELECT Name, Capital, Region, Subregion, Population, Gini, Flag FROM Countries";

                command = new SQLiteCommand(sql, connectionCountries);
               
                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Countries.Add(new Country
                    {
                        Name = (string)reader["Name"],
                        Capital = (string)reader["Capital"],
                        Region = (string)reader["Region"],
                        Subregion = (string)reader["Subregion"],
                        Population = (int)reader["Population"],
                        Gini = (string)reader["Gini"],
                        Flag = (string)reader["Flag"]
                    });
                }

                connectionCountries.Close();

                return Countries;
            }
            catch (Exception e)
            {
                dialogService.ShowMessage("Erro", e.Message);
                return null;
            }
        }


        /// <summary>
        ///  Method to delete the data, everytime we connect to the Api, so that we always have the Sqlite file up to date
        /// </summary>
        public void DeleteCountriesData()
        {
            try
            {
                string sql = "DELETE FROM Countries";

                command = new SQLiteCommand(sql, connectionCountries);

                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                dialogService.ShowMessage("Erro", e.Message);
            }
        }
    }
}
