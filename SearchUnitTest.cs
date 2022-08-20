using System.Diagnostics;
using System.Text;
using Search.Interfaces;

namespace SearchTest
{
	[TestClass]
	public class SearchUnitTest
	{
		const string pattern = "456";
		const string buffer = "12345678.......abcdef.......123456..#@%45/////..........456";
		static Memory<byte> patternBytes = Encoding.UTF8.GetBytes(pattern).AsMemory();
		static Memory<byte> bufferBytes = Encoding.UTF8.GetBytes(buffer).AsMemory();

		[TestMethod]
		public void TestBoyerMoore()
		{
			Trace.WriteLine($"{nameof(TestBoyerMoore)} [{DateTimeOffset.Now.ToString("O")}]");
			ISearch bm = new Search.BoyerMoore(patternBytes.Span);
			bm.Search(patternBytes.Span, bufferBytes.Span, 0, (int offset) =>
			{
				Trace.WriteLine($"Found \"{pattern}\" at offset: {offset}");
				return true;
			});
		}

		[TestMethod]
		public void TestTurboBoyerMoore()
		{
			Trace.WriteLine($"{nameof(TestTurboBoyerMoore)} [{DateTimeOffset.Now.ToString("O")}]");
			ISearch tbm = new Search.TurboBoyerMoore(patternBytes.Span);
			tbm.Search(patternBytes.Span, bufferBytes.Span, 0, (int offset) =>
			{
				Trace.WriteLine($"Found \"{pattern}\" at offset: {offset}");
				return true;
			});
		}

		[TestMethod]
		public void TestZsuTakaoka()
		{
			Trace.WriteLine($"{nameof(TestZsuTakaoka)} [{DateTimeOffset.Now.ToString("O")}]");
			ISearch zt = new Search.ZsuTakaoka(patternBytes.Span);
			zt.Search(patternBytes.Span, bufferBytes.Span, 0, (int offset) =>
			{
				Trace.WriteLine($"Found \"{pattern}\" at offset: {offset}");
				return true;
			});
		}

		[TestMethod]
		public void TestHorspool()
		{
			Trace.WriteLine($"{nameof(TestHorspool)} [{DateTimeOffset.Now.ToString("O")}]");
			ISearch h = new Search.Horspool(patternBytes.Span);
			h.Search(patternBytes.Span, bufferBytes.Span, 0, (int offset) =>
			{
				Trace.WriteLine($"Found \"{pattern}\" at offset: {offset}");
				return true;
			});
		}

		[TestMethod]
		public void TestRaita()
		{
			Trace.WriteLine($"{nameof(TestRaita)} [{DateTimeOffset.Now.ToString("O")}]");
			ISearch r = new Search.Raita(patternBytes.Span);
			r.Search(patternBytes.Span, bufferBytes.Span, 0, (int offset) =>
			{
				Trace.WriteLine($"Found \"{pattern}\" at offset: {offset}");
				return true;
			});
		}


	};	//END: class SearchUnitTest
};	//END: namespace SearchTest



