using System.Diagnostics;
using System.Text;
using Search.Interfaces;

namespace SearchTest
{
	[TestClass]
	public class SearchUnitTest
	{
		[TestMethod]
		public void TestBoyerMoore()
		{
			Trace.WriteLine($"{nameof(TestBoyerMoore)} [{DateTimeOffset.Now.ToString("O")}]");

			string pattern = "456";
			string buffer = "12345678.......abcdef.......123456";

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

			string pattern = "456";
			string buffer = "12345678.......abcdef.......123456";

			ISearch tbm = new Search.TurboBoyerMoore();
			tbm.Search(Encoding.UTF8.GetBytes(pattern), Encoding.UTF8.GetBytes(buffer), 0, (int offset) =>
			{
				Trace.WriteLine($"Found \"{pattern}\" at offset: {offset}");
				return true;
			});

		}

	};	//END: class SearchUnitTest
};	//END: namespace SearchTest



