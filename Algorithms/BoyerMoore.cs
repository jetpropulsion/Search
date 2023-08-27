namespace Search.Algorithms
{
	using Search.Attributes;
	using Search.Common;
	using Search.Interfaces;

	using System.Runtime.CompilerServices;

	/// <summary>
	//	name:										Boyer-Moore algorithm
	//	direction:							right to left
	//	preprocess complexity:	O(m+s) time and space
	//	search complexity:			O(mn) time
	//	worst case:							3n text character comparisons (when searching non-periodic pattern)
	//	ref:										BOYER R.S., MOORE J.S., 1977, A fast string searching algorithm. Communications of the ACM. 20:762-772.
	/// </summary>
	[Slow]
	public class BoyerMoore : SearchBase
	{
		public BadCharsBoyerMoore? BadChars { get; protected set; } = null;
		public GoodSuffixesBoyerMoore? GoodSuffixes { get; protected set; } = null;


		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public override void Init(in ReadOnlyMemory<byte> patternMemory, ISearch.OnMatchFoundDelegate patternMatched)
		{
			base.Init(patternMemory, patternMatched);
			this.BadChars = new BadCharsBoyerMoore(patternMemory.Span);
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

			int j = offset;
			int m = pattern.Length;
			int n = size;
			int mm1 = m - 1;
			int mp1 = m + 1;
			Type type = this.GetType();

			while (j <= n - m)
			{
				int i = mm1;
				while (i >= 0 && pattern[i] == buffer[i + j])
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
					j += Math.Max(this.GoodSuffixes![i], this.BadChars![buffer[i + j]] - mp1 + i);
				}
			}
		}
	}
};  //END: namespace Search

