using NAudio.Wave;
using System;

namespace BrainfuckToNote
{
	public class SineWaveProvider32 : WaveProvider32, ISampleProvider
	{
		double phaseAngle;

		public SineWaveProvider32()
		{
			Frequency = 1000;
			Amplitude = .1f;
		}

			public SineWaveProvider32(int sampleRate, int channels) : base(sampleRate, channels)
			{
			}

			public float Frequency { get; set; }
		public float Amplitude { get; set; }

		public override int Read(float[] buffer, int offset, int sampleCount)
		{
			int sampleRate = WaveFormat.SampleRate;
			for (int n = 0; n < sampleCount; n++)
			{
				buffer[n + offset] = (float)(Amplitude * Math.Sin(phaseAngle));
				if (this.WaveFormat.Channels == 1 || (n + offset) % 2 == 0)
					phaseAngle += 2 * Math.PI * Frequency / sampleRate;
				if (phaseAngle > 2 * Math.PI)
					phaseAngle -= 2 * Math.PI;
			}
			return sampleCount;
		}
	}
}
