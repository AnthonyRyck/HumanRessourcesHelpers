using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_Helpers.Models
{
	public class FileLog
	{
		public string NameFile { get; set; }

		public string PathFile { get; set; }

		public FileLog(string nameFile, string pathFile)
		{
			NameFile = nameFile;
			PathFile = pathFile;
		}
	}
}
