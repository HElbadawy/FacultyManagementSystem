using System;
using System.Windows.Forms;

namespace FacultyMIS_PROJECT
{
    public partial class Start : Form
    {
        private const string UNAME = "admin";
        private const string PWORD = "admin";

        public Start()
        {
            InitializeComponent();
        }

        private void login_Click(object sender, EventArgs e)
        {
            if (username.Text == UNAME && password.Text == PWORD)
            {
                username.Enabled = false;
                password.Enabled = false;
                label2.Enabled = false;
                label3.Enabled = false;
                error.Visible = false;
                login.Enabled = false;

                menu.Enabled = true;
                return;
            }

            error.Visible = true;
        }

        private void password_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                login_Click(sender, e);
            }
        }

        private void accessStudentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StudentsForm sform = new StudentsForm(this);
            sform.Show();
            this.Hide();
        }

        private void viewStudentsReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StudentReport SR = new StudentReport(this);
            SR.Show();
            this.Hide();
        }

        private void accessCoursesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CoursesForm cform = new CoursesForm(this);
            cform.Show();
            this.Hide();
        }

        private void viewCoursesReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CoursesReport CR = new CoursesReport(this);
            CR.Show();
            this.Hide();
        }
    }
}
