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
    public partial class StudentManager : Form
    {
        public StudentManager()
        {
            InitializeComponent();
            toolStripStatusLabel3.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            timer1.Start();
            Table();

             
        }

        private void 学生管理_Load(object sender, EventArgs e)
        {

        }
        //让表显示数据
        private void Table()
        {
            string sql = "select * from Student";
            Dao dao = new Dao();
            IDataReader dr = dao.Read(sql);
            while (dr.Read())
            {
                string sno, name,classroom,birthday,jg;
                sno = dr["Id"].ToString();
                name = dr["Name"].ToString();
                classroom = dr["Class"].ToString();
                birthday = dr["Birthday"].ToString();
                jg = dr["JG"].ToString();
                string[] str = { sno, name, classroom, birthday, jg};
                dataGridView1.Rows.Add(str);
                
            }
            dr.Close();//关闭连接
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel3.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void StudentManager_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();//结束整个程序 
        }
        //刷新列表，重新加载
        private void 刷新列表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            Table();
        }

        private void 添加学生信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StudentInfoManager studentInfoManager = new StudentInfoManager();
            studentInfoManager.ShowDialog();
            dataGridView1.Rows.Clear();
            Table();


        }

        private void 修改学生信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] str = {dataGridView1.SelectedCells[0].Value.ToString(), dataGridView1.SelectedCells[1].Value.ToString(), dataGridView1.SelectedCells[2].Value.ToString(),
                            dataGridView1.SelectedCells[3].Value.ToString(),dataGridView1.SelectedCells[4].Value.ToString()};
            
            StudentInfoManager studentInfoManager = new StudentInfoManager(str);
            studentInfoManager.ShowDialog();
            dataGridView1.Rows.Clear();
            Table();

        }

        private void 删除学生信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("确定要删除吗？","提示",MessageBoxButtons.OKCancel);
            if (r == DialogResult.OK)
            {
                string id, name;
                id = dataGridView1.SelectedCells[0].Value.ToString();
                name = dataGridView1.SelectedCells[1].Value.ToString();
                string sql = "delete from Student where Id='" + id + "'and Name='" + name + "'";
                Dao dao = new Dao();
                dao.Excute(sql);
                dataGridView1.Rows.Clear();
                Table();
            }  
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Table();

        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();//退出系统
        }

        private void 重启ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dao dao = new Dao();
            dao.restart();
        }
    }
}
