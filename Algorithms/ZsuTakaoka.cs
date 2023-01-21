using Search.Common;
using Search.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Search.Algorithms
{
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

		public ZsuTakaoka() : base()
		{
		}
		public ZsuTakaoka(in ReadOnlyMemory<byte> patternMemory, ISearch.OnMatchFoundDelegate patternMatched) : base(patternMemory, patternMatched)
		{
		}
		public override void Init(in ReadOnlyMemory<byte> patternMemory, ISearch.OnMatchFoundDelegate patternMatched)
		{
			base.Init(patternMemory, patternMatched);
			BadChars = new BadCharsZsuTakaoka(patternMemory.Span);
			GoodSuffixes = new GoodSuffixesBoyerMoore(patternMemory.Span);
		}

		public override void Validate()
		{
			base.Validate();

			if (BadChars == null) throw new ArgumentNullException(nameof(BadChars));
			if (GoodSuffixes == null) throw new ArgumentNullException(nameof(GoodSuffixes));
		}
		public override void Search(in ReadOnlyMemory<byte> bufferMemory, int offset)
		{
			Validate();

			//Searching
			ReadOnlySpan<byte> pattern = PatternSpan;
			ReadOnlySpan<byte> buffer = bufferMemory.Span;

			int m = pattern.Length;
			int n = buffer.Length;

			int mm1 = m - 1;
			int mm2 = m - 2;

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
					if (!OnPatternMatches!(j, GetType()))
					{
						return;
					}
					j += GoodSuffixes![0];
				}
				else
				{
					j += Math.Max(GoodSuffixes![i], BadChars![buffer[j + mm2], buffer[j + mm1]]);
				}
			}

		}
	}
};  //END: namespace Search
