using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using Newtonsoft.Json;
using HatFramework;
using HatPluginMySql;

namespace Hat
{
    public class ExampleTest1
    {
        Tester tester;

        public async void Main(Form browserWindow)
        {
            tester = new Tester(browserWindow);

            await setUp();
            await test();
            await tearDown();
        }

        public async Task setUp()
        {
            tester.Description("Тест #1-2 проверяет авторизацию на сайте");
            await tester.BrowserFullScreenAsync();
        }

		/* Запросы для теста:
		 * INSERT INTO test_table VALUES(NULL, 'tName', 100, 'tPost')
		 * UPDATE test_table SET age = 111 WHERE name = 'tName'
		 * DELETE FROM test_table WHERE name = 'tName'
		*/

        public async Task test()
        {
            await tester.TestBeginAsync();
            await tester.GoToUrlAsync("https://somovstudio.github.io/test.html", 5);
            await tester.WaitVisibleElementByIdAsync("login", 15);
            await tester.SetValueInElementByIdAsync("login", "admin");
            await tester.WaitAsync(2);
            await tester.SetValueInElementByIdAsync("pass", "0000");
            await tester.WaitAsync(2);
            await tester.ClickElementByIdAsync("buttonLogin");
            await tester.WaitVisibleElementByIdAsync("result", 5);
            string actual = await tester.GetValueFromElementByIdAsync("textarea");
            string expected = "Вы успешно авторизованы";
            await tester.AssertEqualsAsync(expected, actual);
            
            // -- Проверка базы данных -------------------------------
            TesterMySql testerMySql = new TesterMySql(tester);
            await testerMySql.ConnectionOpenAsync("server=127.0.0.1;uid=root;pwd=;database=test_db");
            
            await testerMySql.GetCountEntriesAsync("SELECT * FROM test_table");
            
            await testerMySql.SetEntryAsync("INSERT INTO test_table VALUES(NULL, 'tName', 100, 'tPost')");
            await testerMySql.AssertHaveInTableAsync("test_table", "name", "'tName'");
            await testerMySql.FindEntryAsync("test_table", "name", "'tName'");
            
            await testerMySql.GetCountEntriesAsync("SELECT * FROM test_table");
            
            await testerMySql.EditEntryAsync("UPDATE test_table SET age = 111 WHERE name = 'tName'");
            await testerMySql.AssertHaveInTableAsync("test_table", "age", "111");
            await testerMySql.FindEntryAsync("test_table", "age", "111");
            
            await testerMySql.RemoveEntryAsync("DELETE FROM test_table WHERE name = 'tName'");
            await testerMySql.AssertDontHaveInTableAsync("test_table", "name", "'tName'");
            await testerMySql.FindEntryAsync("test_table", "name", "'tName'");
            
            await testerMySql.GetCountEntriesAsync("SELECT * FROM test_table");
            
            List<List<string>> entries = new List<List<string>>();
            entries = await testerMySql.GetEntriesFromTableAsync("test_table");
            if (entries != null){
            	foreach(List<string> entry in entries)
	            {
					foreach(string value in entry)
					{
						tester.ConsoleMsg(value);
					}
	            }
            }
            
            await testerMySql.ConnectionCloseAsync();
            // -- Завершение проверки базы данных --------------------
            
            await tester.TestEndAsync();
        }

        public async Task tearDown()
        {
            // await tester.BrowserCloseAsync();
        }
    }
}
