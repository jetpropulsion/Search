using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Search.Common
{
	//In reference literature and/or implementation, "SuffixesBase" is known as "suff"
	public class SuffixesBase
	{
		protected int[] Suffixes;

		public SuffixesBase(ReadOnlySpan<byte> pattern)
		{
			int m = pattern.Length;

			//Allocate
			this.Suffixes = new int[m];

			int mm1 = m - 1;
			int mm2 = m - 2;
			int f = 0;
			int g = mm1;
			this.Suffixes[mm1] = m;

			for (int i = mm2; i >= 0; --i)
			{
				if ((i > g) && (this.Suffixes[i + mm1 - f] < i - g))
				{
					this.Suffixes[i] = this.Suffixes[i + mm1 - f];
				}
				else
				{
					if (i < g)
					{
						g = i;
					}
					f = i;
					while ((g >= 0) && (pattern[g] == pattern[g + mm1 - f]))
					{
						--g;
					}
					this.Suffixes[i] = f - g;
				}
			}
		}

		public virtual int this[int index]
		{
			get
			{
				if (index < 0 || index > this.Suffixes.Length)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				return this.Suffixes[index];
			}
			protected set
			{
				if (index < 0 || index > this.Suffixes.Length)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				this.Suffixes[index] = value;
			}
		}
	};
};


