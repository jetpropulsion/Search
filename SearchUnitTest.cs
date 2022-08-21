using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Net.Http.Headers;
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

		public static List<int> createOffsets(Type type, int offset)
		{
			return new List<int>() { offset };
		}
		public static List<int> appendOffsets(Type type, List<int> offsets, int offset)
		{
			int[] result = new int[offsets.Count + 1];
			offsets.CopyTo(result, 0);
			result[offsets.Count] = offset;
			return result.ToList();
		}
		public static bool DisplayOffset(int offset, Type caller)
		{
			resultsMap.AddOrUpdate<int>(caller, createOffsets, appendOffsets, offset);

			//Trace.WriteLine($"({caller.FullName}) has found \"{pattern}\" at offset: {offset}");
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

		public static int GetRandom(int min, int max) => Random.Shared.Next(min, max);

		public struct Stats
		{
			public List<int> Offsets = new List<int>();
			public TimeSpan InitTime = TimeSpan.Zero;
			public TimeSpan SearchTime = TimeSpan.Zero; //{ get; set; }
			public Stats()
			{
			}
		};

		[TestMethod]
		//[Timeout(120000)]
		public void Test_All_ISearch_Derivates()
		{
			Assembly asm = typeof(Search.Interfaces.ISearch).Assembly;
			if (asm == null)
			{
				throw new ApplicationException("Something wrong has happened.");
			}

			string single = string.Concat(Enumerable.Repeat<char>('-', 80 - 1));

			int maxTestIterations = 20;
			Dictionary<Type, Stats> dict = new();
			for (int testIteration = 1; testIteration <= maxTestIterations; ++testIteration)
			{
				dict.Keys.ToList().ForEach(k => dict[k].Offsets.Clear());

				int minPatternSize = 3;
				int maxPatternSize = 273;
				int patternSize = GetRandom(minPatternSize, maxPatternSize);
				int minBufferSize = 1048576;
				int maxBufferSize = 1048576 * 256;
				int bufferSize = GetRandom(minBufferSize, maxBufferSize);

				byte[] testPattern = new byte[patternSize];
				Random.Shared.NextBytes(testPattern);
				byte[] testBuffer = new byte[bufferSize];
				Array.Fill<byte>(testBuffer, 0, 0, bufferSize);

				Trace.WriteLine($"Generator: iteration={testIteration}, patternSize={patternSize}, bufferSize={bufferSize}");

				int testOffset = 0;
				List<int> testOffsets = new ();
				int lastOffset = bufferSize - patternSize;
				for (int i = 0; i < 1000 && testOffset + patternSize < lastOffset; ++i)
				{
					int offset = Random.Shared.Next(testOffset, Math.Min(testOffset + (bufferSize / patternSize), lastOffset));
					testOffset = offset + patternSize;
					testOffsets.Add(offset);
					testPattern.CopyTo(testBuffer, offset);

					//Trace.WriteLine($"Generator: inserting at {offset}");
				}

				Stopwatch initWatch = new();
				Stopwatch searchWatch = new();
				foreach (Type type in ((TypeInfo[])asm.DefinedTypes).Select(t => t.UnderlyingSystemType))
				{
					bool hasMetric = type.GetInterfaces().Contains(typeof(Search.Interfaces.ISearch));
					if (!type.IsClass || type.IsAbstract || !hasMetric)
					{
						continue;
					}
					if (typeof(Search.Common.SearchBase).Equals(type))
					{
						continue;
					}

					if (!dict.ContainsKey(type)) dict.Add(type, new Stats());
					Stats stat = dict[type];

					if (string.IsNullOrWhiteSpace(type.FullName)) throw new ApplicationException("unexpected type behavior");

					Assembly assembly = type.Assembly;
					ISearch genericSearch = (ISearch)(assembly.CreateInstance(type.FullName, false) ?? throw new ApplicationException(type.FullName));

					//Trace.WriteLine($"Running \"{type.FullName}\"");

					initWatch.Restart();
					genericSearch.Init(testPattern, (int offset, Type caller) => { dict[caller].Offsets.Add(offset); return true; });
					initWatch.Stop();
					TimeSpan elapsedInit = initWatch.Elapsed;
					stat.InitTime += elapsedInit;


					searchWatch.Restart();
					genericSearch.Search(testBuffer, 0);
					searchWatch.Stop();
					TimeSpan elapsedSearch = searchWatch.Elapsed;
					stat.SearchTime += elapsedSearch;

					dict[type] = stat;
				}

				//foreach(Type type in dict.Keys.OrderBy(t => t.FullName, StringComparer.Ordinal))
				//{
				//	Trace.WriteLine($"Algorithm \"{type.FullName}\" - Init: {dict[type].InitTime.TotalMilliseconds:F4} ms, Search: {dict[type].SearchTime.TotalMilliseconds:F4} ms");
				//}
				//Trace.WriteLine(single);

				List<int> referenceOffsets = new List<int>();
				ISearch referenceSearch = new Search.BruteForce();
				referenceSearch.Init(testPattern, (int offset, Type caller) => { referenceOffsets.Add(offset); return true; });
				referenceSearch.Search(testBuffer, 0);
				referenceOffsets.Sort();

				//for (int i = 0; i < referenceOffsets.Count; ++i)
				//{
				//	Trace.WriteLine($"Expected match at position {i}: {referenceOffsets[i]}");
				//}
				Assert.IsTrue(testOffsets.Count == referenceOffsets.Count && testOffsets.SequenceEqual(referenceOffsets));

				int discrepancies = 0;
				foreach (Type key in dict.Keys.OrderBy(x => x.FullName, StringComparer.Ordinal))
				{
					List<int> offsets = dict[key].Offsets;
					offsets.Sort();
					if(offsets.Count != referenceOffsets.Count || !offsets.SequenceEqual(referenceOffsets))
					{
						++discrepancies;
						Trace.WriteLine($"results of the algorithm run \"{key}\" differs from brute force");
						for (int i = 0; i < offsets.Count; ++i)
						{
							Trace.WriteLine($"algorithm \"{key.FullName}\" match at position {i}: {offsets[i]}");
						}
					}
				}
				if(discrepancies != 0)
				{
					Debug.WriteLine($"Total {discrepancies} discrepancies.");
					Assert.Fail();
				}
			} //END: for(int testIteration

			foreach (Type type in dict.Keys.OrderBy(t => t.FullName, StringComparer.Ordinal))
			{
				Trace.WriteLine($"Algorithm \"{type.FullName}\" - Init: {dict[type].InitTime.TotalMilliseconds:F4} ms, Search: {dict[type].SearchTime.TotalMilliseconds:F4} ms");
			}
			Trace.WriteLine(single);

		}

	};	//END: class SearchUnitTest
};	//END: namespace SearchTest



