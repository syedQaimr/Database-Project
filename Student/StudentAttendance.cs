using CRUD_Operations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mid_ProjectDB.Student
{
    public partial class StudentAttendance : Form
    {
        public SqlDataReader dr;
        public StudentAttendance()
        {
            InitializeComponent();
            this.Refresh();
            LoadData();
        }

        private void btn_adddate_Click(object sender, EventArgs e)
        {

            classpopup C = new classpopup();
            C.Show();
        }
       
        private void btn_addA_Click(object sender, EventArgs e)
        {
            var Con = Configuration.getInstance().getConnection();
          
            try
            {
                if (cmb_sid.Text != "" && cmb_aid.Text != "" && cmb_attstatus.Text != "")
                {
                    string query = "insert into StudentAttendance values(@AttendanceId,@StudentId,@AttendanceStatus)";
                    SqlCommand q1 = new SqlCommand(query, Con);
                    q1.Parameters.AddWithValue("@Attendanceid", cmb_aid.SelectedValue);
                    q1.Parameters.AddWithValue("@StudentId", cmb_sid.SelectedValue);
                    q1.Parameters.AddWithValue("@AttendanceStatus", cmb_attstatus.SelectedValue);
                    q1.ExecuteNonQuery();
                    MessageBox.Show("Added Successfully", "StudentAttendance");
                    LoadData();
                    Cmb_attendance();
                    Cmb_student();
                }
                else { MessageBox.Show("Enter the proper field", "ERROR"); };
             }
             catch (Exception ex) { MessageBox.Show(ex.Message, "ERROR"); }; 
        }
        private void btn_updateA_Click(object sender, EventArgs e)
        {
            var Con = Configuration.getInstance().getConnection();


            try
            {
                if (cmb_sid.Text != "" && cmb_aid.Text != "" && cmb_attstatus.Text != "")
                {
                    string query = "update StudentAttendance set AttendanceId='" +cmb_aid.SelectedValue+  "',StudentId='" + cmb_sid.SelectedValue+"',AttendanceStatus='"+ cmb_attstatus.SelectedValue+"' where AttendanceId='"+cmb_aid.SelectedValue+"'";
                    SqlCommand q1 = new SqlCommand(query, Con);
                    q1.ExecuteNonQuery();
                    MessageBox.Show("Update Successfully", "StudentAttendance");
                    LoadData();
                    Cmb_attendance();
                    Cmb_student();
                }
                else { MessageBox.Show("Enter the proper field", "ERROR"); };
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "ERROR"); };
        }
        private void btn_del_Click(object sender, EventArgs e)
        {
            var Con = Configuration.getInstance().getConnection();
            try
            {
                if (cmb_sid.Text!= "")
                {
                    string query = "delete from StudentAttendance where StudentId='" + cmb_sid.SelectedValue + "'";
                    SqlCommand q1 = new SqlCommand(query, Con);
                    q1.ExecuteNonQuery();
                    MessageBox.Show("Deleted Successfully", "StudentAttendance");
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
             
                    SqlCommand cmd = new SqlCommand("select * from StudentAttendance where "+cmb_filter.Text+ " like'" + txt_search.Text + "%'", Con);
                    SqlDataAdapter dta = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    dta.Fill(dt);
                    dgv_attendance.DataSource = dt;

            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "ERROR"); };
        }


        public void LoadData()
        {
            var Con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from StudentAttendance", Con);
            SqlDataAdapter dt = new SqlDataAdapter(cmd);
            DataTable dta = new DataTable();
            dt.Fill(dta);
            dgv_attendance.DataSource = dta;
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
        public void Cmb_attendance()
        {
            var Con = Configuration.getInstance().getConnection();
            DataTable t = new DataTable("ClassAttendance");
            SqlCommand cmd = new SqlCommand("Select ID,AttendanceDate From ClassAttendance", Con);
            
            dr = cmd.ExecuteReader();
            t.Load(dr);
            cmb_aid.DisplayMember = "AttendanceDate";
            cmb_aid.ValueMember = "ID";
            cmb_aid.DataSource = t;
            StudentAttendance Form = new StudentAttendance();
            Form.Refresh();

        }
        public void Cmb_attenstatus()
        {
            var Con = Configuration.getInstance().getConnection();
            DataTable t = new DataTable("Loookup");
            SqlCommand cmd = new SqlCommand("Select Lookupid,Name,Category From Lookup where LookupId=1 or LookupId=2 or LookupId=3 or LookupId=4", Con);
            dr = cmd.ExecuteReader();
            t.Load(dr);
            cmb_attstatus .DisplayMember = "Name";
            cmb_attstatus.ValueMember = "Lookupid";
            cmb_attstatus.DataSource = t;

        }

        private void StudentAttendance_Load(object sender, EventArgs e)
        {
            Cmb_attenstatus();
            Cmb_attendance();
            Cmb_student();
            StudentAttendance Form = new StudentAttendance();
            Form.Refresh();
            
        }

        private void dgv_attendance_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            if (dgv_attendance.CurrentRow.Index != -1)
            {

                cmb_aid.SelectedValue = dgv_attendance.CurrentRow.Cells[0].Value.ToString();
                cmb_sid.SelectedValue = dgv_attendance.CurrentRow.Cells[1].Value.ToString();
                cmb_attstatus.SelectedValue = dgv_attendance.CurrentRow.Cells[2].Value.ToString();

            }
        }

      
    }
}
