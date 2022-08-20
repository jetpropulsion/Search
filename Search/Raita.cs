using System;
using System.Text;
using Search.Interfaces;
using Search.Common;
using System.Text.RegularExpressions;

namespace Search
{
	/// <summary>
	//	name:										Boyer-Moore algorithm
	//	direction:							right to left
	//	preprocess complexity:	O(m+s) time and space
	//	search complexity:			O(mn) time
	//	worst case:							3n text character comparisons (when searching non-periodic pattern)
	//	ref:										BOYER R.S., MOORE J.S., 1977, A fast string searching algorithm. Communications of the ACM. 20:762-772.
	/// </summary>
	public class Raita : SearchBase
	{
		protected BadCharsBoyerMoore? BadChars = null;

		public Raita() : base()
		{
		}
		public Raita(ReadOnlyMemory<byte> patternMemory, ISearch.OnMatchFoundDelegate onFound) : base(patternMemory, onFound)
		{
		}
		public override void Init(ReadOnlyMemory<byte> patternMemory, ISearch.OnMatchFoundDelegate onFound)
		{
			base.Init(patternMemory, onFound);
			this.BadChars = new BadCharsBoyerMoore(patternMemory.Span);
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
			base.Validate();
			ReadOnlySpan<byte> pattern = base.PatternMemory!.Value.Span;
			ReadOnlySpan<byte> buffer = bufferMemory.Span;

			//Searching
			int j = offset;
			int m = pattern.Length;
			int n = buffer.Length;
			int mm1 = m - 1;
			int mm2 = mm1 - 1;
			int mr1 = m >> 1;

			byte first = pattern[0];
			byte middle = pattern[mr1];
			byte last = pattern[mm1];

			ReadOnlySpan<byte> patternCut = pattern[1..mm2];

			while (j <= n - m)
			{
				byte c = buffer[j + mm1];

				if ((last == c) &&
						(middle == buffer[j + mr1]) &&
						(first == buffer[j]) &&
						patternCut.SequenceEqual(buffer[(j + 1)..(j + mm2)])
				)
				{
					if(!this.OnFound!(j))
					{
						return;
					}
				}

				j += this.BadChars![c];
			}
		}
	}
};  //END: namespace Search

