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
    public partial class classpopup : Form
    {
        public classpopup()
        {
            InitializeComponent();
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            var Con = Configuration.getInstance().getConnection();
            try
            {
                string query1 = "insert into ClassAttendance values(@AttendanceDate)";
                SqlCommand q2 = new SqlCommand(query1, Con);
                q2.Parameters.AddWithValue("@AttendanceDate", dateTimePicker1.Value);
                q2.ExecuteNonQuery();
                MessageBox.Show("Date Added Successfully", "ClassAttendance");
                StudentAttendance Form = new StudentAttendance();
                Form.Refresh();

            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "ERROR"); };

        
    }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
