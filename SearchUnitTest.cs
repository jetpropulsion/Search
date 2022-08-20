using System.Diagnostics;
using System.Reflection;
using System.Text;
using Search.Interfaces;

namespace SearchTest
{
	[TestClass]
	public class SearchUnitTest
	{
		const string pattern = "456";
		const string buffer = "12345678.......abcdef.......123456..#@%45/////..........456455////////4$$$$$////////////456";
		static Memory<byte> patternMemory = Encoding.UTF8.GetBytes(pattern).AsMemory();
		static Memory<byte> bufferMemory = Encoding.UTF8.GetBytes(buffer).AsMemory();

		public static bool DisplayOffset(int offset)
		{
			Trace.WriteLine($"Found \"{pattern}\" at offset: {offset}");
			return true;
		}

		[TestMethod]
		[Timeout(15000)]
		public void TestBoyerMoore()
		{
			Trace.WriteLine($"[{DateTimeOffset.Now.ToString("O")}] {nameof(TestBoyerMoore)}");
			ISearch search = new Search.BoyerMoore();
			search.Init(patternMemory, (int offset) => DisplayOffset(offset));
			search.Search(bufferMemory, 0);
		}

		[TestMethod]
		[Timeout(15000)]
		public void TestTurboBoyerMoore()
		{
			Trace.WriteLine($"[{DateTimeOffset.Now.ToString("O")}] {nameof(TestTurboBoyerMoore)}");
			ISearch search = new Search.TurboBoyerMoore();
			search.Init(patternMemory, (int offset) => DisplayOffset(offset));
			search.Search(bufferMemory, 0);
		}

		[TestMethod]
		[Timeout(15000)]
		public void TestZsuTakaoka()
		{
			Trace.WriteLine($"[{DateTimeOffset.Now.ToString("O")}] {nameof(TestZsuTakaoka)}");
			ISearch search = new Search.ZsuTakaoka();
			search.Init(patternMemory, (int offset) => DisplayOffset(offset));
			search.Search(bufferMemory, 0);
		}

		[TestMethod]
		[Timeout(15000)]
		public void TestHorspool()
		{
			Trace.WriteLine($"[{DateTimeOffset.Now.ToString("O")}] {nameof(TestHorspool)}");
			ISearch search = new Search.Horspool();
			search.Init(patternMemory, (int offset) => DisplayOffset(offset));
			search.Search(bufferMemory, 0);
		}

		[TestMethod]
		[Timeout(15000)]
		public void TestRaita()
		{
			Trace.WriteLine($"[{DateTimeOffset.Now.ToString("O")}] {nameof(TestRaita)}");
			ISearch search = new Search.Raita();
			search.Init(patternMemory, (int offset) => DisplayOffset(offset));
			search.Search(bufferMemory, 0);
		}

		[TestMethod]
		[Timeout(15000)]
		public void TestQuickSearch()
		{
			Trace.WriteLine($"[{DateTimeOffset.Now.ToString("O")}] {nameof(TestQuickSearch)}");
			ISearch search = new Search.QuickSearch();
			search.Init(patternMemory, (int offset) => DisplayOffset(offset));
			search.Search(bufferMemory, 0);
		}

		/*****************************************************************************************************************
		*******************************************************************************************************************
		*** Changes for Visual Studio 2022
		*******************************************************************************************************************
		***
		*** https://docs.microsoft.com/en-us/visualstudio/test/mstest-update-to-mstestv2?view=vs-2022
		*** Remove the assembly reference to Microsoft.VisualStudio.QualityTools.UnitTestFramework from your unit test project.
		*** Add NuGet package references to MSTestV2 including the MSTest.TestFramework and the MSTest.TestAdapter packages on nuget.org. You can install packages in the NuGet Package Manager Console with the following commands:
		*** Console
		*** Copy
		*** PM> Install-Package MSTest.TestAdapter -Version 2.1.2
		*** PM> Install-Package MSTest.TestFramework -Version 2.1
		***
		********************************************************************************************************************
		 *****************************************************************************************************************/

		[TestMethod]
		[Timeout(60000)]
		public void TestAllDerivingFromISearch()
		{
			Assembly asm = typeof(Search.Interfaces.ISearch).Assembly;
			if (asm == null)
			{
				throw new ApplicationException("Something wrong happened.");
			}

			Dictionary<string, List<int>> dict = new Dictionary<string, List<int>>();
			foreach (TypeInfo ti in (TypeInfo[])asm.DefinedTypes)
			{
				bool hasMetric = ti.GetInterfaces().Contains(typeof(Search.Interfaces.ISearch));
				if (!ti.IsClass || ti.IsAbstract || !hasMetric)
				{
					continue;
				}
				if(string.IsNullOrWhiteSpace(ti.FullName))
				{
					throw new ApplicationException("unexpected type behavior");
				}
				string algorithm = ti.FullName!;
				if(algorithm.Equals("Search.Common.SearchBase"))
				{
					continue;
				}
				//string sep = string.Concat(Enumerable.Repeat<char>('-', Console.WindowWidth - 1));

				//Trace.WriteLine($"{algorithm}");
				Assembly assembly = ti.Assembly;
				ISearch genericSearch = (ISearch)(assembly.CreateInstance(algorithm, false) ?? throw new ApplicationException(algorithm));
				genericSearch.Init(patternMemory, (int offset) => { dict[algorithm].Add(offset); return true; });
				if(!dict.ContainsKey(algorithm))
				{
					dict.Add(algorithm, new List<int>());
				}
				genericSearch.Search(bufferMemory, 0);
			}
			
			List<int> expectedOffsets = new List<int>();
			ISearch referenceSearch = new Search.BruteForce();
			referenceSearch.Init(patternMemory, (int offset) => { expectedOffsets.Add(offset); return true; });
			referenceSearch.Search(bufferMemory, 0);
			expectedOffsets.Sort();
			for (int i = 0; i < expectedOffsets.Count; ++i)
			{
				Trace.WriteLine($"Expected match at position {i}: {expectedOffsets[i]}");
			}

			int discrepancies = 0;
			foreach (string key in dict.Keys.OrderBy(x => x))
			{
				List<int> offsets = dict[key];
				offsets.Sort();
				if(offsets.Count != expectedOffsets.Count || !offsets.SequenceEqual(expectedOffsets))
				{
					++discrepancies;
					Trace.WriteLine($"results of the algorithm run \"{key}\" differs from brute force");
					for (int i = 0; i < offsets.Count; ++i)
					{
						Trace.WriteLine($"algorithm \"{key}\" match at position {i}: {offsets[i]}");
					}
				}
			}
			if(discrepancies != 0)
			{
				Debug.WriteLine($"Total {discrepancies} discrepancies.");
				Assert.Fail();
			}
		}

	};	//END: class SearchUnitTest
};	//END: namespace SearchTest



