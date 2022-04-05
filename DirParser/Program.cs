using DirParser.Models;
using DirParser.Searchers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace DirParser
{
	
	class Program
	{
		
		/// <summary>
		/// I would write this with a better method breakdown than a long method.  But this should still be readable.
		/// i.e. 
		/// SerializeIndex();
		/// SearchLoop();
		/// </summary>
		/// <param name="args"></param>
		static void Main(string[] args)
		{
			Console.WriteLine("Welcome, specify a directory (hit enter for default):");
			var dir = Console.ReadLine();

			//house cleaning
			if (string.IsNullOrEmpty(dir)) //use executing path, expects test_data directory to be there
			{
				var execDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location); // the dir this application is running from
				dir = Path.Combine(execDir, "test_data");
			}
			else
			{
				if (!Directory.Exists(dir))
				{
					Console.WriteLine("Directory not found.  Press enter to exit.");
					Console.ReadLine();
					Environment.Exit(-1);
				}
			}

			//create index (always, so that on every run changes to the directory are taken into account)
			//this line can be added to only create index if it doesn't exist
			//if (!(File.Exists(Path.Combine(dir, "DirectoryIndex.index"))))

			Console.WriteLine("Indexing . . . ");
			//select
			DirectoryInfo dirInfo = new DirectoryInfo(dir);
			FileInfo[] oldFiles =
				dirInfo.EnumerateFiles("*.*", SearchOption.AllDirectories)
						.AsParallel()   // <- quick performance improvement
						.ToArray();

			//convert to target type that we serialize
			var targetList = oldFiles
				.Select(x => new DirectoryIndex() { FileName = x.Name, FilePath=x.DirectoryName, Type = x.Extension.Replace(".",""), Size = x.Length })
				.AsParallel()
				.ToList();

			//house clean, exclude our serialized file if it exists
			targetList.Remove(targetList.Where(a => a.FileName.Equals("DirectoryIndex.index")).SingleOrDefault());

			//serialize
			var serialized = JsonSerializer.Serialize(targetList);
			File.WriteAllText(Path.Combine(dir, "DirectoryIndex.index"), serialized);
			//deserialize index 
			var index = JsonSerializer.Deserialize<List<DirectoryIndex>>(serialized);

			var keepSearching = true;
			while (keepSearching)
			{
				Console.WriteLine("Ready to search.  Enter terms and press enter.");
				Console.WriteLine("Name=X to search for file name. i.e. N=Test to search for names that equal or contain Test");
				Console.WriteLine("Type=X to search for file type. i.e. T=pdf");
				Console.WriteLine("Size=X to search file size.  i.e. S=>1000 (for 1 k) or S=<10000 (for s is less than 10k)");
				var searchTerm = Console.ReadLine();

				//validate.  the strings should be consts
				if (!searchTerm.Contains("Size") && !searchTerm.StartsWith("Name") && !searchTerm.StartsWith("Type"))
				{
					Console.WriteLine("Invalid term.");
					continue;
				}



				//split to search type and search value
				var tempArray = searchTerm.Split("="); //could be part of validation
				var searchType = tempArray[0];
				var searchExpression = tempArray[1]; //right hand size of equals

				//the actual search using interfaces and factory
				var searchResult = SearcherFactory.GetSearcher(searchType).DoSearch(searchExpression, index);

				if (searchResult.Count == 0)
				{
					Console.WriteLine("Nothing found.");
				}
				foreach (var result in searchResult)
				{
					Console.WriteLine(result);
				}

				Console.WriteLine("Press Enter to Continue, type 'exit' and press enter to exit app");
				var exitResult = Console.ReadLine();
				if (exitResult.Equals("exit"))
				{
					keepSearching = false;
				}


			}		
		}
	}
}
