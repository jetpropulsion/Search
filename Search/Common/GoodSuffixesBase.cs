using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Search.Common
{
	//In reference literature and/or implementation, "GoodSuffixesBase" is known as "bmGs"
	public class GoodSuffixesBoyerMoore : SuffixesBase
	{
		public readonly int[] GoodSuffixes;
		public GoodSuffixesBoyerMoore(in ReadOnlySpan<byte> pattern) : base(pattern)
		{
			int m = pattern.Length;

			this.GoodSuffixes = new int[m];

			int mm1 = m - 1;
			int mm2 = m - 2;

			for (int i = 0; i < m; ++i)
			{
				this.GoodSuffixes[i] = m;
			}

			int j = 0;
			for (int i = mm1; i >= 0; --i)
			{
				if (base.Suffixes[i] == i + 1)
				{
					while (j < mm1 - i)
					{
						if (this.GoodSuffixes[j] == m)
						{
							this.GoodSuffixes[j] = mm1 - i;
						}
						++j;
					}
				}
			}

			for (int i = 0; i <= mm2; ++i)
			{
				this.GoodSuffixes[mm1 - base.Suffixes[i]] = mm1 - i;
			}
		}

		public override int this[int index]
		{
			get
			{
				if (index < 0 || index > this.GoodSuffixes.Length) throw new ArgumentOutOfRangeException("index");
				return this.GoodSuffixes[index];
			}
			protected set
			{
				if (index < 0 || index > this.GoodSuffixes.Length) throw new ArgumentOutOfRangeException("index");
				this.GoodSuffixes[index] = value;
			}
		}
	};

};

