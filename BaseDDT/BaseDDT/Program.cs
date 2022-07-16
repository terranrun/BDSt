using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace BaseDDT
{
    class Program
    {

        private static string connectionString = ConfigurationManager.ConnectionStrings["StudentDB"].ConnectionString;

        private static SqlConnection sqlConnection = null;
        static void Main(string[] args)
        {
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            Console.WriteLine("StudentApp");

            SqlDataReader sqlDataReader = null;

            string command = string.Empty;
            
            while(true)
            {
                try 
                {
                
                Console.Write("> ");
                command = Console.ReadLine();
                #region Exit
                if (command.ToLower().Equals("exit"))
                {
                    if(sqlConnection.State == ConnectionState.Open)
                    {
                        sqlConnection.Close();
                    }
                    if(sqlDataReader != null)
                    {
                        sqlDataReader.Close();
                    }
                    break;
                }
                #endregion

                if (command.ToLower().Equals("clear"))
                    {
                        Console.Clear();
                        continue;
                    }

                SqlCommand sqlCommand =null;

                    //SELECT * FROM [Students] WHERE Id=1

                string[] commandArray = command.ToLower().Split(' ');

                switch (commandArray[0])
                    {
                        case "selectall":
                            sqlCommand = new SqlCommand("SELECT * FROM [Students]", sqlConnection);
                            sqlDataReader = sqlCommand.ExecuteReader();

                            while (sqlDataReader.Read())
                            {
                                Console.WriteLine($"{sqlDataReader["Id"]}" +
                                                  $"{sqlDataReader["FIO"]}" +
                                                  $"{sqlDataReader["Birthday"]}" +
                                                  $"{sqlDataReader["University"]}" +
                                                  $"{sqlDataReader["Group_number"]}" +
                                                  $"{sqlDataReader["Average_score"]}"
                                                  );
                                Console.WriteLine(new string('-', 30));
                            }

                            if (sqlDataReader != null)
                            {
                                sqlDataReader.Close();
                            }
                            break;
                        case "select":
                            sqlCommand = new SqlCommand(command, sqlConnection);
                            sqlDataReader = sqlCommand.ExecuteReader();

                            while (sqlDataReader.Read())
                            {
                                Console.WriteLine($"{sqlDataReader["Id"]}" +
                                                  $"{sqlDataReader["FIO"]}" +
                                                  $"{sqlDataReader["Birthday"]}" +
                                                  $"{sqlDataReader["University"]}" +
                                                  $"{sqlDataReader["Group_number"]}" +
                                                  $"{sqlDataReader["Average_score"]}" 
                                                  );
                                Console.WriteLine(new string('-', 30));
                            }

                            if(sqlDataReader != null)
                            {
                                sqlDataReader.Close();
                            }

                            break;

                        case "insert":
                                sqlCommand = new SqlCommand(command, sqlConnection);
                                Console.WriteLine($"Добавлено: {sqlCommand.ExecuteNonQuery()} строк(а)");

                            break;
                        case "update":
                                sqlCommand = new SqlCommand(command, sqlConnection);
                                Console.WriteLine($"Изменено: {sqlCommand.ExecuteNonQuery()} строк(а)");

                            break;
                        case "delete":
                                sqlCommand = new SqlCommand(command, sqlConnection);
                                Console.WriteLine($"Удалено: {sqlCommand.ExecuteNonQuery()} строк(а)");
                    
                            break;
                        case "sortby":
                                //sortby fio asc

                                sqlCommand = new SqlCommand($"SELECT * FROM [Students] ORDER BY {commandArray[1]} {commandArray[2]}", sqlConnection);

                                sqlDataReader = sqlCommand.ExecuteReader();

                                while (sqlDataReader.Read())
                                {
                                    Console.WriteLine($"{sqlDataReader["Id"]}" +
                                                      $"{sqlDataReader["FIO"]}" +
                                                      $"{sqlDataReader["Birthday"]}" +
                                                      $"{sqlDataReader["University"]}" +
                                                      $"{sqlDataReader["Group_number"]}" +
                                                      $"{sqlDataReader["Average_score"]}"
                                                      );
                                    Console.WriteLine(new string('-', 30));
                                }

                                if (sqlDataReader != null)
                                {
                                    sqlDataReader.Close();
                                }

                                break;
                        default:
                            Console.WriteLine($"Команда {command} некорректна!");
                            break;

                }
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"ошибка {ex.Message}");
                }

            }

            Console.WriteLine("najhmi knopku");
            Console.ReadKey();
        }
    }

}