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
    public partial class Student : Form
    {
        public SqlDataReader dr;
        public Student()
        {
            InitializeComponent();
            LoadData();
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            var Con = Configuration.getInstance().getConnection();
            try
            {
                if (txt_regno.Text.Length >= 9 && txt_regno.Text != "" && txt_contact.Text != "" && txt_email.Text != "")
                {
                    if (txt_email.Text.Contains("@gmail.com"))
                    {

                        string query = "insert into Student values(@FirstName,@LastName,@Contact,@Email,@RegistrationNumber,@Status)";
                        SqlCommand q1 = new SqlCommand(query, Con);
                        q1.Parameters.AddWithValue("@FirstName", txt_fname.Text);
                        q1.Parameters.AddWithValue("@LastName", txt_lname.Text);
                        q1.Parameters.AddWithValue("@Contact", txt_contact.Text);
                        q1.Parameters.AddWithValue("@Email", txt_email.Text);
                        q1.Parameters.AddWithValue("@RegistrationNumber", txt_regno.Text);
                        q1.Parameters.AddWithValue("@Status", cmb_status.SelectedValue);
                        q1.ExecuteNonQuery();
                        MessageBox.Show("Added Successfully", "Student");
                        LoadData();
                    }
                    else { MessageBox.Show("Enter the Correct Email"); };
                }
                else { MessageBox.Show("Enter the proper field", "ERROR"); };
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "ERROR"); };
        }

        private void btn_up_Click(object sender, EventArgs e)
        {
            var Con = Configuration.getInstance().getConnection();

            try
            {
                if (txt_regno.Text.Length >= 9 && txt_regno.Text != "" && txt_fname.Text != "" && txt_lname.Text != "")
                {
                    if (txt_email.Text.Contains("@gmail.com"))
                    {
                        string q1 = "update Student set FirstName='" + txt_fname.Text + "' ,LastName='" + txt_lname.Text + "' ,Contact='" + txt_contact.Text + "' ,Email='" + txt_email.Text + "',RegistrationNumber='" + txt_regno.Text + "',Status='" + cmb_status.SelectedValue + "' where RegistrationNumber='" + txt_regno.Text + "'";
                        SqlCommand q2 = new SqlCommand(q1, Con);
                        q2.ExecuteNonQuery();
                        MessageBox.Show("Updated Successfully", "Student");
                        LoadData();
                    }
                    else { MessageBox.Show("Incorrect Session", "ERROR"); };
                }
                else { MessageBox.Show("Enter the proper field", "ERROR"); };
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "ERROR"); };
        }

        private void btn_del_Click(object sender, EventArgs e)
        {
            var Con = Configuration.getInstance().getConnection();
            //try
            //{
            //    string q = "delete from StudentAttendance where StudentId=" +25+ "";
            //    SqlCommand q2 = new SqlCommand(q, Con);
            //    q2.ExecuteNonQuery();

            //}
            //catch(Exception ex) { MessageBox.Show(ex.Message, "ERROR"); };
            try
            {
                if (txt_regno.Text.Length >= 9 && txt_regno.Text != "")
                {
                    
                    string query = "delete from Student where RegistrationNumber='" + txt_regno.Text + "'";

                    SqlCommand q1 = new SqlCommand(query, Con);
                    q1.ExecuteNonQuery();
                    MessageBox.Show("Deleted Successfully", "Student");
                    LoadData();
                }
                else { MessageBox.Show("Incorrect Reg Number", "ERROR"); };
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "ERROR"); };
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            var Con = Configuration.getInstance().getConnection();

            try
            {
                if (txt_search.Text != "")
                {
                    SqlCommand cmd = new SqlCommand("select * from Student where RegistrationNumber like'" + txt_search.Text + "%'", Con);
                    SqlDataAdapter dta = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    dta.Fill(dt);
                    dgv_student.DataSource = dt;
                }
                else { MessageBox.Show("Empty Search", "ERROR"); };


            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "ERROR"); };
        }
        public void LoadData()
        {
            var Con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from Student", Con);
            SqlDataAdapter dt = new SqlDataAdapter(cmd);
            DataTable dta = new DataTable();
            dt.Fill(dta);
            dgv_student.DataSource = dta;
        }
        public void Combo_status()
        {
            var Con = Configuration.getInstance().getConnection();
            DataTable t = new DataTable("Lookup");
            SqlCommand cmd = new SqlCommand("Select Lookupid,Name,Category From Lookup where LookupID=5 or lookupID=6", Con);
            dr = cmd.ExecuteReader();
            t.Load(dr);
            cmb_status.DisplayMember = "Name";
            cmb_status.ValueMember = "Lookupid";
            cmb_status.DataSource = t;

        }

        private void dgv_student_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv_student.CurrentRow.Index != -1)
            {

                txt_fname.Text = dgv_student.CurrentRow.Cells[1].Value.ToString();
                txt_lname.Text = dgv_student.CurrentRow.Cells[2].Value.ToString();
                txt_contact.Text = dgv_student.CurrentRow.Cells[3].Value.ToString();
                txt_email.Text = dgv_student.CurrentRow.Cells[4].Value.ToString();
                txt_regno.Text = dgv_student.CurrentRow.Cells[5].Value.ToString();
                cmb_status.SelectedValue = dgv_student.CurrentRow.Cells[6].Value.ToString();

            }
        }

        private void Student_Load(object sender, EventArgs e)
        {
            Combo_status();
        }
    }
}
