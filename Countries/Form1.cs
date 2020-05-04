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
        public Form1()
        {
            InitializeComponent();

            LoadRates();
        }

        private async void LoadRates()
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

            var rates = JsonConvert.DeserializeObject<List<Country>>(result); 

            comboBox1.DataSource = rates;
            comboBox1.DisplayMember = "Name";//controla o que que propriedade da class é disposto na comboBox, também podes controlar isto com o override toString() na Class Rate

            //corrige bug da microsoft
            //comBoxDestino.BindingContext = new BindingContext();//liga os objectos do interface ao código

            //comBoxDestino.DataSource = Rates;
            //comBoxDestino.DisplayMember = "name";

            //progressBar1.Value = 100;
        }
    }
}
