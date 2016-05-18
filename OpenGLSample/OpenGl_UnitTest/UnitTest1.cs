using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

// Zoom, Select Test
// Refactoring
// function delegate


namespace OpenGl_UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod_PAN()
        {
            OpenGLSample.Form1 wForm = Init();
            
            wForm.Triangle_List.Add(new Point(250, 0));
            wForm.Triangle_List.Add(new Point(350, 0));
            wForm.Triangle_List.Add(new Point(350, 50));
            
            Assert.AreEqual(Test_Pan(wForm), true);
        }
        [TestMethod]
        public void TestMethod_ZOOM()
        {
            OpenGLSample.Form1 wForm = Init();

            wForm.Triangle_List.Add(new Point(250, 0));
            wForm.Triangle_List.Add(new Point(350, 0));
            wForm.Triangle_List.Add(new Point(350, 50));
            
            Assert.AreEqual(Test_Zoom(wForm), true);
        }
        [TestMethod]
        public void TestMethod_PixelZOOM()
        {
            OpenGLSample.Form1 wForm = Init();
            wForm.Triangle_List.Add(new Point(0, 0));
            wForm.Triangle_List.Add(new Point(100, 0));
            wForm.Triangle_List.Add(new Point(100, 50));

            int LimitPixel = 1;

            wForm.Ractangle_List.Add(new Point(100 - LimitPixel, 50 - LimitPixel));
            wForm.Ractangle_List.Add(new Point(100 - LimitPixel, 50 + LimitPixel));
            wForm.Ractangle_List.Add(new Point(100 + LimitPixel, 50 + LimitPixel));
            wForm.Ractangle_List.Add(new Point(100 + LimitPixel, 50 - LimitPixel));
            Assert.AreEqual(Test_PixelZoom(wForm), true);
        }
        [TestMethod]
        public void TestMethod_SELECT()
        {
            OpenGLSample.Form1 wForm = Init();
            wForm.Triangle_List.Add(new Point(0, 0));
            wForm.Triangle_List.Add(new Point(100, 0));
            wForm.Triangle_List.Add(new Point(100, 50));

            wForm.Triangle_List.Add(new Point(50, 30));
            wForm.Triangle_List.Add(new Point(150, 30));
            wForm.Triangle_List.Add(new Point(150, 80));

            Assert.AreEqual(Test_Select(wForm), true);
        }

        private OpenGLSample.Form1 Init()
        {
            OpenGLSample.Form1 wForm = new OpenGLSample.Form1();

            GameWindow gwindow = new GameWindow(wForm.glc_Main.Width, wForm.glc_Main.Height);
            GL.ClearColor(Color.Blue);
            wForm.UpdateProjection();
            wForm.RePaint();

            return wForm;
        }

        public bool Test_Pan(OpenGLSample.Form1 wForm)
        {
            //int delta_pixel = 3;
            //Point p2_Y = new Point(Line1Point.X + delta_pixel, Line1Point.Y - delta_pixel);
            wForm.UpdateProjection();
            wForm.RePaint();
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

//             wForm.UpdateProjection();
             wForm.RePaint();
            //====================================================================================
           // if (false == wForm.CompareColor(Color.Yellow, wForm.getColor(p2_Y.X + Convert.ToInt32(TES.X), p2_Y.Y + Convert.ToInt32(TES.Y)))) return false;
            if (false == wForm.CompareColor(Color.Yellow, wForm.getColor(360 , p2_Y.Y + Convert.ToInt32(TES.Y)))) return false;
            if (false == wForm.CompareColor(Color.Blue, wForm.getColor(p1_B.X + Convert.ToInt32(TES.X), p1_B.Y + Convert.ToInt32(TES.Y)))) return false;
            if (false == wForm.CompareColor(Color.Blue, wForm.getColor(p3_B.X + Convert.ToInt32(TES.X), p3_B.Y + Convert.ToInt32(TES.Y)))) return false;

            return true;
        }

        public bool Test_Zoom(OpenGLSample.Form1 wForm)
        {
            //wForm.Triangle_List.Clear();
            wForm.UpdateProjection();
            wForm.RePaint();

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
            wForm.RePaint();
            wForm.Scale_FtoW = 2f;
            wForm.UpdateProjection();
            wForm.RePaint();
            wForm.Pan_Move(FE, FP_MouseStart);

            wForm.UpdateProjection();
            wForm.RePaint();
            //====================================================================================

            Point TP1_Y = new Point(160, 3);
            Point TP2_B = new Point(160, 7);
            Point TP3_B = new Point(400, 3);

            if (false == wForm.CompareColor(Color.Yellow, wForm.getColor(TP1_Y))) return false;
            if (false == wForm.CompareColor(Color.Blue, wForm.getColor(TP2_B))) return false;
            if (false == wForm.CompareColor(Color.Blue, wForm.getColor(TP3_B))) return false;

            return true;
        }

        public bool Test_PixelZoom(OpenGLSample.Form1 wForm)
        {
            wForm.RePaint();

            Vector2 FP_MouseStart = new Vector2(100, 50); //100,50
            wForm.Move_PixelZoom(FP_MouseStart);
            wForm.RePaint();

            Point RP1_B = new Point(202, 302);
            Point RP2_B = new Point(198, 298);
            Point RP3_B = new Point(302, 202);
            Point RP4_B = new Point(298, 198);

            Point RP5_R = new Point(250, 250);

            if (false == wForm.CompareColor(Color.Blue, wForm.getColor(RP1_B))) return false;
            if (false == wForm.CompareColor(Color.Blue, wForm.getColor(RP2_B))) return false;
            if (false == wForm.CompareColor(Color.Blue, wForm.getColor(RP3_B))) return false;
            if (false == wForm.CompareColor(Color.Blue, wForm.getColor(RP4_B))) return false;
            if (false == wForm.CompareColor(Color.Red, wForm.getColor(RP5_R))) return false;

            return true;
        }

        public bool Test_Select(OpenGLSample.Form1 wForm)
        {
            // mouse point 입력
            //return wForm.Mode_Select(new Vector2(100, 450));

            //wForm.rePaint();
            wForm.View_Reset();
            Assert.AreEqual(wForm.Mode_Select(new Vector2(101, 1)), 1);
            wForm.View_Reset();
            Assert.AreEqual(wForm.Mode_Select(new Vector2(100, 50)), 2);
            wForm.View_Reset();
            Assert.AreEqual(wForm.Mode_Select(new Vector2(99, 1)), 1);
            wForm.View_Reset();
            Assert.AreEqual(wForm.Mode_Select(new Vector2(101, 31)), 2);
            wForm.View_Reset();
            Assert.AreEqual(wForm.Mode_Select(new Vector2(351, 31)), 0);
            wForm.View_Reset();
            return true;
        }


    }
}
