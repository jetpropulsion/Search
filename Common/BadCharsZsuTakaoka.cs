﻿namespace Search.Common
{
	using Search.Interfaces;

	using System.Runtime.CompilerServices;

	//public class BadCharsZsuTakaoka
	//In reference literature and/or implementation, "BadCharsZsuTakaoka" is known as "ztBc"
	//Used by: Zsu-Takaoka
	public class BadCharsZsuTakaoka
	{
		public readonly int[,] BadChars;

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public BadCharsZsuTakaoka(in ReadOnlySpan<byte> pattern)
		{
			int maxAlphabet = ISearch.MaxAlphabetSize;

			//Allocate
			this.BadChars = new int[maxAlphabet, maxAlphabet];

			//Init
			int m = pattern.Length;
			int mm1 = m - 1;

			for (int i = 0; i < maxAlphabet; ++i)
			{
				for (int j = 0; j < maxAlphabet; ++j)
				{
					this.BadChars[i, j] = m;
				}
			}
			for (int i = 0; i < maxAlphabet; ++i)
			{
				this.BadChars[i, pattern[0]] = mm1;
			}
			for (int i = 1; i < mm1; ++i)
			{
				this.BadChars[pattern[i - 1], pattern[i]] = mm1 - i;
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
	};  //END: class BadCharsZsuTakaoka
};  //END: namespace Search
