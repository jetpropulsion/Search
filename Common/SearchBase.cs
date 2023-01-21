using Search.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Search.Common
{
	public class SearchBase : ISearch
	{
		public ReadOnlyMemory<byte>? PatternMemory
		{
			get;
			protected set;
		}
		= null;

		public ReadOnlySpan<byte> PatternSpan
		{
			get
			{
				if (PatternMemory == null)
				{
					throw new ArgumentNullException(nameof(PatternMemory));
				}
				if (!PatternMemory.HasValue)
				{
					throw new ArgumentException("Pattern has not been set", nameof(PatternMemory));
				}
				return this.PatternMemory.Value.Span;
			}
		}
		public ISearch.OnMatchFoundDelegate? OnPatternMatches
		{
			get;
			protected set;
		} = null;

		public SearchBase()
		{
		}

		public SearchBase(in ReadOnlyMemory<byte> patternMemory, ISearch.OnMatchFoundDelegate patternMatched)
		{
			(this as ISearch).Init(patternMemory, patternMatched);
		}

		public virtual void Init(in ReadOnlyMemory<byte> patternMemory, ISearch.OnMatchFoundDelegate patternMatched)
		{
			this.OnPatternMatches = patternMatched;
			this.PatternMemory = patternMemory;
		}

		public virtual void Validate()
		{
			ArgumentNullException.ThrowIfNull(PatternMemory, nameof(PatternMemory));
			ArgumentNullException.ThrowIfNull(OnPatternMatches, nameof(OnPatternMatches));
		}

		public virtual void Search(in ReadOnlyMemory<byte> bufferMemory, int offset)
		{
			throw new NotImplementedException(nameof(Search));
		}
	};
};
