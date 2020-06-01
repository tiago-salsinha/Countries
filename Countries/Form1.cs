using Countries.Modelos;
using Countries.Servicos;
using Newtonsoft.Json;
using Svg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace Countries
{
    public partial class Form1 : Form
    {
        //public List<Country> Countries { get; set; } = new List<Country>();

        public Form1()
        {
            InitializeComponent();

            LoadCountries();
        }

        private async void LoadCountries()
        {

            var client = new HttpClient();
            client.BaseAddress = new Uri("http://restcountries.eu");

            var response = await client.GetAsync("/rest/v2/all"); 
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show(response.ReasonPhrase);
                return;
            }

            
            //Para corrigir o erro de valores nulos, não me carregava a list countries
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            var countries = JsonConvert.DeserializeObject<List<Country>>(result, settings);



            //teste para Sacar as bandeiras
            if (!Directory.Exists("Flags"))
            {
                Directory.CreateDirectory("Flags");
            }

            foreach (var getFlag in countries)
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
                    var pathFlag = @"Flags\" + $"{flagName}.jpg";//Salva a imagem como jpg

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
            


            listBoxCountries.DataSource = countries;
            listBoxCountries.DisplayMember = "Name";//controla o que que propriedade da class é disposto na comboBox, também podes controlar isto com o override toString() na Class Rate


            //teste consoante o pais selcionado aparecer o nome na label:
            //RegionalBloc selectedCountry = (RegionalBloc)listBoxCountries.SelectedItem;

            //if (selectedCountry != null)
            //{
            //    lblNome.Text = selectedCountry.Name;
            //}
            
            


            //corrige bug da microsoft
            //comBoxDestino.BindingContext = new BindingContext();//liga os objectos do interface ao código

            //comBoxDestino.DataSource = countries;
            //comBoxDestino.DisplayMember = "Name";

            //progressBar1.Value = 100;
        }

        private void listBoxCountries_SelectedIndexChanged(object sender, EventArgs e)
        {
            //var selectedCountry = listBoxCountries.GetItemText(listBoxCountries.SelectedValue);
            //lblNome.Text = selectedCountry;
           
            Country selectedCountry = (Country)listBoxCountries.SelectedItem;

            if (selectedCountry != null)
            {
                lblNome.Text = selectedCountry.Name;
                lblCapital.Text = selectedCountry.Capital;
                lblRegiao.Text = selectedCountry.Region;
                lblSubRegiao.Text = selectedCountry.Subregion;
                lblPopulacao.Text = selectedCountry.Population.ToString();
                lblGini.Text = selectedCountry.Gini.ToString();

                //pictureBox1.ImageLocation = selectedCountry.Flag;

                //buscar a bandeira
                string flagName = selectedCountry.Flag.Split('/')[4].Split('.')[0];

                //BitmapImage img = new BitmapImage();
                //img.BeginInit();

                //img.UriSource = new Uri(Environment.CurrentDirectory + "/Flags" + $"{flagName}.jpg");

                //img.EndInit();
                pictureBox1.ImageLocation = flagName;
            }
           
        }
    }
}
