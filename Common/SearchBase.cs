namespace Search.Common
{
	using Search.Interfaces;

	using System.Runtime.CompilerServices;

	public class SearchBase : ISearch
	{
		public ReadOnlyMemory<byte>? PatternMemory
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			get;
			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			protected set;
		}

		public ReadOnlySpan<byte> PatternSpan
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			get
			{
				ArgumentNullException.ThrowIfNull(this.PatternMemory, nameof(this.PatternMemory));
				return !this.PatternMemory.HasValue
					? throw new ArgumentException("Pattern has not been set", nameof(this.PatternMemory))
					: this.PatternMemory.Value.Span;
			}
		}
		public ISearch.OnMatchFoundDelegate? OnPatternMatches
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			get;
			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			protected set;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public SearchBase()
		{
			this.PatternMemory = null;
			this.OnPatternMatches = null;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public SearchBase(in ReadOnlyMemory<byte> patternMemory, ISearch.OnMatchFoundDelegate patternMatched) => (this as ISearch).Init(patternMemory, patternMatched);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public virtual void Init(in ReadOnlyMemory<byte> patternMemory, ISearch.OnMatchFoundDelegate patternMatched)
		{
			this.OnPatternMatches = patternMatched;
			this.PatternMemory = patternMemory;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public virtual void Validate()
		{
			ArgumentNullException.ThrowIfNull(this.PatternMemory, nameof(this.PatternMemory));
			ArgumentNullException.ThrowIfNull(this.OnPatternMatches, nameof(this.OnPatternMatches));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public virtual void Search(in ReadOnlyMemory<byte> bufferMemory, int offset) => throw new NotImplementedException(nameof(Search));
	};
};
