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
        public static TweetCompiler TweetCompiler { get; private set; }
        protected override void OnStartup(StartupEventArgs e)
        {

            base.OnStartup(e);
            List<TwitterCredentials> twitterCredsList = LoadTwitterCredentialsList();
            if (twitterCredsList.Count == 0)
            {
                MessageBox.Show("Cannot find Twitter credential files.", "Error");
                Shutdown();
                return;
            }
            TweetCompiler = new TweetCompiler(twitterCredsList);
            TweetCompiler.CreateTweetModelList(TweetCompiler._friendsList);

        }
        private List<TwitterCredentials> LoadTwitterCredentialsList()
        {
            var result = new List<TwitterCredentials>();
            FileInfo[] fileInfos = new DirectoryInfo(System.AppDomain.CurrentDomain.BaseDirectory)
                                            .GetFiles("*_Creds.json");
            if (fileInfos.Count() > 0)
            {
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
            return result;
        }
    }
}
