namespace Countries
{
    using Countries.Modelos;
    using Countries.Servicos;
    using Svg;
    using System;
    using System.Collections.Generic;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using System.Windows.Media.Imaging;


    public partial class FormHome : Form
    {
        #region Atributes

        private List<Country> Countries;

        private NetworkService networkService;

        private ApiService apiService;

        private DialogService dialogService;

        private DataService dataService;

        #endregion


        public FormHome()
        {
            InitializeComponent();

            networkService = new NetworkService();
            apiService = new ApiService();
            dialogService = new DialogService();
            dataService = new DataService();

            LoadCountries();
        }

        
        /// <summary>
        /// Method to check if data is loaded from the internet or from the sqlite file, also controls if App is up to date and also controls first time user
        /// </summary>
        private async void LoadCountries()
        {
            bool load;

            //Checks internet connection
            lblResultado.Text = "Updating App...";

            var connection = networkService.CheckConnection();

            if (!connection.IsSuccess)
            {
                LoadLocalCountries();
                load = false;                
            }
            else
            {
                await LoadApiCountries();
                load = true;
            }


            
            //Reports to the first time user to connect to the internet, if the connection doesn´t exist
            if (Countries.Count == 0)
            {
                lblStatus.Text = "Please connect to the internet.";

                lblResultado.Text = "Error...";

                MessageBox.Show("If you´re using the Countries App for the first time make sure you have internet connection.");

                return;
            }
            

            //fills the listBoxCountries with data
            listBoxCountries.DataSource = Countries;
            listBoxCountries.DisplayMember = "Name";


            //App status
            if (load)
            {
                lblStatus.Text = string.Format("Data uploaded from the internet in {0:F}", DateTime.Now);
            }
            else
            {
                lblStatus.Text = string.Format("Data uploaded from the DataBase.");
            }

            //Reports if the App is up to date
            progressBar1.Value = 100;

            listBoxCountries.Enabled = true;

            lblResultado.Text = "App is up to date...";
        }


        /// <summary>
        /// Method to load data from the Sqlite file, when the app is offline
        /// </summary>
        private void LoadLocalCountries()
        {
            Countries = dataService.GetCountriesData(); 
        }


        /// <summary>
        /// Method to load data from the Api
        /// </summary>        
        private async Task LoadApiCountries()
        {
            progressBar1.Value = 0;

            var response = await apiService.GetCountries("http://restcountries.eu", "/rest/v2/all");

            Countries = (List<Country>)response.Result;

            dataService.DeleteCountriesData();//evertyime we run the app with internet connection, the sqlite file is deleted at first

            dataService.SaveCountriesData(Countries);//next updates the sqlite file with fresh data from the Api

            GetCountriesFlags();
        }


        /// <summary>
        /// Method to download Flags from the Api and converts them into .jpg
        /// </summary>
        private void GetCountriesFlags()
        {
            if (!Directory.Exists("Flags"))
            {
                Directory.CreateDirectory("Flags");
            }

            foreach (var getFlag in Countries)
            {
                try
                {
                    string flagName = getFlag.Flag.Split('/')[4].Split('.')[0];
                    var path = @"Flags\" + $"{flagName}.svg";

                    string svgFile = "http://restcountries.eu" + $"/data/{flagName}.svg";

                    using (WebClient webClient = new WebClient())
                    {
                        webClient.DownloadFile(svgFile, path);
                    }

                    string flag = flagName;
                    var pathFlag = @"Flags\" + $"{flagName}.jpg";

                    var svgDoc = SvgDocument.Open(path);
                    var bitMap = svgDoc.Draw(100, 100);

                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }

                    if (!File.Exists(pathFlag))
                    {
                        bitMap.Save(pathFlag, ImageFormat.Jpeg);
                    }
                }
                catch
                {
                    continue;
                }
            }
        }
        //********************END OF METHODS**************************************************


        private void listBoxCountries_SelectedIndexChanged(object sender, EventArgs e)
        {   
            
            Country selectedCountry = (Country)listBoxCountries.SelectedItem;

            if (selectedCountry != null)
            {
                lblNome.Text = selectedCountry.Name;
                lblCapital.Text = selectedCountry.Capital;
                lblRegiao.Text = selectedCountry.Region;
                lblSubRegiao.Text = selectedCountry.Subregion;
                lblPopulacao.Text = selectedCountry.Population.ToString();
                lblGini.Text = selectedCountry.Gini;              

                //Grabs Flag Name:
                string flagName = selectedCountry.Flag.Split('/')[4].Split('.')[0];

                BitmapImage img = new BitmapImage();
                img.BeginInit();

                if (File.Exists(Environment.CurrentDirectory + "/Flags" + $"/{flagName}.jpg"))
                {
                    img.UriSource = new Uri(Environment.CurrentDirectory + "/Flags" + $"/{flagName}.jpg");
                }
                else
                {
                    img.UriSource = new Uri(Environment.CurrentDirectory + "/NoImage" + "/NoImageAvailable.jpg");//if no image is available from the Api, this .jpg is presented instead                                       
                }

                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox1.ImageLocation = img.ToString();

                img.EndInit();
            }


            //If any field from the Api is empty this Text appears:
            if (lblNome.Text == string.Empty)
            {
                lblNome.Text = "(Unknown)";
            }
            if (lblCapital.Text == string.Empty)
            {
                lblCapital.Text = "(Unknown)";
            }
            if (lblRegiao.Text == string.Empty)
            {
                lblRegiao.Text = "(Unknown)";
            }
            if (lblSubRegiao.Text == string.Empty)
            {
                lblSubRegiao.Text = "(Unknown)";
            }
            if (lblPopulacao.Text == string.Empty)
            {
                lblPopulacao.Text = "(Unknown)";
            }
            if (lblGini.Text == string.Empty)
            {
                lblGini.Text = "(Unknown)";
            }
        }


        private void btnFormAbout_Click(object sender, EventArgs e)
        {
            FormAbout formAbout = new FormAbout(this);
            this.Hide();
            formAbout.Show();
        }


        private void btnExitApp_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        

        //-------------------not used--------------------------------
        private void button1_Click(object sender, EventArgs e)
        {
        }
        private void btnSair_Click(object sender, EventArgs e)
        {
        }
        private void lblSubRegiao_Click(object sender, EventArgs e)
        {
        }
        //-----------------------------------------------------------
    }
}
