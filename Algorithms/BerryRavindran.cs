namespace Search.Algorithms
{
	using Search.Common;
	using Search.Interfaces;

	using System.Diagnostics;
	using System.Numerics;
	using System.Runtime.CompilerServices;

	/// <summary>
	//	name:										Berry Ravindran algorithm
	//	search direction:				?
	//	preprocess complexity:	O(m+sigma^2) space and time complexity
	//	search complexity:			O(mn) time complexity
	//	worst case:							?
	//	ref:										T. Berry and S. Ravindran., Proceedings of the Prague Stringology Club Workshop '99, pp.16--28, ctu, (1999).
	/// </summary>

	[Experimental(nameof(BerryRavindran))]
	public class BerryRavindran : SearchBase
	{
		public BadCharsBerryRavindran? BadChars { get; protected set; } = null;

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public BerryRavindran() :
			base()
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public BerryRavindran(in ReadOnlyMemory<byte> patternMemory, ISearch.OnMatchFoundDelegate patternMatched) :
			base(patternMemory, patternMatched)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public override void Init(in ReadOnlyMemory<byte> patternMemory, ISearch.OnMatchFoundDelegate patternMatched)
		{
			base.Init(patternMemory, patternMatched);
			this.BadChars = new BadCharsBerryRavindran(patternMemory.Span);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public override void Validate()
		{
			base.Validate();
			ArgumentNullException.ThrowIfNull(this.BadChars, nameof(this.BadChars));
		}

#if DEBUG
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
#else
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#endif
		public override void Search(in ReadOnlyMemory<byte> bufferMemory, int offset)
		{
			this.Validate();

			//Searching
			ReadOnlySpan<byte> pattern = this.PatternSpan;
			ReadOnlySpan<byte> buffer = bufferMemory.Span;

			int m = pattern.Length;
			int n = buffer.Length;

			int mm1 = m - 1;
			int mm2 = m - 2;
			int mp1 = m + 1;
			int nmm = n - m;
			//Searching

			//y[n + 1] = '\0';

///*
			int j = 0;
			while (j <= nmm)
			{
				if (pattern.SequenceEqual(buffer.Slice(j, m)))
				{
					if (!this.OnPatternMatches!(j, this.GetType()))
					{
						return;
					}
				}
				j += this.BadChars![ buffer[j + m], buffer[j + mp1] ];
			}
		}
//*/
/*
			int j = 0;
			while (j <= nmm)
			{
				int i = 0;
				while (i < m && pattern[i] == buffer[j + i])
				{
					++i;
				}
				//for (i = 0; i < m && pattern[i] == buffer[j + i]; i++) ;
				if (i >= m)
				{
					if(!this.OnPatternMatches!(j, this.GetType()))
					{
						return;
					}
				}

				int jpm = j + m;
				int jpmp1 = j + mp1;
				if(jpm > nmm)
				{
					Trace.WriteLine($"j + m > n - m: m={m}, n={n}, n-m={nmm}, j={j}, j+m={jpm}, j+m+1={jpmp1}");
					//break;
					jpm = nmm;
				}
				if (jpmp1 > nmm)
				{
					Trace.WriteLine($"j + m + 1 > n - m: m={m}, n={n}, n-m={nmm}, j={j}, j+m={jpm}, j+m+1={jpmp1}");
					//break;
					jpmp1 = nmm;
				}
				j += this.BadChars![buffer[jpm], buffer[jpmp1]];
				//j += this.BadChars![buffer[j + m], buffer[j + mp1]];
			}
		}
*/
	};  //END: class BerryRavindran
};  //END: namespace Search
