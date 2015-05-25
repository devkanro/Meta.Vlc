using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xZune.Vlc.WinForm.Sample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (vlcPlayer1.VlcMediaPlayer.Media == null)
            {
                vlcPlayer1.LoadMedia(new Uri("http://download.blender.org/peach/bigbuckbunny_movies/big_buck_bunny_480p_surround-fix.avi"));
                vlcPlayer1.Play();
            }
            else
            {
                vlcPlayer1.Play();
            }
        }
    }
}
