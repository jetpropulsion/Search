namespace Search.Algorithms
{
	using Search.Common;
	using Search.Interfaces;

	using System.Runtime.CompilerServices;

	public class BruteForce : SearchBase
	{
#if DEBUG
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
#else
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#endif
		public override void Search(in ReadOnlyMemory<byte> bufferMemory, int offset)
		{
			this.Validate();

			//Searching
			ReadOnlySpan<byte> pattern = base.PatternSpan;
			ReadOnlySpan<byte> buffer = bufferMemory.Span;

			//Searching
			int m = pattern.Length;
			int n = buffer.Length;
			//int mm1 = m - 1;

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
