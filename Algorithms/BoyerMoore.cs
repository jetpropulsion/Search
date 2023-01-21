using System;
using System.Text;
using Search.Interfaces;
using Search.Common;
using System.Reflection;

namespace Search.Algorithms
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
		public BadCharsBoyerMoore? BadChars { get; protected set; } = null;
		public GoodSuffixesBoyerMoore? GoodSuffixes { get; protected set; } = null;

		public BoyerMoore() : base()
		{
		}
		public BoyerMoore(ReadOnlyMemory<byte> patternMemory, ISearch.OnMatchFoundDelegate patternMatched) : base(patternMemory, patternMatched)
		{
		}
		public override void Init(in ReadOnlyMemory<byte> patternMemory, ISearch.OnMatchFoundDelegate patternMatched)
		{
			base.Init(patternMemory, patternMatched);
			BadChars = new BadCharsBoyerMoore(patternMemory.Span);
			GoodSuffixes = new GoodSuffixesBoyerMoore(patternMemory.Span);
		}

		public override void Validate()
		{
			base.Validate();
			ArgumentNullException.ThrowIfNull(BadChars, nameof(BadChars));
			ArgumentNullException.ThrowIfNull(GoodSuffixes, nameof(GoodSuffixes));
		}

		public override void Search(in ReadOnlyMemory<byte> bufferMemory, int offset)
		{
			Validate();

			//Searching
			ReadOnlySpan<byte> pattern = PatternSpan;
			ReadOnlySpan<byte> buffer = bufferMemory.Span;

			int j = offset;
			int m = pattern.Length;
			int n = buffer.Length;
			int mm1 = m - 1;
			int mp1 = m + 1;

			while (j <= n - m)
			{
				int i = mm1;
				while (i >= 0 && pattern[i] == buffer[i + j])
				{
					--i;
				}
				if (i < 0)
				{
					if (!OnPatternMatches!(j, GetType()))
					{
						return;
					}

					j += GoodSuffixes![0];
				}
				else
				{
					j += Math.Max(GoodSuffixes![i], BadChars![buffer[i + j]] - mp1 + i);
				}
			}
		}
	}
};  //END: namespace Search

