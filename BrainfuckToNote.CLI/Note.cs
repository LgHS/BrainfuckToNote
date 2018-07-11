using System;
using System.Collections.Generic;

namespace BrainfuckToNote
{
	public enum Alteration
	{
		Sharp, Flat, None
	}

	public enum Notes
	{
		Do = 1, Do_Diese = 2, Re = 3, Re_Diese = 4, Mi = 5, Fa = 6, Fa_Diese = 7, Sol = 8, Sol_Diese = 9, La = 10, La_Diese = 11, Si = 12
	}

	public class Note
	{
		public Notes Pitch;
		public Alteration Alteration;
		public int Ocatve = 0;

		public double GetFrequency()
		{
			var ReferenceNote = new Note { Pitch = Notes.Do, Ocatve = 4 };

			if (this.Alteration == Alteration.Flat)
			{
				if (this.Pitch == Notes.Do) this.Pitch = Notes.Si;
				else
				{
					this.Pitch -= 1;
					this.Alteration = Alteration.Sharp;
				}
			}

			List<string> notes = new List<string>() { "A", "A#", "B", "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#" };

			string stringNote = String.Format("{0}{1}", Pitch, Alteration == Alteration.Sharp ? "#" : "");
			
			int keyNumber = notes.IndexOf(stringNote);

			if (keyNumber < 3)
			{
				keyNumber = keyNumber + 12 + ((Ocatve - 1) * 12) + 1;
			}
			else
			{
				keyNumber = keyNumber + 12 + ((Ocatve - 1) * 12) + 1;
			}

			double result = 440 * Math.Pow(1.059463, -GetMargin(ReferenceNote, this));

			return result;
		}

		int GetMargin(Note a, Note b)
		{
			int octaveDelta = Math.Abs(a.Ocatve - b.Ocatve);
			int noteDelta = Math.Abs(a.Pitch - b.Pitch);
			return (octaveDelta * 12) + noteDelta;
		}
	}
}