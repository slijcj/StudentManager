using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GDIPlusDemo
{
    public partial class MySelect : Form
    {
        string SID;
        public MySelect(string sID)
        {
            SID = sID;
            InitializeComponent();
            Table();
        }
        private void Table()
        {
            dataGridView1.Rows.Clear();
            string sql = "select * from CourseRecord where sId = '"+SID+"'";
            Dao dao = new Dao();
            IDataReader dr = dao.Read(sql);
            while (dr.Read())
            {
                string cId = dr["cId"].ToString();
                string sql2 = "select * from Course where Id='"+cId+"'";
                IDataReader dr2 = dao.Read(sql2);
                dr2.Read();
                string sno, name, teacher, credit;
                sno = dr2["Id"].ToString();
                name = dr2["Name"].ToString();
                teacher = dr2["Teacher"].ToString();
                credit = dr2["Credit"].ToString();
                string[] str = { sno, name, teacher, credit };
                dataGridView1.Rows.Add(str);
                dr2.Close();

            }
            dr.Close();//关闭连接
        }

        private void MySelect_FormClosed(object sender, FormClosedEventArgs e)
        {
            //this.Close();
            //Application.Exit();//结束整个程序 
        }

        private void MySelect_Load(object sender, EventArgs e)
        {

        }

        private void 取消这门课ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string cID = dataGridView1.SelectedCells[0].Value.ToString();
            string sql = "delete from CourseRecord where sId = '"+SID+"'and cId = '"+cID+"'";
            Dao dao = new Dao();
            dao.Excute(sql);
            Table();
        }
    }
}
