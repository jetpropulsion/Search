namespace Search.Algorithms
{
	using Search.Common;
	using Search.Interfaces;

	using System.Runtime.CompilerServices;

	/// <summary>
	//	name:										Zhu-Takaoka algorithm
	//	search direction:				right to left
	//	preprocess complexity:	O(m+s*s) time and space
	//	search complexity:			O(mn) time
	//	worst case:							n*n text character comparisons (quadratic worst case)
	//	ref:										ZHU R.F., TAKAOKA T., 1987, On improving the average case of the Boyer-Moore string matching algorithm, Journal of Information Processing 10(3):173-177.
	/// </summary>
	public class ZsuTakaoka : SearchBase
	{
		public GoodSuffixesBoyerMoore? GoodSuffixes { get; protected set; } = null;
		public BadCharsZsuTakaoka? BadChars { get; protected set; } = null;

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public override void Init(in ReadOnlyMemory<byte> patternMemory, ISearch.OnMatchFoundDelegate patternMatched)
		{
			base.Init(patternMemory, patternMatched);
			this.BadChars = new BadCharsZsuTakaoka(patternMemory.Span);
			this.GoodSuffixes = new GoodSuffixesBoyerMoore(patternMemory.Span);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public override void Validate()
		{
			base.Validate();
			ArgumentNullException.ThrowIfNull(this.BadChars, nameof(this.BadChars));
			ArgumentNullException.ThrowIfNull(this.GoodSuffixes, nameof(this.GoodSuffixes));
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
			int mm2 = m - 2;

			Type type = this.GetType();

			//Searching
			int j = offset;
			while (j <= n - m)
			{
				int i = mm1;
				while ((uint)i < (uint)m && pattern[i] == buffer[i + j])
				{
					--i;
				}
				if (i < 0)
				{
					if (!this.OnMatchFound!(j, type))
					{
						return;
					}
					j += this.GoodSuffixes![0];
				}
				else
				{
					j += Math.Max(this.GoodSuffixes![i], this.BadChars![buffer[j + mm2], buffer[j + mm1]]);
				}
			}

		}
	}
};  //END: namespace Search
