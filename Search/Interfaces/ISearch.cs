using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Search.Interfaces
{
    public interface ISearch
    {
        //user will received a call to specified Found delegate; if user code returns false, search will be aborted
        delegate bool OnMatchFoundDelegate(int offset);

        //used to support parameterless ctors, after the instance creation, call the Init(...)
    		abstract void Init(ReadOnlyMemory<byte> patternMemory, OnMatchFoundDelegate onFound);

        //actual search, only a reference to the buffer and offset inside it is needed, matches will be reported via the delegate
				abstract void Search(ReadOnlyMemory<byte> bufferMemory, int offset);

        //validates internal object state, i.e., are all necessary fields initialized, this is necessary because late initialization
        abstract void Validate();
    };
};
