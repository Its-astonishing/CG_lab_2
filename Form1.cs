using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CG_lab_2
{
    public partial class Form1 : Form
    {
        Bin bin;
        View view;
        bool loaded = false;
        bool needReload = false;
        bool textureUsed = false;
        bool stripUsed = false;
        int currentLayer = 0;
        int FrameCount = 0;
        int min;
        int tfWidth;
        DateTime NextFPSUpdate = DateTime.Now.AddSeconds(1);
        public Form1()
        {
            InitializeComponent();
            bin = new CG_lab_2.Bin();
            view = new CG_lab_2.View();
            view.min = 0;
            view.max = 1;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog file_dialog = new OpenFileDialog();

            if (DialogResult.OK == file_dialog.ShowDialog())
            {
                string file_name = file_dialog.FileName;
                bin.readBIN(file_name);
                view.SetupView(bin, glControl1.Width, glControl1.Height);
                loaded = true;
                glControl1.Invalidate();
                trackBar1.Maximum = bin.z - 1;
            }
        }

        void displayFPS()
        {
            if (DateTime.Now >= NextFPSUpdate)
            {
                this.Text = String.Format("CT Visualizer (fps={0})", FrameCount);
                NextFPSUpdate = DateTime.Now.AddSeconds(1);
                FrameCount = 0;
            }

            FrameCount++;
        }

        void Application_Idle(object sender, EventArgs e)
        {
            while (glControl1.IsIdle)
            {
                displayFPS();
                glControl1.Invalidate();
            }
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (loaded)
            {
                if (needReload && textureUsed)
                {
                    view.generateTextureImage(bin, currentLayer);
                    view.Load2DTexture();
                    needReload = false;
                }
                else
                    if (stripUsed)
                        view.DrawQuadStrips(bin, currentLayer);
                    else
                        view.DrawQuads(bin, currentLayer);
                glControl1.SwapBuffers();
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            currentLayer = trackBar1.Value;
            needReload = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Application.Idle += Application_Idle;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (true == checkBox1.Checked)
            {
                textureUsed = true;
                checkBox2.Checked = false;
                stripUsed = false;
            } else
            {
                textureUsed = false;
            }
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            view.min = trackBar2.Value;
            needReload = true;
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            view.max = trackBar3.Value + view.min;
            needReload = true;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (true == checkBox2.Checked)
            {
                stripUsed = true;
                checkBox1.Checked = false;
                textureUsed = false;
            } else
            {
                stripUsed = false;
            }
        }
    }
}
