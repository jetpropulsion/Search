namespace Search.Algorithms
{
	using Search.Attributes;
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

	[Experimental]
	public class BerryRavindran : SearchBase
	{
		public BadCharsBerryRavindran? BadChars { get; protected set; } = null;


		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public override void Init(in ReadOnlyMemory<byte> patternMemory, ISearch.OnMatchFoundDelegate patternMatched)
		{
			base.Init(patternMemory, patternMatched);
			this.BadChars = new BadCharsBerryRavindran(patternMemory.Span);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public override void FixSearchBuffer(ref Memory<byte> buffer, int bufferSize, in ReadOnlyMemory<byte> pattern)
		{
			int additionalSize = buffer.Length - bufferSize;
			if(additionalSize >= pattern.Length)
			{
				buffer.Span.Slice(bufferSize, pattern.Length).Fill(0);
				return;
			}
			int additionalSizeToAdd = pattern.Length - additionalSize;
			//This method enlarges the search buffer to allow certain search algorithms to stop, like this algorithm
			byte[] enlargedBuffer;
			SearchBase.EnlargeBuffer(buffer, pattern, additionalSizeToAdd, out bufferSize, out enlargedBuffer);
			buffer.Span.Slice(bufferSize, pattern.Length).Fill(0);
			buffer = new Memory<byte>(enlargedBuffer);
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
			int nmm = n - m;
			//Searching

			//y[n + 1] = '\0';

			int j = 0;
			while (j <= nmm)
			{
				if (pattern.SequenceEqual(buffer.Slice(j, m)))
				{
					if (!this.OnMatchFound!(j, this.GetType()))
					{
						return;
					}
				}
				//if (j > nmm)
				//{
				//	//NOTE: Fix, original was breaking the bounds on very last comparison
				//	break;
				//}
				j += this.BadChars![ buffer[j + m], buffer[j + mp1] ];
			}
		}

	}};  //END: namespace Search
