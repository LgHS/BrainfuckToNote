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

	class SynthRecorder
	{

		string outFile = "@export2.wav";
		public MixingSampleProvider mixer { get; private set; }
		public WaveRecorder recorder { get; private set; }
		public VolumeSampleProvider volumeSampleProvider { get; private set; }

		public SynthRecorder(List<ISampleProvider> synth)
		{

			mixer = new MixingSampleProvider(synth);
			mixer.ReadFully = true;

			recorder = new WaveRecorder(mixer.ToWaveProvider(), "@test.wav");

			volumeSampleProvider = new VolumeSampleProvider(mixer);
			volumeSampleProvider.Volume = 1.0f;
		}
	}

	class Synth
	{
		SynthRecorder recorder;

		private WaveOut sineOut = new WaveOut();
		private WaveOut sawOut = new WaveOut();
		private WaveOut triangleOut = new WaveOut();
		private WaveOut squareOut = new WaveOut();

		public SineWaveProvider32 sineWave = new SineWaveProvider32();
		public SawWaveProvider32 sawWave = new SawWaveProvider32();
		public SquareWaveProvider32 squareWave = new SquareWaveProvider32();
		public TriangleWaveProvider32 triangleWave = new TriangleWaveProvider32();

		float freq;
		bool playing = false;
		WaveType wave = WaveType.sine;

		~Synth()
		{
		}

		public Synth()
		{
		}

		public void SetRecorder(SynthRecorder recorder)
		{
			this.recorder = recorder;
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
			sineOut.Init(recorder.volumeSampleProvider);
			sineOut.Init(recorder.recorder);

			sawWave.SetWaveFormat(44100, 2);
			sawWave.Frequency = (float)0;
			sawWave.Amplitude = .3f;
			sawOut.Init(sawWave);
			sawOut.Init(recorder.volumeSampleProvider);
			sawOut.Init(recorder.recorder);

			squareWave.SetWaveFormat(44100, 2);
			squareWave.Frequency = (float)0;
			squareWave.Amplitude = .3f;
			squareOut.Init(squareWave);
			squareOut.Init(recorder.volumeSampleProvider);
			squareOut.Init(recorder.recorder);

			triangleWave.SetWaveFormat(44100, 2);
			triangleWave.Frequency = (float)0;
			triangleWave.Amplitude = 1.0f;
			triangleOut.Init(triangleWave);
			triangleOut.Init(recorder.volumeSampleProvider);
			triangleOut.Init(recorder.recorder);
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