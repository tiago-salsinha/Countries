using Countries.Modelos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

            var countries = JsonConvert.DeserializeObject<List<RegionalBloc>>(result); 

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

            RegionalBloc selectedCountry = (RegionalBloc)listBoxCountries.SelectedItem;

            if (selectedCountry != null)
            {
                lblNome.Text = selectedCountry.Name;
            }
        }
    }
}
