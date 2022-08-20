using Search.Common;
using Search.Interfaces;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Search
{
	public class QuickSearch : SearchBase
	{
		protected BadCharsBoyerMoore? BadChars = null;
		public QuickSearch() : base()
		{
		}

		public override void Init(ReadOnlyMemory<byte> patternMemory, ISearch.OnMatchFoundDelegate onFound)
		{
			base.Init(patternMemory, onFound);
			this.BadChars = new BadCharsBoyerMoore(patternMemory);
		}

		public override void Validate()
		{
			base.Validate();

			if (this.BadChars == null)
			{
				throw new ArgumentNullException(nameof(this.BadChars));
			}
		}

		public override void Search(ReadOnlyMemory<byte> bufferMemory, int offset)
		{
			this.Validate();
			ReadOnlySpan<byte> pattern = base.PatternMemory!.Value.Span;
			ReadOnlySpan<byte> buffer = bufferMemory.Span;

			int m = pattern.Length;
			int n = buffer.Length;
			int mm1 = m - 1;
			int mp1 = m + 1;

			int j = offset;
			int nmm = n - m;
			while (j <= nmm)
			{
				if(pattern.SequenceEqual(buffer[j..(j + m)]))
				{
					if (!base.OnFound!(j))
					{
						return;
					}
				}
				j += this.BadChars![buffer[j + m - 1]];
			}
		}
	}
}
