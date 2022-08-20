using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Search.Interfaces;

namespace Search
{
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
		public TurboBoyerMoore() : base()
		{

		}

		public override void Search(ReadOnlyMemory<byte> patternMemory, ReadOnlyMemory<byte> bufferMemory, int offset, ISearch.Found found)
		{
			//Searching
			ReadOnlySpan<byte> pattern = patternMemory.Span;
			ReadOnlySpan<byte> buffer = bufferMemory.Span; 
			int m = pattern.Length;
			int n = buffer.Length;

			int bcShift;
			int i;
			int shift;
			int v;
			int turboShift;
			int mm1 = m - 1;
			int mp1 = m + 1;
			int nmm = n - m;

			//Searching
			int j = offset;
			int u = offset;		//TODO: test, it was 0
			shift = m;
			while (j <= nmm)
			{
				i = m - 1;
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
					if(!found(j))
					{
						return;
					}
					shift = this.GoodSuffixes[0];
					u = m - shift;
				}
				else
				{
					v = mm1 - i;
					turboShift = u - v;
					bcShift = this.BadChars[ buffer[i + j] ] - mp1 + i;
					shift = Math.Max(turboShift, bcShift);
					shift = Math.Max(shift, this.GoodSuffixes[i]);
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
	};	//END: class TurboBoyerMoore
};	//END: namespace Search


