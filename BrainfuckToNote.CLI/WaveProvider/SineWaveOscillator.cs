using NAudio.Dsp;
using NAudio.Wave;
using System;

namespace BrainfuckToNote
{
	class SineWaveOscillator : WaveProvider16, ISampleProvider
	{
		double phaseAngle;
		public EnvelopeGenerator envelope = new EnvelopeGenerator();

		public SineWaveOscillator(int sampleRate) :
		  base(sampleRate, 1)
		{
			envelope.AttackRate = 300;
			envelope.DecayRate = 0;
			envelope.SustainLevel = 1f;
			envelope.ReleaseRate = 300;
		}

		public double Frequency { set; get; }
		public short Amplitude { set; get; }
		public double Pitch { set; get; }

		public int Read(float[] buffer, int offset, int sampleCount)
		{
			for (int index = 0; index < sampleCount; index++)
			{
				float envelopeAmplitude = envelope.Process();

				buffer[offset + index] = (short)(Amplitude * envelopeAmplitude * Math.Sin(phaseAngle));
				phaseAngle += 2 * Math.PI * Frequency / WaveFormat.SampleRate;

				if (phaseAngle > 2 * Math.PI)
					phaseAngle -= 2 * Math.PI;
			}
			return sampleCount;
		}

		public override int Read(short[] buffer, int offset, int sampleCount)
		{
			for (int index = 0; index < sampleCount; index++)
			{
				float envelopeAmplitude = envelope.Process();

				buffer[offset + index] = (short)(Amplitude * envelopeAmplitude * Math.Sin(phaseAngle));
				phaseAngle += 2 * Math.PI * Frequency / WaveFormat.SampleRate;

				if (phaseAngle > 2 * Math.PI)
					phaseAngle -= 2 * Math.PI;
			}
			return sampleCount;
		}
	}
}
