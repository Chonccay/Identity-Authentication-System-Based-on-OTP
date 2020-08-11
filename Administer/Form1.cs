using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace adm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection conn;
        SqlDataAdapter sda;
        DataSet ds = null;

        public void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            cha a = new cha();
            a.getid(label2.Text,dataGridView1.CurrentCell.RowIndex);
            a.Show(this);

        }

        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            add a = new add();
            a.Show(this);
            
        }

        public void 查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            conn = new SqlConnection("server=PC201801031527;database=db_15;uid=sa;pwd=ccc1519803907");
            sda = new SqlDataAdapter("select * from tb_people", conn);
            ds = new DataSet();
            sda.Fill(ds, "people");
            dataGridView1.DataSource = ds.Tables[0];
            string msg = String.Format("第{0}行,第{1}列", dataGridView1.CurrentCell.RowIndex, dataGridView1.CurrentCell.ColumnIndex);
            label1.Text = "选择的单元格为：" + msg;
            int a = dataGridView1.CurrentCell.RowIndex;
            label2.Text = dataGridView1.Rows[a].Cells[0].Value.ToString();
        }



        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            class1 DBC = new class1();
            string strSql = "Delete from tb_people where tb_ID=" + label2.Text;
            string str2 = "Delete from tb_person where tb_ID=" + label2.Text;
            if (DBC.deleteDB(strSql) && DBC.deleteDB(str2))
            {
                MessageBox.Show("删除成功");
            }
            else
            {
                MessageBox.Show("删除失败");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: 这行代码将数据加载到表“db_15DataSet1.tb_people”中。您可以根据需要移动或删除它。
            this.tb_peopleTableAdapter1.Fill(this.db_15DataSet1.tb_people);
            // TODO: 这行代码将数据加载到表“db_15DataSet.tb_people”中。您可以根据需要移动或删除它。
            this.tb_peopleTableAdapter.Fill(this.db_15DataSet.tb_people);
            this.label2.Visible = false;
        }

        public void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            conn = new SqlConnection("server=PC201801031527;database=db_15;uid=sa;pwd=ccc1519803907");
            sda = new SqlDataAdapter("select * from tb_people", conn);
            ds = new DataSet();
            sda.Fill(ds, "people");
            string msg = String.Format("第{0}行,第{1}列", dataGridView1.CurrentCell.RowIndex, dataGridView1.CurrentCell.ColumnIndex);
            label1.Text = "选择的单元格为：" + msg;
            int a = dataGridView1.CurrentCell.RowIndex;
            label2.Text = dataGridView1.Rows[a].Cells[0].Value.ToString();
            label2.Visible = false;
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
