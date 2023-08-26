namespace Search.Algorithms
{
	using Search.Attributes;
	using Search.Common;
	using Search.Interfaces;

	using System.Runtime.CompilerServices;

	/// <summary>
	/// name:										Raita algorithm
	/// direction:							any
	/// preprocess complexity:	O(m+s) time and O(s) space
	/// search complexity:			O(mn) time
	/// worst case:							n*n text character comparisons (quadratic worst case)
	/// ref:										RAITA T., 1992, Tuning the Boyer-Moore-Horspool string searching algorithm, Software - Practice & Experience, 22(10):879-884.
	/// </summary>

	[Unstable("Hallucinates Offsets on Pattern.Length - 1 Pattern Copy")]
	public class Raita : SearchBase
	{
		protected BadCharsBoyerMoore? BadChars = null;

		int m;
		int mm1;
		int mm2;
		//int mm3;
		int mr1;

		byte first;
		byte middle;
		byte last;

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public override void Init(in ReadOnlyMemory<byte> pattern, ISearch.OnMatchFoundDelegate patternMatched)
		{
			base.Init(pattern, patternMatched);
			this.BadChars = new BadCharsBoyerMoore(pattern.Span);
			this.m = pattern.Length;
			this.mm1 = m - 1;
			this.mm2 = m - 2;
			//mm3 = m - 3;
			mr1 = m >> 1;

			this.first = pattern.Span[0];
			this.middle = pattern.Span[mr1];
			this.last = pattern.Span[mm1];
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
			base.Validate();

			//Searching
			ReadOnlySpan<byte> pattern = this.PatternSpan;
			ReadOnlySpan<byte> innerPattern = pattern[1..mm2];
			ReadOnlySpan<byte> buffer = bufferMemory.Span;

			int j = offset;
			int n = size;

			Type type = this.GetType();

			while (j <= n - m)
			{
				byte c = buffer[j + mm1];

				if (last == c &&
						middle == buffer[j + mr1] &&
						first == buffer[j] &&
						innerPattern.SequenceEqual(buffer[(j + 1)..(j + mm2)])
						//innerPattern.SequenceEqual(buffer.Slice(j + 1, mm2))
				)
				{
					if (!this.OnMatchFound!(j, type))
					{
						return;
					}
				}

				j += this.BadChars![c];
			}
		}
	}
};  //END: namespace Search

