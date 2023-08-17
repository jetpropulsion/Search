namespace Search.Algorithms
{
	using Search.Interfaces;

	using System.Runtime.CompilerServices;

	/// <summary>
	//	name:										Turbo Boyer-Moore algorithm
	//	direction:							right to left
	//	preprocess complexity:	O(m+s) time and space
	//	search complexity:			O(n)
	//	worst case:							2n text character comparisons
	//	ref:										CROCHEMORE, M., CZUMAJ A., GASIENIEC L., JAROMINEK S., LECROQ T., PLANDOWSKI W., RYTTER W., 1992, Deux m�thodes pour acc�l�rer l'algorithme de Boyer-Moore, in Th�orie des Automates et Applications, Actes des 2e Journ�es Franco-Belges, D. Krob ed., Rouen, France, 1991, pp 45-63, PUR 176, Rouen, France.
	/// </summary>
	public class TurboBoyerMoore : BoyerMoore, ISearch
	{

#if DEBUG
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
#else
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#endif
		public override void Search(in ReadOnlyMemory<byte> bufferMemory, int offset, int size)
		{
			this.Validate();

			//Searching
			ReadOnlySpan<byte> pattern = base.PatternSpan;
			ReadOnlySpan<byte> buffer = bufferMemory.Span;

			int m = pattern.Length;
			int n = size;

			int mm1 = m - 1;
			int mp1 = m + 1;
			int nmm = n - m;

			//Searching
			int j = offset;
			int u = offset;   //TODO: test, it was 0
			int shift = m;
			while (j <= nmm)
			{
				int i = m - 1;
				while ((i >= 0) && (pattern[i] == buffer[i + j]))
				{
					--i;
					if ((u != 0) && (i == mm1 - shift))
					{
						i -= u;
					}
				}
				if (i < 0)
				{
					if (!this.OnPatternMatches!(j, this.GetType()))
					{
						return;
					}
					shift = this.GoodSuffixes![0];
					u = m - shift;
				}
				else
				{
					int v = mm1 - i;
					int turboShift = u - v;
					int bcShift = this.BadChars![buffer[i + j]] - mp1 + i;
					shift = Math.Max(turboShift, bcShift);
					shift = Math.Max(shift, this.GoodSuffixes![i]);
					if (shift == this.GoodSuffixes[i])
					{
						u = Math.Min(m - shift, v);
					}
					else
					{
						if (turboShift < bcShift)
						{
							shift = Math.Max(shift, u + 1);
						}
						u = 0;
					}
				}
				j += shift;
			}
		}
	};
};  //END: namespace Search


