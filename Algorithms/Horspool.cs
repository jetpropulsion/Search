using System;
using System.Text;
using Search.Interfaces;
using Search.Common;

namespace Search.Algorithms
{
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
		public Horspool() : base()
		{
		}
		public Horspool(in ReadOnlyMemory<byte> patternMemory, ISearch.OnMatchFoundDelegate patternMatched) : base(patternMemory, patternMatched)
		{
		}

		public override void Init(in ReadOnlyMemory<byte> patternMemory, ISearch.OnMatchFoundDelegate patternMatched)
		{
			base.Init(patternMemory, patternMatched);
			BadChars = new BadCharsBoyerMoore(patternMemory.Span);
		}

		public override void Validate()
		{
			base.Validate();

			if (BadChars == null) throw new ArgumentNullException(nameof(BadChars));
		}
		public override void Search(in ReadOnlyMemory<byte> bufferMemory, int offset)
		{
			Validate();

			//Searching
			ReadOnlySpan<byte> pattern = PatternSpan;
			ReadOnlySpan<byte> buffer = bufferMemory.Span;

			int m = pattern.Length;
			int mm1 = m - 1;
			int n = buffer.Length;
			int nmm = n - m;

			//Searching
			int j = offset;
			while (j <= nmm)
			{
				byte c = buffer[j + mm1];
				if (pattern[mm1] == c && pattern.SequenceEqual(buffer[j..(j + m)]))
				{
					if (!OnPatternMatches!(j, GetType()))
					{
						return;
					}
				}
				j += BadChars![c];
			}
		}
	}
};  //END: namespace Search

