using NAudio.Dsp;
using NAudio.Wave;
using System;

namespace BrainfuckToNote
{
	public class SquareWaveProvider32 : WaveProvider32
	{
		double phaseAngle;
		public EnvelopeGenerator envelope = new EnvelopeGenerator();

		public SquareWaveProvider32()
		{
			Frequency = 1000;
			Amplitude = 0.25f;
			envelope.AttackRate = 44100;
			envelope.DecayRate = 44100;
			envelope.SustainLevel = 0.6f;
			envelope.ReleaseRate = 44100;
		}

		public float Frequency { get; set; }
		public float Amplitude { get; set; }

		public override int Read(float[] buffer, int offset, int sampleCount)
		{
			int sampleRate = WaveFormat.SampleRate;
			for (int n = 0; n < sampleCount; n++)
			{
				buffer[n + offset] = (float)(Amplitude * Math.Sign(Math.Sin(phaseAngle)));
				if (this.WaveFormat.Channels == 1 || (n + offset) % 2 == 0)
					phaseAngle += 2 * Math.PI * Frequency / sampleRate;
				if (phaseAngle > 2 * Math.PI)
					phaseAngle -= 2 * Math.PI;
			}
			return sampleCount;
		}
	}
}
