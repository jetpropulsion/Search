using System.Diagnostics;
using System.Text;
using Search.Interfaces;

namespace SearchTest
{
	[TestClass]
	public class SearchUnitTest
	{
		string pattern = "456";
		string buffer = "12345678.......abcdef.......123456..#@%45/////..........456";

		[TestMethod]
		public void TestBoyerMoore()
		{
			Trace.WriteLine($"{nameof(TestBoyerMoore)} [{DateTimeOffset.Now.ToString("O")}]");
			ISearch bm = new Search.BoyerMoore();
			bm.Search(Encoding.UTF8.GetBytes(pattern), Encoding.UTF8.GetBytes(buffer), 0, (int offset) =>
			{
				Trace.WriteLine($"Found \"{pattern}\" at offset: {offset}");
				return true;
			});
		}

		[TestMethod]
		public void TestTurboBoyerMoore()
		{
			Trace.WriteLine($"{nameof(TestTurboBoyerMoore)} [{DateTimeOffset.Now.ToString("O")}]");
			ISearch tbm = new Search.TurboBoyerMoore();
			tbm.Search(Encoding.UTF8.GetBytes(pattern), Encoding.UTF8.GetBytes(buffer), 0, (int offset) =>
			{
				Trace.WriteLine($"Found \"{pattern}\" at offset: {offset}");
				return true;
			});
		}

		[TestMethod]
		public void TestZsuTakaoka()
		{
			Trace.WriteLine($"{nameof(TestZsuTakaoka)} [{DateTimeOffset.Now.ToString("O")}]");
			ISearch zt = new Search.ZsuTakaoka();
			zt.Search(Encoding.UTF8.GetBytes(pattern), Encoding.UTF8.GetBytes(buffer), 0, (int offset) =>
			{
				Trace.WriteLine($"Found \"{pattern}\" at offset: {offset}");
				return true;
			});
		}


	};	//END: class SearchUnitTest
};	//END: namespace SearchTest



