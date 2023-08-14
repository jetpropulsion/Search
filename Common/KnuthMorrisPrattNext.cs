namespace Search.Common
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.CompilerServices;
	using System.Text;
	using System.Threading.Tasks;

	public class KnuthMorrisPrattNext
	{
		public readonly int[] Next;
		public KnuthMorrisPrattNext(in ReadOnlySpan<byte> pattern)
		{
			int m = pattern.Length;
			//this.Next = new int[m + 1];
			this.Next = Enumerable.Repeat<int>(0, m + 1).ToArray();
			this.Next[0] = -1;

			int i = 0;
			int j = -1;

			while (i < m)
			{
				while (j > -1 && pattern[i] != pattern[j])
				{
					j = this.Next[j];
				}
				i++;
				j++;
				if (i < m && pattern[i] == pattern[j])
					this.Next[i] = this.Next[j];
				else
					this.Next[i] = j;
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

	};	//END: public class Knuth
}
