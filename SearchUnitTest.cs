using System.Collections.Concurrent;
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
		static readonly Memory<byte> patternMemory = Encoding.UTF8.GetBytes(pattern).AsMemory();
		static readonly Memory<byte> bufferMemory = Encoding.UTF8.GetBytes(buffer).AsMemory();
		static readonly List<int> simpleExpectedOffsets = new List<int>() { 3, 31, 56, 88 };

		static readonly ConcurrentDictionary<Type, List<int>> resultsMap = new ConcurrentDictionary<Type, List<int>>();

		public static List<int> addOffset(Type type, int offset)
		{
			return new List<int>() { offset };
		}
		public static List<int> updateOffsets(Type type, List<int> offsets, int offset)
		{
			int[] result = new int[offsets.Count + 1];
			offsets.CopyTo(result, 0);
			result[offsets.Count] = offset;
			return result.ToList();
		}
		public static bool DisplayOffset(int offset, Type caller)
		{
			resultsMap.AddOrUpdate<int>(caller, addOffset, updateOffsets, offset);

			Trace.WriteLine($"({caller.FullName}) has found \"{pattern}\" at offset: {offset}");
			return true;
		}

		[TestMethod]
		[Timeout(15000)]
		public void Test_Boyer_Moore()
		{
			Type type = typeof(Search.BoyerMoore);
			Trace.WriteLine($"[{DateTimeOffset.Now.ToString("O")}] {type.FullName}");
			ISearch search = new Search.BoyerMoore(patternMemory, (int offset, Type caller) => DisplayOffset(offset, caller));
			search.Search(bufferMemory, 0);
			Assert.IsTrue(simpleExpectedOffsets.Count <= 0 || resultsMap.ContainsKey(type));
			Assert.IsTrue(resultsMap[type].SequenceEqual(simpleExpectedOffsets));
		}

		[TestMethod]
		[Timeout(15000)]
		public void Test_Turbo_Boyer_Moore()
		{
			Type type = typeof(Search.TurboBoyerMoore);
			Trace.WriteLine($"[{DateTimeOffset.Now.ToString("O")}] {type.FullName}");
			ISearch search = new Search.TurboBoyerMoore(patternMemory, (int offset, Type caller) => DisplayOffset(offset, caller));
			search.Search(bufferMemory, 0);
			Assert.IsTrue(simpleExpectedOffsets.Count <= 0 || resultsMap.ContainsKey(type));
			Assert.IsTrue(resultsMap[type].SequenceEqual(simpleExpectedOffsets));
		}

		[TestMethod]
		[Timeout(15000)]
		public void Test_Zsu_Takaoka()
		{
			Type type = typeof(Search.ZsuTakaoka);
			Trace.WriteLine($"[{DateTimeOffset.Now.ToString("O")}] {type.FullName}");
			ISearch search = new Search.ZsuTakaoka(patternMemory, (int offset, Type caller) => DisplayOffset(offset, caller));
			search.Search(bufferMemory, 0);
			Assert.IsTrue(simpleExpectedOffsets.Count <= 0 || resultsMap.ContainsKey(type));
			Assert.IsTrue(resultsMap[type].SequenceEqual(simpleExpectedOffsets));
		}

		[TestMethod]
		[Timeout(15000)]
		public void Test_Horspool()
		{
			Type type = typeof(Search.Horspool);
			Trace.WriteLine($"[{DateTimeOffset.Now.ToString("O")}] {type.FullName}");
			ISearch search = new Search.Horspool(patternMemory, (int offset, Type caller) => DisplayOffset(offset, caller));
			search.Search(bufferMemory, 0);
			Assert.IsTrue(simpleExpectedOffsets.Count <= 0 || resultsMap.ContainsKey(type));
			Assert.IsTrue(resultsMap[type].SequenceEqual(simpleExpectedOffsets));
		}

		[TestMethod]
		[Timeout(15000)]
		public void Test_Raita()
		{
			Type type = typeof(Search.Raita);
			Trace.WriteLine($"[{DateTimeOffset.Now.ToString("O")}] {type.FullName}");
			ISearch search = new Search.Raita(patternMemory, (int offset, Type caller) => DisplayOffset(offset, caller));
			search.Search(bufferMemory, 0);
			Assert.IsTrue(simpleExpectedOffsets.Count <= 0 || resultsMap.ContainsKey(type));
			Assert.IsTrue(resultsMap[type].SequenceEqual(simpleExpectedOffsets));
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
		public void Test_All_ISearch_Derivates()
		{
			Assembly asm = typeof(Search.Interfaces.ISearch).Assembly;
			if (asm == null)
			{
				throw new ApplicationException("Something wrong has happened.");
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
				genericSearch.Init(patternMemory, (int offset, Type caller) => { dict[algorithm].Add(offset); return true; });
				if(!dict.ContainsKey(algorithm))
				{
					dict.Add(algorithm, new List<int>());
				}
				genericSearch.Search(bufferMemory, 0);
			}
			
			List<int> referenceOffsets = new List<int>();
			ISearch referenceSearch = new Search.BruteForce();
			referenceSearch.Init(patternMemory, (int offset, Type caller) => { referenceOffsets.Add(offset); return true; });
			referenceSearch.Search(bufferMemory, 0);
			referenceOffsets.Sort();
			for (int i = 0; i < referenceOffsets.Count; ++i)
			{
				Trace.WriteLine($"Expected match at position {i}: {referenceOffsets[i]}");
			}

			int discrepancies = 0;
			foreach (string key in dict.Keys.OrderBy(x => x))
			{
				List<int> offsets = dict[key];
				offsets.Sort();
				if(offsets.Count != referenceOffsets.Count || !offsets.SequenceEqual(referenceOffsets))
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



