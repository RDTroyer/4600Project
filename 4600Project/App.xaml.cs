using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace _4600Project
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MessageBox.Show("Start up");
            List<TwitterCredentials> twitterCredsList = LoadTwitterCredentialsList();
            MessageBox.Show("Twitter Cred List Created");
            if (twitterCredsList.Count == 0)
            {
                MessageBox.Show("Cannot find Twitter credential files.", "Error");
                Shutdown();
                return;
            }
            MessageBox.Show("Connected to Twitter Successfully");

        }
        private List<TwitterCredentials> LoadTwitterCredentialsList()
        {
            var result = new List<TwitterCredentials>();
            int bigger = 1;
            FileInfo[] fileInfos = new DirectoryInfo(System.AppDomain.CurrentDomain.BaseDirectory)
                                            .GetFiles("*_Creds.json");
            MessageBox.Show("New FileInfos from creds json");
            if (fileInfos.Count() > 0)
            {
                bigger = 2;
                foreach (var fileInfo in fileInfos)
                {
                    TwitterCredentials twitterCreds = null;
                    if (File.Exists(fileInfo.FullName))
                    {
                        try
                        {
                            twitterCreds = JsonHelper.DeserializeFromFile<TwitterCredentials>(fileInfo.FullName);
                            if (twitterCreds != null && !result.Any(x => x.ScreenName == twitterCreds.ScreenName))
                            {
                                result.Add(twitterCreds);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            MessageBox.Show(Convert.ToString(bigger));
            return result;
        }
    }
}
