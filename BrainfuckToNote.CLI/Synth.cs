using NAudio.MediaFoundation;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BrainfuckToNote
{
	public enum WaveType
	{
		sine,
		saw,
		square,
		triangle
	}

	class Synth
	{

		string outFile = "@export2.wav";
		MixingSampleProvider mixer;
		WaveRecorder recorder;
		private WaveOut sineOut = new WaveOut();
		private WaveOut sawOut = new WaveOut();
		private WaveOut triangleOut = new WaveOut();
		private WaveOut squareOut = new WaveOut();

		SineWaveProvider32 sineWave = new SineWaveProvider32();
		SawWaveProvider32 sawWave = new SawWaveProvider32();
		SquareWaveProvider32 squareWave = new SquareWaveProvider32();
		TriangleWaveProvider32 triangleWave = new TriangleWaveProvider32();

		float freq;
		bool playing = false;
		WaveType wave = WaveType.triangle;

		~Synth()
		{
		}

		public Synth()
		{
			mixer = new MixingSampleProvider(new List<ISampleProvider> { sineWave.ToSampleProvider(), sawWave.ToSampleProvider(), squareWave.ToSampleProvider(), triangleWave.ToSampleProvider() });
			recorder = new WaveRecorder(mixer.ToWaveProvider(), "@test.wav");
			InitWaves();
			mixer.ReadFully = true;

			//WaveFormat waveFormat = new WaveFormat(mixer.WaveFormat.SampleRate, mixer.WaveFormat.BitsPerSample, mixer.WaveFormat.Channels);
			//WaveFileWriter.CreateWaveFile(outFile, mixer.ToWaveProvider());

			/*WaveFileWriter.CreateWaveFile(outFile, triangleWave);
			WaveFileWriter.CreateWaveFile(outFile, sineWave);
			WaveFileWriter.CreateWaveFile(outFile, sawWave);
			WaveFileWriter.CreateWaveFile(outFile, squareWave);*/
		}

		public void SetSynthType(WaveType wave)
		{
			this.wave = wave;
		}

		public void StopWave()
		{
			sineOut.Stop();
			sawOut.Stop();
			squareOut.Stop();
			triangleOut.Stop();
			playing = false;
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
				List<string> notes = new List<string>() { "A", "A#", "B", "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#" };

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

				double result = 440 * Math.Pow(2, (keyNumber - 49) / 12);

				Console.WriteLine(result);

				return result;
			}

		public void PlayWave(double _frequency)
		{
			switch(wave)
			{
				case WaveType.sine:
					{
						sineWave.Frequency = (float)_frequency;

						if (sineOut.PlaybackState == PlaybackState.Stopped)
							sineOut.Play();
						else
							sineOut.Resume();

						break;
					}
				case WaveType.saw:
					{
						sawWave.Frequency = (float)_frequency;

						if (sawOut.PlaybackState == PlaybackState.Stopped)
							sawOut.Play();
						else
							sawOut.Resume();
						break;
					}
				case WaveType.square:
					{
						squareWave.Frequency = (float)_frequency;

						if (squareOut.PlaybackState == PlaybackState.Stopped)
							squareOut.Play();
						else
							squareOut.Resume();
						break;
					}
				case WaveType.triangle:
					{
						triangleWave.Frequency = (float)_frequency;

						if (triangleOut.PlaybackState == PlaybackState.Stopped)
							triangleOut.Play();
						else
							triangleOut.Resume();
						break;
					}
			}
			playing = true;
		}

		public void InitWaves()
		{
			sineWave.SetWaveFormat(44100, 2);
			sineWave.Frequency = (float)0;
			sineWave.Amplitude = .9f;
			sineOut.Init(sineWave);
			sineOut.Init(recorder);

			sawWave.SetWaveFormat(44100, 2);
			sawWave.Frequency = (float)0;
			sawWave.Amplitude = .3f;
			sawOut.Init(sawWave);
			sawOut.Init(recorder);

			squareWave.SetWaveFormat(44100, 2);
			squareWave.Frequency = (float)0;
			squareWave.Amplitude = .3f;
			squareOut.Init(squareWave);
			squareOut.Init(recorder);

			triangleWave.SetWaveFormat(44100, 2);
			triangleWave.Frequency = (float)0;
			triangleWave.Amplitude = 1f;
			triangleOut.Init(triangleWave);
			triangleOut.Init(recorder);

		}

		public void SetFrequency(float _freq)
		{
			switch (wave)
			{
				case WaveType.saw:
					sawWave.Frequency = _freq;
					break;
				case WaveType.sine:
					sineWave.Frequency = _freq;
					break;
				case WaveType.square:
					squareWave.Frequency = _freq;
					break;
				case WaveType.triangle:
					triangleWave.Frequency = _freq;
					break;
			}
		}
	}
}