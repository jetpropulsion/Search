using Search.Common;
using Search.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Search.Interfaces.ISearch;

namespace Search
{
/*
	public class TunedBoyerMoore : ISearch
	{
		TunedBadCharsBase TunedBadChars;
		public TunedBoyerMoore()
		{
		}

		public virtual void Init(ReadOnlySpan<byte> pattern)
		{
			this.TunedBadChars = new TunedBadCharsBase(pattern);
		}

		public virtual void Search(ReadOnlySpan<byte> pattern, ReadOnlySpan<byte> buffer, int offset, ISearch.Found found)
		{
			//Initialize
			Init(pattern);

			//Searching
			int j;
			int k;
			int shift;
			int m = pattern.Length;
			int n = buffer.Length;
			int mm1 = m - 1;
			int mp1 = m + 1;
			int nmm = n - m;

			shift = this.TunedBadChars[ pattern[mm1] ];
			//this.TunedBadChars[ pattern[mm1] ] = 0;
			//memset(buffer + n, pattern[mm1], m);

			j = 0;
			while (j <= nmm)
			{
				k = this.TunedBadChars[ buffer[j + mm1] ];
				while (k != 0)
				{
					j += k;
					k = this.TunedBadChars[ buffer[j + mm1] ];
					j += k;
					k = this.TunedBadChars[ buffer[j + mm1] ];
					j += k;
					k = this.TunedBadChars[ buffer[j + mm1] ];
				}
				if(pattern.SequenceEqual(buffer.Slice(j, m)) && j <= nmm)
				//if (memcmp(pattern, buffer + j, mm1) == 0 && j <= nmm)
				{
					if (j <= nmm)
					{
						if(found(j))
						{
							return;
						}
					}
				}
				j += shift;
			}

		}   //END: void Search()
	};  //END: class TunedBoyerMoore
*/
};  //END: namespace Search
