using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ConvertToAnnotation
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
            Application.Run(new Form1());
        }
    }
}
