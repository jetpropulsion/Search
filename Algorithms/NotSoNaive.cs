namespace Search.Algorithms
{
	using Search.Common;
	using Search.Interfaces;

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.CompilerServices;
	using System.Text;
	using System.Threading.Tasks;

	//[Experimental(nameof(NotSoNaive))]
	public class NotSoNaive : SearchBase
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public NotSoNaive() :
			base()
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public NotSoNaive(in ReadOnlyMemory<byte> patternMemory, ISearch.OnMatchFoundDelegate patternMatched) :
			base(patternMemory, patternMatched)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public override void Init(in ReadOnlyMemory<byte> patternMemory, ISearch.OnMatchFoundDelegate patternMatched)
		{
			base.Init(patternMemory, patternMatched);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public override void Validate()
		{
			base.Validate();
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
						if(!this.OnPatternMatches!(j, this.GetType()))
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
