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

namespace HatPluginMySQL
{
    public class TesterMySQL
    {
        private Tester _tester;
        private MySqlConnection _connection = null;
        private MySqlCommand _command = null;

        public TesterMySQL(Tester tester)
        {
            _tester = tester;
        }

        /* Открыть соединение */
        public void ConnectionOpen(string connectionString)
        {
            if (_tester.DefineTestStop() == true) return;
            _tester.SendMessageDebug($"ConnectionOpen({connectionString})", $"ConnectionOpen({connectionString})", Tester.PROCESS, "Подключение к базе данных и открытие соединения", "Connecting to a database and opening a connection", Tester.IMAGE_STATUS_MESSAGE);
            
            try
            {
                _connection = new MySqlConnection();
                _connection.ConnectionString = connectionString;
                _connection.Open();
                _tester.SendMessageDebug($"ConnectionOpen({connectionString})", $"ConnectionOpen({connectionString})", Tester.PASSED, "Подключение к базе данных открыто", "The connection to the database is open", Tester.IMAGE_STATUS_PASSED);
            }
            catch (MySqlException ex)
            {
                _tester.SendMessageDebug($"ConnectionOpen({connectionString})", $"ConnectionOpen({connectionString})", Tester.FAILED,
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

        /* Получать записи */
        public void GetEntries()
        {

        }

        /* Вставить запись */
        public void SetEntry()
        {

        }

        /* Изменить запись */
        public void EditEntry()
        {

        }

        /* Удалить запись */
        public void RemoveEntry()
        {

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

        
    }
}
