using CRUD_Operations;
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

namespace Mid_ProjectDB.Student
{
    public partial class StudentResult : Form
    {
        public SqlDataReader dr;
        public StudentResult()
        {
            InitializeComponent();
            LoadData();
        }

       

        private void btn_add_Click(object sender, EventArgs e)
        {
            var Con = Configuration.getInstance().getConnection();
            try
            {
               if (cmb_sid.Text != "" && cmb_rubmeasid.Text != "" && cmb_asscomid.Text != "")
                {
                    string query = "insert into StudentResult values(@StudentID,@AssessmentComponentId,@RubricMeasurementId,@EvaluationDate)";
                    SqlCommand q1 = new SqlCommand(query, Con);
                    q1.Parameters.AddWithValue("@StudentID", cmb_sid.SelectedValue);
                    q1.Parameters.AddWithValue("@AssessmentComponentId", cmb_asscomid.SelectedValue);
                    q1.Parameters.AddWithValue("@RubricMeasurementId", cmb_rubmeasid.SelectedValue);
                    q1.Parameters.AddWithValue("@EvaluationDate", dateTimePicker1.Value);
                    q1.ExecuteNonQuery();
                    MessageBox.Show("Added Successfully", "StudentResult");
                    LoadData();
                }
                else { MessageBox.Show("Empty field", "ERROR"); };
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "ERROR"); };
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            var Con = Configuration.getInstance().getConnection();

            try
            {
                if (cmb_sid.Text != "" && cmb_asscomid.Text != "" && cmb_rubmeasid.Text != "")
                {
                    string query = "update StudentResult set StudentID='" + cmb_sid.SelectedValue + "',AssessmentComponentId='" + cmb_asscomid.SelectedValue + "',RubricMeasurementId='" + cmb_rubmeasid.SelectedValue + "',EvaluationDate='" + dateTimePicker1.Value + "' where StudentId='" + cmb_sid.SelectedValue + "'";
                    SqlCommand q1 = new SqlCommand(query, Con);
                    q1.ExecuteNonQuery();
                    MessageBox.Show("Update Successfully", "StudentResult");
                    LoadData();
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
                if (cmb_sid.Text != "")
                {
                    string query = "delete from StudentResult where StudentId='" + cmb_sid.SelectedValue + "'";
                    SqlCommand q1 = new SqlCommand(query, Con);
                    q1.ExecuteNonQuery();
                    MessageBox.Show("Deleted Successfully", "StudentResult");
                    LoadData();
                }
                else { MessageBox.Show("Incorrect ID Number", "ERROR"); };
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "ERROR"); };
        }

        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            var Con = Configuration.getInstance().getConnection();

            try
            {

                SqlCommand cmd = new SqlCommand("select * from StudentResult where StudentID like'" + txt_search.Text + "%'", Con);
                SqlDataAdapter dta = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                dta.Fill(dt);
                dgv_result.DataSource = dt;

            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "ERROR"); };
        }

        private void StudentResult_Load(object sender, EventArgs e)
        {
            Cmb_asscomp();
            Cmb_rubmid();
            Cmb_student();

        }

        public void Cmb_student()
        {
            var Con = Configuration.getInstance().getConnection();
            DataTable t = new DataTable("Student");
            SqlCommand cmd = new SqlCommand("Select ID,FirstName,LastName,Contact,Email,RegistrationNumber,Status From Student", Con);
            dr = cmd.ExecuteReader();
            t.Load(dr);
            cmb_sid.DisplayMember = "RegistrationNumber";
            cmb_sid.ValueMember = "ID";
            cmb_sid.DataSource = t;

        }
        public void Cmb_asscomp()
        {
            var Con = Configuration.getInstance().getConnection();
            DataTable t = new DataTable("AssessmentComponent");
            SqlCommand cmd = new SqlCommand("Select * From AssessmentComponent", Con);
            dr = cmd.ExecuteReader();
            t.Load(dr);
            cmb_asscomid.DisplayMember = "ID";
            cmb_asscomid.ValueMember = "ID";
            cmb_asscomid.DataSource = t;
            StudentAttendance Form = new StudentAttendance();
            Form.Refresh();

        }
        public void Cmb_rubmid()
        {
            var Con = Configuration.getInstance().getConnection();
            DataTable t = new DataTable("RubricLevel");
            SqlCommand cmd = new SqlCommand("Select * From RubricLevel", Con);
            dr = cmd.ExecuteReader();
            t.Load(dr);
            cmb_rubmeasid.DisplayMember = "ID";
            cmb_rubmeasid.ValueMember = "ID";
            cmb_rubmeasid.DataSource = t;

        }
        public void LoadData()
        {
            var Con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from StudentResult", Con);
            SqlDataAdapter dt = new SqlDataAdapter(cmd);
            DataTable dta = new DataTable();
            dt.Fill(dta);
            dgv_result.DataSource = dta;
        }

        

    }
}
