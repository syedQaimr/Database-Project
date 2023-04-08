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

namespace Mid_ProjectDB.Assessment
{
    public partial class Assessment : Form
    {
        public SqlDataReader dr;
        public Assessment()
        {
            InitializeComponent();
          //  LoadData();
        }

        private void Assessment_Load(object sender, EventArgs e)
        {
            Cmb_assid();
            Cmb_rid();
        }
        private void btn_ass_Click(object sender, EventArgs e)
        {

            var Con = Configuration.getInstance().getConnection();
            try
            {
                if (txt_title.Text != "" && txt_totalmarks.Text != "" && txt_totalweig.Text != "")
                {
                    string query = "insert into Assessment values(@Title,@DateCreated,@TotalMarks,@TotalWeightage)";
                    SqlCommand q1 = new SqlCommand(query, Con);
                    q1.Parameters.AddWithValue("@Title", txt_title.Text);
                    q1.Parameters.AddWithValue("@DateCreated", dateTime.Value);
                    q1.Parameters.AddWithValue("@TotalMarks", txt_totalmarks.Text);
                    q1.Parameters.AddWithValue("@TotalWeightage", txt_totalweig.Text);
                    q1.ExecuteNonQuery();
                    MessageBox.Show("Added Successfully", "Assessment");
                    Cmb_assid();
                    Cmb_rid();
                }
                else { MessageBox.Show("Empty field", "ERROR"); };
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "ERROR"); };
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            var Con = Configuration.getInstance().getConnection();
            try
            {
                if (txt_name.Text != "" && txt_tmarks.Text != "" && cmb_assid.Text != "" && cmb_rid.Text != "")
                {
                    for (int i = 0; i < dgv_assessment.Rows.Count; i++)
                    {

                        SqlCommand cmd = new SqlCommand("insert into AssessmentComponent (Name,RubricId,TotalMarks,DateCreated,DateUpdated,AssessmentId) values('" + dgv_assessment.Rows[i].Cells[0].Value + "'," + dgv_assessment.Rows[i].Cells[1].Value + "," + dgv_assessment.Rows[i].Cells[2].Value + ",'" + dgv_assessment.Rows[i].Cells[3].Value + "','" + dgv_assessment.Rows[i].Cells[4].Value + "'," + dgv_assessment.Rows[i].Cells[5].Value + ")", Con);
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Added Successfully", "AssessmentComponent");
                    Cmb_assid();
                    Cmb_rid();
                    dgv_assessment.Rows.Clear();
                }
                else { MessageBox.Show("Empty field", "ERROR"); };
             }
            catch (Exception ex) { MessageBox.Show(ex.Message, "ERROR"); };
        }

       

        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            var Con = Configuration.getInstance().getConnection();

            try
            {

                SqlCommand cmd = new SqlCommand("select * from AssessmentComponent Ac where Ac.AssessmentId= any(select A.Id from Assessment A where A.Title like '" +txt_search.Text+"%')", Con);
                SqlDataAdapter dta = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                dta.Fill(dt);
                dgv_search.DataSource = dt;
                dgv_search.Visible = true;
                
                //LoadData();

            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "ERROR"); };
        }

        private void dgv_assessment_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv_assessment.CurrentRow.Index != -1)
            {

                //txt_title.Text = dgv_assessment.CurrentRow.Cells[1].Value.ToString();
                //txt_totalmarks.Text = dgv_assessment.CurrentRow.Cells[2].Value.ToString();
                //txt_totalweig.Text = dgv_assessment.CurrentRow.Cells[3].Value.ToString();
               // dateTime.Value = dgv_assessment.CurrentRow.Cells[4].Value;
               
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txt_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cmb_rid.Focus();
            }
        }

        private void cmb_rid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt_tmarks.Focus();
            }
        }

        private void txt_tmarks_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cmb_assid.Focus();
            }
        }

        private void cmb_assid_KeyDown(object sender, KeyEventArgs e)
        {
            var Con = Configuration.getInstance().getConnection();
            if (e.KeyCode == Keys.Enter)
            {
                dgv_assessment.Rows.Add(txt_name.Text, cmb_rid.SelectedValue, txt_tmarks.Text, dateTime.Value,dateupdated.Value,cmb_assid.SelectedValue);
                txt_name.Focus();
            }
        }

        public void Cmb_rid()
        {
            var Con = Configuration.getInstance().getConnection();
            DataTable t = new DataTable("Rubric");
            SqlCommand cmd = new SqlCommand("Select * From Rubric", Con);
            dr = cmd.ExecuteReader();
            t.Load(dr);
            cmb_rid.DisplayMember = "ID";
            cmb_rid.ValueMember = "ID";
            cmb_rid.DataSource = t;

        }
        public void Cmb_assid()
        {
            var Con = Configuration.getInstance().getConnection();
            DataTable t = new DataTable("Assessment");
            SqlCommand cmd = new SqlCommand("Select * From Assessment", Con);
            dr = cmd.ExecuteReader();
            t.Load(dr);
            cmb_assid.DisplayMember = "Title";
            cmb_assid.ValueMember = "ID";
            cmb_assid.DataSource = t;

        }

        private void txt_search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                dgv_search.Visible = false;

            }
        }

       
    }
}
