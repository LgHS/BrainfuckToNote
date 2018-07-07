using BackFuck;
using NAudio.Dsp;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Text;

namespace BrainfuckToNote
{
    class Synth
    {
		private WaveOut sineOut = new WaveOut();
		private WaveOut sawOut = new WaveOut();
		private WaveOut triangleOut = new WaveOut();
		private WaveOut squareOut = new WaveOut();

		WavePlayback.SineWaveProvider32 sineWave = new WavePlayback.SineWaveProvider32();
		WavePlayback.SawWaveProvider32 sawWave = new WavePlayback.SawWaveProvider32();
		WavePlayback.SquareWaveProvider32 squareWave = new WavePlayback.SquareWaveProvider32();
		WavePlayback.TriangleWaveProvider32 triangleWave = new WavePlayback.TriangleWaveProvider32();

		float freq;
		bool playing = false;
		string wave = "sine";

		public Synth()
		{
			InitWaves();

		}

		public double GetFrequency(Note note)
		{
			if (note.Alteration == Alteration.Flat)
			{
				if (note.Pitch == Notes.Do) note.Pitch = Notes.Si;
				else
				{
					note.Pitch -= 1;
					note.Alteration = Alteration.Sharp;
				}
			}
			List<string> notes = new List<string> () { "A", "A#", "B", "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#" };

			string stringNote = String.Format("{0}{1}", note.Pitch, note.Alteration == Alteration.Sharp ? "#" : "");

			int keyNumber = notes.IndexOf(stringNote);

			if (keyNumber < 3)
			{
				keyNumber = keyNumber + 12 + ((note.Ocatve - 1) * 12) + 1;
			}
			else
			{
				keyNumber = keyNumber + ((note.Ocatve - 1) * 12) + 1;
			}


			return 440 * Math.Pow(2, (keyNumber - 49) / 12);
		}

		public void PlayWave(double _frequency)
		{
			if (wave == "sine")
			{
				sineWave.Frequency = (float)_frequency;

				if (sineOut.PlaybackState == PlaybackState.Stopped)
				{
					//sineWave.envelope.Gate(true);
					sineOut.Play();
				}
				else
				{
					sineOut.Resume();
				}
			}
			else if (wave == "saw")
			{
				sawWave.Frequency = (float)_frequency;

				if (sawOut.PlaybackState == PlaybackState.Stopped)
					sawOut.Play();
				else
					sawOut.Resume();
			}
			else if (wave == "square")
			{
				squareWave.Frequency = (float)_frequency;

				if (squareOut.PlaybackState == PlaybackState.Stopped)
				{
					squareOut.Play();
				}
				else
					squareOut.Resume();
			}
			else if (wave == "triangle")
			{
				triangleWave.Frequency = (float)_frequency;

				if (triangleOut.PlaybackState == PlaybackState.Stopped)
					triangleOut.Play();
				else
					triangleOut.Resume();
			}
			playing = true;
		}

		public void InitWaves()
		{
			sineWave.SetWaveFormat(44100, 2);
			sineWave.Frequency = (float)0;
			sineWave.Amplitude = .9f;
			sineOut.Init(sineWave);

			sawWave.SetWaveFormat(44000, 2);
			sawWave.Frequency = (float)0;
			sawWave.Amplitude = .3f;
			sawOut.Init(sawWave);

			squareWave.SetWaveFormat(44100, 2);
			squareWave.Frequency = (float)0;
			squareWave.Amplitude = .3f;
			squareOut.Init(squareWave);

			triangleWave.SetWaveFormat(44000, 2);
			triangleWave.Frequency = (float)0;
			triangleWave.Amplitude = 1f;
			triangleOut.Init(triangleWave);
		}

		public void SetFrequency(float _freq)
		{
			if (wave == "sine")
			{
				sineWave.Frequency = _freq;
			}
			else if (wave == "square")
			{
				squareWave.Frequency = _freq;
			}
			else if (wave == "triangle")
			{
				triangleWave.Frequency = _freq;
			}
			else if (wave == "saw")
			{
				sawWave.Frequency = _freq;
			}
		}
	}
}

namespace WavePlayback
{

	public abstract class WaveProvider32 : IWaveProvider
	{
		private WaveFormat waveFormat;

		public WaveProvider32()
			: this(44100, 1)
		{

		}

		public WaveProvider32(int sampleRate, int channels)
		{
			SetWaveFormat(sampleRate, channels);
		}

		public void SetWaveFormat(int sampleRate, int channels)
		{
			this.waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channels);
		}

		public int Read(byte[] buffer, int offset, int count)
		{
			WaveBuffer waveBuffer = new WaveBuffer(buffer);
			int samplesRequired = count / 4;
			int samplesRead = Read(waveBuffer.FloatBuffer, offset / 4, samplesRequired);
			return samplesRead * 4;
		}

		public abstract int Read(float[] buffer, int offset, int sampleCount);

		public WaveFormat WaveFormat
		{
			get { return waveFormat; }
		}
	}

	class SineWaveOscillator : WaveProvider16
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

	public class SineWaveProvider32 : WaveProvider32
	{
		double phaseAngle;

		public SineWaveProvider32()
		{
			Frequency = 1000;
			Amplitude = .1f;
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
