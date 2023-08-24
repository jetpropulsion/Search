namespace Search.Algorithms
{
	using Search.Common;
	using Search.Interfaces;

	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Runtime.CompilerServices;
	using System.Text;
	using System.Threading.Tasks;
	using static Search.Algorithms.MaximalShift;

	//[Experimental(nameof(MaximalShift))]
	public class MaximalShift : SearchBase
	{

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public override void Init(in ReadOnlyMemory<byte> patternMemory, ISearch.OnMatchFoundDelegate patternMatched)
		{
			base.Init(patternMemory, patternMatched);
			//this.Next = new MorrisPrattNext(patternMemory.Span);

			int m = patternMemory.Length;
			int mp1 = m + 1;
			this.qsBc = new int[ISearch.MaxAlphabetSize];
			this.adaptedGs = new int[mp1];
			this.minShift = new int[mp1];
			this.patterns = new PatternLocation[mp1];

			ReadOnlySpan<byte> pattern = patternMemory.Span;

			int i;
			int j;

			//void computeMinShift(unsigned char *x, int m) 
			for (i = 0; i < m; ++i)
			{
				for (j = i - 1; j >= 0; --j)
				{
					if (pattern[i] == pattern[j])
					{
						break;
					}
				}
				minShift[i] = i - j;
			}

			//void orderPattern(unsigned char *x, int m, int (*pcmp)(), pattern *pat)
			for (i = 0; i < m; ++i)
			{
				//pat[i] = new PatternLocation();
				patterns[i].loc = i;
				patterns[i].c = pattern[i];
			}

			List<PatternLocation> p = patterns.ToList();
			p.Sort((PatternLocation pat1, PatternLocation pat2) =>
			{
				int dsh;
				dsh = minShift[pat2.loc] - minShift[pat1.loc];
				return (dsh != 0 ? dsh : (pat2.loc - pat1.loc));
			});

			//void preQsBc(unsigned char *x, int m, int qbc[])
			for (i = 0; i < ISearch.MaxAlphabetSize; i++)
			{
				qsBc[i] = mp1;
			}
			for (i = 0; i < m; i++)
			{
				qsBc[pattern[i]] = m - i;
			}


			//void preAdaptedGs(unsigned char *x, int m, int adaptedGs[], pattern *pat)
			int lshift, ploc;

			Func<ReadOnlyMemory<byte>, int, int, PatternLocation[], int> matchShift = (x1, ploc, lshift, patterns) =>
			{
				ReadOnlySpan<byte> x = x1.Span;

				int i, j;
				for (; lshift < m; ++lshift)
				{
					i = ploc;
					while (--i >= 0)
					{
						j = patterns[i].loc - lshift;
						if (j < 0)
						{
							continue;
						}
						if (patterns[i].c != x[j])
						{
							break;
						}
					}
					if (i < 0)
					{
						break;
					}
				}
				return lshift;
			};

			ReadOnlyMemory<byte> pmem = pattern.ToArray().AsMemory();

			adaptedGs[0] = lshift = 1;
			for (ploc = 1; ploc <= m; ++ploc)
			{
				lshift = matchShift(pmem, ploc, lshift, patterns);
				adaptedGs[ploc] = lshift;
			}
			for (ploc = 0; ploc < m; ++ploc)
			{
				lshift = adaptedGs[ploc];
				while (lshift < m)
				{
					i = patterns[ploc].loc - lshift;
					if (i < 0 || patterns[ploc].c != pattern[i])
					{
						break;
					}
					++lshift;
					lshift = matchShift(pmem, ploc, lshift, patterns);
				}
				adaptedGs[ploc] = lshift;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public override void Validate()
		{
			base.Validate();
			//ArgumentNullException.ThrowIfNull(this.Next, nameof(this.Next));
		}

		public struct PatternLocation
		{
			public int loc;
			public byte c;
		};

		private int[] qsBc;
		private int[] adaptedGs;
		private int[] minShift;
		private PatternLocation[] patterns;

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

			int i = 0;
			int j = offset;
			int m = pattern.Length;
			int n = size;
			int nmm = n - m;

			//Search

			j = offset;
			while (j <= nmm)
			{
				i = 0;
				while (i < m && patterns[i].c == buffer[j + patterns[i].loc])
				{
					++i;
				}
				if (i >= m)
				{
					if(!this.OnMatchFound!(j, this.GetType()))
					{
						return;
					}
				}
				//if (j >= nmm)
				//{
				//	//NOTE: Fix, original was breaking the bounds on very last comparison
				//	break;
				//}
				j += Math.Max(adaptedGs[i], qsBc[buffer[j + m]]);
			}

		}
	};  //END: class MorrisPratt
} //END: namespace

