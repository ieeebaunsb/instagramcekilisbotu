using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender,     EventArgs e)
        {

        }
        public IWebDriver driver { get; set; }
        public async void DriverBaslat(string proxy)
        {
            ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;
            ChromeOptions chromeOptions = new ChromeOptions();
            if (!string.IsNullOrEmpty(proxy))
            {
                chromeOptions.AddArgument("--proxy-server=" + proxy);
            }
            chromeOptions.AddExcludedArgument("enable-automation");
            chromeOptions.AddAdditionalCapability("useAutomationExtension", false);
            chromeOptions.AddArguments("--allow-file-access");
            chromeOptions.AddArgument("--disable-web-security");
            chromeOptions.AddArgument("--allow-running-insecure-content");
            driver = new ChromeDriver(chromeDriverService, chromeOptions, TimeSpan.FromMinutes(30.0));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(10);
            await Task.Delay(2000);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                try
                {
                    DriverBaslat("");
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(59);
                    driver.Navigate().GoToUrl(textBox1.Text);
                    var carpi = driver.FindElement(By.ClassName("xqRnw"));
                    carpi.Click();
                    IList<IWebElement> yorumlar = driver.FindElements(By.ClassName("gElp9"));


                    if (yorumlar.Count > 11)
                    {
                        string xpath = "//*[@id='react-root']/section/main/div/div[1]/article/div[3]/div[1]/ul/li/div/button";
                        var loadMorebtn = driver.FindElement(By.XPath(xpath));




                        while (loadMorebtn.Displayed)
                        {

                            loadMorebtn.Click();
                            try
                            {
                                loadMorebtn = driver.FindElement(By.XPath(xpath));
                            }
                            catch (Exception ex)
                            {

                                break;
                            }
                            Thread.Sleep(1000);


                        }
                    }





                     yorumlar = driver.FindElements(By.ClassName("gElp9"));

                    foreach (var item in yorumlar)
                    {
                        var container = item.FindElement(By.ClassName("C4VMK"));
                        var kullaniciAdi = container.FindElement(By.ClassName("_6lAjh")).Text;
                        listBox1.Items.Add(kullaniciAdi);
                    }
                    listBox1.Items.RemoveAt(0);
                    
                    driver.Dispose();
                    driver.Quit();
                    

                    int kazananKisiSayisi = Convert.ToInt32(comboBox1.SelectedItem);
                    int yedekKisiSayisi = Convert.ToInt32(comboBox2.SelectedItem);

                    for (int i = 0; i < kazananKisiSayisi; i++)
                    {
                        int cikanKisi = rnd.Next(0, listBox1.Items.Count);
                        listBox2.Items.Add(listBox1.Items[cikanKisi]);
                        listBox1.Items.RemoveAt(cikanKisi);

                    }
                    for (int i = 0; i < yedekKisiSayisi; i++)
                    {
                        int cikanKisi = rnd.Next(0, listBox1.Items.Count);
                        listBox3.Items.Add(listBox1.Items[cikanKisi]);
                        listBox1.Items.RemoveAt(cikanKisi);

                    }



                }
                catch (Exception x)
                {

                    MessageBox.Show(x.Message);
                }

            });

        }

        Random rnd;

        private void Form1_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            rnd = new Random();

        }

        public void DosyaKaydet(ListBox lst, string adi)
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\" + adi;
                using (StreamWriter sw= new StreamWriter(path))
                {
                    foreach(var item in lst.Items)
                    {
                        sw.WriteLine(item.ToString());
                    }
                    sw.Close();
                    MessageBox.Show("Masaüstüne \""+adi+" \" olarak kayıt edildi");

                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DosyaKaydet(listBox1, "Katılımcılar.txt");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DosyaKaydet(listBox2, "Kazananlar.txt");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DosyaKaydet(listBox3, "Yedekler.txt");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
