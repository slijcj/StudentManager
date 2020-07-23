using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;

namespace GDIPlusDemo
{
    public partial class SelectCourse : Form
    {
        string SID;//记录登录选课系统账户的学号
        public SelectCourse(string sId)
        {
            SID = sId;
            InitializeComponent();
            toolStripStatusLabel3.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            toolStripStatusLabel1.Text = "欢迎学号为" + SID + "的同学登录选课系统！";
            timer1.Start();
            Table();
        }
        private void Table()
        {
            string sql = "select * from Course";
            Dao dao = new Dao();
            IDataReader dr = dao.Read(sql);
            while (dr.Read())
            {
                string sno, name, teacher, credit;
                sno = dr["Id"].ToString();
                name = dr["Name"].ToString();
                teacher = dr["Teacher"].ToString();
                credit = dr["Credit"].ToString();
                string[] str = { sno, name, teacher, credit };
                dataGridView1.Rows.Add(str);

            }
            dr.Close();//关闭连接
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel3.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            
        }

        private void SelectCourse_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();//关闭整个程序
        }

        private void SelectCourse_Load(object sender, EventArgs e)
        {

        }

        private void 选课ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string cId = dataGridView1.SelectedCells[0].Value.ToString();//获取选中的课程号
            string sql_1 = "select *from CourseRecord where sId = '"+SID+"'and cId = '"+cId+"'";
            Dao dao = new Dao();
            IDataReader dc = dao.Read(sql_1);

            if (!dc.Read())
            {
                string sql = "insert into CourseRecord values('" + SID + "','" + cId + "')";
                int i = dao.Excute(sql);
                if (i > 0)
                {
                    MessageBox.Show("选课成功!");
                }
            }
            else
            {
                MessageBox.Show("不允许重复选课！");
            }  
        }

        private void 我的课程ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MySelect mySelect = new MySelect(SID);
            mySelect.Show();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dao dao = new Dao();
            dao.restart();
           
        }

        private void 修改个人密码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModifyPassWord modifyPassWord = new ModifyPassWord(SID);
            modifyPassWord.ShowDialog();

        }

        private void 重启ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dao dao = new Dao();
            dao.restart();
        }
    }
}
