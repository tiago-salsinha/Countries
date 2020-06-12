namespace Countries
{
    using System;
    using System.Windows.Forms;


    public partial class FormAbout : Form
    {
        public FormHome _formHome { get; set; }

        public FormAbout(FormHome formHome)
        {
            _formHome = formHome;

            InitializeComponent();
        }

        private void btnReturnHome_Click(object sender, EventArgs e)
        {
            this.Close();
            _formHome.Show();
        }


        //------------------not used-------------------------
        private void label1_Click(object sender, EventArgs e)
        {
        }
        //--------------------------------------------------
    }
}
