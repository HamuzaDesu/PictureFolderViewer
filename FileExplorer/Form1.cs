using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace FileExplorer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Debug.WriteLine("START");
            PopulateWithPictures();

            timer1.Tick += new EventHandler(HandleOffload);
            timer1.Interval = 500;
            timer1.Start();

        }

        void PopulateWithPictures()
        {
            DirectoryInfo rootPath = new DirectoryInfo(@"W:");

            List<String> pictures = GetPictures(rootPath);

            foreach(String imagePath in pictures)
            {
                PictureBox pictureBox = new PictureBox();

                SetPictureBoxInfo(imagePath, pictureBox);

                flowLayoutPanel1.Controls.Add(pictureBox);
            }
        }

        void SetPictureBoxInfo(string path, PictureBox pictureBoxControl)
        {
            pictureBoxControl.ImageLocation = path;

            pictureBoxControl.ClientSize = new Size(200, 200);
            pictureBoxControl.SizeMode = PictureBoxSizeMode.Normal;
            pictureBoxControl.Tag = path;

            pictureBoxControl.MouseHover += (sender, args) =>
            { 
                ToolTip tt = new ToolTip();
                IWin32Window win = this;
                tt.Show(Path.GetFileName(path), win, MousePosition, 1000);
            };
        }

        List<String> GetPictures(DirectoryInfo rootPath)
        {
            List<String> imagePaths = new List<String>();

            foreach (FileInfo file in rootPath.GetFiles())
            {
                string fileExtension = file.Extension;
                if (fileExtension == ".png" || fileExtension == ".jpg" || fileExtension == ".jfif")
                {
                    imagePaths.Add(file.FullName);
                }
                Debug.WriteLine(file);
            }
            foreach (DirectoryInfo directory in rootPath.GetDirectories())
            {
                List<String> subImagePaths = GetPictures(directory);
                foreach (String item in subImagePaths)
                {
                    imagePaths.Add(item);
                }
            }

            return imagePaths;
        }

        void HandleOffload(Object myObject, EventArgs myEventArgs)
        {
            int active = 0;

            foreach (PictureBox control in flowLayoutPanel1.Controls)
            {
                if (isControlVisible(control))
                {
                    //control.BackColor = Color.Green;
                    control.ImageLocation = control.Tag.ToString();
                    active++;
                }
                else
                {
                    //control.BackColor = Color.Red;
                    control.ImageLocation = null;
                }
            }
            Debug.WriteLine(active);
        }

        bool isControlVisible(Control control)
        {
            Rectangle rect = this.DesktopBounds;

            if (rect.Top - 200 > control.Top)
                return false;

            if (rect.Bottom + 200 < control.Bottom)
                return false;
            return true;
        }
    }
}
