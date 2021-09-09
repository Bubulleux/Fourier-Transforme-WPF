using System;
using System.Net.NetworkInformation;

namespace fourierTransforme
{
	public static class FourierTransphorm
	{
		public struct Circle
		{
			public double Frequency;
			public double Amplitude;
			public double Offset;
		}

		public static Circle GetCircleByFrequency(double frequency, double[] table)
		{
			double real = 0;
			double imaginary = 0;
			int N = table.Length;

			for (int i = 0; i < N; i++)
			{
				double phy = Math.PI * 2 / N * frequency * i;
				real += Math.Cos(phy) * table[i];
				imaginary -= Math.Sin(phy) * table[i];
			}

			real /= N;
			imaginary /= N;

			double amplitude = Math.Sqrt(real * real + imaginary * imaginary);
			double offset = Math.Atan2(imaginary, real);

			return new Circle()
			{
				Frequency = frequency,
				Amplitude = amplitude,
				Offset = offset,
			};
		}

		public static Circle[] GetAllCircles(double startFrequency, double endFrequency, int countOfCircle,
			double[] table)
		{
			Circle[] circles = new Circle[countOfCircle];

			for (int i = 0; i < countOfCircle; i++)
			{
				circles[i] = GetCircleByFrequency(LearpFrequancy(startFrequency, endFrequency, countOfCircle, i),
					table);
			}

			return circles;
		}

		private static double LearpFrequancy(double start, double stop, double count, double index)
		{
			return start + (stop - start) * (index / count);
		}
	}
}