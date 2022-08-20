using System;
using System.Text;
using Search.Interfaces;
using Search.Common;

namespace Search
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

		public override void Init(ReadOnlyMemory<byte> patternMemory, ISearch.OnMatchFoundDelegate onFound)
		{
			base.Init(patternMemory, onFound);
			this.BadChars = new BadCharsBoyerMoore(patternMemory);
		}

		public override void Validate()
		{
			base.Validate();

			if (this.BadChars == null)
			{
				throw new ArgumentNullException(nameof(this.BadChars));
			}
		}
		public override void Search(ReadOnlyMemory<byte> bufferMemory, int offset)
		{
			this.Validate();

			ReadOnlySpan<byte> pattern = PatternMemory!.Value.Span;
			ReadOnlySpan<byte> buffer = bufferMemory.Span;

			int m = pattern.Length;
			int mm1 = m - 1;
			int n = buffer.Length;
			int nmm = n - m;

			//Searching
			int j = 0;
			while (j <= nmm)
			{
				byte c = buffer[j + mm1];
				if ((pattern[mm1] == c) && pattern.SequenceEqual(buffer[j..(j + m)]))
				{
					if(!base.OnFound!(j))
					{
						return;
					}
				}
				j += this.BadChars![c];
			}
		}
	}
};  //END: namespace Search

