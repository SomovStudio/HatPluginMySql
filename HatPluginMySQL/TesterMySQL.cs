using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HatPluginMySQL
{
    public class TesterMySQL
    {
        private Form browserWindow;                 // объект: окно приложения (основная форма браузера Hat)
        private MethodInfo browserConsoleMsg;       // функция: ConsoleMsg - вывод сообщения в консоль приложения
        private MethodInfo browserConsoleMsgError;  // функция: ConsoleMsgErrorReport - вывод сообщения об ошибке в консоль приложения
        private MethodInfo browserSystemConsoleMsg; // функция: SystemConsoleMsg - вывод сообщения в системную консоль
        private MethodInfo browserSendMessageStep;  // функция: SendMessageStep - вывести сообщение в таблицу "тест"

        public TesterMySQL(Form _browserForm)
        {
            browserWindow = _browserForm;
            browserConsoleMsg = browserWindow.GetType().GetMethod("ConsoleMsg");
            browserConsoleMsgError = browserWindow.GetType().GetMethod("ConsoleMsgErrorReport");
            browserSystemConsoleMsg = browserWindow.GetType().GetMethod("SystemConsoleMsg");
            browserSendMessageStep = browserWindow.GetType().GetMethod("SendMessageStep");
        }

        /* Открыть соединение */
        public void ConnectionOpen()
        {

        }

        /* Закрыть соединение */
        public void ConnectionClose()
        {

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
