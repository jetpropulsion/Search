using Search.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Search
{
	public class BruteForce : ISearch
	{
		public BruteForce()
		{
		}

		void ISearch.Init(ReadOnlyMemory<byte> patternMemory)
		{
		}

		void ISearch.Search(ReadOnlyMemory<byte> patternMemory, ReadOnlyMemory<byte> bufferMemory, int offset, ISearch.Found found)
		{
			ReadOnlySpan<byte> pattern = patternMemory.Span;
			ReadOnlySpan<byte> buffer = bufferMemory.Span;

			//Searching
			int m = pattern.Length;
			int n = buffer.Length;
			int mm1 = m - 1;

			for (int j = offset; j <= n - m; ++j)
			{
				int i = 0;
				while( i < m && pattern[i] == buffer[i + j])
				{
					++i;
				}
				if (i >= m)
				{
					if(!found(j))
					{
						return;
					}
				}
			}
		}
	}
}
