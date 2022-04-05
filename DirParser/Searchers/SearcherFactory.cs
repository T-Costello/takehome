using DirParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirParser.Searchers
{
	public static class SearcherFactory
	{
		public static ISearcher GetSearcher(string searchType)
		{
			//add searchers to list.
			List<ISearcher> searchers = new List<ISearcher>() { new NameSearcher(), new SizeSearcher(), new TypeSearcher()};

			var enumSearch = (SearchTypeEnum)Enum.Parse(typeof(SearchTypeEnum), searchType);

			return searchers.Where(a => a.SearchType == enumSearch).SingleOrDefault();
		}
	}
}
