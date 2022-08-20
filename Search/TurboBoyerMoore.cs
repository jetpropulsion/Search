using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Search.Interfaces;

namespace Search
{
	public class TurboBoyerMoore : BoyerMoore
	{
		public override void Search(ReadOnlySpan<byte> pattern, ReadOnlySpan<byte> buffer, int offset, ISearch.Found found)
		{
			//Initialize
			base.Init(pattern);

			//Searching
			int m = pattern.Length;
			int n = buffer.Length;

			int bcShift;
			int i;
			int shift;
			int v;
			int turboShift;
			int mm1 = m - 1;
			int mp1 = m + 1;
			int nmm = n - m;

			//Searching
			int j = offset;
			int u = offset;		//TODO: test, it was 0
			shift = m;
			while (j <= nmm)
			{
				i = m - 1;
				while ((i >= 0) && (pattern[i] == buffer[i + j]))
				{
					--i;
					if ((u != 0) && (i == mm1 - shift))
					{
						i -= u;
					}
				}
				if (i < 0)
				{
					if(!found(j))
					{
						return;
					}
					shift = this.GoodSuffixes[0];
					u = m - shift;
				}
				else
				{
					v = mm1 - i;
					turboShift = u - v;
					bcShift = this.BadChars[ buffer[i + j] ] - mp1 + i;
					shift = Math.Max(turboShift, bcShift);
					shift = Math.Max(shift, this.GoodSuffixes[i]);
					if (shift == this.GoodSuffixes[i])
					{
						u = Math.Min(m - shift, v);
					}
					else
					{
						if (turboShift < bcShift)
						{
							shift = Math.Max(shift, u + 1);
						}
						u = 0;
					}
				}
				j += shift;
			}
		}
	} //END: class TurboBoyerMoore
};

