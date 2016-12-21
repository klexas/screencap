using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpAvi;

namespace screencap
{
    public partial class Form1 : Form
    {
        private Recorder recording { get; set; }
        public Form1()
        {
            InitializeComponent();
            // INIT recorder
            recording = new Recorder();
        }
        Bitmap bmp;
        Graphics gr;

        private void btn_start_Click(object sender, EventArgs e)
        {
            // Early out. 
            if (string.IsNullOrEmpty(txt_name.Text) || string.IsNullOrEmpty(txt_name.Text))
            {
                MessageBox.Show("Please enter name and location for the ouput. ");
                txt_name.Focus();
                return;
            }
            // Clean the filename if needed : 
            if (!txt_name.Text.Contains(".avi"))
            {
                txt_name.Text = txt_name.Text + ".avi";
            }
            // Build recorder 
            recording.BuildRecorder(txt_name.Text, txt_location.Text, Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height, num_framerate.Value, num_quality.Value);
            // Start recording
            recording.StartRecording();
            //Update status 
            lbl_status.Text = "Recording...";
            // Start Timer
            timer1.Start();
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            btn_save.Enabled = true;
            recording.StopRecording();
            timer1.Stop();
            lbl_status.Text = "Recording Stopped...";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                bmp = new Bitmap(Screen.PrimaryScreen.WorkingArea.Width,
                             Screen.PrimaryScreen.WorkingArea.Height);
                gr = Graphics.FromImage(bmp);
                gr.CopyFromScreen(0, 0, 0, 0, new Size(bmp.Width, bmp.Height));
                pictureBox1.Image = bmp;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                // Now save the image to the recorder 
                recording.AddToStream(bmp);
                // Dispose of frame. 
                //bmp.Dispose();
            }
            catch (ArgumentException ex)
            {
                // We know this error is expected.. 
                //throw;
            }
        }

        private void btn_location_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fd = new FolderBrowserDialog();
            var fdres = fd.ShowDialog();
            if (fdres == DialogResult.OK)
            {
                //TODO: Do we need to check if folder exists ?
                txt_location.Text = fd.SelectedPath;
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            // Early out.
            if (this.recording == null)
            {
                MessageBox.Show("Nothing to save. ");
                return;
            }
            if (recording.SaveRecording())
            {
                MessageBox.Show("Recording has been saved!");
            }
            else
            {
                MessageBox.Show("Recording could not be saved. ");
            }
            this.btn_save.Enabled = false;
            lbl_status.Text = "Ready...";
            // Here we save her up, try at least. 

        }

        private void btn_debug_Click(object sender, EventArgs e)
        {
            //var rec = new RecorderT(txt_name.Text, KnownFourCCs.Codecs.Uncompressed, 600);
            
        }
    }

}
