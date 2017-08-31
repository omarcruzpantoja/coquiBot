using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;


namespace ServerChange
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //Verify if client is correctly set up
            verifyClient();
            //Check buttons 
            verifyButtons();
        }


        //*** VERIFYING DATA FUNCTIONS *** //
        //*** START ***//
        
        //Function to check if given directory actually manages to access the settings 
        //If so, remove the textbox 
        private bool verifyClient()
        {
            //string path = System.Environment.CurrentDirectory.Replace("\\", "/") + "/.config";
            string path =  getLocal(0) ;
            if (File.Exists(path))
            {
                clientPath.Opacity = 0;
                return true;
            }
            else
            {
                clientPath.Opacity = 100;
                return false;
            }

           
        }

        private void verifyButtons()
        {
            //Check NA button
            string check = getLocal(2);
            if (check == "true")
                naCheckbox.IsChecked = true;

            check = getLocal(4);
            if (check == "true")
                lanCheckbox.IsChecked = true;

        }

        //*** END ***//
        //*** VERIFYING DATA FUNCTIONS ***//

        //*** CLICK FUNCTIONS ***//
        //*** START ***//

        //Change Region from NA to LAN
        private void LAN_Click(object sender, RoutedEventArgs e)
        {
            if (verifyClient())
                change("LA1");
            else
            {
                System.Windows.Forms.MessageBox.Show("ERROR: Incorrect client directory, please make sure client directory is up to date.");
            }
        }

        //Change Region from LAN to NA
        private void NA_Click(object sender, RoutedEventArgs e)
        {
            if (verifyClient())
                change("NA");
            else
                System.Windows.Forms.MessageBox.Show("ERROR: Incorrect client directory, please make sure client directory is up to date.");
        }
        private void locateClient_Click(object sender, RoutedEventArgs e)
        {
            //Stay in loop until person actually finds the client or cancels search 
            string path = "";
            while (true)
            {
                path = findDirectory();
                if (path == "")
                {
                    verifyClient();
                    return;
                }
                else if (path != "ERROR")
                {
                    break;
                }
            }

            string path2 = System.Environment.CurrentDirectory.Replace("\\", "/") + "/.config";
            string[] lines = File.ReadAllLines(path2);
            //Rewrite the file 
            using (StreamWriter writer = new StreamWriter(path2))
            {
                for (int currentLine = 1; currentLine <= lines.Length; ++currentLine)
                {
                    if (currentLine == 1)
                    {
                        writer.WriteLine("client=\"" + path + "\"");
                    }
                    else
                    {
                        writer.WriteLine(lines[currentLine - 1]);
                    }
                }
            }
            verifyClient();
        }
        private void naAcc_Click(object sender, RoutedEventArgs e)
        {
            userInput add = new userInput(1);
            add.ShowDialog() ;
        }

        private void lanAcc_Click(object sender, RoutedEventArgs e)
        {
            userInput add = new userInput(3);
            add.ShowDialog();
        }

        private void modifyCheck(int line, string value)
        {
            string path = System.Environment.CurrentDirectory.Replace("\\", "/") + "/.config";
            //string path = "C:/CoquiBot/.config";
            string[] lines = File.ReadAllLines(path);
            //Rewrite the file 
            string server;
            if (line == 3)
                server = "nacheck";
            else
                server = "lancheck";
            using (StreamWriter writer = new StreamWriter(path))
            {
                for (int currentLine = 1; currentLine <= lines.Length; ++currentLine)
                {
                    if (currentLine == line)
                    {
                        writer.WriteLine(server + "=\"" + value + "\"");
                    }
                    else
                    {
                        writer.WriteLine(lines[currentLine - 1]);
                    }
                }
            }
        }

        //*** END ***//
        //*** CLICK FUNCTIONS ***//


        //*** MISCELANIOUS FUNCTIONS***///
        //*** START ***///
        private void naCheckboxChecked(object sender, RoutedEventArgs e)
        {
            int pos = 2;
            string check = getLocal(pos);
            if (check == "true")
                modifyCheck(pos + 1, "false");
            else
                modifyCheck(pos + 1, "true");

        }

        private void lanCheckboxChecked(object sender, RoutedEventArgs e)
        {
            int pos = 4;
            string check = getLocal(pos);
            if (check == "true")
                modifyCheck(pos + 1, "false");
            else
                modifyCheck(pos + 1, "true");
        }
        //Function to change the regions, parameter sent will be the new region 
        private void change(string server)
        {
            int lineToChange = 1;
            string path = getLocal(0);
            int pos;
            if (server == "NA")
                pos = 2;
            else
                pos = 4;
            string newServer = "        region: " + '"' + server + '"';
            //Get the line to be modified
            using (var reader = new StreamReader(path))
            {
                byte[] b = new byte[1024];
                UTF8Encoding temp = new UTF8Encoding(true);

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line.StartsWith("        region:"))
                        break;
                    lineToChange++;
                }

            }

            int lineToChange2 = 1;
            using (var reader = new StreamReader(path))
            {
                byte[] b = new byte[1024];
                UTF8Encoding temp = new UTF8Encoding(true);

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line.StartsWith("        username:"))
                        break;
                    lineToChange2++;
                }

            }
            //Get the old file
            string[] lines = File.ReadAllLines(path);
           
            //Rewrite the file with new region 
            using (StreamWriter writer = new StreamWriter(path))
            {
                for (int currentLine = 1; currentLine <= lines.Length; ++currentLine)
                {

                    if (currentLine == lineToChange)
                    {
                        writer.WriteLine(newServer);
                    }
                    else if(currentLine == lineToChange2 && getLocal(pos) == "true")
                    {
                        writer.WriteLine("        username: \"" + getLocal(pos - 1) + "\"");
                    }
                    else
                    {
                        writer.WriteLine(lines[currentLine - 1]);
                    }
                }
                writer.Close();
            }

            //Display box saying change has been completed
            System.Windows.Forms.MessageBox.Show("Successfully changed server to : " + server, "ChangeSuccessful");
        }

        //Find the directory using the search button 
        string findDirectory()
        {
            using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
            {
                fbd.Description = "Please locate League of Legends Folder";
                System.Windows.Forms.DialogResult result = fbd.ShowDialog();
                string path = fbd.SelectedPath.Replace("\\", "/") + "/Config/LeagueClientSettings.yaml";
                if (File.Exists(path))
                    return path;
                else if (result == System.Windows.Forms.DialogResult.Cancel)
                    return "";
                else
                {

                    return "ERROR";
                }
            }
        }

        //Function to retrieve config information 
        private string getLocal(int line)
        {
            string path = System.Environment.CurrentDirectory.Replace("\\", "/") + "/.config";
            //string path = "C:/CoquiBot/.config";
            string[] lines = File.ReadAllLines(path);
            string[] linexline = lines[line].Split('=');

            return linexline[1].Replace("\"", "");
        }
        //*** END ***///
        //*** MISCELANIOUS FUNCTIONS***///

    }
}
