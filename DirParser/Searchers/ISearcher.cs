using DirParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirParser
{
	public interface ISearcher
	{
		SearchTypeEnum SearchType{ get; }
		List<DirectoryIndex> DoSearch(string term, List<DirectoryIndex> index);
	}
}
