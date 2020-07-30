using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GDIPlusDemo
{
    public partial class ModifyPassWord : Form
    {
        string SID;
        public ModifyPassWord()
        {
            InitializeComponent();
        }

        public ModifyPassWord(string sid)
        {
            
            InitializeComponent();
            SID = sid;
            string sql = "select * from Student where Id = '"+SID+"'";
            Dao dao = new Dao();
            IDataReader dr = dao.Read(sql);
            dr.Read();
            textBox1.Text = dr["PassWord"].ToString();
            dr.Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(textBox2.Text == textBox3.Text)
            {
                string sql = "update Student set PassWord = '" + textBox2.Text + "' where Id = '" + SID + "'";
                Dao dao = new Dao();
                int i = dao.Excute(sql);
                if (i > 0)
                {

                    MessageBox.Show("修改成功！");
                    this.Close();
                    dao.restart();
                }
            }
            else
            {
                MessageBox.Show("两次输入的密码不一致，请检查后再试！");
            }
            
        }

        private void ModifyPassWord_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
