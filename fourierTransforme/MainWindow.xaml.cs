using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace fourierTransforme
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		
		private const double TimeSize = 3f;
		private const int KeyFrequency = 100;

		private readonly double[] signal;

		private double curTime = 0;
		private List<double> output = new List<double>();
		private FourierTransphorm.Circle[] circles;
		

		public MainWindow()
		{
			signal = GetSignal(TimeSize, KeyFrequency);
			circles = FourierTransphorm.GetAllCircles(0, signal.Length, signal.Length, signal);

			double maxAmplitude = 0f;
			double frequancyMax = -1f;
			foreach (var circle in circles)
			{
				if (circle.Amplitude > maxAmplitude)
				{
					maxAmplitude = circle.Amplitude;
					frequancyMax = circle.Frequency;
				}
			}

			InitializeComponent();

			CompositionTarget.Rendering += UpdateWindow;
		}

		private void UpdateWindow(object sender, EventArgs e)
		{
			UpdateOutput();
		}

		public void UpdateOutput()
		{
			CanvasOutput.Children.Clear();
			double oldPosX = 0.5d;
			double oldPosY = 0.5d;
			double size = 0.5d;
			foreach (var circle in circles)
			{
				DrawCircle(oldPosX, oldPosY, circle.Amplitude * size, CanvasOutput);
				double x = oldPosX + Math.Cos(curTime * circle.Frequency + circle.Offset) * circle.Amplitude * size;
				double y = oldPosY + Math.Sin(curTime * circle.Frequency + circle.Offset) * circle.Amplitude * size;
				DrawLine(oldPosX, oldPosY, x, y, Colors.Green, 2, CanvasOutput);
				oldPosX = x;
				oldPosY = y;
			}
			output.Add(oldPosX);
			curTime += 0.01d;

		}

		

		private double[] GetSignal(double size, double keyFrequency)
		{
			double[] result = new double[Convert.ToInt32(size * keyFrequency)];

			for (int i = 0; i < result.Length; i++)
			{
				double time = i / keyFrequency;
				result[i] = Math.Sin(time * Math.PI * 2);
			}

			return result;
		}

		private void DrawSignal()
		{
			double xTrans(double px) => px / TimeSize;
			double yTrans(double py) => (py + 1) / 2f;
			
			CanvasSignal.Children.Clear();

			

			Color color = Color.FromRgb(255, 0, 0);
			double oldX = 0;
			double oldY = 0;
			
			for (int i = 0; i < signal.Length; i++)
			{
				double x = i / (double)KeyFrequency;
				double y = signal[i];
				
				if (i != 0)
					DrawLine(xTrans(oldX), yTrans(oldY), xTrans(x), yTrans(y), color, 3, CanvasSignal);
				
				oldX = x;
				oldY = y;
			}
		}
		
		
		private void DrawLine(double x1, double y1, double x2, double y2, Color color, float thickness, Canvas canvas)
		{
			Line line = new Line
			{
				X1 = x1 * canvas.ActualWidth,
				Y1 = y1 * canvas.ActualHeight,
				
				X2 = x2 * canvas.ActualWidth,
				Y2 = y2 * canvas.ActualHeight,
				
				StrokeThickness = thickness,
				Stroke = new SolidColorBrush(color)
			};

			canvas.Children.Add(line);
		}
		
		public void DrawCircle(double x, double y, double radius, Canvas canvas)
		{
			Ellipse ellipse = new Ellipse();
			ellipse.Stroke = Brushes.Green;
			ellipse.StrokeThickness = 3;
			Canvas.SetLeft(ellipse, (x - radius) * canvas.ActualWidth);
			Canvas.SetTop(ellipse, (y - radius) * canvas.ActualHeight);
			ellipse.Width = 2 * radius * canvas.ActualWidth;
			ellipse.Height = 2 * radius * canvas.ActualHeight;
			canvas.Children.Add(ellipse);
		}

		
		private void CanvasLoaded(object sender, RoutedEventArgs e)
		{
			DrawSignal();
		}

		private void WindowSizeChanged(object sender, SizeChangedEventArgs e)
		{
			DrawSignal();
		}
	}
}