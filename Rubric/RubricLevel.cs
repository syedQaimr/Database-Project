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
    public partial class RubricLevel : Form
    {
        public SqlDataReader dr;
        public RubricLevel()
        {
            InitializeComponent();
            LoadData();
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            var Con = Configuration.getInstance().getConnection();
            try

            {
                if (txt_rldetail.Text != "" && cmb_rlid.Text != "" && txt_mid.Text != "")
                {
                    string query = "insert into RubricLevel values(@RubricId,@Details,@MeasurementLevel)";
                    SqlCommand q1 = new SqlCommand(query, Con);
                    q1.Parameters.AddWithValue("@RubricId", cmb_rlid.SelectedValue);
                    q1.Parameters.AddWithValue("@Details", txt_rldetail.Text);
                    q1.Parameters.AddWithValue("@MeasurementLevel", txt_mid.Text);
                    q1.ExecuteNonQuery();
                    MessageBox.Show("Added Successfully", "StudentResult");
                    LoadData();
                }
                else { MessageBox.Show("Empty field", "ERROR"); };
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "ERROR"); };


        }
        private void btn_up_Click(object sender, EventArgs e)
        {
            var Con = Configuration.getInstance().getConnection();

            try
            {
                if (txt_mid.Text != "")
                {

                    string q1 = "update RubricLevel set RubricId='" + cmb_rlid.SelectedValue + "' ,Details='" + txt_rldetail.Text + "' ,MeasurementLevel='" + txt_mid.Text + "' where RubricId='" + cmb_rlid.SelectedValue + "'";
                    SqlCommand q2 = new SqlCommand(q1, Con);
                    q2.ExecuteNonQuery();
                    MessageBox.Show("Updated Successfully", "RubricLevel");
                    LoadData();
                    Cmb_rlid();

                }
                else { MessageBox.Show("Enter the proper field", "Rubric"); };
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "ERROR"); };
        }




        public void LoadData()
        {
            var Con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from RubricLevel", Con);
            SqlDataAdapter dt = new SqlDataAdapter(cmd);
            DataTable dta = new DataTable();
            dt.Fill(dta);
            dgv_rubric.DataSource = dta;
        }







        public void Cmb_rlid()
        {
            var Con = Configuration.getInstance().getConnection();
            DataTable t = new DataTable("RubricLevel");
            SqlCommand cmd = new SqlCommand("Select * From RubricLevel", Con);
            dr = cmd.ExecuteReader();
            t.Load(dr);
            cmb_rlid.DisplayMember = "ID";
            cmb_rlid.ValueMember = "ID";
            cmb_rlid.DataSource = t;
            StudentAttendance Form = new StudentAttendance();
            Form.Refresh();

        }

      

        private void RubricLevel_Load(object sender, EventArgs e)
        {
            Cmb_rlid();
        }

       
        private void dgv_rubric_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv_rubric.CurrentRow.Index != -1)
            {

                cmb_rlid.SelectedValue = dgv_rubric.CurrentRow.Cells[1].Value.ToString();
                txt_rldetail.Text = dgv_rubric.CurrentRow.Cells[2].Value.ToString();
                txt_mid.Text = dgv_rubric.CurrentRow.Cells[3].Value.ToString();
            }
         }
}
}
