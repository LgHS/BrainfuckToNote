namespace BackFuck
{
	public enum Notes
	{
		Do = 1, Re = 2, Mi = 3, Fa = 4, Sol = 5, La = 6, Si = 7
	}

	public class Note
	{
		public Notes Pitch;
		public Alteration Alteration;
		public int Ocatve = 0;
	}
}