namespace Search.Common
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	using Search.Interfaces;

	using System.Runtime.CompilerServices;

	//public class BadCharsBerryRavindran
	//In reference literature and/or implementation, "BadCharsBerryRavindran" is known as "brBc"
	//Used by: Zsu-Takaoka
	public class BadCharsBerryRavindran
	{
		public readonly int[,] BadChars;

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public BadCharsBerryRavindran(in ReadOnlySpan<byte> pattern)
		{
			int maxAlphabet = ISearch.MaxAlphabetSize;

			//Allocate
			this.BadChars = new int[maxAlphabet, maxAlphabet];

			//Init
			int m = pattern.Length;
			int mp2 = m + 2;
			int mp1 = m + 1;
			int mm1 = m - 1;

			for (int i = 0; i < maxAlphabet; ++i)
			{
				for (int j = 0; j < maxAlphabet; ++j)
				{
					this.BadChars[i, j] = mp2;
				}
			}
			for (int i = 0; i < maxAlphabet; ++i)
			{
				this.BadChars[i, pattern[0]] = mp1;
			}
			for (int i = 0; i < mm1; ++i)
			{
				this.BadChars[pattern[i], pattern[i + 1]] = m - i;
			}
			for (int i = 0; i < maxAlphabet; ++i)
			{
				this.BadChars[pattern[mm1], i] = 1;
			}
		}

		public virtual int this[int x, int y]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			get
			{
				return x < 0 || x > this.BadChars.GetLength(0)
					? throw new ArgumentOutOfRangeException("x")
					: y < 0 || y > this.BadChars.GetLength(1) ? throw new ArgumentOutOfRangeException("y") : this.BadChars[x, y];
			}
		}
	};  //END: class BadCharsBerryRavindran
};  //END: namespace Search
