using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCadDms.AutoCad.Addin
{
    public class AutoCadAddin
    {
        public const string HelloWorldCommand = "HelloWorld";

        [CommandMethod(HelloWorldCommand)]
        public void HelloWorld()
        {
            var form = new Form1();
            form.ShowDialog();
        }
    }
}
