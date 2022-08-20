using System.Diagnostics;
using System.Text;

namespace SearchTest
{
	[TestClass]
	public class SearchUnitTest
	{
		[TestMethod]
		public void TestMethod1()
		{
			Trace.WriteLine($"{nameof(TestMethod1)} [{DateTimeOffset.Now.ToString("O")}]");

			string pattern = "456";
			string buffer = "12345678.......abcdef.......123456";

			Search.ISearch bm = new Search.BoyerMoore();
			bm.Search(Encoding.UTF8.GetBytes(pattern), Encoding.UTF8.GetBytes(buffer), (int offset) =>
			{
				Trace.WriteLine($"Found \"{pattern}\" at offset: {offset}");
			});

		}
	};
};



