﻿using System;
using System.Text;
using Search.Interfaces;
using Search.Common;

namespace Search
{
	/// <summary>
	//	name:										Boyer-Moore algorithm
	//	direction:							right to left
	//	preprocess complexity:	O(m+s) time and space
	//	search complexity:			O(mn) time
	//	worst case:							3n text character comparisons (when searching non-periodic pattern)
	//	ref:										BOYER R.S., MOORE J.S., 1977, A fast string searching algorithm. Communications of the ACM. 20:762-772.
	/// </summary>
	public class BoyerMoore : ISearch
	{
		protected BadCharsBoyerMoore BadChars;
		protected GoodSuffixesBoyerMoore GoodSuffixes;

		public BoyerMoore(ReadOnlySpan<byte> pattern)
		{
			this.Init(pattern);
		}
		public void Init(ReadOnlySpan<byte> pattern)
		{
			this.GoodSuffixes = new GoodSuffixesBoyerMoore(pattern);
			this.BadChars = new BadCharsBoyerMoore(pattern);
		}

		public virtual void Search(ReadOnlySpan<byte> pattern, ReadOnlySpan<byte> buffer, int offset, ISearch.Found found)
		{
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
	};  //END: public class BoyerMoore

};  //END: namespace Search

