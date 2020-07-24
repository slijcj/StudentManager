using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GDIPlusDemo
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new LoginIndex());
            Application.Run(new StudentManager());
            //Application.Run(new SelectCourse());
            //Application.Run(new SelectCourse("20172659"));
            //Application.Run(new Admin());
        }
    }
}
