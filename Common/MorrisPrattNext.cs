namespace Search.Common
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.CompilerServices;
	using System.Text;
	using System.Threading.Tasks;

	public class MorrisPrattNext
	{
		public readonly int[] Next;
		public MorrisPrattNext(in ReadOnlySpan<byte> pattern)
		{
			int m = pattern.Length;

			this.Next = Enumerable.Repeat<int>(0, m + 1).ToArray();
			Next[0] = -1;

			int i = 0;
			int j = -1;

			while (i < m)
			{
				while (j > -1 && pattern[i] != pattern[j])
				{
					j = this.Next[j];
				}
				this.Next[++i] = ++j;
			}
		}
		public virtual int this[int index]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			get => index < 0 || index > this.Next.Length ? throw new ArgumentOutOfRangeException(nameof(index)) : this.Next[index];

			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			protected set
			{
				if (index < 0 || index > this.Next.Length)
				{
					throw new ArgumentOutOfRangeException(nameof(index));
				}

				this.Next[index] = value;
			}
		}

	};  //END: public class MorrisPrattNext
}
