using DirParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirParser.Searchers
{
	public class TypeSearcher : ISearcher
	{
		public SearchTypeEnum SearchType
		{
			get
			{
				return SearchTypeEnum.Type;
			}
		}

		public List<DirectoryIndex> DoSearch(string term, List<DirectoryIndex> index)
		{
			term = term.Replace(".", ""); //don't care if they type .pdf or pdf upper or lower
			return index.Where(a => a.Type.ToLower().Equals(term.ToLower())).ToList();
		}
	}
}
