namespace Search.Algorithms
{
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

	public class Raita : SearchBase
	{
		protected BadCharsBoyerMoore? BadChars = null;

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Raita() :
			base()
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public Raita(in ReadOnlyMemory<byte> patternMemory, ISearch.OnMatchFoundDelegate patternMatched) :
			base(patternMemory, patternMatched)
		{
		}

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
			base.Validate();

			//Searching
			ReadOnlySpan<byte> pattern = this.PatternSpan;
			ReadOnlySpan<byte> buffer = bufferMemory.Span;

			int j = offset;
			int m = pattern.Length;
			int n = size;
			int mm1 = m - 1;
			int mm2 = mm1 - 1;
			int mr1 = m >> 1;

			byte first = pattern[0];
			byte middle = pattern[mr1];
			byte last = pattern[mm1];

			ReadOnlySpan<byte> innerPattern = pattern[1..mm2];

			while (j <= n - m)
			{
				byte c = buffer[j + mm1];

				if (last == c &&
						middle == buffer[j + mr1] &&
						first == buffer[j] &&
						innerPattern.SequenceEqual(buffer[(j + 1)..(j + mm2)])
				)
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

