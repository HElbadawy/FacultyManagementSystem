using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.DataAccess.Client;

namespace FacultyMIS_PROJECT
{
    public partial class StudentsForm : Form
    {
        string ordb = "Data source=orcl;User Id=scott; Password=tiger;";
        OracleConnection conn;

        Start start;

        public StudentsForm(Start s)
        {
            InitializeComponent();
            start = s;
        }

        private void StudentsForm_Load(object sender, EventArgs e)
        {
            conn = new OracleConnection(ordb);
            conn.Open();

            OracleCommand cmd = new OracleCommand();

            cmd.Connection = conn;
            cmd.CommandText = "SELECT student_id FROM students";
            cmd.CommandType = CommandType.Text;

            OracleDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                Student_ID.Items.Add(dr[0]);
            }
            dr.Close();
        }

        private void StudentsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            conn.Dispose();
            start.Show();
        }

        private void Index_Change(object sender, EventArgs e)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT contact_number, student_fname, student_lname, dob, no_of_credits_earned FROM students WHERE student_id=:id";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("id", Student_ID.SelectedItem.ToString());

            OracleDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                //numbers from 5 to 12 correspond to text boxes
                textBox5.Text = "0" + dr[0].ToString();
                textBox6.Text = dr[1].ToString();
                textBox7.Text = dr[2].ToString();
                textBox8.Text = dr[3].ToString();
                textBox12.Text = dr[4].ToString();
            }
            dr.Close();

            textBox10.Text = "";
            course.Items.Clear();
        }

        private void InsertButton_Click(object sender, EventArgs e)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "INSERT into Students values (:id,:cnumber,:fname,:lname,:dob, :gpa, :credits)";
            cmd.Parameters.Add("id", Student_ID.Text);
            cmd.Parameters.Add("cnumber", textBox5.Text);
            cmd.Parameters.Add("fname", textBox6.Text);
            cmd.Parameters.Add("lname", textBox7.Text);
            cmd.Parameters.Add("dob", textBox8.Text);
            cmd.Parameters.Add("gpa", textBox10.Text);
            cmd.Parameters.Add("credits", textBox12.Text);
            try
            {
                int r = cmd.ExecuteNonQuery();
                if (r != -1)
                {
                    Student_ID.Items.Add(Student_ID.Text);
                    Student_ID.Text = "";
                    textBox5.Text = "";
                    textBox6.Text = "";
                    textBox7.Text = "";
                    textBox8.Text = "";
                    textBox12.Text = "";
                    MessageBox.Show("New student added");
                }
            }
            catch (OracleException err)
            {
                switch (err.Number)
                {
                    case 1:
                        MessageBox.Show("Student already exists.");
                        break;
                    case 1843:
                        MessageBox.Show("Incorrect date formatting. (DD-MONTH-YY) ex: 26-AUG-01");
                        break;
                    default:
                        MessageBox.Show("Error inserting new student. Please check your formatting and data." + err.Message);
                        break;
                }
            }
        }

        private void showGPA_Click(object sender, EventArgs e)
        {

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "showGPA";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("id", Student_ID.Text);
            cmd.Parameters.Add("gpa", OracleDbType.Double, ParameterDirection.Output);
            cmd.ExecuteNonQuery();
            textBox10.Text = cmd.Parameters["gpa"].Value.ToString();
        }

        private void showCourses_Click(object sender, EventArgs e)
        {
            course.Items.Clear();

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "showcoursestaken";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("id", Student_ID.Text);
            cmd.Parameters.Add("courses", OracleDbType.RefCursor, ParameterDirection.Output);

            OracleDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                string courseItem = dr[0].ToString() + "             " + dr[1].ToString();
                course.Items.Add(courseItem);
            }
            dr.Close();

            if (course.Items.Count.Equals(0))
            {
                course.Items.Add("NO REGISTERED COURSES");
            }
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            if (textBox12.Text.ToString() == "0")
            {
                textBox10.Text = "4";
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "DELETE FROM Students WHERE STUDENT_ID = :id";
            cmd.Parameters.Add("id", Student_ID.Text);
            try
            {
                int r = cmd.ExecuteNonQuery();
                if (r != -1)
                {
                    Student_ID.Items.RemoveAt(Student_ID.SelectedIndex);
                    Student_ID.Text = "";
                    textBox5.Text = "";
                    textBox6.Text = "";
                    textBox7.Text = "";
                    textBox8.Text = "";
                    textBox12.Text = "";
                    MessageBox.Show("Student deleted.");
                }
            }
            catch (OracleException err)
            {
                MessageBox.Show("Error deleting student. Please check the ID. " + err.Message);
            }
            
        }
    }

}
