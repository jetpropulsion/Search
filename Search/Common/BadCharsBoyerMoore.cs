using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Search.Common
{
	//In reference literature and/or implementation, "BadCharsBase" is known as "bmBc"
	//Used by: Boyer-Moore and derivatives (Turbo BM, Tuned BM, Horspool, Raita, Zsu-Takaoka)
	public class BadCharsBoyerMoore
	{
		protected int[] BadChars;
		public BadCharsBoyerMoore(ReadOnlyMemory<byte> patternMemory)
		{
			ReadOnlySpan<byte> pattern = patternMemory.Span;

			const int MaxAlphabetSize = 256;

			//Allocate
			this.BadChars = new int[MaxAlphabetSize];

			//Init
			int m = pattern.Length;
			for (int i = 0; i < this.BadChars.Length; ++i)
			{
				this.BadChars[i] = m;
			}
			for (int i = 0; i < m - 1; ++i)
			{
				this.BadChars[pattern[i]] = m - i - 1;
			}
		}

		public virtual int this[int index]
		{
			get
			{
				if (index < 0 || index > this.BadChars.Length)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				return this.BadChars[index];
			}
			protected set
			{
				if (index < 0 || index > this.BadChars.Length)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				this.BadChars[index] = value;
			}
		}
	};

};


