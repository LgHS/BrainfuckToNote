using NAudio.Wave;

namespace BrainfuckToNote
{

	namespace WavePlayback
	{
		public class SawWaveProvider32 : WaveProvider32
	{
		double phaseAngle;

		public SawWaveProvider32()
		{
			Frequency = 1000;
			Amplitude = 0.25f;
		}

		public float Frequency { get; set; }
		public float Amplitude { get; set; }

		public override int Read(float[] buffer, int offset, int sampleCount)
		{
			int sampleRate = WaveFormat.SampleRate;
			for (int n = 0; n < sampleCount; n++)
			{
				buffer[n + offset] = (float)(Amplitude * ((phaseAngle % 1.0) * 2 - 1));
				if (this.WaveFormat.Channels == 1 || (n + offset) % 2 == 0)
					phaseAngle += (Frequency / sampleRate);
				//if (phaseAngle > 2 * Math.PI)
				//    phaseAngle -= 2 * Math.PI;
			}
			return sampleCount;
		}
	}
}
