﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using System.Threading;

namespace OpenGLSample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Vector2 VCP_MouseDownPosition;
        Vector2 WCP_CameraPosition = new Vector2(0, 0);
        public float Scale_FtoW = 1;


        public List<Point> Triangle_List = new List<Point>(6);
        public List<Point> Ractangle_List = new List<Point>(8);

        public Point Line1Point = new Point(250, 0);
        public Point Line2Point = new Point(350, 0);
        public Point Line3Point = new Point(350, 50);
        private Bitmap img;
        private int Objectidx = 10;

        public delegate void DPaint();
        
        private void glc_Main_Paint(object sender, PaintEventArgs e)
        {
            if(0 == Triangle_List.Count)
            {
                Triangle_List.Add(new Point(0, 0));
                Triangle_List.Add(new Point(100, 0));
                Triangle_List.Add(new Point(100, 50));

                Triangle_List.Add(new Point(20, 30));
                Triangle_List.Add(new Point(120, 30));
                Triangle_List.Add(new Point(120, 80));
            }
            RePaint();
        }

        public void RePaint()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.InitNames();
            for (int i = 0; i < (Triangle_List.Count / 3); i++ )
            {
                GL.PushName(Objectidx++);
                GL.Color3(Color.Yellow);
                GL.Begin(BeginMode.Triangles);
                GL.Vertex2(Triangle_List[(3 * i) + 0].X, Triangle_List[(3 * i) + 0].Y);
                GL.Vertex2(Triangle_List[(3 * i) + 1].X, Triangle_List[(3 * i) + 1].Y);
                GL.Vertex2(Triangle_List[(3 * i) + 2].X, Triangle_List[(3 * i) + 2].Y);
                GL.End();
                GL.PopName();
            }
            for (int i = 0; i < (Ractangle_List.Count / 4); i++)
            {
                GL.Color3(Color.Red);
                GL.Begin(BeginMode.Polygon);
                GL.Vertex2(Ractangle_List[(4 * i) + 0].X, Ractangle_List[(4 * i) + 0].Y);
                GL.Vertex2(Ractangle_List[(4 * i) + 1].X, Ractangle_List[(4 * i) + 1].Y);
                GL.Vertex2(Ractangle_List[(4 * i) + 2].X, Ractangle_List[(4 * i) + 2].Y);
                GL.Vertex2(Ractangle_List[(4 * i) + 3].X, Ractangle_List[(4 * i) + 3].Y);
                GL.End();
            }
            UpdateImage();
            OpenTK.Graphics.GraphicsContext.CurrentContext.SwapBuffers();
        }
        private void ReDraw()
        {
            glc_Main.Invalidate();
        }

        private void UpdateImage()
        {
            img = new Bitmap(glc_Main.Width, glc_Main.Height);
            System.Drawing.Imaging.BitmapData data = img.LockBits(glc_Main.ClientRectangle, System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            GL.ReadPixels(0, 0, glc_Main.Width, glc_Main.Height, PixelFormat.Bgr, PixelType.UnsignedByte, data.Scan0);
            img.UnlockBits(data);
            //img.RotateFlip(RotateFlipType.RotateNoneFlipY);

            img.Save(@"d:\img.bmp");
        }
        public Color getColor(Point pt)
        {
            return img.GetPixel(pt.X,pt.Y);
        }
        public Color getColor(int X, int Y)
        {
            Point XY = new Point(X, Y);
            return getColor(XY);
        }

        private void glc_Main_Load(object sender, EventArgs e)
        {
            GL.ClearColor(Color.Blue);
            UpdateProjection();
        }
        public Vector2 TransForm_VCPtoWCP(Vector2 VCP)
        {
            return VCP / Scale_FtoW;
        }
        private Vector2 TransForm_WCPtoVCP(Vector2 WCP)
        {
            return WCP * Scale_FtoW;
        }
        private Vector2 Transform_SCPtoVCP(Vector2 SCP)
        {
            return new Vector2(SCP.X, glc_Main.Height - SCP.Y);
        }

        public void Pan_Move(Vector2 VS ,Vector2 VE)
        {
            Vector2 T_FES = VE - VS;
            Vector2 w = TransForm_VCPtoWCP(T_FES);
            WCP_CameraPosition -= w;
            UpdateProjection();
        }
        public void UpdateProjection()
        {
            GL.ClearColor(Color.Blue);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            
            GL.Ortho(0, glc_Main.Width / Scale_FtoW, 0, glc_Main.Height / Scale_FtoW, -100, 100); // Bottom-left corner pixel has coordinate (0, 0)      안원근법
            {
                Vector3 V3Eye = new Vector3(WCP_CameraPosition);
                V3Eye.Z = 100;
                Vector3 V3Target = new Vector3(WCP_CameraPosition);
                V3Target.Z = -1;
                Vector3 V3Up = Vector3.UnitY;

                Matrix4 m4 = Matrix4.LookAt(V3Eye, V3Target, V3Up);

                GL.MultMatrix(ref m4);
            }
            GL.Viewport(0, 0, glc_Main.Width, glc_Main.Height);
            ReDraw();
        }

        public void View_Reset()
        {
            Scale_FtoW = 1;
            WCP_CameraPosition = new Vector2(0, 0);
        }
        

        public int Mode_Select(Vector2 VCP_MouseClickedPosition)
        {
            int[] SelectBuffer = new int[64];
            GL.SelectBuffer(64, SelectBuffer);
            GL.RenderMode(RenderingMode.Select);

            // 확대 (scale 계산)
            Move_PixelZoom(VCP_MouseClickedPosition);
            RePaint();
            int hits = GL.RenderMode(RenderingMode.Render);
            Console.WriteLine("Hit: " + hits);
            // 원복 (추후 구현)

            return hits;
        }


        private bool isMouseRightDown = false;
        private void glControl_Main_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                isMouseRightDown = true;
                VCP_MouseDownPosition = Transform_SCPtoVCP(new Vector2(e.X, e.Y));
            }
        }

        private void glControl_Main_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                isMouseRightDown = false;
                Vector2 VCP_MouseUpPosition = Transform_SCPtoVCP(new Vector2(e.X, e.Y));
                Pan_Move(VCP_MouseDownPosition, VCP_MouseUpPosition);
            }
        }

        private void glControl_Main_MouseWheel(object sender, MouseEventArgs e)
        {
            Vector2 VCP_MouseClickedPosition = new Vector2(0, 0);
            VCP_MouseDownPosition = new Vector2(e.X, glc_Main.Height - e.Y);

            Pan_Move(VCP_MouseDownPosition, VCP_MouseClickedPosition);
            if (e.Delta > 0)
            {
                if (Scale_FtoW > 7) Scale_FtoW = 7;
                Scale_FtoW +=0.1f;
            }
            else
            {
                Scale_FtoW -=0.1f;
                if (Scale_FtoW < 0.1) Scale_FtoW = 0.1f;
            }
            Pan_Move(VCP_MouseClickedPosition, VCP_MouseDownPosition);
        }

        private void glControl_Main_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseRightDown)
            {
                Vector2 VCP_MouseCurrentPosition = Transform_SCPtoVCP(new Vector2(e.X, e.Y));
                Pan_Move(VCP_MouseDownPosition, VCP_MouseCurrentPosition);
                VCP_MouseDownPosition = VCP_MouseCurrentPosition;
            }
        }

        private void glControl_Main_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Triangle_List.Clear();

            Triangle_List.Add(new Point(0, 0));
            Triangle_List.Add(new Point(100, 0));
            Triangle_List.Add(new Point(100, 50));

            Triangle_List.Add(new Point(20, 30));
            Triangle_List.Add(new Point(120, 30));
            Triangle_List.Add(new Point(120, 80));

            UpdateProjection();
            RePaint();

            Mode_Select(Transform_SCPtoVCP(new Vector2(e.X, e.Y)));
        }

        public bool Move_PixelZoom(Vector2 VCP_MouseClickedPoint)
        {
            Vector2 VCP_OriginOfCoordination = new Vector2(0, 0);

            Pan_Move(VCP_MouseClickedPoint, VCP_OriginOfCoordination);
            
            Scale_FtoW = (glc_Main.Width / 10f);

            Vector2 VCP_TargetPosition = new Vector2(glc_Main.Width / 2, glc_Main.Height / 2);
            Pan_Move(VCP_OriginOfCoordination, VCP_TargetPosition);

            return true;
        }

        public bool CompareColor(Color StdColor, Color ComColor)
        {
            if ((StdColor.R == ComColor.R) && (StdColor.G == ComColor.G) && (StdColor.B == ComColor.B))
            {
                return true;
            }
            return false;
        }
    }
}
