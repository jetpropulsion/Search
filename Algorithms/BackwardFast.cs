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

	//[Experimental]
	[Unstable]
	public class BackwardFast : SearchBase
	{
		//public GoodSuffixesBoyerMoore? GoodSuffixes { get; protected set; } = null;
		//public BadCharsZsuTakaoka? BadChars { get; protected set; } = null;

		protected int[,] bm_gs;

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public BackwardFast() :
			base()
		{
			this.bm_gs = null;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public override void Init(in ReadOnlyMemory<byte> patternMemory, ISearch.OnMatchFoundDelegate patternMatched)
		{
			base.Init(patternMemory, patternMatched);

			bm_gs = new int[patternMemory.Length + 1, ISearch.MaxAlphabetSize];

			int m = patternMemory.Length;
			ReadOnlySpan<byte> x = patternMemory.Span;

			int i, j, p, c, h, last, suffix_len;
			int[] temp = new int[patternMemory.Length + 1];
			suffix_len = 1;
			last = m - 1;
			for (i = 0; i <= m; i++)
			{
				for (j = 0; j < ISearch.MaxAlphabetSize; j++)
				{
					bm_gs[i, j] = m;
				}
			}
			for (i = 0; i <= m; i++) temp[i] = -1;
			for (i = m - 2; i >= 0; i--)
				if (x[i] == x[last])
				{
					temp[last] = i;
					last = i;
				}
			suffix_len++;
			while (suffix_len <= m)
			{
				last = m - 1;
				i = temp[last];
				while (i >= 0)
				{
					if (i - suffix_len + 1 >= 0)
					{
						if (x[i - suffix_len + 1] == x[last - suffix_len + 1])
						{
							temp[last] = i;
							last = i;
						}
						if (bm_gs[m - suffix_len + 1, x[i - suffix_len + 1]] > m - 1 - i)
						{
							bm_gs[m - suffix_len + 1, x[i - suffix_len + 1]] = m - 1 - i;
						}
					}
					else
					{
						temp[last] = i;
						last = i;
						for (c = 0; c < ISearch.MaxAlphabetSize; c++)
						{
							if (bm_gs[m - suffix_len + 1, c] > m - 1 - i)
							{
								bm_gs[m - suffix_len + 1, c] = m - 1 - i;
							}
						}
					}
					i = temp[i];
				}
				temp[last] = -1;
				suffix_len++;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public override void Validate()
		{
			base.Validate();
			ArgumentNullException.ThrowIfNull(this.bm_gs, nameof(this.bm_gs));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public override void FixSearchBuffer(ref Memory<byte> buffer, int bufferSize, in ReadOnlyMemory<byte> pattern)
		{
			int additionalSize = buffer.Length - bufferSize;
			if (additionalSize >= pattern.Length)
			{
				//buffer.Span.Slice(bufferSize, pattern.Length).Fill(0);
				pattern.Span.CopyTo(buffer.Slice(bufferSize, pattern.Length).Span);
				return;
			}
			int additionalSizeToAdd = pattern.Length - additionalSize;

			//This method enlarges the search buffer to allow certain search algorithms to stop, like this algorithm
			byte[] enlargedBuffer;
			SearchBase.EnlargeBuffer(buffer, pattern, additionalSizeToAdd, out bufferSize, out enlargedBuffer);

			//Array.Fill<byte>(enlargedBuffer, 0, bufferSize, pattern.Length);
			Span<byte> enlargedBufferEnd = enlargedBuffer.AsSpan().Slice(bufferSize, pattern.Length);
			pattern.Span.CopyTo(enlargedBufferEnd);
			buffer = new Memory<byte>(enlargedBuffer);
		}

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

			//Searching
			int[] bc = new int[ISearch.MaxAlphabetSize];
			int first;

			//Preprocessing
			for (int i = 0; i < ISearch.MaxAlphabetSize; i++) bc[i] = m;
			for (int i = 0; i < m; i++) bc[pattern[i]] = m - i - 1;
			//for (int i = 0; i < m; i++) y[n + i] = x[i];
			//PreBFS(x, m, gs);

			//Searching
			int s = mm1;
			first = bm_gs[1, pattern[0]];
			Type type = this.GetType();

			int j, k;
			while (s < n)
			{
				while ((k = bc[buffer[s]]) != 0)
				{
					s += k;
				}
				for (j = s - 1, k = mm1; k > 0 && pattern[k - 1] == buffer[j]; k--, j--) ;
				if (k == 0)
				{
					if (s < n)
					{
						if(!this.OnMatchFound!(s - mp1, type))
						{
							return;
						}
					}
					s += first;
				}
				else s += bm_gs[k, buffer[j]];
			}
		}
	}
};  //END: namespace Search
