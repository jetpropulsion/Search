using System;
using System.Text;
using Search.Interfaces;
using Search.Common;

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
	public class BoyerMoore : SearchBase
	{
		protected BadCharsBoyerMoore? BadChars = null;
		protected GoodSuffixesBoyerMoore? GoodSuffixes = null;

		public BoyerMoore() : base()
		{
		}
		public BoyerMoore(ReadOnlyMemory<byte> patternMemory, ISearch.OnMatchFoundDelegate onFound) : base(patternMemory, onFound)
		{
		}
		public override void Init(ReadOnlyMemory<byte> patternMemory, ISearch.OnMatchFoundDelegate onFound)
		{
			base.Init(patternMemory, onFound);
			this.BadChars = new BadCharsBoyerMoore(patternMemory.Span);
			this.GoodSuffixes = new GoodSuffixesBoyerMoore(patternMemory.Span);
		}

		public override void Validate()
		{
			base.Validate();

			if (this.BadChars == null)
			{
				throw new ArgumentNullException(nameof(this.BadChars));
			}
			if (this.GoodSuffixes == null)
			{
				throw new ArgumentNullException(nameof(this.GoodSuffixes));
			}
		}
		public override void Search(ReadOnlyMemory<byte> bufferMemory, int offset)
		{
			this.Validate();

			//Searching
			ReadOnlySpan<byte> pattern = base.PatternMemory!.Value.Span;
			ReadOnlySpan<byte> buffer = bufferMemory.Span;

			int j = offset;
			int m = pattern.Length;
			int n = buffer.Length;
			int mm1 = m - 1;
			int mp1 = m + 1;

			while(j <= n - m)
			{
				int i = mm1;
				while(i >= 0 && pattern[i] == buffer[i + j])
				{
					--i;
				}
				if(i < 0)
				{
					if(!this.OnFound!(j))
					{
						return;
					}

					j += this.GoodSuffixes![0];
				}
				else
				{
					j += Math.Max(this.GoodSuffixes![i], this.BadChars![ buffer[i + j] ] - mp1 + i);
				}
			}
		}
	}
};  //END: namespace Search

