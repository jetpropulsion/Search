using System;
using System.Text;
using Search.Interfaces;
using Search.Common;
using System.Text.RegularExpressions;

namespace Search
{
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

		public Raita() : base()
		{
		}
		public Raita(ReadOnlyMemory<byte> patternMemory, ISearch.OnMatchFoundDelegate patternMatched) : base(patternMemory, patternMatched)
		{
		}
		public override void Init(ReadOnlyMemory<byte> patternMemory, ISearch.OnMatchFoundDelegate patternMatched)
		{
			base.Init(patternMemory, patternMatched);
			this.BadChars = new BadCharsBoyerMoore(patternMemory.Span);
		}

		public override void Validate()
		{
			base.Validate();
			if (this.BadChars == null) throw new ArgumentNullException(nameof(this.BadChars));
		}

		public override void Search(ReadOnlyMemory<byte> bufferMemory, int offset)
		{
			base.Validate();

			//Searching
			ReadOnlySpan<byte> pattern = base.PatternSpan;
			ReadOnlySpan<byte> buffer = bufferMemory.Span;

			int j = offset;
			int m = pattern.Length;
			int n = buffer.Length;
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

				if ((last == c) &&
						(middle == buffer[j + mr1]) &&
						(first == buffer[j]) &&
						innerPattern.SequenceEqual(buffer[(j + 1)..(j + mm2)])
				)
				{
					if(!this.OnPatternMatches!(j, this.GetType()))
					{
						return;
					}
				}

				j += this.BadChars![c];
			}
		}
	}
};  //END: namespace Search

