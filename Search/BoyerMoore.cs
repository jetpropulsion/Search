using System;
using System.Text;

namespace Search
{
	public interface ISearch
	{
		delegate void Found(int offset);
		void Search(ReadOnlySpan<byte> pattern, ReadOnlySpan<byte> buffer, Found found);
	};  //END: public interface ISearch

	public class BoyerMoore : ISearch
	{
		public int MaxSearchStringLength { get; protected set; } = -1;
		public int MaxAlphabetSize { get; protected set; } = -1;

		private int[] bmGs;
		private int[] bmBc;
		private int[] suff;

		public BoyerMoore()
		{
			this.bmGs = Array.Empty<int>();
			this.bmBc = Array.Empty<int>();
			this.suff = Array.Empty<int>();
		}
		private void PreBmBc(ReadOnlySpan<byte> pattern)
		{
			int m = pattern.Length;
			for (int i = 0; i < this.bmBc.Length; ++i)
			{
				this.bmBc[i] = m;
			}
			for (int i = 0; i < m - 1; ++i)
			{
				this.bmBc[ pattern[i] ] = m - i - 1;
			}
		}
		private void PreBmSuffixes(ReadOnlySpan<byte> pattern)
		{
			int m = pattern.Length;
			int mm1 = m - 1;
			int mm2 = m - 2;
			int f = 0;
			int g = mm1;
			suff[mm1] = m;

			int i;
			for (i = mm2; i >= 0; --i)
			{
				if ((i > g) && ((suff[i + mm1 - f] < i - g)))
				{
					suff[i] = suff[i + mm1 - f];
				}
				else
				{
					if (i < g)
					{
						g = i;
					}
					f = i;
					while ((g >= 0) && (pattern[g] == pattern[g + mm1 - f]))
					{
						--g;
					}
					suff[i] = f - g;
				}
			}
		}

		private void PreBmGs(ReadOnlySpan<byte> pattern)
		{
			int m = pattern.Length;
			int mm1 = m - 1;
			int mm2 = m - 2;

			this.PreBmSuffixes(pattern);

			for (int i = 0; i < m; ++i)
			{
				bmGs[i] = m;
			}

			int j = 0;
			for (int i = mm1; i >= 0; --i)
			{
				if (suff[i] == i + 1)
				{
					for (; j < mm1 - i; ++j)
					{
						if (bmGs[j] == m)
						{
							bmGs[j] = mm1 - i;
						}
					}
				}
			}

			for (int i = 0; i <= mm2; ++i)
			{
				bmGs[mm1 - suff[i]] = mm1 - i;
			}
		}
		public void Search(ReadOnlySpan<byte> pattern, ReadOnlySpan<byte> buffer, ISearch.Found found)
		{
			//Init
			this.MaxSearchStringLength = pattern.Length;
			this.MaxAlphabetSize = 256;

			this.bmGs = new int[MaxSearchStringLength];
			this.bmBc = new int[MaxAlphabetSize];
			this.suff = new int[MaxSearchStringLength];

			//Init common structures
			this.PreBmGs(pattern);
			this.PreBmBc(pattern);

			//Searching
			int j = 0;
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
					found(j);

					j += this.bmGs[0];
				}
				else
				{
					j += Math.Max(this.bmGs[i], this.bmBc[ buffer[i + j] ] - mp1 + i);
				}
			}
		}
	};	//END: public class CTXBM

};  //END: namespace Search

