using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Sripirom.EnumGenerator.Services
{
    public class DataTableLoader
    {
        private readonly string _stringConnection;
        public DataTableLoader(string stringConnection)
        {
            _stringConnection = stringConnection;
        }
        public IEnumerable<Tuple<int, string, string>> Load(string columnId, string tableName)
        {
            IList<Tuple<int, string, string>> dataList = new List<Tuple<int, string, string>>();

            using (var connection = new SqlConnection(_stringConnection))
            {
                SqlCommand command = new SqlCommand(
                $"SELECT {columnId}, Name, Description FROM {tableName};",
                connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Console.WriteLine("{0}\t{1}", reader.GetInt32(0), reader.GetString(1));
                        dataList.Add(new Tuple<int, string, string>(reader.GetInt32(0), reader.GetString(1), reader.GetString(2)));
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }
                reader.Close();
            }
            
            return dataList;
        }
    }
}