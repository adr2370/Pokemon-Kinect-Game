/////////////////////////////////////////////////////////////////////////
//
// This module contains code to do a basic green screen.
//
// Copyright © Microsoft Corporation.  All rights reserved.  
// This code is licensed under the terms of the 
// Microsoft Kinect for Windows SDK (Beta) from Microsoft Research 
// License Agreement: http://research.microsoft.com/KinectSDK-ToU
//
/////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Text;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Net.Sockets;
using Microsoft.Research.Kinect.Nui;
using Coding4Fun.Kinect.Wpf;
using ShapeGame_Speech;
using ShapeGame_Utils;
using System.Drawing;

namespace SkeletalTracking
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Recognizer recognizer = null;
        JointsCollection currentJoints = null;
        float RightHandX = 0;
        float RightHandY = 0;
        int currentAshX = 3;
        int currentAshY = 6;
        Bitmap currentBitmap = null;
        public MainWindow()
        {
            InitializeComponent();
        }

        //Kinect Runtime
        Runtime nui = new Runtime();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            #region bitmapRetrieval
            try
            {
                #region makeHouse
                Bitmap ashHouseBitmap = new Bitmap(@"C:\\Users\\Alexander Ramirez\\Desktop\\Blue Map\\Ash House.bmp");
                Bitmap ashHouseUpstairs = new Bitmap(16 * 16, 16 * 16);
                for (int a = 0; a < 16; a++)
                {
                    for (int k = 0; k < 16; k++)
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            for (int j = 0; j < 16; j++)
                            {
                                ashHouseUpstairs.SetPixel(i + 16 * k, j + 16 * a, ashHouseBitmap.GetPixel(i + 16 * 1, j + 16 * 0));
                            }
                        }
                    }
                }
                for (int i = 0; i < 32; i++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        ashHouseUpstairs.SetPixel(i + 16 * 0, j + 16 * 0, ashHouseBitmap.GetPixel(i + 32 * 0, j + 16 * 4));
                    }
                }
                for (int i = 0; i < 32; i++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        ashHouseUpstairs.SetPixel(i + 16 * 0, j + 16 * 1, ashHouseBitmap.GetPixel(i + 32 * 0, j + 16 * 2));
                    }
                }
                for (int i = 0; i < 32; i++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        ashHouseUpstairs.SetPixel(i + 16 * 0, j + 16 * 2, ashHouseBitmap.GetPixel(i + 32 * 1, j + 16 * 4));
                    }
                }
                for (int i = 0; i < 32; i++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        ashHouseUpstairs.SetPixel(i + 16 * 0, j + 16 * 3, ashHouseBitmap.GetPixel(i + 32 * 1, j + 16 * 3));
                    }
                }
                for (int k = 2; k < 10; k++)
                {
                    for (int i = 0; i < 16; i++)
                    {
                        for (int j = 0; j < 16; j++)
                        {
                            ashHouseUpstairs.SetPixel(i + 16 * k, j + 16 * 0, ashHouseBitmap.GetPixel(i + 16 * 0, j + 16 * 0));
                        }
                    }
                }
                for (int k = 6; k < 10; k++)
                {
                    for (int i = 0; i < 16; i++)
                    {
                        for (int j = 0; j < 16; j++)
                        {
                            ashHouseUpstairs.SetPixel(i + 16 * k, j + 16 * 1, ashHouseBitmap.GetPixel(i + 16 * 0, j + 16 * 0));
                        }
                    }
                }
                for (int k = 12; k < 14; k++)
                {
                    for (int i = 0; i < 16; i++)
                    {
                        for (int j = 0; j < 16; j++)
                        {
                            ashHouseUpstairs.SetPixel(i + 16 * k, j + 16 * 0, ashHouseBitmap.GetPixel(i + 16 * 0, j + 16 * 0));
                        }
                    }
                }
                for (int k = 12; k < 14; k++)
                {
                    for (int i = 0; i < 16; i++)
                    {
                        for (int j = 0; j < 16; j++)
                        {
                            ashHouseUpstairs.SetPixel(i + 16 * k, j + 16 * 1, ashHouseBitmap.GetPixel(i + 16 * 0, j + 16 * 0));
                        }
                    }
                }
                for (int i = 0; i < 32; i++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        ashHouseUpstairs.SetPixel(i + 16 * 2, j + 16 * 1, ashHouseBitmap.GetPixel(i + 16 * 6, j + 16 * 2));
                    }
                }
                for (int i = 0; i < 16; i++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        ashHouseUpstairs.SetPixel(i + 16 * 4, j + 16 * 1, ashHouseBitmap.GetPixel(i + 16 * 7, j + 16 * 2));
                    }
                }
                for (int i = 0; i < 16; i++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        ashHouseUpstairs.SetPixel(i + 16 * 5, j + 16 * 1, ashHouseBitmap.GetPixel(i + 16 * 9, j + 16 * 2));
                    }
                }
                for (int i = 0; i < 16; i++)
                {
                    for (int j = 0; j < 32; j++)
                    {
                        ashHouseUpstairs.SetPixel(i + 16 * 2, j + 16 * 2, ashHouseBitmap.GetPixel(i + 16 * 12, j + 16 * 2));
                    }
                }
                for (int i = 0; i < 16; i++)
                {
                    for (int j = 0; j < 32; j++)
                    {
                        ashHouseUpstairs.SetPixel(i + 16 * 3, j + 16 * 2, ashHouseBitmap.GetPixel(i + 16 * 10, j + 16 * 2));
                    }
                }
                for (int i = 0; i < 16; i++)
                {
                    for (int j = 0; j < 32; j++)
                    {
                        ashHouseUpstairs.SetPixel(i + 16 * 4, j + 16 * 2, ashHouseBitmap.GetPixel(i + 16 * 10, j + 16 * 2));
                    }
                }
                for (int i = 0; i < 16; i++)
                {
                    for (int j = 0; j < 32; j++)
                    {
                        ashHouseUpstairs.SetPixel(i + 16 * 5, j + 16 * 2, ashHouseBitmap.GetPixel(i + 16 * 11, j + 16 * 2));
                    }
                }
                for (int i = 0; i < 32; i++)
                {
                    for (int j = 0; j < 32; j++)
                    {
                        ashHouseUpstairs.SetPixel(i + 16 * 10, j + 16 * 0, ashHouseBitmap.GetPixel(i + 16 * 4, j + 16 * 2));
                    }
                }
                for (int i = 0; i < 32; i++)
                {
                    for (int j = 0; j < 32; j++)
                    {
                        ashHouseUpstairs.SetPixel(i + 16 * 14, j + 16 * 0, ashHouseBitmap.GetPixel(i + 16 * 4, j + 16 * 2));
                    }
                }
                for (int i = 0; i < 32; i++)
                {
                    for (int j = 0; j < 32; j++)
                    {
                        ashHouseUpstairs.SetPixel(i + 16 * 14, j + 16 * 2, ashHouseBitmap.GetPixel(i + 16 * 10, j + 16 * 0));
                    }
                }
                for (int i = 0; i < 32; i++)
                {
                    for (int j = 0; j < 32; j++)
                    {
                        ashHouseUpstairs.SetPixel(i + 16 * 6, j + 16 * 8, ashHouseBitmap.GetPixel(i + 16 * 6, j + 16 * 0));
                    }
                }
                for (int i = 0; i < 32; i++)
                {
                    for (int j = 0; j < 32; j++)
                    {
                        ashHouseUpstairs.SetPixel(i + 16 * 6, j + 16 * 10, ashHouseBitmap.GetPixel(i + 16 * 14, j + 16 * 0));
                    }
                }
                for (int i = 0; i < 32; i++)
                {
                    for (int j = 0; j < 32; j++)
                    {
                        ashHouseUpstairs.SetPixel(i + 16 * 0, j + 16 * 12, ashHouseBitmap.GetPixel(i + 16 * 13, j + 16 * 2));
                    }
                }
                for (int i = 0; i < 32; i++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        ashHouseUpstairs.SetPixel(i + 16 * 0, j + 16 * 14, ashHouseBitmap.GetPixel(i + 16 * 13, j + 16 * 3));
                    }
                }
                for (int i = 0; i < 16; i++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        ashHouseUpstairs.SetPixel(i + 16 * 0, j + 16 * 15, ashHouseBitmap.GetPixel(i + 16 * 15, j + 16 * 3));
                    }
                }
                for (int i = 0; i < 16; i++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        ashHouseUpstairs.SetPixel(i + 16 * 1, j + 16 * 15, ashHouseBitmap.GetPixel(i + 16 * 15, j + 16 * 2));
                    }
                }
                for (int i = 0; i < 32; i++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        ashHouseUpstairs.SetPixel(i + 16 * 12, j + 16 * 12, ashHouseBitmap.GetPixel(i + 16 * 4, j + 16 * 4));
                    }
                }
                for (int i = 0; i < 32; i++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        ashHouseUpstairs.SetPixel(i + 16 * 12, j + 16 * 13, ashHouseBitmap.GetPixel(i + 16 * 8, j + 16 * 0));
                    }
                }
                for (int i = 0; i < 32; i++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        ashHouseUpstairs.SetPixel(i + 16 * 12, j + 16 * 14, ashHouseBitmap.GetPixel(i + 16 * 6, j + 16 * 4));
                    }
                }
                for (int i = 0; i < 32; i++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        ashHouseUpstairs.SetPixel(i + 16 * 12, j + 16 * 15, ashHouseBitmap.GetPixel(i + 16 * 8, j + 16 * 1));
                    }
                }
                #endregion
                currentBitmap = ashHouseUpstairs;
                GetAshHouse(currentAshX, currentAshY);
            }
            catch (ArgumentException)
            {
            }
            #endregion

            #region VideoStuff
            nui.VideoFrameReady += new EventHandler<ImageFrameReadyEventArgs>(nui_VideoFrameReady);

            //Initialize to do skeletal tracking
            nui.Initialize(RuntimeOptions.UseSkeletalTracking | RuntimeOptions.UseColor);

            nui.VideoStream.Open(ImageStreamType.Video, 2, ImageResolution.Resolution640x480, ImageType.Color);

            #region TransformSmooth
            //Must set to true and set after call to Initialize
            nui.SkeletonEngine.TransformSmooth = true;

            //Use to transform and reduce jitter
            var parameters = new TransformSmoothParameters
            {
                Smoothing = 0.75f,
                Correction = 0.0f,
                Prediction = 0.0f,
                JitterRadius = 0.05f,
                MaxDeviationRadius = 0.04f
            };

            nui.SkeletonEngine.SmoothParameters = parameters;

            #endregion

            //add event to receive skeleton data
            nui.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(nui_SkeletonFrameReady);

            #endregion

            #region voiceRecognition
            /*
            try
            {
                recognizer = new Recognizer();
            }
            catch
            {
                recognizer = null;
            }
            if ((recognizer == null) || !recognizer.IsValid())
            {
                recognizer = null;
            }
            else
                recognizer.SaidSomething += recognizer_SaidSomething;
            */
            #endregion
        }
        private void GetAshHouse(int x, int y)
        {
            #region findCurrentScreen
            Bitmap Current = new Bitmap(20 * 16, 18 * 16);
            for (int i = 0; i < 20 * 16; i++)
            {
                for (int j = 0; j < 18 * 16; j++)
                {
                    int xValue = i + (2 * x - 8) * 16;
                    int yValue = j + (2 * y - 8) * 16;
                    if (xValue >= 0 && xValue < 16 * 16 && yValue < 16 * 16 && yValue >= 0)
                    {
                        Current.SetPixel(i, j, currentBitmap.GetPixel(xValue, yValue));
                    }
                    else
                    {
                        Current.SetPixel(i, j, System.Drawing.Color.Gray);
                    }
                }
            }
            #endregion

            #region setScreen
            var hbit = Current.GetHbitmap();
            BitmapSource bitSrc = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hbit, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            ImageSource thing = bitSrc;
            AshHouse.Source = thing;
            Bitmap ashBitmap = new Bitmap(@"C:\\Users\\Alexander Ramirez\\Desktop\\Pokemon Kinect Game\\Ash Image.png");
            Bitmap ash = new Bitmap(2 * 16, 2 * 16);
            for (int i = 0; i < 32; i++)
            {
                for (int j = 0; j < 32; j++)
                {
                    if (!ashBitmap.GetPixel(i, j).Equals(ashBitmap.GetPixel(0, 0)))
                    {
                        ash.SetPixel(i, j, ashBitmap.GetPixel(i, j));
                    }
                    else
                    {
                        ash.SetPixel(i, j, Current.GetPixel(i + 32 * 4, j + (32 * 4) - 11));
                    }
                }
            }
            hbit = ash.GetHbitmap();
            bitSrc = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hbit, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            thing = bitSrc;
            Ash.Source = thing;
            #endregion
        }

        void recognizer_SaidSomething(object sender, Recognizer.SaidSomethingArgs e)
        {
            switch (e.Verb)
            {
                case Recognizer.Verbs.Print:
                    Console.WriteLine("X Value "+currentJoints[JointID.HandRight].Position.X);
                    break;
            }
        }

        void nui_VideoFrameReady(object sender, ImageFrameReadyEventArgs e)
        {
            //Manually create BitmapSource for Video
            PlanarImage imageData = e.ImageFrame.Image;
            image1.Source = BitmapSource.Create(imageData.Width, imageData.Height, 96, 96, PixelFormats.Bgr32, null, imageData.Bits, imageData.Width * imageData.BytesPerPixel);
        }

        void nui_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            SkeletonFrame allSkeletons = e.SkeletonFrame;

            //get the first tracked skeleton
            SkeletonData skeleton = (from s in allSkeletons.Skeletons
                                     where s.TrackingState == SkeletonTrackingState.Tracked
                                     select s).FirstOrDefault();

            currentJoints = skeleton.Joints;
            SetEllipsePosition(RHand, skeleton.Joints[JointID.HandRight]);

            if (RightHandX > 640.0 * 5 / 6 && RightHandY < 480.0 * 1 / 6)
            {
                currentAshX = 3;
                currentAshY = 3;
            }
            else if (RightHandX > 640.0 * 5 / 6)
            {
                int newAshX = currentAshX + 1;
                int newAshY = currentAshY;
                if (worksInAshHouseUpstairs(newAshX, newAshX))
                {
                    currentAshX = newAshX;
                    currentAshY = newAshY;
                }
            }
            else if (RightHandX < 640.0 * 1 / 6)
            {
                int newAshX = currentAshX - 1;
                int newAshY = currentAshY;
                if (worksInAshHouseUpstairs(newAshX, newAshX))
                {
                    currentAshX = newAshX;
                    currentAshY = newAshY;
                }
            }
            else if (RightHandY > 480.0 * 5 / 6)
            {
                int newAshX = currentAshX;
                int newAshY = currentAshY + 1;
                if (worksInAshHouseUpstairs(newAshX, newAshX))
                {
                    currentAshX = newAshX;
                    currentAshY = newAshY;
                }
            }
            else if (RightHandY < 480.0 * 1 / 6)
            {
                int newAshX = currentAshX;
                int newAshY = currentAshY - 1;
                if (worksInAshHouseUpstairs(newAshX, newAshX))
                {
                    currentAshX = newAshX;
                    currentAshY = newAshY;
                }
            }
            GetAshHouse(currentAshX, currentAshY);
        }

        private bool worksInAshHouseUpstairs(int x, int y)
        {
            if (y == 0) return false;
            if (x <= 2 && y == 1) return false;
            if (x == 7 && y == 1) return false;
            if (x == 0 && y >= 6) return false;
            if (x == 6 && y >= 6) return false;
            if (x == 3 && (y == 4 || y == 5)) return false;
            if (y < 0 || x < 0) return false;
            if (y >= 8 || x >= 8) return false;
            return true;
        }

        private void SetEllipsePosition(FrameworkElement ellipse, Joint joint)
        {
            var scaledJoint = joint.ScaleTo(640 * 3, 480 * 3, .5f, .5f);

            Canvas.SetLeft(ellipse, scaledJoint.Position.X - 640);
            Canvas.SetTop(ellipse, scaledJoint.Position.Y - 480);
            RightHandX = scaledJoint.Position.X - 640;
            RightHandY = scaledJoint.Position.Y - 480;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //Cleanup
            nui.Uninitialize();
        }

        private void keyPressed(object sender, KeyEventArgs e)
        {
            int newAshX = 0;
            int newAshY = 0;
            switch (e.Key.ToString().ToLower().ToCharArray()[0])
            {
                case 'p':
                    currentAshX = 3;
                    currentAshY = 3;
                    break;
                case 'd':
                    newAshX = currentAshX + 1;
                    newAshY = currentAshY;
                    if (worksInAshHouseUpstairs(newAshX, newAshY))
                    {
                        currentAshX = newAshX;
                        currentAshY = newAshY;
                    }
                    break;
                case 'a':
                    newAshX = currentAshX - 1;
                    newAshY = currentAshY;
                    if (worksInAshHouseUpstairs(newAshX, newAshY))
                    {
                        currentAshX = newAshX;
                        currentAshY = newAshY;
                    }
                    break;
                case 's':
                    newAshX = currentAshX;
                    newAshY = currentAshY + 1;
                    if (worksInAshHouseUpstairs(newAshX, newAshY))
                    {
                        currentAshX = newAshX;
                        currentAshY = newAshY;
                    }
                    break;
                case 'w':
                    newAshX = currentAshX;
                    newAshY = currentAshY - 1;
                    if (worksInAshHouseUpstairs(newAshX, newAshY))
                    {
                        currentAshX = newAshX;
                        currentAshY = newAshY;
                    }
                    break;
                default:
                    break;
            }
            GetAshHouse(currentAshX, currentAshY);
        }
    }
}