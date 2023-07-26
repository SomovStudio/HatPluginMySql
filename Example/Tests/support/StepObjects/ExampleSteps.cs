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

namespace Hat
{
    public class ExampleSteps : Tester
    {
        public ExampleSteps(Form browserWindow): base(browserWindow) {}

        public async Task FillForm()
        {
            await this.WaitVisibleElementByIdAsync(ExamplePage.InputLogin, 15);
            await this.SetValueInElementByIdAsync(ExamplePage.InputLogin, "admin");
            await this.WaitAsync(2);
            await this.SetValueInElementByIdAsync(ExamplePage.InputPass, "0000");
            await this.WaitAsync(2);
        }
    }
}
