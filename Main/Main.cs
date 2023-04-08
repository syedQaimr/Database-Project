using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mid_ProjectDB.Main
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void btn_student_Click(object sender, EventArgs e)
        {
            Student.Student st = new Student.Student();
            st.Show();
           
        }

        private void slide_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btn_atten_Click(object sender, EventArgs e)
        {
            Student.StudentAttendance sa = new Student.StudentAttendance();
            sa.Show();
           
        }

        private void btn_rubric_Click(object sender, EventArgs e)
        {
            Rubric.Rubric R=new Rubric.Rubric();
            R.Show();

        }

        private void btn_rublevel_Click(object sender, EventArgs e)
        {
            Rubric.RubricLevel RL=new Rubric.RubricLevel();
            RL.Show();
        }

        private void btn_assess_Click(object sender, EventArgs e)
        {
            Assessment.Assessment A=new Assessment.Assessment();    
            A.Show();
        }

        private void btn_result_Click(object sender, EventArgs e)
        {
            Student.StudentResult SR=new Student.StudentResult();   
            SR.Show();
        }

        private void btn_report_Click(object sender, EventArgs e)
        {
            Student.Report Re=new Student.Report();
            Re.Show();
        }
    }
}
