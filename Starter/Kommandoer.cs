using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.ApplicationServices;

using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.GraphicsSystem;
using System.Windows.Interop;



[assembly: CommandClass(typeof(Fargemannen_GR.MyCommands))]

namespace Fargemannen_GR
{
    public class MyCommands
    {

        [CommandMethod("Fargemannen_GR")]
        public void KjørFargemannen()
        {

            View.MainWindow MainWindow = new View.MainWindow();

            WindowInteropHelper helper = new WindowInteropHelper(MainWindow)
            {
                Owner = Application.MainWindow.Handle
            };
            MainWindow.Show();
        }
    }
}
