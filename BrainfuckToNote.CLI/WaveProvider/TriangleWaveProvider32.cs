using NAudio.Wave;
using System;

namespace BrainfuckToNote
{

	namespace WavePlayback
	{
		public class TriangleWaveProvider32 : WaveProvider32
	{
		double phaseAngle;

		public TriangleWaveProvider32()
		{
			Frequency = 1000;
			Amplitude = 1f;
		}

		public float Frequency { get; set; }
		public float Amplitude { get; set; }

		//public override int Read(float[] buffer, int offset, int sampleCount)
		//{
		//    int sampleRate = WaveFormat.SampleRate;
		//    for (int n = 0; n < sampleCount; n++)
		//    {
		//        buffer[n + offset] = (float)((1 - Math.Abs((((sample * Frequency) / sampleRate) % 1.0) - 0.5) * 4) * Amplitude);
		//        if (this.WaveFormat.Channels == 1 || (n + offset) % 2 == 0)
		//            sample++;
		//        if (sample >= sampleRate) sample = 0;
		//    }
		//    return sampleCount;
		//}
		public override int Read(float[] buffer, int offset, int sampleCount)
		{
			int sampleRate = WaveFormat.SampleRate;
			for (int n = 0; n < sampleCount; n++)
			{
				buffer[n + offset] = (float)(Amplitude * (1 - Math.Abs((phaseAngle % 1.0) - 0.5) * 4));
				if (this.WaveFormat.Channels == 1 || (n + offset) % 2 == 0)
					phaseAngle += (Frequency / sampleRate);
				//if (phaseAngle > 2 * Math.PI)
				//    phaseAngle -= 2 * Math.PI;
			}
			return sampleCount;
		}
	}
}
