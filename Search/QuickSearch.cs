using Search.Common;
using Search.Interfaces;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Search
{
	public class QuickSearch : ISearch
	{
		BadCharsBoyerMoore BadChars;
		public QuickSearch()
		{
		}

		void ISearch.Init(ReadOnlyMemory<byte> patternMemory)
		{
			this.BadChars = new BadCharsBoyerMoore(patternMemory);
		}

		void ISearch.Search(ReadOnlyMemory<byte> patternMemory, ReadOnlyMemory<byte> bufferMemory, int offset, ISearch.Found found)
		{
			ReadOnlySpan<byte> pattern = patternMemory.Span;
			ReadOnlySpan<byte> buffer = bufferMemory.Span;

			int m = pattern.Length;
			int n = buffer.Length;
			int mm1 = m - 1;
			int mp1 = m + 1;

			int j = offset;
			int nmm = n - m;
			while (j <= nmm)
			{
				if(pattern.SequenceEqual(buffer[j..(j + m)]))
				{
					if (!found(j))
					{
						return;
					}
				}
				j += this.BadChars[buffer[j + m - 1]];
			}
		}
	}
}
