namespace Search.Algorithms
{
	using Search.Common;
	using Search.Interfaces;

	using System.Runtime.CompilerServices;

	public class Horspool : SearchBase
	{
		protected BadCharsBoyerMoore? BadChars = null;

		/// <summary>
		//	name:										Horspool algorithm
		//	direction:							any
		//	preprocess complexity:	O(m+s) time and O(s) space
		//	search complexity:			O(mn) time
		//	worst case:							n*n text character comparisons (quadratic worst case)
		//	ref:										HORSPOOL R.N., 1980, Practical fast searching in strings, Software - Practice & Experience, 10(6):501-506.
		/// </summary>

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public override void Init(in ReadOnlyMemory<byte> patternMemory, ISearch.OnMatchFoundDelegate patternMatched)
		{
			base.Init(patternMemory, patternMatched);
			this.BadChars = new BadCharsBoyerMoore(patternMemory.Span);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public override void Validate()
		{
			base.Validate();
			ArgumentNullException.ThrowIfNull(this.BadChars, nameof(this.BadChars));
		}

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

			int m = pattern.Length;
			int n = size;
			int mm1 = m - 1;
			int nmm = n - m;

			//Searching
			int j = offset;
			while (j <= nmm)
			{
				byte c = buffer[j + mm1];
				if (pattern[mm1] == c && pattern.SequenceEqual(buffer[j..(j + m)]))
				{
					if (!this.OnMatchFound!(j, this.GetType()))
					{
						return;
					}
				}
				j += this.BadChars![c];
			}
		}
	}
};  //END: namespace Search

