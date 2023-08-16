﻿namespace Search.Algorithms
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
		//protected BadCharsBoyerMoore? BadChars = null;

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public MaximalShift() :
			base()
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public MaximalShift(in ReadOnlyMemory<byte> patternMemory, ISearch.OnMatchFoundDelegate patternMatched) :
			base(patternMemory, patternMatched)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public override void Init(in ReadOnlyMemory<byte> patternMemory, ISearch.OnMatchFoundDelegate patternMatched)
		{
			base.Init(patternMemory, patternMatched);
			//this.Next = new MorrisPrattNext(patternMemory.Span);

			int m = patternMemory.Length;
			this.qsBc = new int[ISearch.MaxAlphabetSize];
			this.adaptedGs = new int[m + 1];
			this.minShift = new int[m + 1];
			this.pat = new Pattern[m + 1];

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
				//pat[i] = new Pattern();
				pat[i].loc = i;
				pat[i].c = pattern[i];
			}
			//qsort(pat, m, sizeof(pattern), pcmp);
			/*
			int maxShiftPcmp(pattern *pat1, pattern *pat2) {
				 int dsh;
				 dsh = minShift[pat2->loc] - minShift[pat1->loc];
				 return(dsh ? dsh : (pat2->loc - pat1->loc));
			}
			*/
			List<Pattern> p = pat.ToList();
			p.Sort((Pattern pat1, Pattern pat2) =>
			{
				int dsh;
				dsh = minShift[pat2.loc] - minShift[pat1.loc];
				return (dsh != 0 ? dsh : (pat2.loc - pat1.loc));
			});

			//void preQsBc(unsigned char *x, int m, int qbc[])
			for (i = 0; i < ISearch.MaxAlphabetSize; i++) qsBc[i] = m + 1;
			for (i = 0; i < m; i++) qsBc[pattern[i]] = m - i;


			//void preAdaptedGs(unsigned char *x, int m, int adaptedGs[], pattern *pat)
			int lshift, ploc;

			Func<ReadOnlyMemory<byte>, int, int, Pattern[], int> matchShift = (x1, ploc, lshift, pat) =>
			{
				ReadOnlySpan<byte> x = x1.Span;

				int i, j;
				for (; lshift < m; ++lshift)
				{
					i = ploc;
					while (--i >= 0)
					{
						j = pat[i].loc - lshift;
						if (j < 0)
						{
							continue;
						}
						if (pat[i].c != x[j])
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
				lshift = matchShift(pmem, ploc, lshift, pat);
				adaptedGs[ploc] = lshift;
			}
			for (ploc = 0; ploc < m; ++ploc)
			{
				lshift = adaptedGs[ploc];
				while (lshift < m)
				{
					i = pat[ploc].loc - lshift;
					if (i < 0 || pat[ploc].c != pattern[i])
						break;
					++lshift;
					lshift = matchShift(pmem, ploc, lshift, pat);
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

		public struct Pattern
		{
			public int loc;
			public byte c;
		};

		int[] qsBc;
		int[] adaptedGs;
		int[] minShift;
		Pattern[] pat;

#if DEBUG
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
#else
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#endif
		public override void Search(in ReadOnlyMemory<byte> bufferMemory, int offset)
		{
			this.Validate();

			//Searching
			ReadOnlySpan<byte> pattern = this.PatternSpan;
			ReadOnlySpan<byte> buffer = bufferMemory.Span;

			int i = 0;
			int j = offset;
			int m = pattern.Length;
			int n = buffer.Length;
			int nmm = n - m;

			//Search

			j = offset;
			while (j <= nmm)
			{
				i = 0;
				while (i < m && pat[i].c == buffer[j + pat[i].loc])
				{
					++i;
				}
				if (i >= m)
				{
					if(!this.OnPatternMatches!(j, this.GetType()))
					{
						return;
					}
					if(j == nmm)
					{
						break;
					}
				}
				j += Math.Max(adaptedGs[i], qsBc[buffer[j + m]]);
			}

		}
	};  //END: class MorrisPratt
} //END: namespace
