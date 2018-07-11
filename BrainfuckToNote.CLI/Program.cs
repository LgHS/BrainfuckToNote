using BrainfuckToNote;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace BrainfuckToNote
{

	class Harmony
	{
	}

	class BrainfuckToMusicInterpreter
	{
		private const int APPCOMMAND_VOLUME_MUTE = 0x80000;
		private const int WM_APPCOMMAND = 0x319;

		[DllImport("user32.dll")]
		public static extern IntPtr SendMessageW(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

		private static string VER = "0.0.0.1";
		private static readonly int BUFSIZE = 65535;
		private int[] buf = new int[BUFSIZE];
		private int ptr { get; set; }
		private bool echo { get; set; }

		CachedSound kick;

		public BrainfuckToMusicInterpreter()
		{
			this.ptr = 0;
			this.Reset();
			kick = new CachedSound("Kick 001 Apollo.wav");
		}

		public static void PrintHelp()
		{
			Console.WriteLine("BachFuck Interpreter " + VER);
			Console.WriteLine("Parameter: -h: Print Help");
			Console.WriteLine("Parameter: -e: Enable Echo Input Text");
			Console.WriteLine("Parameter: -d: Disable Echo Input Text");
			Console.WriteLine("Parameter: -p: Enable Keyboard Input");
			Console.WriteLine("Parameter: -v: Print Version");
			Console.WriteLine("Parameter: FileName");
		}

		public void Reset()
		{
			Array.Clear(this.buf, 0, this.buf.Length);
		}

		public Notes GetPitchA(string s, int i)
		{
			switch (s[i])
			{
				case '>':
					return Notes.Do;
				case '<':
					return Notes.Re_Diese;
				case '.':
					return Notes.Sol;
				case '+':
					return Notes.Do;
				case '-':
					return Notes.Re_Diese;
				case ',':
					return Notes.Sol;
				case '[':
				case ']':
				default:
					return Notes.Do;
			}
		}

		public Notes GetPitchB(string s, int i)
		{
			switch (s[i])
			{
				case '>':
					return Notes.Do;
				case '<':
					return Notes.Fa;
				case '.':
					return Notes.Sol_Diese;
				case '+':
					return Notes.Do;
				case '-':
					return Notes.Fa;
				case ',':
					return Notes.Sol_Diese;
				case '[':
				case ']':
				default:
					return Notes.Do;
			}
		}

		public Notes GetPitchC(string s, int i)
		{
			switch (s[i])
			{
				case '>':
					return Notes.Si;
				case '<':
					return Notes.Re;
				case '.':
					return Notes.Fa;
				case '+':
					return Notes.Si;
				case '-':
					return Notes.Re;
				case ',':
					return Notes.Fa;
				case '[':
				case ']':
				default:
					return Notes.Si;
			}
		}

		public List<Note> Parse(string s)
		{
			List<Note> notes = new List<Note>();
			int i = 0;
			int harmony = 0;
			int right = s.Length;
			Note note = new Note();
			note.Ocatve = 3;
			while (i<right)
			{
				if(harmony % 4 == 0)
					note.Pitch = GetPitchA(s, i);
				if (harmony % 4 == 1)
					note.Pitch = GetPitchB(s, i);
				if (harmony % 4 == 2)
					note.Pitch = GetPitchC(s, i);
				if (harmony % 4 == 3)
					note.Pitch = GetPitchA(s, i);

				if (i % 16 == 0) harmony += 1;
				if (harmony == 5) harmony = 0;

				if (i > 0 && i < right - 1 && s[i] == s[i+1])
					note.Ocatve += 1;

				if (note.Ocatve > 8)
					note.Ocatve = 8;
				if (note.Ocatve < 3)
					note.Ocatve = 3;

				if (i > 1 && s[i] != s[i - 1])
				{
					notes.Add(note);
					note = new Note();
				}
				i++;
			}
			return notes;
		}

		Stopwatch watch;

		public void Interpret(string s)
		{
			var notes = Parse(s);
			int i = 0;
			watch = new Stopwatch();
			
			Synth synth = new Synth();

			SynthRecorder recorer = new SynthRecorder(new List<ISampleProvider> { synth.sineWave.ToSampleProvider(), synth.sawWave.ToSampleProvider(), synth.squareWave.ToSampleProvider(), synth.triangleWave.ToSampleProvider() });

			synth.SetRecorder(recorer);
			synth.InitWaves();

			synth.SetSynthType(WaveType.saw);
			foreach (var n in notes)
			{
				watch.Start();
				watch.Reset();		
				Console.WriteLine("{0} - {1} - {2}", n.Pitch, n.Ocatve, n.GetFrequency());
				synth.PlayWave(n.GetFrequency());
				if (i % 4 == 0) AudioPlaybackEngine.Instance.PlaySound(kick);
				i++;

				watch.Stop();
				if ((int)watch.ElapsedMilliseconds > 100) Debugger.Break();

				int waitTime = 100 - (int)watch.ElapsedMilliseconds;

				Thread.Sleep(waitTime);
				synth.StopWave();
			}
		}

		static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				Console.WriteLine("Try -h, Thanks!");
			}
			else
			{
				BrainfuckToMusicInterpreter bf = new BrainfuckToMusicInterpreter();
				foreach (string s in args)
				{
					if (s[0] == '-') // switch options
					{
						for (int i = 1; i < s.Length; i++)
						{
							switch (s[i])
							{
								case 'h':
									{
										PrintHelp();
										break;
									}
								case 'd':
									{
										bf.echo = false;
										break;
									}
								case 'v':
									{
										Console.WriteLine(VER);
										break;
									}
								case 'e':
									{
										bf.echo = true;
										break;
									}
								case 'p':
									{
										string src = Console.In.ReadToEnd();
										bf.Interpret(src);
										break;
									}
							}
						}
					}
					else
					{
						if (File.Exists(s))
						{
							bf.Interpret(File.ReadAllText(s));
						}
						else
						{
							Console.WriteLine("File Open Error: " + s);
						}
					}
				}
			}

			Console.ReadKey();
		}
	}
}