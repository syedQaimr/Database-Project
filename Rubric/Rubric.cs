using CRUD_Operations;
using Mid_ProjectDB.Student;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mid_ProjectDB.Rubric
{
    public partial class Rubric : Form
    {
        public SqlDataReader dr;
        public Rubric()
        {
            InitializeComponent();
            LoadData();
        }


        private void btn_new_Click(object sender, EventArgs e)
        {
            String ConnectionStr = @"Data Source=.\sqlexpress;Initial Catalog=ProjectB;Integrated Security=True";

            int pid = 0;
            using (SqlConnection con = new SqlConnection(ConnectionStr))
            {
                using (SqlCommand cmd = new SqlCommand("Select ISNULL(MAX(ID), 1) AS ID FROM Rubric", con))
                {
                    con.Open();
                    pid = Convert.ToInt32(cmd.ExecuteScalar()) + 1;
                    txt_rid.Text = pid.ToString();
                    Cmb_clo();
                    con.Close();
                }
            }

        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            var Con = Configuration.getInstance().getConnection();
            try
            {
                if (txt_rid.Text != "" && txt_detail.Text != "" && cmb_clo.Text != "")
                {
                    string query = "insert into Rubric values(@ID,@Details,@CloID)";
                    SqlCommand q1 = new SqlCommand(query, Con);
                    q1.Parameters.AddWithValue("@ID", txt_rid.Text);
                    q1.Parameters.AddWithValue("@Details", txt_detail.Text);
                    q1.Parameters.AddWithValue("@CloID", cmb_clo.SelectedValue);
                    q1.ExecuteNonQuery();
                    Cmb_clo();
                }
                else { MessageBox.Show("Empty field", "ERROR"); };
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "ERROR"); };


        }


        private void btn_del_Click(object sender, EventArgs e)
        {
            var Con = Configuration.getInstance().getConnection();

            try
            {
                if ( txt_rid.Text != "" )
                {
                   
                        string q1 = "update Rubric set ID='" + txt_rid.Text + "' ,Details='" + txt_detail.Text + "' ,CloId='" + cmb_clo.SelectedValue+ "' where ID='" + txt_rid.Text + "'";
                        SqlCommand q2 = new SqlCommand(q1, Con);
                        q2.ExecuteNonQuery();
                        MessageBox.Show("Updated Successfully", "Rubric");
                    LoadData();
                    Cmb_clo();
                       
                     }
                else { MessageBox.Show("Enter the proper field", "Rubric"); };
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "ERROR"); };
        }

        private void Rubric_Load(object sender, EventArgs e)
        {
            Cmb_clo();
        }
        public void LoadData()
        {
            var Con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from Rubric", Con);
            SqlDataAdapter dt = new SqlDataAdapter(cmd);
            DataTable dta = new DataTable();
            dt.Fill(dta);
            dgv_rubric.DataSource = dta;
        }

        private void btn_ass_Click(object sender, EventArgs e)
        {
            var Con = Configuration.getInstance().getConnection();
            try
            {
                if (txt_name.Text != "")
                {
                    string query = "insert into Clo values(@Name,@DateCreated,@DateUpdated)";
                    SqlCommand q1 = new SqlCommand(query, Con);
                    q1.Parameters.AddWithValue("@Name", txt_name.Text);
                    q1.Parameters.AddWithValue("@DateCreated", dateTime.Value);
                    q1.Parameters.AddWithValue("@DateUpdated", dateupdated.Value);
                    q1.ExecuteNonQuery();
                    MessageBox.Show("Add Successfully", "CLO");
                    Cmb_clo();

                }
                else { MessageBox.Show("Empty field", "Clo"); };
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "ERROR"); };
        }

        public void Cmb_clo()
        {
            var Con = Configuration.getInstance().getConnection();
            DataTable t = new DataTable("clo");
            SqlCommand cmd = new SqlCommand("Select * From clo", Con);
            dr = cmd.ExecuteReader();
            t.Load(dr);
            cmb_clo.DisplayMember = "Name";
            cmb_clo.ValueMember = "ID";
            cmb_clo.DataSource = t;

        }

        private void dgv_rubric_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            if (dgv_rubric.CurrentRow.Index != -1)
            {

                txt_rid.Text = dgv_rubric.CurrentRow.Cells[0].Value.ToString();
                txt_detail.Text = dgv_rubric.CurrentRow.Cells[1].Value.ToString();
                cmb_clo.SelectedValue= dgv_rubric.CurrentRow.Cells[2].Value.ToString();
            }
        }
    }
}
