using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirParser.Models
{
	public class DirectoryIndex
	{
		public string FileName { get; set; }
		public string FilePath { get; set; }
		public string Type { get; set; }
		public long Size { get; set; }

		public override string ToString()
		{
			return $"Name: {FileName}\t Path: {FilePath}\tType: {Type}\t Size: {Size}";
		}
	}
}
