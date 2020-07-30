using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace GDIPlusDemo
{
    class Dao
    {

        public void MysqlConnect()
        {
            connectiont();//数据库链接函数
        }
        public void restart()
        {
            Application.Exit();//退出系统
            System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);//重启系统
        }
        public void LogOut()
        {
            Application.Exit();//退出系统
            LoginIndex login = new LoginIndex();
            login.ShowDialog();
            System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);//重启系统
        }
  
        public MySqlConnection connectiont()
        {
            try
            {
                string SQL_ConnectStr = "server=122.51.231.110;port=3306;user=root;password=v9736783peng;database=demo";
                MySqlConnection mySql = new MySqlConnection(SQL_ConnectStr);
                mySql.Open();
                return mySql;

            }
            
            finally
            {
                Console.WriteLine("数据库链接成功");
                
            }
        }
        public MySqlCommand command(string sql)
        {
            
            MySqlCommand mySqlCommand = new MySqlCommand(sql, connectiont());
            return mySqlCommand;
        }
        /// <summary>
        /// 用于delete upadate insert
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int Excute(string sql)
        {
            return command(sql).ExecuteNonQuery();
        }
        /// <summary>
        /// 用于select
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public MySqlDataReader Read(string sql)
        {
            return command(sql).ExecuteReader();
        }



        

    }
}
