using CRUD_Operations;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data;
using System.Data.SqlClient;
using System.IO;


namespace Mid_ProjectDB.Student
{
    public partial class Report : Form
    {
        public Report()
        {
            InitializeComponent();
        }

        private void btn_show_Click(object sender, EventArgs e)
        {
            if (combo.Text != "")
            {
                if (combo.Text == "CLO_wise_Result")
                {
                    var Con = Configuration.getInstance().getConnection();
                    SqlCommand cmd = new SqlCommand("select distinct S.FirstName,clo.Name,R.Details as Rubric_Detail,RL.Details  as RubricLevel_Detail,RL.MeasurementLevel from Clo,Rubric R,RubricLevel RL,StudentResult SR,Student S where R.CloId= any(select Clo.Id from Clo) and clo.Name='CLO1' and RL.RubricId=any(select R.Id from Rubric) and S.Id=any(select SR.StudentId from StudentResult where Sr.RubricMeasurementId=any(select RubricId from Rubric))", Con);
                    SqlDataAdapter dt = new SqlDataAdapter(cmd);
                    DataTable dta = new DataTable();
                    dt.Fill(dta);
                    dgv_report.DataSource = dta;
                }
            }
            else { MessageBox.Show("Select Report Value","ERROR"); };
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
                // Get data from the database
                DataTable dt = GetDataFromDatabase();

                // Create a new PDF document
                Document doc = new Document();
                PdfWriter.GetInstance(doc, new FileStream("StudentResultReport.pdf", FileMode.Create));
                doc.Open();

                // Add a table with headers
                PdfPTable table = new PdfPTable(5);
                table.AddCell("Student ID");
                table.AddCell("Assessment Component");
                table.AddCell("Rubric Level");
                table.AddCell("Evaluation Date");
                table.AddCell("Obtained Marks");

                // Add data rows to the table
                foreach (DataRow row in dt.Rows)
                {
                    int studentID = Convert.ToInt32(row["StudentID"]);
                    int component = Convert.ToInt32(row["AssessmentComponentId"]);
                    int rubricLevel = Convert.ToInt32(row["RubricMeasurementId"]);
                    DateTime evalDate = Convert.ToDateTime(row["EvaluationDate"]);
                // assume this function returns the maximum rubric level for the given component
                    double maxRubricLevel = GetMaxRubricLevel(component);
                    double obtainedMarks = (rubricLevel / maxRubricLevel) * GetComponentMarks(component);  // assume this function returns the marks assigned to the given component
                table.AddCell(studentID.ToString());
                    table.AddCell(component.ToString());
                    table.AddCell(rubricLevel.ToString());
                    table.AddCell(evalDate.ToString());
                    table.AddCell(obtainedMarks.ToString());
                }

                // Add the table to the document
                doc.Add(table);

                // Close the document
                doc.Close();

                MessageBox.Show("PDF report generated successfully!");
         }

        private DataTable GetDataFromDatabase()
        {
            var Con = Configuration.getInstance().getConnection();
            string query = "SELECT StudentID, AssessmentComponentId, RubricMeasurementId, EvaluationDate FROM StudentResult";
           
                SqlCommand command = new SqlCommand(query, Con);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;

        }







        private double GetMaxRubricLevel(int com)
        {
            var Con = Configuration.getInstance().getConnection();
            //string query = $"SELECT MAX(RML.RubricMeasurementId) FROM StudentResult SR JOIN RubricLevel RML ON SR.RubricMeasurementId = RML.Id WHERE SR.AssessmentComponentId = {component}";
            string query = $"SELECT MAX(RML.MeasurementLevel) FROM StudentResult SR JOIN RubricLevel RML ON SR.RubricMeasurementId = RML.Id";
           
                SqlCommand command = new SqlCommand(query, Con);
                
                object result = command.ExecuteScalar();
                if (result != null && double.TryParse(result.ToString(), out double maxLevel))
                {
                    MessageBox.Show("Max Rubric:" + result);
                    return maxLevel;
                }
                else
                {
                    throw new ArgumentException($"No data found for component:");
                }
                
            
        }

        private double GetComponentMarks(int component)
        {
            var Con = Configuration.getInstance().getConnection();
            string query = $"SELECT TotalMarks FROM AssessmentComponent WHERE Id = {component}";
            
                SqlCommand command = new SqlCommand(query, Con);
                
                object result = command.ExecuteScalar();
                if (result != null && double.TryParse(result.ToString(), out double marks))
                {
                    return marks;
                }
                else
                {
                    throw new ArgumentException($"Invalid component: {component}");
                }
         
        }
        private double GetObtainedMarks(int studentId, int component)
        {
            var Con = Configuration.getInstance().getConnection();
            double maxLevel = GetMaxRubricLevel(component);
            double componentMarks = GetComponentMarks(component);
            string query = $"SELECT RubricMeasurementId FROM StudentResult WHERE StudentID = {studentId} AND AssessmentComponentId = {component}";
            
                SqlCommand command = new SqlCommand(query, Con);
           
                SqlDataReader reader = command.ExecuteReader();
                double obtainedLevel = 0;
                while (reader.Read())
                {
                    int rubricLevelId = reader.GetInt32(0);
                    double measurementLevel = GetMeasurementLevel(rubricLevelId);
                    obtainedLevel += measurementLevel;
                }
                reader.Close();
                double obtainedMarks = (obtainedLevel / maxLevel) * componentMarks;
                return obtainedMarks;
            
        }

        private double GetMeasurementLevel(int rubricLevelId)
        {
            var Con = Configuration.getInstance().getConnection();
            
            string query = $"SELECT MeasurementLevel FROM RubricLevel WHERE Id = {rubricLevelId}";
           
                SqlCommand command = new SqlCommand(query, Con);
               
                object result = command.ExecuteScalar();
                if (result != null && double.TryParse(result.ToString(), out double measurementLevel))
                {
                    return measurementLevel;
                }
                else
                {
                    throw new ArgumentException($"Invalid rubric level: {rubricLevelId}");
                }
            }
        }

    }
