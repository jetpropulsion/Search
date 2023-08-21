namespace Search.Common
{
	using Search.Interfaces;

	using System.Runtime.CompilerServices;

	public class SearchBase : ISearch
	{
		public ReadOnlyMemory<byte>? Pattern
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
				ArgumentNullException.ThrowIfNull(this.Pattern, nameof(this.Pattern));
				return !this.Pattern.HasValue
					? throw new ArgumentException("Pattern has not been set", nameof(this.Pattern))
					: this.Pattern.Value.Span;
			}
		}
		public ISearch.OnMatchFoundDelegate? OnMatchFound
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			get;
			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			protected set;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public SearchBase()
		{
			this.Pattern = null;
			this.OnMatchFound = null;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public SearchBase(in ReadOnlyMemory<byte> pattern, ISearch.OnMatchFoundDelegate patternMatched) => (this as ISearch).Init(pattern, patternMatched);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public virtual void Init(in ReadOnlyMemory<byte> pattern, ISearch.OnMatchFoundDelegate patternMatched)
		{
			this.OnMatchFound = patternMatched;
			this.Pattern = pattern;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public virtual void Validate()
		{
			ArgumentNullException.ThrowIfNull(this.Pattern, nameof(this.Pattern));
			ArgumentNullException.ThrowIfNull(this.OnMatchFound, nameof(this.OnMatchFound));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		//Method is intentionally throwing an exception on Search() attempt. To resolve, inherit from SearchBase
		public virtual void Search(in ReadOnlyMemory<byte> bufferMemory, int offset, int size) => throw new NotImplementedException(nameof(Search));

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public virtual bool IsEnlargementNeeded() => false;

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public virtual void GetEnlargedBuffer(in ReadOnlyMemory<byte> buffer, in ReadOnlyMemory<byte> pattern, out int bufferSize, out byte[] enlargedBuffer) =>
			GetEnlargedBuffer(buffer, pattern, 0, out bufferSize, out enlargedBuffer);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static void GetEnlargedBuffer(in ReadOnlyMemory<byte> buffer, in ReadOnlyMemory<byte> pattern, int additionalSize, out int bufferSize, out byte[] enlargedBuffer)
		{
			bufferSize = buffer.Length;
			//TODO: increment enlargedSize in inherited classes, as needed
			int enlargedSize = buffer.Length + additionalSize;
			enlargedBuffer = new byte[enlargedSize];
			Array.Copy(buffer.ToArray(), 0, enlargedBuffer, 0, bufferSize);
		}
	};
};

