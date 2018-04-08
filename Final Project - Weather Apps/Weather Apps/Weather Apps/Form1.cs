using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net;
//using Newtonsoft.Json;
using System.Runtime.Serialization.Json;
using System.Text.RegularExpressions;




namespace Weather_Apps
{
    public partial class Form1 : Form
    {

        static string WeatherKey = "046a7bef335a47d2b62520e4ab36a89b";

        decimal[] temp = new decimal[8];
        decimal minTemp = 300M;
        decimal maxTemp = -300M;

        Boolean bCelsius = true;
       
        TextBox txtCity = new TextBox();
        
        RadioButton rbCelsius = new RadioButton();
        RadioButton rbFahrenheit = new RadioButton();

        Label labelCity = new Label();
        Label labelTemperature = new Label();

        // Forecast  *****************************************************************/

        Label labelTemperature0 = new Label();
        Label labelTemperature1 = new Label();
        Label labelTemperature2 = new Label();
        Label labelTemperature3 = new Label();
        Label labelTemperature4 = new Label();
        Label labelTemperature5 = new Label();
        Label labelTemperature6 = new Label();
        Label labelTemperature7 = new Label();


        Label lblTemperature0 = new Label();
        Label lblTemperature1 = new Label();
        Label lblTemperature2 = new Label();
        Label lblTemperature3 = new Label();
        Label lblTemperature4 = new Label();
        Label lblTemperature5 = new Label();
        Label lblTemperature6 = new Label();
        Label lblTemperature7 = new Label();

        Label labelMinMaxTemp = new Label();
        Label lblMinMaxTemp = new Label();

        /*****************************************************************************/

        Label labelPressure = new Label();
        Label labelSunrise = new Label();
        Label labelSunset = new Label();
        Label labelDescrip = new Label();
        Label labelHumidity = new Label();
        Label labelWind = new Label();
        Label labelCityName = new Label();

        Label lblTemperature = new Label();

        Label lblPressure = new Label();
        Label lblSunrise = new Label();
        Label lblDescrip = new Label();
        Label lblHumidity = new Label();
        Label lblSunset = new Label();
        Label lblWind = new Label();

        PictureBox pictureBox = new PictureBox();

        Button forecastBtn = new Button();

        DateTimePicker dtp = new DateTimePicker();
    

        public Form1()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            txtCity.Text = "";
            lblTemperature.Text = "";
            lblSunset.Text = "";
            lblSunrise.Text = "";
            lblPressure.Text = "";
            lblHumidity.Text = "";
            lblDescrip.Text = "";
            lblWind.Text = "";
            pictureBox.Visible = false;

            lblTemperature0.Text = "";
            lblTemperature1.Text = "";
            lblTemperature2.Text = "";
            lblTemperature3.Text = "";
            lblTemperature4.Text = "";
            lblTemperature5.Text = "";
            lblTemperature6.Text = "";
            lblTemperature7.Text = "";

            lblMinMaxTemp.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string line;
            Boolean bFound = false;

            string text = "\"" + txtCity.Text + "\"";
            string UpperFirstLetterCity = FormatWordsWithFirstCapital(text.ToLower());

            System.IO.StreamReader file =
            new System.IO.StreamReader("city.list.us.json");

            while ((line = file.ReadLine()) != null)
            {
                if (line.Contains(UpperFirstLetterCity))
                {
                    bFound = true;
                    try
                    {
                        string weatherRequest = CreateRequest(UpperFirstLetterCity);
                        Response weatherResponse = MakeRequest(weatherRequest);
                        ProcessResponse(weatherResponse);
                    }
                    catch (Exception excep)
                    {
                        Console.WriteLine(excep.Message);
                        Console.Read();
                    }

                    pictureBox.Visible = true;
                }
            }
            
            if (bFound == false )
                MessageBox.Show("Either entering City is not available or spelling is incorrect", "Warning Message");
            
        } // end of button

        public static string FormatWordsWithFirstCapital(string Phrase)
        {
            MatchCollection Matches = Regex.Matches(Phrase, "\\b\\w");
            Phrase = Phrase.ToLower();
            foreach (Match Match in Matches)
                Phrase = Phrase.Remove(Match.Index, 1).Insert(Match.Index, Match.Value.ToUpper());

            return Phrase;
        }

        public static string CreateRequest(string sCity)
        {
            string UrlRequest = "http://api.openweathermap.org/data/2.5/weather?q=" +
                                 sCity +
                                 ",us&appid=" 
                                 + WeatherKey;
            return (UrlRequest);
        }

        public static string CreateRequest5Days(string sCity)
        {
            string UrlRequest = "http://api.openweathermap.org/data/2.5/forecast?q=" +
                                 sCity +
                                 ",us&appid="
                                 + WeatherKey;
            return (UrlRequest);
        }

        private void ProcessResponse(Response weatherResponse)
        {
            string sTemp = weatherResponse.main.Temp.ToString();
            decimal dTemp = Decimal.Parse(sTemp);

                 
            if (rbCelsius.Checked)
            {
                dTemp = dTemp - 273.15M;
                lblTemperature.Text = dTemp.ToString("N1") + "°C";
            }
            else
            {
                dTemp = (dTemp * 9.0M/5.0M) - 459.67M;
                lblTemperature.Text = dTemp.ToString("N1") + "°F";
            }
           

            lblDescrip.Text = weatherResponse.Weather[0].description.ToString();
            string sPresure = weatherResponse.main.Pressure.ToString();
            decimal dPressure = Decimal.Parse(sPresure);

            dPressure = 0.02952998751M * dPressure;
            lblPressure.Text = String.Format("{0:0.##}",dPressure) + " in Hg";
            lblHumidity.Text = weatherResponse.main.Humidity.ToString() + " %";

            string sWind = weatherResponse.wind.Speed.ToString();
            decimal dWind = Decimal.Parse(sWind);
            dWind = (3600M / 1609.34M) * dWind;
            lblWind.Text = Math.Round(dWind).ToString() + " mph";

            string sSunrise = weatherResponse.sys.Sunrise.ToString();
            Double dSunrise = Double.Parse(sSunrise);
            DateTime dtSunrise = UnixTimeStampToDateTime(dSunrise);
            lblSunrise.Text = String.Format("{0:t}", dtSunrise);

            string sSunset = weatherResponse.sys.Sunset.ToString();
            Double dSunset = Double.Parse(sSunset);
            DateTime dtSunset = UnixTimeStampToDateTime(dSunset);
            lblSunset.Text = String.Format("{0:t}", dtSunset);

            string UrlIcon = "http://openweathermap.org/img/w/" +
                                 weatherResponse.Weather[0].icon +
                                 ".png";

            pictureBox.ImageLocation = UrlIcon;
            
            
        }

        private void ProcessResponse5Days(Response5Days weatherResponse)
        {

            string[] arrTemp = new string[40];
            decimal[] darrTemp = new decimal[40];

            minTemp = 300M;
            maxTemp = -300M;

            lblTemperature0.Text = weatherResponse.List[0].main1.Temp1.ToString();
            lblTemperature1.Text = weatherResponse.List[1].main1.Temp1.ToString();
            lblTemperature2.Text = weatherResponse.List[2].main1.Temp1.ToString();
            lblTemperature3.Text = weatherResponse.List[3].main1.Temp1.ToString();
            lblTemperature4.Text = weatherResponse.List[4].main1.Temp1.ToString();
            lblTemperature5.Text = weatherResponse.List[5].main1.Temp1.ToString();
            lblTemperature6.Text = weatherResponse.List[6].main1.Temp1.ToString();
            lblTemperature7.Text = weatherResponse.List[7].main1.Temp1.ToString();

            for (int i = 0; i < weatherResponse.List.Length; i++)
            {
                arrTemp[i] = weatherResponse.List[i].main1.Temp1.ToString();
                darrTemp[i] = Decimal.Parse(arrTemp[i]);
            }

            System.DateTime dt = dtp.Value;
            int diffDays = (int)Math.Round((dtp.Value - DateTime.Now).TotalDays);

            int offset = (diffDays - 1) * 8;

            if (rbCelsius.Checked)
            {
                for (int j = 0; j < 8; j++)
                {
                    darrTemp[offset + j] = darrTemp[offset + j] - 273.15M;
                    temp[j] = darrTemp[offset + j];
                    
                    if (minTemp > temp[j]) minTemp = temp[j];
                    if (maxTemp < temp[j]) maxTemp = temp[j];
                }
                   
                lblTemperature0.Text = darrTemp[offset].ToString("N1") + "°C";
                lblTemperature1.Text = darrTemp[offset + 1].ToString("N1") + "°C";
                lblTemperature2.Text = darrTemp[offset + 2].ToString("N1") + "°C";
                lblTemperature3.Text = darrTemp[offset + 3].ToString("N1") + "°C";
                lblTemperature4.Text = darrTemp[offset + 4].ToString("N1") + "°C";
                lblTemperature5.Text = darrTemp[offset + 5].ToString("N1") + "°C";
                lblTemperature6.Text = darrTemp[offset + 6].ToString("N1") + "°C";
                lblTemperature7.Text = darrTemp[offset + 7].ToString("N1") + "°C";
                lblMinMaxTemp.Text = minTemp.ToString("N1") + "°C" + " / " + maxTemp.ToString("N1") + "°C";
            }
            else
            {
                for (int j = 0; j < 8; j++)
                {
                    darrTemp[offset + j] = (darrTemp[offset + j] * 9.0M / 5.0M) - 459.67M;
                    temp[j] = darrTemp[offset + j];

                    if (minTemp > temp[j]) minTemp = temp[j];
                    if (maxTemp < temp[j]) maxTemp = temp[j];
                }
                    
                lblTemperature0.Text = darrTemp[offset].ToString("N1") + "°F";
                lblTemperature1.Text = darrTemp[offset + 1].ToString("N1") + "°F";
                lblTemperature2.Text = darrTemp[offset + 2].ToString("N1") + "°F";
                lblTemperature3.Text = darrTemp[offset + 3].ToString("N1") + "°F";
                lblTemperature4.Text = darrTemp[offset + 4].ToString("N1") + "°F";
                lblTemperature5.Text = darrTemp[offset + 5].ToString("N1") + "°F";
                lblTemperature6.Text = darrTemp[offset + 6].ToString("N1") + "°F";
                lblTemperature7.Text = darrTemp[offset + 7].ToString("N1") + "°F";
                lblMinMaxTemp.Text = minTemp.ToString("N1") + "°F" + " / " + maxTemp.ToString("N1") + "°F";
            }

        }

        public DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        private static Response MakeRequest(string requestUrl)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).",
                        response.StatusCode,
                        response.StatusDescription));
                    DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Response));
                    object objResponse = jsonSerializer.ReadObject(response.GetResponseStream());
                    Response jsonResponse
                    = objResponse as Response;
                    return jsonResponse;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

        }

        private static Response5Days MakeRequest5Days(string requestUrl)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).",
                        response.StatusCode,
                        response.StatusDescription));
                    DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Response5Days));
                    object objResponse = jsonSerializer.ReadObject(response.GetResponseStream());
                    Response5Days jsonResponse
                    = objResponse as Response5Days;
                    return jsonResponse;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            labelCity.Location = new Point(35, 39);
            labelCity.Size = new Size(33, 20);
            labelCity.Text = "City :";
            this.Controls.Add(labelCity);
            labelCity.Font = new Font("Microsoft Sans Serif",8.25f,FontStyle.Bold);

            txtCity.Location = new Point(75, 38);
            txtCity.Size = new Size(159, 20);
            this.Controls.Add(txtCity);
            txtCity.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);

            rbCelsius.Location = new Point(328, 37);
            rbCelsius.Size = new Size(65, 17);
            rbCelsius.Text = "Celsius";
            this.Controls.Add(rbCelsius);
            rbCelsius.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);
            rbCelsius.Click += new EventHandler(rbCelsius_Click);

            rbFahrenheit.Location = new Point(395, 37);
            rbFahrenheit.Size = new Size(85, 17);
            rbFahrenheit.Text = "Fahrenheit";
            this.Controls.Add(rbFahrenheit);
            rbFahrenheit.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);
            rbFahrenheit.Click += new EventHandler(rbFahrenheit_Click);

            pictureBox.Location = new Point(532, 155);
            pictureBox.Size = new Size(75, 65);
            pictureBox.BackgroundImageLayout = ImageLayout.Stretch;
            this.Controls.Add(pictureBox);

            labelTemperature.Location = new Point(35, 88);
            labelTemperature.Size = new Size(166, 23);
            labelTemperature.Text = "Temperature :";
            this.Controls.Add(labelTemperature);
            labelTemperature.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);

            lblTemperature.Location = new Point(75, 111);
            lblTemperature.Size = new Size(126, 29);
            this.Controls.Add(lblTemperature);
            lblTemperature.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);

            // Forecast ******************************************************************************/

            labelTemperature0.Location = new Point(35, 88);
            labelTemperature0.Size = new Size(166, 23);
            labelTemperature0.Text = "Temperature (0:00 AM)";
            this.Controls.Add(labelTemperature0);
            labelTemperature0.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);

            lblTemperature0.Location = new Point(75, 111);
            lblTemperature0.Size = new Size(126, 23);
            this.Controls.Add(lblTemperature0);
            lblTemperature0.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);

            labelTemperature1.Location = new Point(35, 134);
            labelTemperature1.Size = new Size(166, 23);
            labelTemperature1.Text = "Temperature (3:00 AM)";
            this.Controls.Add(labelTemperature1);
            labelTemperature1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);

            lblTemperature1.Location = new Point(75, 157);
            lblTemperature1.Size = new Size(126, 23);
            this.Controls.Add(lblTemperature1);
            lblTemperature1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);

            labelTemperature2.Location = new Point(35, 180);
            labelTemperature2.Size = new Size(166, 23);
            labelTemperature2.Text = "Temperature (6:00 AM)";
            this.Controls.Add(labelTemperature2);
            labelTemperature2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);

            lblTemperature2.Location = new Point(75, 203);
            lblTemperature2.Size = new Size(126, 23);
            this.Controls.Add(lblTemperature2);
            lblTemperature2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);

            labelTemperature3.Location = new Point(35, 226);
            labelTemperature3.Size = new Size(166, 23);
            labelTemperature3.Text = "Temperature (9:00 AM)";
            this.Controls.Add(labelTemperature3);
            labelTemperature3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);

            lblTemperature3.Location = new Point(75, 249);
            lblTemperature3.Size = new Size(126, 23);
            this.Controls.Add(lblTemperature3);
            lblTemperature3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);

            labelTemperature4.Location = new Point(220, 88);
            labelTemperature4.Size = new Size(166, 23);
            labelTemperature4.Text = "Temperature (12:00 PM)";
            this.Controls.Add(labelTemperature4);
            labelTemperature4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);

            lblTemperature4.Location = new Point(250, 111);
            lblTemperature4.Size = new Size(126, 23);
            this.Controls.Add(lblTemperature4);
            lblTemperature4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);

            labelTemperature5.Location = new Point(220, 134);
            labelTemperature5.Size = new Size(166, 23);
            labelTemperature5.Text = "Temperature (3:00 PM)";
            this.Controls.Add(labelTemperature5);
            labelTemperature5.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);

            lblTemperature5.Location = new Point(250, 157);
            lblTemperature5.Size = new Size(126, 23);
            this.Controls.Add(lblTemperature5);
            lblTemperature5.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);

            labelTemperature6.Location = new Point(220, 180);
            labelTemperature6.Size = new Size(166, 23);
            labelTemperature6.Text = "Temperature (6:00 PM)";
            this.Controls.Add(labelTemperature6);
            labelTemperature6.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);

            lblTemperature6.Location = new Point(250, 203);
            lblTemperature6.Size = new Size(126, 23);
            this.Controls.Add(lblTemperature6);
            lblTemperature6.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);

            labelTemperature7.Location = new Point(220, 226);
            labelTemperature7.Size = new Size(166, 23);
            labelTemperature7.Text = "Temperature (9:00 PM)";
            this.Controls.Add(labelTemperature7);
            labelTemperature7.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);

            lblTemperature7.Location = new Point(250, 249);
            lblTemperature7.Size = new Size(126, 23);
            this.Controls.Add(lblTemperature7);
            lblTemperature7.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);

            labelMinMaxTemp.Location = new Point(442, 153);
            labelMinMaxTemp.Size = new Size(166, 23);
            labelMinMaxTemp.Text = "Min / Max Temperature";
            this.Controls.Add(labelMinMaxTemp);
            labelMinMaxTemp.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);

            lblMinMaxTemp.Location = new Point(477, 176);
            lblMinMaxTemp.Size = new Size(126, 23);
            this.Controls.Add(lblMinMaxTemp);
            lblMinMaxTemp.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);

            
            /******************************************************************************************/
                   

            labelPressure.Location = new Point(235, 88);
            labelPressure.Size = new Size(119, 23);
            labelPressure.Text = "Pressure :";
            this.Controls.Add(labelPressure);
            labelPressure.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);

            lblPressure.Location = new Point(266, 111);
            lblPressure.Size = new Size(119, 23);
            this.Controls.Add(lblPressure);
            lblPressure.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);

            labelSunrise.Location = new Point(395, 88);
            labelSunrise.Size = new Size(103, 23);
            labelSunrise.Text = "Sunrise Time :";
            this.Controls.Add(labelSunrise);
            labelSunrise.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);

            lblSunrise.Location = new Point(420, 111);
            lblSunrise.Size = new Size(78, 23);
            this.Controls.Add(lblSunrise);
            lblSunrise.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);

            labelDescrip.Location = new Point(38, 161);
            labelDescrip.Size = new Size(119, 23);
            labelDescrip.Text = "Description :";
            this.Controls.Add(labelDescrip);
            labelDescrip.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);

            lblDescrip.Location = new Point(78, 188);
            lblDescrip.Size = new Size(85, 32);
            this.Controls.Add(lblDescrip);
            lblDescrip.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);

            labelHumidity.Location = new Point(238, 161);
            labelHumidity.Size = new Size(119, 23);
            labelHumidity.Text = "Humidity:";
            this.Controls.Add(labelHumidity);
            labelHumidity.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);

            lblHumidity.Location = new Point(269, 188);
            lblHumidity.Size = new Size(85, 32);
            this.Controls.Add(lblHumidity);
            lblHumidity.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);

            labelSunset.Location = new Point(398, 161);
            labelSunset.Size = new Size(100, 23);
            labelSunset.Text = "Sunset Time :";
            this.Controls.Add(labelSunset);
            labelSunset.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);

            lblSunset.Location = new Point(420, 184);
            lblSunset.Size = new Size(78, 23);
            this.Controls.Add(lblSunset);
            lblSunset.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);

            labelWind.Location = new Point(238, 230);
            labelWind.Size = new Size(100, 23);
            labelWind.Text = "Wind Speed :";
            this.Controls.Add(labelWind);
            labelWind.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);

            lblWind.Location = new Point(269, 253);
            lblWind.Size = new Size(78, 23);
            this.Controls.Add(lblWind);
            lblWind.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);

            forecastBtn.Location = new Point(129, 296);
            forecastBtn.Size = new Size(107, 37);
            forecastBtn.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular);
            forecastBtn.Text = "Forecast Weather Info";
            this.Controls.Add(forecastBtn);
            forecastBtn.Click += new EventHandler(forecastBtn_Click);

            dtp.Location = new Point(413, 88);
            dtp.MaxDate = DateTime.Now.AddDays(5);
            dtp.MinDate = DateTime.Now.AddDays(1);
            this.Controls.Add(dtp);

            rbCelsius.Checked = true;
            BackgroundImageLayout = ImageLayout.Stretch;

            labelCity.BackColor = Color.Transparent;
            labelCity.ForeColor = Color.White;
            labelTemperature.BackColor = Color.Transparent;
            labelTemperature.ForeColor = Color.White;
            labelDescrip.BackColor = Color.Transparent;
            labelDescrip.ForeColor = Color.White;
            labelPressure.BackColor = Color.Transparent;
            labelPressure.ForeColor = Color.White;
            labelHumidity.BackColor = Color.Transparent;
            labelHumidity.ForeColor = Color.White;
            labelSunrise.BackColor = Color.Transparent;
            labelSunrise.ForeColor = Color.White;
            labelSunset.BackColor = Color.Transparent;
            labelSunset.ForeColor = Color.White;
            labelWind.BackColor = Color.Transparent;
            labelWind.ForeColor = Color.White;
            lblGuide.BackColor = Color.Transparent;
            lblGuide.ForeColor = Color.White;
            

            rbCelsius.BackColor = Color.Transparent;
            rbCelsius.ForeColor = Color.White;
            rbFahrenheit.BackColor = Color.Transparent;
            rbFahrenheit.ForeColor = Color.White;

            lblTemperature.BackColor = Color.Transparent;
            lblTemperature.ForeColor = Color.White;
            lblPressure.BackColor = Color.Transparent;
            lblPressure.ForeColor = Color.White;
            lblSunrise.BackColor = Color.Transparent;
            lblSunrise.ForeColor = Color.White;
            lblSunset.BackColor = Color.Transparent;
            lblSunset.ForeColor = Color.White;
            lblDescrip.BackColor = Color.Transparent;
            lblDescrip.ForeColor = Color.White;
            lblHumidity.BackColor = Color.Transparent;
            lblHumidity.ForeColor = Color.White;
            lblWind.BackColor = Color.Transparent;
            lblWind.ForeColor = Color.White;
           
            pictureBox.BackColor = Color.Transparent;

            txtCity.Visible = false;
            labelCity.Visible = false;
            labelTemperature.Visible = false;
            labelDescrip.Visible = false;
            labelPressure.Visible = false;
            labelHumidity.Visible = false;
            labelSunrise.Visible = false;
            labelSunset.Visible = false;
            labelWind.Visible = false;

            lblTemperature.Visible = false;
            lblHumidity.Visible = false;
            lblDescrip.Visible = false;
            lblPressure.Visible = false;
            lblSunrise.Visible = false;
            lblSunset.Visible = false;

            rbCelsius.Visible = false;
            rbFahrenheit.Visible = false;
            button1.Visible = false;
            btnExit.Visible = false;

            forecastBtn.Visible = false;

            pictureBox.Visible = false;

            dtp.Visible = false;

            // Forecast ******************************************************************************/

            labelTemperature0.BackColor = Color.Transparent;
            labelTemperature0.ForeColor = Color.White;
            labelTemperature1.BackColor = Color.Transparent;
            labelTemperature1.ForeColor = Color.White;
            labelTemperature2.BackColor = Color.Transparent;
            labelTemperature2.ForeColor = Color.White;
            labelTemperature3.BackColor = Color.Transparent;
            labelTemperature3.ForeColor = Color.White;
            labelTemperature4.BackColor = Color.Transparent;
            labelTemperature4.ForeColor = Color.White;
            labelTemperature5.BackColor = Color.Transparent;
            labelTemperature5.ForeColor = Color.White;
            labelTemperature6.BackColor = Color.Transparent;
            labelTemperature6.ForeColor = Color.White;
            labelTemperature7.BackColor = Color.Transparent;
            labelTemperature7.ForeColor = Color.White;

            lblTemperature0.BackColor = Color.Transparent;
            lblTemperature0.ForeColor = Color.White;
            lblTemperature1.BackColor = Color.Transparent;
            lblTemperature1.ForeColor = Color.White;
            lblTemperature2.BackColor = Color.Transparent;
            lblTemperature2.ForeColor = Color.White;
            lblTemperature3.BackColor = Color.Transparent;
            lblTemperature3.ForeColor = Color.White;
            lblTemperature4.BackColor = Color.Transparent;
            lblTemperature4.ForeColor = Color.White;
            lblTemperature5.BackColor = Color.Transparent;
            lblTemperature5.ForeColor = Color.White;
            lblTemperature6.BackColor = Color.Transparent;
            lblTemperature6.ForeColor = Color.White;
            lblTemperature7.BackColor = Color.Transparent;
            lblTemperature7.ForeColor = Color.White;

            labelMinMaxTemp.BackColor = Color.Transparent;
            labelMinMaxTemp.ForeColor = Color.Red;
            lblMinMaxTemp.BackColor = Color.Transparent;
            lblMinMaxTemp.ForeColor = Color.Red;

            labelTemperature0.Visible = false;
            labelTemperature1.Visible = false;
            labelTemperature2.Visible = false;
            labelTemperature3.Visible = false;
            labelTemperature4.Visible = false;
            labelTemperature5.Visible = false;
            labelTemperature6.Visible = false;
            labelTemperature7.Visible = false;

            lblTemperature0.Visible = false;
            lblTemperature1.Visible = false;
            lblTemperature2.Visible = false;
            lblTemperature3.Visible = false;
            lblTemperature4.Visible = false;
            lblTemperature5.Visible = false;
            lblTemperature6.Visible = false;
            lblTemperature7.Visible = false;

            labelMinMaxTemp.Visible = false;
            lblMinMaxTemp.Visible = false;
            // ******************************************************************************/
        }


        private void forecastBtn_Click(object sender, EventArgs e)
        {
            string line;
            Boolean bFound = false;

            string text = "\"" + txtCity.Text + "\"";
            string UpperFirstLetterCity = FormatWordsWithFirstCapital(text.ToLower());

            System.IO.StreamReader file =
            new System.IO.StreamReader("city.list.us.json");

            while ((line = file.ReadLine()) != null)
            {
                if (line.Contains(UpperFirstLetterCity))
                {
                    bFound = true;

                    try
                    {
                        string weatherRequest = CreateRequest5Days(txtCity.Text);
                        Response5Days weatherResponse = MakeRequest5Days(weatherRequest);
                        ProcessResponse5Days(weatherResponse);
                    }
                    catch (Exception excep)
                    {
                        Console.WriteLine(excep.Message);
                        Console.Read();
                    }
                }
            }

            if (bFound == false)
                MessageBox.Show("Either entering City is not available or spelling is incorrect", "Warning Message");

        }

        private void rbCelsius_Click(object sender, EventArgs e)
        {
            if (bCelsius == false)
            {
                if (string.IsNullOrEmpty(lblTemperature.Text) == false)
                {
                    string sTemp = lblTemperature.Text.ToString();
                    sTemp = sTemp.Replace("°F", "");
                    Decimal fahrenheit = Decimal.Parse(sTemp);
                    Decimal celsius = (fahrenheit - 32) * (5M / 9M);
                    lblTemperature.Text = celsius.ToString("N1") + "°C";
                }

                for (int j = 0; j < 8; j++)
                    temp[j] = (temp[j] - 32) * (5M / 9M);

                minTemp = (minTemp - 32) * (5M / 9M);
                maxTemp = (maxTemp - 32) * (5M / 9M);

                lblTemperature0.Text = temp[0].ToString("N1") + "°C";
                lblTemperature1.Text = temp[1].ToString("N1") + "°C";
                lblTemperature2.Text = temp[2].ToString("N1") + "°C";
                lblTemperature3.Text = temp[3].ToString("N1") + "°C";
                lblTemperature4.Text = temp[4].ToString("N1") + "°C";
                lblTemperature5.Text = temp[5].ToString("N1") + "°C";
                lblTemperature6.Text = temp[6].ToString("N1") + "°C";
                lblTemperature7.Text = temp[7].ToString("N1") + "°C";
                lblMinMaxTemp.Text = minTemp.ToString("N1") + "°C" + " / " + maxTemp.ToString("N1") + "°C";

                bCelsius = true;
            }

        }  

        private void rbFahrenheit_Click(object sender, EventArgs e)
        {
            if (bCelsius == true)
            {
                if (string.IsNullOrEmpty(lblTemperature.Text) == false)
                {
                    string sTemp = lblTemperature.Text.ToString();
                    sTemp = sTemp.Replace("°C", "");
                    Decimal celsius = Decimal.Parse(sTemp);
                    Decimal fahrenheit = (celsius * (9M / 5M)) + 32;
                    lblTemperature.Text = fahrenheit.ToString("N1") + "°F";
                }

                for (int j = 0; j < 8; j++)
                    temp[j] = (temp[j] * (9M / 5M)) + 32;

                minTemp = (minTemp * (9M / 5M)) + 32;
                maxTemp = (maxTemp * (9M / 5M)) + 32;

                lblTemperature0.Text = temp[0].ToString("N1") + "°F";
                lblTemperature1.Text = temp[1].ToString("N1") + "°F";
                lblTemperature2.Text = temp[2].ToString("N1") + "°F";
                lblTemperature3.Text = temp[3].ToString("N1") + "°F";
                lblTemperature4.Text = temp[4].ToString("N1") + "°F";
                lblTemperature5.Text = temp[5].ToString("N1") + "°F";
                lblTemperature6.Text = temp[6].ToString("N1") + "°F";
                lblTemperature7.Text = temp[7].ToString("N1") + "°F";
                lblMinMaxTemp.Text = minTemp.ToString("N1") + "°F" + " / " + maxTemp.ToString("N1") + "°F";

                bCelsius = false;
            }
        

        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void currentWeatherToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            lblTemperature.Text = "";
            lblSunset.Text = "";
            lblSunrise.Text = "";
            lblPressure.Text = "";
            lblHumidity.Text = "";
            lblDescrip.Text = "";
            lblWind.Text = "";

            lblGuide.Visible = false;
            dtp.Visible = false;

            labelTemperature0.Visible = false;
            labelTemperature1.Visible = false;
            labelTemperature2.Visible = false;
            labelTemperature3.Visible = false;
            labelTemperature4.Visible = false;
            labelTemperature5.Visible = false;
            labelTemperature6.Visible = false;
            labelTemperature7.Visible = false;

            lblTemperature0.Visible = false;
            lblTemperature1.Visible = false;
            lblTemperature2.Visible = false;
            lblTemperature3.Visible = false;
            lblTemperature4.Visible = false;
            lblTemperature5.Visible = false;
            lblTemperature6.Visible = false;
            lblTemperature7.Visible = false;

            labelMinMaxTemp.Visible = false;
            lblMinMaxTemp.Visible = false;

            txtCity.Visible = true;
            labelCity.Visible = true;
            labelTemperature.Visible = true;
            labelPressure.Visible = true;
            labelDescrip.Visible = true;
            labelHumidity.Visible = true;
            labelSunrise.Visible = true;
            labelSunset.Visible = true;
            labelWind.Visible = true;

            lblTemperature.Visible = true;
            lblHumidity.Visible = true;
            lblDescrip.Visible = true;
            lblPressure.Visible = true;
            lblSunrise.Visible = true;
            lblSunset.Visible = true;

            rbCelsius.Visible = true;
            rbFahrenheit.Visible = true;
            button1.Visible = true;
            btnExit.Visible = true;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About aboutForm = new About();
            aboutForm.Show();
        }

        private void foreCastToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lblTemperature0.Text = "";
            lblTemperature1.Text = "";
            lblTemperature2.Text = "";
            lblTemperature3.Text = "";
            lblTemperature4.Text = "";
            lblTemperature5.Text = "";
            lblTemperature6.Text = "";
            lblTemperature7.Text = "";
            lblMinMaxTemp.Text = "";

            labelTemperature.Visible = false;
            labelPressure.Visible = false;
            labelDescrip.Visible = false;
            labelHumidity.Visible = false;
            labelSunrise.Visible = false;
            labelSunset.Visible = false;
            labelWind.Visible = false;
            pictureBox.Visible = false;
            lblGuide.Visible = false;
            button1.Visible = false;

            lblTemperature.Visible = false;
            lblHumidity.Visible = false;
            lblDescrip.Visible = false;
            lblPressure.Visible = false;
            lblSunrise.Visible = false;
            lblSunset.Visible = false;

            dtp.Visible = true;
            txtCity.Visible = true;
            labelCity.Visible = true;

            labelTemperature0.Visible = true;
            labelTemperature1.Visible = true;
            labelTemperature2.Visible = true;
            labelTemperature3.Visible = true;
            labelTemperature4.Visible = true;
            labelTemperature5.Visible = true; 
            labelTemperature6.Visible = true;
            labelTemperature7.Visible = true;

            lblTemperature0.Visible = true;
            lblTemperature1.Visible = true;
            lblTemperature2.Visible = true;
            lblTemperature3.Visible = true;
            lblTemperature4.Visible = true;
            lblTemperature5.Visible = true;
            lblTemperature6.Visible = true;
            lblTemperature7.Visible = true;

            labelMinMaxTemp.Visible = true;
            lblMinMaxTemp.Visible = true;

            rbCelsius.Visible = true;
            rbFahrenheit.Visible = true;
            btnExit.Visible = true;

            forecastBtn.Visible = true;
        }

    }
}
