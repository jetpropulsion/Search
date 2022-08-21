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
        public ReadOnlyMemory<byte>? PatternMemory { get; protected set; } = null;
        public ReadOnlySpan<byte> PatternSpan => this.PatternMemory.GetValueOrDefault().Span;
        public ISearch.OnMatchFoundDelegate? OnPatternMatches { get; protected set; } = null;
        public SearchBase()
        {
        }
        public SearchBase(ReadOnlyMemory<byte> patternMemory, ISearch.OnMatchFoundDelegate patternMatched)
        {
            (this as ISearch).Init(patternMemory, patternMatched);
        }
        public virtual void Init(ReadOnlyMemory<byte> patternMemory, ISearch.OnMatchFoundDelegate patternMatched)
        {
            this.OnPatternMatches = patternMatched;
            this.PatternMemory = patternMemory;
        }
        public virtual void Validate()
        {
            if (this.PatternMemory == null) throw new ArgumentNullException(nameof(PatternMemory));
            if (this.OnPatternMatches == null) throw new ArgumentNullException(nameof(OnPatternMatches));
        }
        public virtual void Search(ReadOnlyMemory<byte> bufferMemory, int offset)
        {
            throw new NotImplementedException(nameof(Search));
        }
    };
};
