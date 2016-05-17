using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenGl_UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            OpenGLSample.Form1 wForm = new OpenGLSample.Form1();

            GameWindow gwindow = new GameWindow(wForm.glControl1.Width, wForm.glControl1.Height);
            GL.ClearColor(Color.Blue);
            wForm.UpdateProjection();
            wForm.rePaint();
            Assert.AreEqual(Test_Pan(wForm), true);
            Assert.AreEqual(Test_Zoom(wForm), true);
        }

        public bool Test_Pan(OpenGLSample.Form1 wForm)
        { 
            //int delta_pixel = 3;
            //Point p2_Y = new Point(Line1Point.X + delta_pixel, Line1Point.Y - delta_pixel);
            Point p2_Y = new Point(253, 1); //노란
            Point p1_B = new Point(249, 1); //
            Point p3_B = new Point(253, 20);

            if (false == wForm.CompareColor(Color.Yellow, wForm.getColor(p2_Y))) return false;
            if (false == wForm.CompareColor(Color.Blue, wForm.getColor(p1_B))) return false;
            if (false == wForm.CompareColor(Color.Blue, wForm.getColor(p3_B))) return false;
            //====================================================================================
            Vector2 TS = new Vector2(250, 0);
            Vector2 TE = new Vector2(350, 100);
            Vector2 TES = TE - TS;

            wForm.Pan_Move(TS, TE);

            wForm.UpdateProjection();
            wForm.rePaint();
            //====================================================================================
           // if (false == wForm.CompareColor(Color.Yellow, wForm.getColor(p2_Y.X + Convert.ToInt32(TES.X), p2_Y.Y + Convert.ToInt32(TES.Y)))) return false;
            if (false == wForm.CompareColor(Color.Yellow, wForm.getColor(360 , p2_Y.Y + Convert.ToInt32(TES.Y)))) return false;
            if (false == wForm.CompareColor(Color.Blue, wForm.getColor(p1_B.X + Convert.ToInt32(TES.X), p1_B.Y + Convert.ToInt32(TES.Y)))) return false;
            if (false == wForm.CompareColor(Color.Blue, wForm.getColor(p3_B.X + Convert.ToInt32(TES.X), p3_B.Y + Convert.ToInt32(TES.Y)))) return false;

            return true;
        }

        public bool Test_Zoom(OpenGLSample.Form1 wForm)
        {
            Point p2_Y = new Point(253, 1); //노란
            Point p1_B = new Point(249, 1); //
            Point p3_B = new Point(253, 20);

            if (false == wForm.CompareColor(Color.Yellow, wForm.getColor(p2_Y))) return false;
            if (false == wForm.CompareColor(Color.Blue, wForm.getColor(p1_B))) return false;
            if (false == wForm.CompareColor(Color.Blue, wForm.getColor(p3_B))) return false;

            //====================================================================================
            //기준점
            Vector2 FE = new Vector2(0, 0);
            Vector2 FP_MouseStart = new Vector2(350, 0);
            wForm.Pan_Move(FP_MouseStart, FE);
            wForm.UpdateProjection();
            wForm.rePaint();
            wForm.Scale_FtoW = 2f;
            wForm.UpdateProjection();
            wForm.rePaint();
            wForm.Pan_Move(FE, FP_MouseStart);

            wForm.UpdateProjection();
            wForm.rePaint();
            //====================================================================================

            Point TP1_Y = new Point(160, 3);
            Point TP2_B = new Point(160, 7);
            Point TP3_B = new Point(400, 3);

            if (false == wForm.CompareColor(Color.Yellow, wForm.getColor(TP1_Y))) return false;
            if (false == wForm.CompareColor(Color.Blue, wForm.getColor(TP2_B))) return false;
            if (false == wForm.CompareColor(Color.Blue, wForm.getColor(TP3_B))) return false;

            return true;
        }

    }
}
