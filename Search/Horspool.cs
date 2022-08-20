using System;
using System.Text;
using Search.Interfaces;
using Search.Common;

namespace Search
{
	public class Horspool : ISearch
	{
		protected BadCharsBoyerMoore BadChars;

		/// <summary>
		//	name:										Horspool algorithm
		//	direction:							any
		//	preprocess complexity:	O(m+s) time and O(s) space
		//	search complexity:			O(mn) time
		//	worst case:							n*n text character comparisons (quadratic worst case)
		//	ref:										HORSPOOL R.N., 1980, Practical fast searching in strings, Software - Practice & Experience, 10(6):501-506.
		/// </summary>
		public Horspool(ReadOnlySpan<byte> pattern)
		{
			this.Init(pattern);
		}
		public void Init(ReadOnlySpan<byte> pattern)
		{
			this.BadChars = new BadCharsBoyerMoore(pattern);
		}

		public virtual void Search(ReadOnlySpan<byte> pattern, ReadOnlySpan<byte> buffer, int offset, ISearch.Found found)
		{
			//Initialize
			Init(pattern);

			//Searching
			int m = pattern.Length;
			int mm1 = m - 1;
			int n = buffer.Length;
			int nmm = n - m;

			//Searching
			int j = 0;
			while (j <= nmm)
			{
				byte c = buffer[j + mm1];
				//if ((pattern[mm1] == c) && !memcmp(pattern, buffer + j, mm1))
				if ((pattern[mm1] == c) && pattern.SequenceEqual(buffer[j..(j+m)]))
				{
					if(!found(j))
					{
						return;
					}
				}
				j += this.BadChars[c];
			}
		}
	};  //END: public class Horspool

};  //END: namespace Search

