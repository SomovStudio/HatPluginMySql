using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using HatFramework;
using System.Data;

namespace HatPluginMySql
{
    public class TesterMySql
    {
        private Tester _tester;
        private MySqlConnection _connection = null;
        private MySqlCommand _command = null;

        public TesterMySql(Tester tester)
        {
            _tester = tester;
        }

        /* Открыть соединение */
        public void ConnectionOpen(string connectionString)
        {
            if (_tester.DefineTestStop() == true) return;
            _tester.SendMessageDebug($"ConnectionOpen(\"{connectionString}\")", $"ConnectionOpen(\"{connectionString}\")", Tester.PROCESS, "Подключение к базе данных и открытие соединения", "Connecting to a database and opening a connection", Tester.IMAGE_STATUS_MESSAGE);
            
            try
            {
                _connection = new MySqlConnection();
                _connection.ConnectionString = connectionString;
                _connection.Open();
                _tester.SendMessageDebug($"ConnectionOpen(\"{connectionString}\")", $"ConnectionOpen(\"{connectionString}\")", Tester.PASSED, "Подключение к базе данных открыто", "The connection to the database is open", Tester.IMAGE_STATUS_PASSED);
            }
            catch (MySqlException ex)
            {
                _tester.SendMessageDebug($"ConnectionOpen(\"{connectionString}\")", $"ConnectionOpen(\"{connectionString}\")", Tester.FAILED,
                "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
                _tester.ConsoleMsgError(ex.ToString());
            }
        }

        /* Закрыть соединение */
        public void ConnectionClose()
        {
            if (_tester.DefineTestStop() == true && _connection == null) return;
            
            try
            {
                _connection.Close();
                _tester.SendMessageDebug($"ConnectionClose()", $"ConnectionClose()", Tester.PASSED, "Подключение к базе данных закрыто", "The connection to the database is closed", Tester.IMAGE_STATUS_PASSED);
            }
            catch (MySqlException ex)
            {
                _tester.SendMessageDebug($"ConnectionClose()", $"ConnectionClose()", Tester.FAILED,
                "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
                _tester.ConsoleMsgError(ex.ToString());
            }
        }

        /* Получать все записи из указанной таблицы */
        public List<List<string>> GetEntriesFromTable(string tableName)
        {
            if (_tester.DefineTestStop() == true) return null;
            List<List<string>> entries = new List<List<string>>();
            List<string> entry = new List<string>();

            try
            {
                _command = new MySqlCommand();
                _command.Connection = _connection;
                _command.CommandType = CommandType.TableDirect;
                _command.CommandText = tableName;
                MySqlDataReader reader = _command.ExecuteReader();
                while (reader.Read())
                {
                    entry = new List<string>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        entry.Add(reader[i].ToString());
                    }
                    entries.Add(entry);
                }
                reader.Close();
                _tester.SendMessageDebug($"GetEntriesFromTable(\"{tableName}\")", $"GetEntriesFromTable(\"{tableName}\")", Tester.PASSED, $"Получены все записи из таблицы \"{tableName}\"", $"All entries from the table \"{tableName}\" have been received", Tester.IMAGE_STATUS_PASSED);
            }
            catch (MySqlException ex)
            {
                _tester.SendMessageDebug($"GetEntriesFromTable(\"{tableName}\")", $"GetEntriesFromTable(\"{tableName}\")", Tester.FAILED,
                "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
                _tester.ConsoleMsgError(ex.ToString());
            }

            return entries;
        }

        /* Получить записи */
        public List<List<string>> GetEntries(string sqlQuertSelect)
        {
            if (_tester.DefineTestStop() == true) return null;
            List<List<string>> entries = new List<List<string>>();
            List<string> entry = new List<string>();

            try
            {
                _command = new MySqlCommand();
                _command.Connection = _connection;
                _command.CommandType = CommandType.Text;
                _command.CommandText = sqlQuertSelect;
                MySqlDataReader reader = _command.ExecuteReader();
                while (reader.Read())
                {
                    entry = new List<string>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        entry.Add(reader[i].ToString());
                    }
                    entries.Add(entry);
                }
                reader.Close();
                _tester.SendMessageDebug($"GetEntries(\"{sqlQuertSelect}\")", $"GetEntries(\"{sqlQuertSelect}\")", Tester.PASSED, $"Получены записи из таблицы", $"Entries from the table are received", Tester.IMAGE_STATUS_PASSED);
            }
            catch (MySqlException ex)
            {
                _tester.SendMessageDebug($"GetEntries(\"{sqlQuertSelect}\")", $"GetEntries(\"{sqlQuertSelect}\")", Tester.FAILED,
                "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
                _tester.ConsoleMsgError(ex.ToString());
            }

            return entries;
        }

        /* Вставить запись (возвращает номер записи или -1 если запрос не выполнен) */
        public int SetEntry(string sqlQuertInsert)
        {
            if (_tester.DefineTestStop() == true) return -1;
            int result = -1;

            try
            {
                _command = new MySql.Data.MySqlClient.MySqlCommand();
                _command.Connection = _connection;
                _command.CommandType = CommandType.Text;
                _command.CommandText = sqlQuertInsert;
                result = _command.ExecuteNonQuery();

                if (result < 0)
                {
                    _tester.SendMessageDebug($"SetEntry(\"{sqlQuertInsert}\")", $"SetEntry(\"{sqlQuertInsert}\")", Tester.FAILED, "Неудалось добавить данные в базу данных", "Failed to add data to the database", Tester.IMAGE_STATUS_FAILED);
                    _tester.TestStopAsync();
                }
                else
                {
                    _tester.SendMessageDebug($"SetEntry(\"{sqlQuertInsert}\")", $"SetEntry(\"{sqlQuertInsert}\")", Tester.PASSED, "Данные успешно добавлены в базу данных", "The data has been successfully added to the database", Tester.IMAGE_STATUS_PASSED);
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                _tester.SendMessageDebug($"SetEntry(\"{sqlQuertInsert}\")", $"SetEntry(\"{sqlQuertInsert}\")", Tester.FAILED,
                "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
                _tester.ConsoleMsgError(ex.ToString());
            }

            return result;
        }

        /* Изменить запись (возвращает номер записи или -1 если запрос не выполнен) */
        public int EditEntry(string sqlQuertUpdate)
        {
            if (_tester.DefineTestStop() == true) return -1;
            int result = -1;

            try
            {
                _command = new MySql.Data.MySqlClient.MySqlCommand();
                _command.Connection = _connection;
                _command.CommandType = CommandType.Text;
                _command.CommandText = sqlQuertUpdate;
                result = _command.ExecuteNonQuery();

                if (result < 0)
                {
                    _tester.SendMessageDebug($"EditEntry(\"{sqlQuertUpdate}\")", $"EditEntry(\"{sqlQuertUpdate}\")", Tester.FAILED, "Неудалось обновить данные в базу данных", "Data could not be updated in the database", Tester.IMAGE_STATUS_FAILED);
                    _tester.TestStopAsync();
                }
                else
                {
                    _tester.SendMessageDebug($"EditEntry(\"{sqlQuertUpdate}\")", $"EditEntry(\"{sqlQuertUpdate}\")", Tester.PASSED, "Данные успешно обновлены в базу данных", "The data has been successfully updated to the database", Tester.IMAGE_STATUS_PASSED);
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                _tester.SendMessageDebug($"EditEntry(\"{sqlQuertUpdate}\")", $"EditEntry(\"{sqlQuertUpdate}\")", Tester.FAILED,
                "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
                _tester.ConsoleMsgError(ex.ToString());
            }

            return result;
        }

        /* Удалить запись (возвращает номер записи или -1 если запрос не выполнен) */
        public int RemoveEntry(string sqlQuertDelete)
        {
            if (_tester.DefineTestStop() == true) return -1;
            int result = -1;

            try
            {
                _command = new MySql.Data.MySqlClient.MySqlCommand();
                _command.Connection = _connection;
                _command.CommandType = CommandType.Text;
                _command.CommandText = sqlQuertDelete;
                result = _command.ExecuteNonQuery();

                if (result < 0)
                {
                    _tester.SendMessageDebug($"RemoveEntry(\"{sqlQuertDelete}\")", $"RemoveEntry(\"{sqlQuertDelete}\")", Tester.FAILED, "Неудалось удалить данные из базе данных", "Could not delete data from the database", Tester.IMAGE_STATUS_FAILED);
                    _tester.TestStopAsync();
                }
                else
                {
                    _tester.SendMessageDebug($"RemoveEntry(\"{sqlQuertDelete}\")", $"RemoveEntry(\"{sqlQuertDelete}\")", Tester.PASSED, "Данные успешно удалены из базы данных", "Data has been successfully deleted from the database", Tester.IMAGE_STATUS_PASSED);
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                _tester.SendMessageDebug($"RemoveEntry(\"{sqlQuertDelete}\")", $"RemoveEntry(\"{sqlQuertDelete}\")", Tester.FAILED,
                "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
                _tester.ConsoleMsgError(ex.ToString());
            }

            return result;
        }

        /* Найти запись */
        public void FindEntry()
        {

        }

        /* Количество записей */
        public void GetCountEntries()
        {

        }

        /* Выполнить запрос */
        public void ExecuteQuery()
        {

        }

        /* Методы для проверки результата =================================== */

        /* Проверяет присутствие значения в таблице базы данных */
        public bool AssertHaveInTable(string tableName, string columnName, string value)
        {
            if (_tester.DefineTestStop() == true) return false;

            try
            {
                _command = new MySqlCommand();
                _command.Connection = _connection;
                _command.CommandType = CommandType.Text;
                _command.CommandText = $"SELECT * FROM {tableName} WHERE {columnName} = {value}";
                MySqlDataReader reader = _command.ExecuteReader();
                
                if (reader.HasRows == false)
                {
                    _tester.SendMessageDebug($"AssertHaveInTable(\"{tableName}\", \"{columnName}\", \"{value}\")", $"AssertHaveInTable(\"{tableName}\", \"{columnName}\", \"{value}\")", Tester.FAILED, $"В таблице {tableName} в колонке {columnName} нет записи со значением {value} {Environment.NewLine}(ЗАПРОС: {_command.CommandText})", $"In the {tableName} table, there is no entry in the {columnName} column with the value {value} {Environment.NewLine}(QUERY: {_command.CommandText})", Tester.IMAGE_STATUS_FAILED);
                    _tester.TestStopAsync();
                    return false;
                }
                else
                {
                    reader.Read();
                    reader.Close();
                    _tester.SendMessageDebug($"AssertHaveInTable(\"{tableName}\", \"{columnName}\", \"{value}\")", $"AssertHaveInTable(\"{tableName}\", \"{columnName}\", \"{value}\")", Tester.PASSED, $"В таблице {tableName} присутствует запись со значением {value} в колонке {columnName}", $"In the table {tableName} there is an entry with the value {value} in the column {columnName}", Tester.IMAGE_STATUS_PASSED);
                    return true;
                }
            }
            catch (MySqlException ex)
            {
                _tester.SendMessageDebug($"AssertHaveInTable(\"{tableName}\", \"{columnName}\", \"{value}\")", $"AssertHaveInTable(\"{tableName}\", \"{columnName}\", \"{value}\")", Tester.FAILED,
                "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
                _tester.ConsoleMsgError(ex.ToString());
            }

            _tester.TestStopAsync();
            return false;
        }

        /* Проверяет отсутствия значения в таблице базы данных */
        public bool AssertDontHaveInTable(string tableName, string columnName, string value)
        {
            if (_tester.DefineTestStop() == true) return false;

            try
            {
                _command = new MySqlCommand();
                _command.Connection = _connection;
                _command.CommandType = CommandType.Text;
                _command.CommandText = $"SELECT * FROM {tableName} WHERE {columnName} = {value}";
                MySqlDataReader reader = _command.ExecuteReader();

                if (reader.HasRows == true)
                {
                    _tester.SendMessageDebug($"AssertDontHaveInTable(\"{tableName}\", \"{columnName}\", \"{value}\")", $"AssertDontHaveInTable(\"{tableName}\", \"{columnName}\", \"{value}\")", Tester.FAILED, $"В таблице {tableName} присутствует запись со значением {value} в колонке {columnName} {Environment.NewLine}(ЗАПРОС: {_command.CommandText})\"", $"In the table {tableName} there is an entry with the value {value} in the column {columnName} {Environment.NewLine}(QUERY: {_command.CommandText})\"", Tester.IMAGE_STATUS_FAILED);
                    _tester.TestStopAsync();
                    return false;
                }
                else
                {
                    reader.Read();
                    reader.Close();
                    _tester.SendMessageDebug($"AssertDontHaveInTable(\"{tableName}\", \"{columnName}\", \"{value}\")", $"AssertDontHaveInTable(\"{tableName}\", \"{columnName}\", \"{value}\")", Tester.PASSED, $"В таблице {tableName} в колонке {columnName} нет записи со значением {value}", $"In the {tableName} table, there is no entry in the {columnName} column with the value {value}", Tester.IMAGE_STATUS_PASSED);
                    return true;
                }
            }
            catch (MySqlException ex)
            {
                _tester.SendMessageDebug($"AssertDontHaveInTable(\"{tableName}\", \"{columnName}\", \"{value}\")", $"AssertDontHaveInTable(\"{tableName}\", \"{columnName}\", \"{value}\")", Tester.FAILED,
                "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
                _tester.ConsoleMsgError(ex.ToString());
            }

            _tester.TestStopAsync();
            return false;
        }



    }
}
