using Search.Common;
using Search.Interfaces;

namespace Search.Algorithms
{
	public class BruteForce : SearchBase
	{
		public BruteForce() : base()
		{
		}
		public BruteForce(ReadOnlyMemory<byte> patternMemory, ISearch.OnMatchFoundDelegate patternMatched) : base(patternMemory, patternMatched)
		{
		}

		public override void Search(in ReadOnlyMemory<byte> bufferMemory, int offset)
		{
			this.Validate();

			//Searching
			ReadOnlySpan<byte> pattern = base.PatternSpan;
			ReadOnlySpan<byte> buffer = bufferMemory.Span;

			//Searching
			int m = pattern.Length;
			int n = buffer.Length;
			int mm1 = m - 1;

			for (int j = offset; j <= n - m; ++j)
			{
				int i = 0;
				while (i < m && pattern[i] == buffer[i + j])
				{
					++i;
				}
				if (i >= m)
				{
					if (!base.OnPatternMatches!(j, this.GetType()))
					{
						return;
					}
				}
			}
		}
	}
}
