using System;
using System.Text;
using Search.Interfaces;
using Search.Common;
using System.Text.RegularExpressions;

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
	public class Raita : ISearch
	{
		protected BadCharsBoyerMoore BadChars;

		public Raita()
		{
		}
		public virtual void Init(ReadOnlyMemory<byte> patternMemory)
		{
			this.BadChars = new BadCharsBoyerMoore(patternMemory);
		}

		public virtual void Search(ReadOnlyMemory<byte> patternMemory, ReadOnlyMemory<byte> bufferMemory, int offset, ISearch.Found found)
		{
			ReadOnlySpan<byte> pattern = patternMemory.Span;
			ReadOnlySpan<byte> buffer = bufferMemory.Span;

			//Searching
			int j = offset;
			int m = pattern.Length;
			int n = buffer.Length;
			int mm1 = m - 1;
			int mm2 = mm1 - 1;
			int mr1 = m >> 1;

			byte first = pattern[0];
			byte middle = pattern[mr1];
			byte last = pattern[mm1];

			ReadOnlySpan<byte> patternCut = pattern[1..mm2];

			while (j <= n - m)
			{
				byte c = buffer[j + mm1];

				if ((last == c) &&
						(middle == buffer[j + mr1]) &&
						(first == buffer[j]) &&
						patternCut.SequenceEqual(buffer[(j + 1)..(j + mm2)])
				)
				{
					if(!found(j))
					{
						return;
					}
				}

				j += this.BadChars[c];
			}
		}
	};  //END: public class Raita

};  //END: namespace Search

