using DirParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirParser.Searchers
{
	public class NameSearcher : ISearcher
	{
		public SearchTypeEnum SearchType
		{
			get
			{
				return SearchTypeEnum.Name;
			}
		}

		public List<DirectoryIndex> DoSearch(string term, List<DirectoryIndex> index)
		{
			//the "or" here prioritizes exact matches, then also may search for a fuzzier match (i.e. a term of "am" will still return "sample.pdf")
			return index.Where(a => a.FileName.Equals(term.ToLower()) || a.FileName.Contains(term.ToLower())).AsParallel().ToList();
		}
	}
}
