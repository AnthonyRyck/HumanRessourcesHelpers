using HR_Helpers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_Helpers.ViewModels
{
	public interface IGestionLog
	{

		public List<FileLog> FilesLog { get; set; }

		public string TextLogSelected { get; set; }

		public string[] AllTextLogSelected { get; set; }



		public void SelectFile(string pathFile);
	}
}
