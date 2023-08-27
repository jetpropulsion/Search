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

	//C. Hancart. Analyse exacte et en moyenne d'algorithmes de recherche d'un motif dans un texte. (1993).

	[Experimental]
	[Slow]
	public class NotSoNaive : SearchBase
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
			ReadOnlySpan<byte> pattern = this.PatternSpan;
			ReadOnlySpan<byte> buffer = bufferMemory.Span;

			int j = offset;
			int m = pattern.Length;
			int n = size;

			int i = 0;

			int k;
			int ell;

			//Preprocessing
			if (pattern[0] == pattern[1])
			{
				k = 2;
				ell = 1;
			}
			else
			{
				k = 1;
				ell = 2;
			}

			Type type = this.GetType();

			//Searching
			while (j <= n - m)
			{
				if (pattern[1] != buffer[j + 1])
				{
					j += k;
				}
				else
				{
					//if (memcmp(x + 2, y + j + 2, m - 2) == 0 && x[0] == y[j])
					if (pattern[2..].Slice(0, m - 2).SequenceEqual(buffer[(j + 2)..].Slice(0, m - 2)) && pattern[0] == buffer[j])
					{
						if(!this.OnMatchFound!(j, type))
						{
							return;
						}
					}
					j += ell;
				}
			}
		}
	};  //END: class MorrisPratt
} //END: namespace
