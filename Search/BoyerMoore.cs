using System;
using System.Text;
using Search.Interfaces;
using Search.Common;

namespace Search
{
	public class BoyerMoore : ISearch
	{
		protected BadCharsBase BadChars;
		protected GoodSuffixesBase GoodSuffixes;

		public BoyerMoore()
		{
		}
		public virtual void Init(ReadOnlySpan<byte> pattern)
		{
			this.GoodSuffixes = new GoodSuffixesBase(pattern);
			this.BadChars = new BadCharsBase(pattern);
		}

		public virtual void Search(ReadOnlySpan<byte> pattern, ReadOnlySpan<byte> buffer, int offset, ISearch.Found found)
		{
			//Initialize
			Init(pattern);

			//Searching
			int j = offset;
			int m = pattern.Length;
			int n = buffer.Length;
			int mm1 = m - 1;
			int mp1 = m + 1;

			while(j <= n - m)
			{
				int i = mm1;
				while(i >= 0 && pattern[i] == buffer[i + j])
				{
					--i;
				}
				if(i < 0)
				{
					if(!found(j))
					{
						return;
					}

					j += this.GoodSuffixes[0];
				}
				else
				{
					j += Math.Max(this.GoodSuffixes[i], this.BadChars[ buffer[i + j] ] - mp1 + i);
				}
			}
		}
	};	//END: public class CTXBM

};  //END: namespace Search

