using NAudio.Wave;
using System;

namespace BrainfuckToNote
{
	public class TriangleWaveProvider32 : WaveProvider32, ISampleProvider
	{
		double phaseAngle;

		public TriangleWaveProvider32()
		{
			Frequency = 1000;
			Amplitude = 1f;
		}

		public float Frequency { get; set; }
		public float Amplitude { get; set; }

		public override int Read(float[] buffer, int offset, int sampleCount)
		{
			int sampleRate = WaveFormat.SampleRate;
			for (int n = 0; n < sampleCount; n++)
			{
				buffer[n + offset] = (float)(Amplitude * (1 - Math.Abs((phaseAngle % 1.0) - 0.5) * 4));
				if (this.WaveFormat.Channels == 1 || (n + offset) % 2 == 0)
					phaseAngle += (Frequency / sampleRate);
			}
			return sampleCount;
		}
	}
}
