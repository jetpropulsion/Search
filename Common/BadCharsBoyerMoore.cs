namespace Search.Common
{
	using Search.Interfaces;

	using System.Runtime.CompilerServices;

	//In reference literature and/or implementation, "BadCharsBase" is known as "bmBc"
	//Used by: Boyer-Moore and derivatives (Turbo BM, Tuned BM, Horspool, Raita, Zsu-Takaoka)
	public class BadCharsBoyerMoore
	{
		public readonly int[] BadChars;

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public BadCharsBoyerMoore(in ReadOnlySpan<byte> pattern)
		{
			//Allocate
			this.BadChars = new int[ISearch.MaxAlphabetSize];

			//Init
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

		public virtual int this[int index]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			get
			{
				return index < 0 || index > this.BadChars.Length
					? throw new ArgumentOutOfRangeException(nameof(index), $"{nameof(index)}={index}, {nameof(this.BadChars.Length)}={this.BadChars.Length}")
					: this.BadChars[index];
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			protected set
			{
				if (index < 0 || index > this.BadChars.Length)
				{
					throw new ArgumentOutOfRangeException(nameof(index), $"{nameof(index)}={index}, {nameof(this.BadChars.Length)}={this.BadChars.Length}");
				}
				this.BadChars[index] = value;
			}
		}
	};

};


