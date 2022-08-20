using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Search.Common
{
	//public class BadCharsZsuTakaoka
	//In reference literature and/or implementation, "BadCharsZsuTakaoka" is known as "ztBc"
	//Used by: Zsu-Takaoka
	public class BadCharsZsuTakaoka
	{
		protected int[,] BadChars;
		public BadCharsZsuTakaoka(ReadOnlyMemory<byte> patternMemory)
		{
			const int MaxAlphabetSize = 256;

			//Allocate
			this.BadChars = new int[MaxAlphabetSize, MaxAlphabetSize];

			//Init
			ReadOnlySpan<byte> pattern = patternMemory.Span;
			int m = pattern.Length;
			int mm1 = m - 1;

			for (int i = 0; i < MaxAlphabetSize; ++i)
			{
				for (int j = 0; j < MaxAlphabetSize; ++j)
				{
					this.BadChars[i, j] = m;
				}
			}
			for (int i = 0; i < MaxAlphabetSize; ++i)
			{
				this.BadChars[i, pattern[0]] = mm1;
			}
			for (int i = 1; i < mm1; ++i)
			{
				this.BadChars[ pattern[i - 1], pattern[i] ] = mm1 - i;
			}
		}

		public virtual int this[int x, int y]
		{
			get
			{
				if (x < 0 || x > this.BadChars.GetLength(0))
				{
					throw new ArgumentOutOfRangeException("x");
				}
				if (y < 0 || y > this.BadChars.GetLength(1))
				{
					throw new ArgumentOutOfRangeException("y");
				}
				return this.BadChars[x, y];
			}
		}
	};  //END: class BadCharsZsuTakaoka
};	//END: namespace Search
