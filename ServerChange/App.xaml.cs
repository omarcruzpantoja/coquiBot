using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ServerChange
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        
        void startup(object sender, StartupEventArgs e)
        {
          
            MainWindow main = new MainWindow();
            main.Show();


            if (!verify())
            {
                if(install())
                    main.clientPath.Opacity = 0;
            }   
        }
        
        bool install()
        {
            string path = "";
            while (true)
            {
                path = findDirectory();
                
                if (path == "")
                    return false;
                else if (path != "ERROR")
                {
                    break;
                }
            }

            //Get the old file
            string path2 = System.Environment.CurrentDirectory.Replace("\\", "/") + "/.config";
            string[] lines = File.ReadAllLines(path2);
            
            //Rewrite the file 
            using (StreamWriter writer = new StreamWriter(path2))
            {
                for (int currentLine = 1; currentLine <= lines.Length; ++currentLine)
                {
                    if (currentLine == 1)
                    {
                        writer.WriteLine("client=\"" + path+ "\"");
                    }
                    else
                    {
                        writer.WriteLine(lines[currentLine - 1]);
                    }
                }
            }
            return true;
        }

        //Return true if client information has been set, else false
        bool verify()
        {
            //  string path = System.Environment.CurrentDirectory.Replace("\\", "/") + "/.config";
            string path = getLocal();
            if (File.Exists(path)) 
                return true;
            return false;
        }
        
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

                    return "ERROR";
                
            }
        }

        string getLocal()
        {
            string path = System.Environment.CurrentDirectory.Replace("\\", "/") + "/.config";
            //string path = "C:/CoquiBot/.config";
            string[] lines = File.ReadAllLines(path);
            string[] linexline = lines[0].Split('=');

            return linexline[1].Replace("\"", "");
        }
    }



}

