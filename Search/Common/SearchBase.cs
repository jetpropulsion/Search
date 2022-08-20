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
        protected ReadOnlyMemory<byte>? PatternMemory = null;
        protected ISearch.OnMatchFoundDelegate? OnFound = null;
        public SearchBase()
        {
        }
        public SearchBase(ReadOnlyMemory<byte> patternMemory, ISearch.OnMatchFoundDelegate onFound)
        {
            (this as ISearch).Init(patternMemory, onFound);
        }
        public virtual void Init(ReadOnlyMemory<byte> patternMemory, ISearch.OnMatchFoundDelegate onFound)
        {
            OnFound = onFound;
            PatternMemory = patternMemory;
        }
        public virtual void Validate()
        {
            if (PatternMemory == null)
            {
                throw new ArgumentNullException(nameof(PatternMemory));
            }
            if (OnFound == null)
            {
                throw new ArgumentNullException(nameof(OnFound));
            }
        }
        public virtual void Search(ReadOnlyMemory<byte> bufferMemory, int offset)
        {
            throw new NotImplementedException(nameof(Search));
        }
    };
};
