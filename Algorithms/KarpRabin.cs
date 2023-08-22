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

	//R. M. Karp and M. O. Rabin.
	//Efficient randomized pattern-matching algorithms. ibmjrd, vol.31, n.2, pp.249--260, (1987).

	[Slow]
	public class KarpRabin : SearchBase
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
			int d, hx, hy;

			//Preprocessing
			for (d = i = 1; i < m; ++i)
			{
				d <<= 1;
			}

			for (hy = hx = i = 0; i < m; ++i)
			{
				hx = ((hx << 1) + pattern[i]);
				hy = ((hy << 1) + buffer[i]);
			}

			Func<int, int, int, int> rehash = (a, b, h) => ((h - a * d) << 1) + b;

			//Searching
			while (j <= n - m)
			{
				if(hx == hy	&& pattern.SequenceEqual(buffer.Slice(j, m)))
				{
					if(!this.OnMatchFound!(j, this.GetType()))
					{
						return;
					}
				}
				if (j + m >= size)
				{
					//NOTE: Fix, original was breaking the bounds on very last comparison
					break;
				}
				hy = rehash(buffer[j], buffer[j + m], hy);
				++j;
			}
		}
	};  //END: class KarpRabin
} //END: namespace
