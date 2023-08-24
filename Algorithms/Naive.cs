namespace Search.Algorithms
{
	using Search.Attributes;
	using Search.Common;
	using Search.Interfaces;

	using System.Runtime.CompilerServices;

	//[Slow]
	public class Naive : SearchBase
	{
#if DEBUG
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
#else
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#endif
		public override void Search(in ReadOnlyMemory<byte> bufferMemory, int offset, int size)
		{
			this.Validate();

			//Searching
			ReadOnlySpan<byte> pattern = base.PatternSpan;
			ReadOnlySpan<byte> buffer = bufferMemory.Span;

			//Searching
			int m = pattern.Length;
			int n = size;

			int j = offset;
			Type type = this.GetType();
			while (j <= n - m)
			{
				if (pattern.SequenceEqual(buffer.Slice(j, m)))
				{
					if (!base.OnMatchFound!(j, type))
					{
						return;
					}
					j += m;
				}
				else
				{
					++j;
				}
			}
		}
	}
}
