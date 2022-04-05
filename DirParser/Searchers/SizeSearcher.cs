using DirParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirParser.Searchers
{
	public class SizeSearcher : ISearcher
	{
		public SearchTypeEnum SearchType
		{
			get
			{
				return SearchTypeEnum.Size;
			}
		}

		public List<DirectoryIndex> DoSearch(string term, List<DirectoryIndex> index)
		{
			var operand = term[0]; //only implementing greater than/less than
			var value = term.Substring(1); //normally we should doing validation against what was entered.
			long actualValue = long.Parse(value.ToString());
			var result = new List<DirectoryIndex>();
			if (operand == '>')
			{
				result = index.Where(a => a.Size >= actualValue).AsParallel().ToList();
			}
			else if (operand == '<')
			{
				result = index.Where(a => a.Size <= actualValue).AsParallel().ToList();
			}

			return result;
		}
	}
}
