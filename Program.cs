using System;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Windows.Forms;
using Sierpinski_Triangle;
using System.Windows;

namespace Serpinski_Triangle
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// \\\
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form1 form = new Form1();
            form.Height = 500;
            form.Width = 500;
            PictureBox pictureBox = new PictureBox();
            pictureBox.Dock = DockStyle.Fill;
            form.Controls.Add(pictureBox);
            form.StartPosition = FormStartPosition.CenterScreen;
            Thread thread = new Thread(() => StartSimulation(pictureBox, form));
            thread.Priority = ThreadPriority.Highest;
            thread.Start();
            Application.Run(form);
        }
        static void StartSimulation(PictureBox pb, Form1 f1)
        {
            Bitmap bm = new Bitmap(f1.Width/2, f1.Width/2);
            Bitmap map = new Bitmap(f1.Width,f1.Width);
            Graphics g = Graphics.FromImage(bm);
            int iterations = 6;
            Color color = Color.White;

            for (int i = 0; i < bm.Height; i++)
            {
                for (int j = 0; j < bm.Width; j++)
                {
                    bm.SetPixel(i,j,Color.Black);
                }
            }
            
            GenerateTriangle(g, iterations, 1, (double)bm.Height - 2, (double)bm.Width / 2.0, 1, (double)bm.Width - 2, (double)bm.Height - 2, color);
            UpdateScreen(bm, map, pb, f1);
            f1.Height += 38;
            f1.Width += 15;
        }
        static void GenerateTriangle(Graphics g, int iterations, double x1, double y1, double x2, double y2, double x3, double y3, Color color)
        {
           
            if (iterations > 1)
            {
                double midx1 = (x1 + x2) / 2;
                double midy1 = (y1 + y2) / 2;
                double midx2 = (x2 + x3) / 2;
                double midy2 = (y2 + y3) / 2;
                double midx3 = (x3 + x1) / 2;
                double midy3 = (y3 + y1) / 2;
                GenerateTriangle(g, iterations - 1, x1, y1, midx1, midy1, midx3, midy3, color);
                GenerateTriangle(g, iterations - 1, midx1, midy1, x2, y2, midx2, midy2, color);
                GenerateTriangle(g, iterations - 1, midx3, midy3, midx2, midy2, x3, y3, color);
            }
            else
            {
                Pen pen = new Pen(new SolidBrush(color)); 
                g.DrawLine(pen, (float)x1, (float)y1, (float)x2, (float)y2); 
                g.DrawLine(pen, (float)x2, (float)y2, (float)x3, (float)y3); 
                g.DrawLine(pen, (float)x3, (float)y3, (float)x1, (float)y1);
            }
        }
        static void UpdateScreen(Bitmap matrix, Bitmap map, PictureBox box, Form1 form1)
        {
            int mapi = 0, mapj = 0;
            for (int i = 0; i < matrix.Height; i++)
            {
                for (int j = 0; j < matrix.Width; j++)
                {
                    Color color = matrix.GetPixel(i,j);

                    map.SetPixel(mapi,mapj,color);
                    map.SetPixel(mapi+1,mapj,color);
                    map.SetPixel(mapi,mapj+1,color);
                    map.SetPixel(mapi+1,mapj+1,color);

                    mapj += 2;
                    
                    if (mapj >= form1.Width)
                    {
                        mapi += 2;
                        mapj = 0;
                    }
                }
            }
            box.Image = map;
        }
    }
}