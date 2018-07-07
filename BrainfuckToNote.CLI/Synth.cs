using BackFuck;
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
		string wave = "saw";

		public Synth()
		{
			InitWaves();

		}

		public void StopWave()
		{
			//sineWave.envelope.Gate(false);
			sineOut.Stop();
			sawOut.Stop();
			squareOut.Stop();
			triangleOut.Stop();
			playing = false;


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
}