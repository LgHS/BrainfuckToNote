/*
 * FORKED FROM by https://HelloACM.com Dr Zhihua Lai
*/
using System;
using System.Collections.Generic;
using System.IO;

namespace BackFuck
{

	class BrainfuckToMusicInterpreter
	{
		private static string VER = "0.0.0.1";
		private static readonly int BUFSIZE = 65535;
		private int[] buf = new int[BUFSIZE];
		private int ptr { get; set; }
		private bool echo { get; set; }

		public BrainfuckToMusicInterpreter()
		{
			this.ptr = 0;
			this.Reset();
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

		public void Interpret(string s)
		{
			List<Note> notes = new List<Note>();
			int i = 0;
			int right = s.Length;
			Note note = new Note();
			while (i < right)
			{

				switch (s[i])
				{
					case '>':
						{
							note.Pitch = Notes.Do;
							break;
						}
					case '<':
						{
							note.Pitch = Notes.Re;
							break;
						}
					case '.':
						{
							note.Pitch = Notes.Mi;
							break;
						}
					case '+':
						{
							note.Pitch = Notes.Fa;
							break;
						}
					case '-':
						{
							note.Pitch = Notes.Sol;
							break;
						}
					case ',':
						{
							note.Pitch = Notes.La;
							break;
						}
					case '[':
					case ']':
						{
							note.Pitch = Notes.Si;
							break;
						}
				}

				note.Ocatve += 1;

				if (i > 1 && s[i] != s[i - 1])
				{
					notes.Add(note);
					note = new Note();
				}
				i++;
			}

			foreach(var n in notes)
			{
				Console.WriteLine("{0}{1}", n.Pitch, n.Ocatve);
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