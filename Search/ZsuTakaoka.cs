using Search.Common;
using Search.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Search
{
	public class ZsuTakaoka : ISearch
	{
		protected GoodSuffixesBase GoodSuffixes;
		protected BadCharsZsuTakaoka BadChars;

		public virtual void Init(ReadOnlySpan<byte> pattern)
		{
			this.GoodSuffixes = new GoodSuffixesBase(pattern); 
			this.BadChars = new BadCharsZsuTakaoka(pattern);
		}

		public virtual void Search(ReadOnlySpan<byte> pattern, ReadOnlySpan<byte> buffer, int offset, ISearch.Found found)
		{
			//Initialize
			this.Init(pattern);

			//Searching
			int m = pattern.Length;
			int n = buffer.Length;

			int mm1 = m - 1;
			int mm2 = m - 2;

			//Searching
			int j = 0;
			while (j <= n - m)
			{
				int i = mm1;
				/*
					The following while loop was originally
					written as follows:

						while( (x[i] == y[i + j])  && (i >= 0) )
						{
							--i;
						}

					This is something that I've changed, in order
					to be able to check indices before any comparison.
				*/
				while (((uint)i < (uint)m) && (pattern[i] == buffer[i + j]))
				{
					--i;
				}
				if (i < 0)
				{
					if(!found(j))
					{
						return;
					}
					j += this.GoodSuffixes[0];
				}
				else
				{
					j += Math.Max(this.GoodSuffixes[i], this.BadChars[ buffer[j + mm2], buffer[j + mm1] ]);
				}
			}

		}
	};  //END: class ZsuTakaoka
};  //END: namespace Search
