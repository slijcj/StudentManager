using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace GDIPlusDemo
{
    public partial class mysql : Form
    {
        public mysql()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string SQL_ConnectStr = "server=122.51.231.110;port=3306;user=root;password=v9736783peng;database=myscoremanage";
                MySqlConnection mySql = new MySqlConnection(SQL_ConnectStr);
                mySql.Open();
                string sql = "select id,stuno,courseid,score,type from stu_sco";
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sql, mySql);
                DataTable a = new DataTable();
                mySqlDataAdapter.Fill(a);
                this.dataGridView1.DataSource = a;


            }

            finally
            {
                Console.WriteLine("数据库链接成功");

            }
        }
    }
}
