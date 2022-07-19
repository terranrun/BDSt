using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text;


/*
 * user Manual
 * Выход                                                  exit
 * Вывод всех строк                                       selectall
 * Сортировка                                             sortby <column> <asc/desc>
 * Поиск                                                  search <column> <text>
 * Минимальное значение колонки "Средний балл"            min
 * Максимальное значение колонки "Средний балл"           max
 * Среднее значение колонки "Средний балл"                avg
 * Сумма значение колонки "Средний балл"                  sum
 * Очистка экрана                                         clear
 * Удаление                                               DELETE FROM <table_name> WHERE <condition>
 * Добавление                                             INSERT [INTO] имя_таблицы [(список_столбцов)] VALUES (значение1,значение2 ... )
 * Изменение                                              UPDATE <имя_таблицы> SET<имя столбца> = <значение> WHERE condition
 */

namespace BaseDDT
{
    class Program
    {

        private static string connectionString = ConfigurationManager.ConnectionStrings["StudentDB1"].ConnectionString;

        private static SqlConnection sqlConnection = null;

        static string command = string.Empty;

        static string result = string.Empty;

        static string[] commandArray;

        private static void WriteToFile()
        {
            using (StreamWriter sW = new StreamWriter(
                                    $"{AppDomain.CurrentDomain.BaseDirectory}/{commandArray[0]}_{DateTime.Now.ToString().Replace(':', '-')}.txt",
                                    true, Encoding.UTF8))
            {
                sW.WriteLine(DateTime.Now.ToString());

                sW.WriteLine(command);

                sW.WriteLine(result);
            }
        }

        static void Main(string[] args)
        {
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            Console.WriteLine("StudentApp");

            SqlDataReader sqlDataReader = null;

            
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

                 commandArray = command.ToLower().Split(' ');

                switch (commandArray[0])
                    {
                        case "fselectall":
                        case "selectall":
                            sqlCommand = new SqlCommand("SELECT * FROM [Students]", sqlConnection);
                            sqlDataReader = sqlCommand.ExecuteReader();

                            while (sqlDataReader.Read())
                            {
                                result += $"{sqlDataReader["Id"]}" +
                                                  $"{sqlDataReader["FIO"]}" +
                                                  $"{sqlDataReader["Birthday"]}" +
                                                  $"{sqlDataReader["University"]}" +
                                                  $"{sqlDataReader["Group_number"]}" +
                                                  $"{sqlDataReader["Average_score"]}\n";
                                result += new string('-', 30) + "\n";
                            }

                            if (sqlDataReader != null)
                            {
                                sqlDataReader.Close();
                            }

                            Console.WriteLine(result);

                            if (commandArray[0][0] == 'f')
                            {
                                WriteToFile();                              
                            }
                            break;
                        case "fselect":
                        case "select":
                            sqlCommand = new SqlCommand(command, sqlConnection);
                            sqlDataReader = sqlCommand.ExecuteReader();

                            while (sqlDataReader.Read())
                            {
                                result += $"{sqlDataReader["Id"]}" +
                                                  $"{sqlDataReader["FIO"]}" +
                                                  $"{sqlDataReader["Birthday"]}" +
                                                  $"{sqlDataReader["University"]}" +
                                                  $"{sqlDataReader["Group_number"]}" +
                                                  $"{sqlDataReader["Average_score"]}\n";
                                result += new string('-', 30) + "\n";
                            }

                            if (sqlDataReader != null)
                            {
                                sqlDataReader.Close();
                            }

                            Console.WriteLine(result);

                            if (commandArray[0][0] == 'f')
                            {
                                WriteToFile();
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
                        case "fsortby":
                        case "sortby":
                                //sortby fio asc

                                sqlCommand = new SqlCommand($"SELECT * FROM [Students] ORDER BY {commandArray[1]} {commandArray[2]}", sqlConnection);

                                sqlDataReader = sqlCommand.ExecuteReader();

                                while (sqlDataReader.Read())
                                {
                                result += $"{sqlDataReader["Id"]}" +
                                               $"{sqlDataReader["FIO"]}" +
                                               $"{sqlDataReader["Birthday"]}" +
                                               $"{sqlDataReader["University"]}" +
                                               $"{sqlDataReader["Group_number"]}" +
                                               $"{sqlDataReader["Average_score"]}\n";
                                result += new string('-', 30) + "\n";
                            }

                            if (sqlDataReader != null)
                            {
                                sqlDataReader.Close();
                            }

                            Console.WriteLine(result);

                            if (commandArray[0][0] == 'f')
                            {
                                WriteToFile();
                            }

                            break;
                        case "fsearch":
                        case "search":
                            if (commandArray[1].Equals("fio"))
                            {
                                sqlCommand = new SqlCommand($"SELECT * FROM [Students] WHERE FIO LIKE N'%{commandArray[2]}%'", sqlConnection);
                            }
                            else if (commandArray[1].Equals("course"))
                            {
                                sqlCommand = new SqlCommand($"SELECT * FROM [Students] WHERE Course ='{commandArray[2]}'", sqlConnection);

                            }
                            else
                            {
                                Console.WriteLine($"аргумент {commandArray[1]} некорректен!");
                            }

                            try
                            {
                                sqlDataReader = sqlCommand.ExecuteReader();

                                while (sqlDataReader.Read())
                                {
                                    result += $"{sqlDataReader["Id"]}" +
                                                  $"{sqlDataReader["FIO"]}" +
                                                  $"{sqlDataReader["Birthday"]}" +
                                                  $"{sqlDataReader["University"]}" +
                                                  $"{sqlDataReader["Group_number"]}" +
                                                  $"{sqlDataReader["Average_score"]}\n";
                                result += new string('-', 30) + "\n";
                            }

                            if (sqlDataReader != null)
                            {
                                sqlDataReader.Close();
                            }

                            }
                            catch(Exception ex)
                            {
                                Console.WriteLine($"Ошибка: {ex.Message}");
                            }
                            finally
                            {
                                if (sqlDataReader != null)
                                {
                                    sqlDataReader.Close();
                                }

                                Console.WriteLine(result);

                                if (commandArray[0][0] == 'f')
                                {
                                    WriteToFile();
                                }
                            }
                            break;
                        case "fmin":
                        case "min":

                            sqlCommand = new SqlCommand("SELECT MIN(Average_score) FROM [Students]", sqlConnection);

                            result =($"Минимальный средний балл : {sqlCommand.ExecuteScalar()}");

                            Console.WriteLine(result);

                            if (commandArray[0][0] == 'f')
                            {
                                WriteToFile();                      
                            }

                            break;
                        case "fmax":
                        case "max":

                            sqlCommand = new SqlCommand("SELECT MAX(Average_score) FROM [Students]", sqlConnection);

                            result = ($"Максимальный средний балл : {sqlCommand.ExecuteScalar()}");
                            Console.WriteLine(result);

                            if (commandArray[0][0] == 'f')
                            {
                                WriteToFile();
                            }
                            break;
                        case "favg":
                        case "avg":
                            sqlCommand = new SqlCommand("SELECT AVG(Average_score) FROM [Students]", sqlConnection);

                            result = ($"Минимальный средний балл : {sqlCommand.ExecuteScalar()}");
                            Console.WriteLine(result);

                            if (commandArray[0][0] == 'f')
                            {
                                WriteToFile();
                            }
                            break;
                        case "fsun":
                        case "sum":
                            sqlCommand = new SqlCommand("SELECT SUM(Average_score) FROM [Students]", sqlConnection);

                            result = ($"Минимальный средний балл : {sqlCommand.ExecuteScalar()}");
                            Console.WriteLine(result);

                            if (commandArray[0][0] == 'f')
                            {
                                WriteToFile();
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
                result = string.Empty;
            }

            Console.WriteLine("najhmi knopku");
            Console.ReadKey();
        }
    }

}