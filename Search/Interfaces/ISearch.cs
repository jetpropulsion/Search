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
        delegate bool Found(int offset);
    		void Init(ReadOnlySpan<byte> pattern);
				void Search(ReadOnlySpan<byte> pattern, ReadOnlySpan<byte> buffer, int offset, Found found);

    };  //END: public interface ISearch

};


