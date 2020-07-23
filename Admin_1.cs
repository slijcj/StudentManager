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
    public partial class admin_1 : Form
    {
        
        string[] str = new string[4];
        public admin_1()
        {
            
            InitializeComponent();
            button3.Visible = false;//插入时隐藏修改按钮

        }
        public admin_1(string[] a)
        {

            InitializeComponent();
            for (int i = 0; i < 4; i++)
            {
                str[i] = a[i];
            }
            textBox1.Text = str[0];
            textBox2.Text = str[1];
            textBox3.Text = str[2];
            comboBox1.Text = str[3];
            button1.Visible = false;//修改窗体时隐藏保存按钮
        }


        private void admin_1_FontChanged(object sender, EventArgs e)
        {
            Application.Exit();//结束整个程序 
        }

        private void admin_1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || comboBox1.Text == "")
            {
                MessageBox.Show("输入不完整，请检查！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string sql = "insert into Teacher values('" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + comboBox1.Text + "')";
                //MessageBox.Show(sql);
                Dao dao = new Dao();
                int i = dao.Excute(sql);
                if (i > 0)
                {

                    MessageBox.Show("插入成功！");
                    this.Close();
                    
                }


            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || comboBox1.Text == "" )
            {
                MessageBox.Show("修改后有空项，请检查！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (textBox1.Text != str[0])
                {
                    string sql = "update Teacher set Id='" + textBox1.Text + "'where Id='" + str[0] + "'and Name='" + str[1] + "'";
                    Dao dao = new Dao();
                    dao.Excute(sql);
                    str[0] = textBox1.Text;
                }
                if (textBox2.Text != str[1])
                {
                    string sql = "update Teacher set Name='" + textBox2.Text + "'where Id='" + str[0] + "'and Name='" + str[1] + "'";
                    Dao dao = new Dao();
                    dao.Excute(sql);
                    str[1] = textBox2.Text;
                }
                if (textBox3.Text != str[2])
                {
                    string sql = "update Teacher set PassWord='" + textBox3.Text + "'where Id='" + str[0] + "'and Name='" + str[1] + "'";
                    Dao dao = new Dao();
                    dao.Excute(sql);
                    str[2] = textBox3.Text;
                }
                if (comboBox1.Text != str[3])
                {
                    string sql = "update Teacher set Birthday='" + comboBox1.Text + "'where Id='" + str[0] + "'and Name='" + str[1] + "'";
                    Dao dao = new Dao();
                    dao.Excute(sql);
                    str[3] = comboBox1.Text;
                }
               
                MessageBox.Show("修改成功！");
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            comboBox1.Text = "";
            
            this.Close();
        }
    }
}
