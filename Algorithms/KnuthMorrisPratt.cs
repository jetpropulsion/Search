namespace Search.Algorithms
{
	using Search.Attributes;
	using Search.Common;
	using Search.Interfaces;

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.CompilerServices;
	using System.Text;
	using System.Threading.Tasks;

	[Slow]
	public class KnuthMorrisPratt : SearchBase
	{
		//protected BadCharsBoyerMoore? BadChars = null;
		protected KnuthMorrisPrattNext? Next = null;

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public override void Init(in ReadOnlyMemory<byte> patternMemory, ISearch.OnMatchFoundDelegate patternMatched)
		{
			base.Init(patternMemory, patternMatched);
			this.Next = new KnuthMorrisPrattNext(patternMemory.Span);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public override void Validate()
		{
			base.Validate();
			ArgumentNullException.ThrowIfNull(this.Next, nameof(this.Next));
		}

#if DEBUG
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
#else
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#endif
		public override void Search(in ReadOnlyMemory<byte> bufferMemory, int offset)
		{
			this.Validate();

			//Searching
			ReadOnlySpan<byte> pattern = this.PatternSpan;
			ReadOnlySpan<byte> buffer = bufferMemory.Span;

			int j = offset;
			int m = pattern.Length;
			int n = buffer.Length;

			int i = 0;
			while (j < n)
			{
				while (i > -1 && pattern[i] != buffer[j])
				{
					i = this.Next![i];
				}
				i++;
				j++;
				if (i >= m)
				{
					if(!this.OnPatternMatches!(j - i, this.GetType()))
					{
						return;
					}
					i = this.Next![i];
				}
			}
		}
	}
}
