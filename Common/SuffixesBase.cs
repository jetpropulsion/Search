namespace Search.Common
{
	using System.Runtime.CompilerServices;

	//In reference literature and/or implementation, "SuffixesBase" is known as "suff"
	public class SuffixesBase
	{
		public readonly int[] Suffixes;

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public SuffixesBase(in ReadOnlySpan<byte> pattern)
		{
			int m = pattern.Length;

			//Allocate
			this.Suffixes = new int[m];

			int mm1 = m - 1;
			int mm2 = m - 2;
			int f = 0;
			int g = mm1;
			this.Suffixes[mm1] = m;

			for (int i = mm2; i >= 0; --i)
			{
				if ((i > g) && (this.Suffixes[i + mm1 - f] < i - g))
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

		public virtual int this[int index]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			get
			{
				return index < 0
					? throw new ArgumentOutOfRangeException(nameof(index), $"provided index is less than zero: {nameof(index)}={index}, {nameof(this.Suffixes.Length)}={this.Suffixes.Length}")
					: index >= this.Suffixes.Length
					? throw new ArgumentOutOfRangeException(nameof(index), $"provided index is greater than the length: {nameof(index)}={index}, {nameof(this.Suffixes.Length)}={this.Suffixes.Length}")
					: this.Suffixes[index];
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			protected set
			{
				if (index < 0)
				{
					throw new ArgumentOutOfRangeException(nameof(index), $"provided index is less than zero: {nameof(index)}={index}, {nameof(this.Suffixes.Length)}={this.Suffixes.Length}");
				}
				if (index >= this.Suffixes.Length)
				{
					throw new ArgumentOutOfRangeException(nameof(index), $"provided index is greater than the length: {nameof(index)}={index}, {nameof(this.Suffixes.Length)}={this.Suffixes.Length}");
				}
				this.Suffixes[index] = value;
			}
		}
	};
};


