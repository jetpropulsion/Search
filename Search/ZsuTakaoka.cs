using Search.Common;
using Search.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Search
{
	/// <summary>
	//	name:										Zhu-Takaoka algorithm
	//	search direction:				right to left
	//	preprocess complexity:	O(m+s*s) time and space
	//	search complexity:			O(mn) time
	//	worst case:							n*n text character comparisons (quadratic worst case)
	//	ref:										ZHU R.F., TAKAOKA T., 1987, On improving the average case of the Boyer-Moore string matching algorithm, Journal of Information Processing 10(3):173-177.
	/// </summary>
	public class ZsuTakaoka : ISearch
	{
		protected GoodSuffixesBoyerMoore GoodSuffixes;
		protected BadCharsZsuTakaoka BadChars;

		public ZsuTakaoka()
		{
		}
		public virtual void Init(ReadOnlyMemory<byte> patternMemory)
		{
			this.GoodSuffixes = new GoodSuffixesBoyerMoore(patternMemory);
			this.BadChars = new BadCharsZsuTakaoka(patternMemory);
		}

		public virtual void Search(ReadOnlyMemory<byte> patternMemory, ReadOnlyMemory<byte> bufferMemory, int offset, ISearch.Found found)
		{
			//Searching
			ReadOnlySpan<byte> pattern = patternMemory.Span;
			ReadOnlySpan<byte> buffer = bufferMemory.Span;

			int m = pattern.Length;
			int n = buffer.Length;

			int mm1 = m - 1;
			int mm2 = m - 2;

			//Searching
			int j = 0;
			while (j <= n - m)
			{
				int i = mm1;
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
