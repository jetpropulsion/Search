using System;
using System.Text;
using Search.Interfaces;

namespace Search
{
	public class BadCharsBase
	{
		protected int[] BadChars;
		public BadCharsBase(int maxAlphabetSize)
		{
			this.BadChars = new int[maxAlphabetSize];
		}
		public virtual void Init(ReadOnlySpan<byte> pattern)
		{
			int m = pattern.Length;
			for (int i = 0; i < this.BadChars.Length; ++i)
			{
				this.BadChars[i] = m;
			}
			for (int i = 0; i < m - 1; ++i)
			{
				this.BadChars[pattern[i]] = m - i - 1;
			}
		}
	};

	public class SuffixesBase
	{
		protected int[] Suffixes;

		public SuffixesBase(int maxSearchStringLength)
		{
			this.Suffixes = new int[maxSearchStringLength];
		}

		public virtual void Init(ReadOnlySpan<byte> pattern)
		{
			int m = pattern.Length;
			int mm1 = m - 1;
			int mm2 = m - 2;
			int f = 0;
			int g = mm1;
			this.Suffixes[mm1] = m;

			for (int i = mm2; i >= 0; --i)
			{
				if ((i > g) && ((this.Suffixes[i + mm1 - f] < i - g)))
				{
					this.Suffixes[i] = this.Suffixes[i + mm1 - f];
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
					this.Suffixes[i] = f - g;
				}
			}

		}
	};

	public class GoodSuffixesBase : SuffixesBase
	{
		protected int[] GoodSuffixes;
		public GoodSuffixesBase(int maxSearchStringLength) : base(maxSearchStringLength)
		{
			this.GoodSuffixes = new int[maxSearchStringLength];
		}
		public override void Init(ReadOnlySpan<byte> pattern)
		{
			int m = pattern.Length;
			int mm1 = m - 1;
			int mm2 = m - 2;

			base.Init(pattern);

			for (int i = 0; i < m; ++i)
			{
				GoodSuffixes[i] = m;
			}

			int j = 0;
			for (int i = mm1; i >= 0; --i)
			{
				if (Suffixes[i] == i + 1)
				{
					for (; j < mm1 - i; ++j)
					{
						if (GoodSuffixes[j] == m)
						{
							GoodSuffixes[j] = mm1 - i;
						}
					}
				}
			}

			for (int i = 0; i <= mm2; ++i)
			{
				GoodSuffixes[mm1 - Suffixes[i]] = mm1 - i;
			}
		}
	};

	public class BoyerMoore : ISearch
	{
		protected int[] GoodSuffixes;
		protected int[] BadChars;
		protected int[] Suffixes;

		public BoyerMoore()
		{
			this.GoodSuffixes = Array.Empty<int>();
			this.BadChars = Array.Empty<int>();
			this.Suffixes = Array.Empty<int>();
		}
		protected void InitBadChars(ReadOnlySpan<byte> pattern)
		{
			int m = pattern.Length;
			for (int i = 0; i < this.BadChars.Length; ++i)
			{
				this.BadChars[i] = m;
			}
			for (int i = 0; i < m - 1; ++i)
			{
				this.BadChars[ pattern[i] ] = m - i - 1;
			}
		}
		protected void InitSuffixes(ReadOnlySpan<byte> pattern)
		{
			int m = pattern.Length;
			int mm1 = m - 1;
			int mm2 = m - 2;
			int f = 0;
			int g = mm1;
			Suffixes[mm1] = m;

			int i;
			for (i = mm2; i >= 0; --i)
			{
				if ((i > g) && ((Suffixes[i + mm1 - f] < i - g)))
				{
					Suffixes[i] = Suffixes[i + mm1 - f];
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
					Suffixes[i] = f - g;
				}
			}
		}

		protected void InitGoodSuffixes(ReadOnlySpan<byte> pattern)
		{
			int m = pattern.Length;
			int mm1 = m - 1;
			int mm2 = m - 2;

			this.InitSuffixes(pattern);

			for (int i = 0; i < m; ++i)
			{
				GoodSuffixes[i] = m;
			}

			int j = 0;
			for (int i = mm1; i >= 0; --i)
			{
				if (Suffixes[i] == i + 1)
				{
					for (; j < mm1 - i; ++j)
					{
						if (GoodSuffixes[j] == m)
						{
							GoodSuffixes[j] = mm1 - i;
						}
					}
				}
			}

			for (int i = 0; i <= mm2; ++i)
			{
				GoodSuffixes[mm1 - Suffixes[i]] = mm1 - i;
			}
		}

		public virtual void Init(ReadOnlySpan<byte> pattern)
		{
			//Init
			int maxSearchStringLength = pattern.Length;
			int maxAlphabetSize = 256;

			this.GoodSuffixes = new int[maxSearchStringLength];
			this.BadChars = new int[maxAlphabetSize];
			this.Suffixes = new int[maxSearchStringLength];

			//Init common structures
			this.InitGoodSuffixes(pattern);
			this.InitBadChars(pattern);
		}

		public virtual void Search(ReadOnlySpan<byte> pattern, ReadOnlySpan<byte> buffer, int offset, ISearch.Found found)
		{
			//Initialize
			Init(pattern);

			//Searching
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
					if(!found(j))
					{
						return;
					}

					j += this.GoodSuffixes[0];
				}
				else
				{
					j += Math.Max(this.GoodSuffixes[i], this.BadChars[ buffer[i + j] ] - mp1 + i);
				}
			}
		}
	};	//END: public class CTXBM

};  //END: namespace Search

