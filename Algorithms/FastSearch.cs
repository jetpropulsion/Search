namespace Search.Algorithms
{
	using Search.Attributes;
	using Search.Common;
	using Search.Interfaces;

	using System.Reflection.PortableExecutable;
	using System.Runtime.CompilerServices;
	using System.Security.Cryptography;


	/// <summary>
	//	name:										BackwardFast algorithm
	//	search direction:				right to left
	//	preprocess complexity:	O(m+s*s) time and space
	//	search complexity:			O(mn) time
	//	worst case:							n*n text character comparisons (quadratic worst case)
	//	ref:										D.Cantone and S.Faro. 	Fast-Search Algorithms: New Efficient Variants of the Boyer-Moore PatternLocation-Matching Algorithm. J.Autom.Lang.Comb., vol.10, n.5/6, pp.589--608, (2005).
	/// </summary>
	/// 

	[Experimental]
	public class FastSearch : SearchBase
	{
		protected int[]? bm_gs;
		protected int[]? bc;
		protected byte? ch;

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public FastSearch() :
			base()
		{
			this.bm_gs = null;
			this.bc = null;
			this.ch = 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public override void Init(in ReadOnlyMemory<byte> pattern, ISearch.OnMatchFoundDelegate patternMatched)
		{
			base.Init(pattern, patternMatched);

			int m = pattern.Length;
			ReadOnlySpan<byte> x = pattern.Span;

			int[] f = new int[m + 1];
			this.bm_gs = new int[pattern.Length + 1];
			this.bc = new int[ISearch.MaxAlphabetSize];
			this.ch = x[m - 1];

			int i, j, p;

			for (i = 0; i < m; i++)
			{
				bm_gs[i] = 0;
			}

			f[m] = m + 1;
			j = m + 1;

			for (i = m; i > 0; i--)
			{
				while (j <= m && x[i - 1] != x[j - 1])
				{
					if (this.bm_gs[j] == 0)
					{
						bm_gs[j] = j - i;
					}
					j = f[j];
				}
				f[i - 1] = --j;
			}
			p = f[0];
			for (j = 0; j <= m; ++j)
			{
				if (bm_gs[j] == 0)
				{
					bm_gs[j] = p;
				}
				if (j == p)
				{
					p = f[p];
				}
			}

			//Preprocessing
			for (int a = 0; a < ISearch.MaxAlphabetSize; a++)
			{
				bc[a] = m;
			}
			for (j = 0; j < m; j++)
			{
				bc[x[j]] = m - j - 1;
			}

		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public override void Validate()
		{
			base.Validate();
			ArgumentNullException.ThrowIfNull(this.bm_gs, nameof(this.bm_gs));
			ArgumentNullException.ThrowIfNull(this.bc, nameof(this.bc));
			ArgumentNullException.ThrowIfNull(this.ch, nameof(this.ch));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public override void FixSearchBuffer(ref Memory<byte> buffer, int bufferSize, in ReadOnlyMemory<byte> pattern)
			=> AppendLastPatternCharFixSearchBuffer(ref buffer, bufferSize, pattern);

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

			int m = pattern.Length;
			int n = size;


			int mm1 = m - 1;
			int mm2 = m - 2;
			int mp1 = m + 1;

			ReadOnlySpan<byte> x = pattern;
			ReadOnlySpan<byte> y = buffer;

			int j, s;

			//Pre_GS(x, m, gs);
			//for (i = 0; i < m; i++)
			//{
			//	y[n + i] = ch;
			//}

			Type type = this.GetType();

			//Searching
			if (pattern.SequenceEqual(buffer.Slice(0, m)))
			{
				if(!this.OnMatchFound!(0, type))
				{
					return;
				}
			}
			s = m;
			while (s < n)
			{
				int k;
				while ((k = this.bc![y[s]]) != 0)
				{
					s += k;
				}
				j = 2;
				while (j <= m && x[m - j] == y[s - j + 1])
				{
					j++;
				}
				if (j > m && s < n)
				{
					if(!this.OnMatchFound!(s - m + 1, type))
					{
						return;
					}
				}
				s += bm_gs![m - j + 1];
			}
		}
	}	//END: class FastSearch
};  //END: namespace Search
