using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ServerChange
{
    /// <summary>
    /// Interaction logic for userInput.xaml
    /// </summary>
    public partial class userInput : Window
    {
        private int server;
        public userInput(int server)
        {
            InitializeComponent();
            
            this.server = server;

            string path = System.Environment.CurrentDirectory.Replace("\\", "/") + "/.config";
            //string path = "C:/CoquiBot/.config";
            string[] lines = File.ReadAllLines(path);
            string[] linexline = lines[server].Split('=');
            if (linexline[1] != "")
                setName.Text = linexline[1].Replace("\"", "") ;

        }

        private void setName_GotFocus(object sender, RoutedEventArgs e)
        {
            if (setName.Text == "Username:")
            {
                setName.Text = "";
                setName.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void setName_LostFocus(object sender, RoutedEventArgs e)
        {
            if(setName.Text == "")
            {
                setName.Text = "Username:";
                setName.Foreground = new SolidColorBrush(Colors.LightGray);
            }
        }

        private void okBut_Click(object sender, RoutedEventArgs e)
        {
            string info;
            if (server == 1)
            {
                info = "nauser=\"";
                server = 2;
            }
            else
            {
                info = "lanuser=\"";
                server = 4;
            }


            string username = setName.Text;
            //string path = "C:/CoquiBot/.config";
            string path = System.Environment.CurrentDirectory.Replace("\\", "/") + "/.config";

            //Get the old file
            string[] lines = File.ReadAllLines(path);

            //Rewrite the file with new region 
            using (StreamWriter writer = new StreamWriter(path))
            {
                for (int currentLine = 1; currentLine <= lines.Length; ++currentLine)
                {
                    if (currentLine == server)
                    {
                        writer.WriteLine(info+username+"\"");
                    }
                    else
                    {
                        writer.WriteLine(lines[currentLine - 1]);
                    }
                }
                writer.Close();
            }
            this.Close();
        }

        private void cancelBut_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
