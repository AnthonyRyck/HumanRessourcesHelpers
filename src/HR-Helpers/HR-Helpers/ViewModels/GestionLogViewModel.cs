using HR_Helpers.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HR_Helpers.ViewModels
{
	public class GestionLogViewModel : IGestionLog
	{
		#region Properties

		/// <summary>
		/// Liste des fichiers de log
		/// </summary>
		public List<FileLog> FilesLog { get; set; }

		/// <summary>
		/// Texte du fichier de log sélectionné.
		/// </summary>
		public string TextLogSelected { get; set; }

		public string[] AllTextLogSelected { get; set; }

		/// <summary>
		/// Chemin de base des fichiers de log
		/// </summary>
		private string _pathBaseLog;

		#endregion

		#region Constructeur

		public GestionLogViewModel()
		{
			FilesLog = new List<FileLog>();
			_pathBaseLog = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");

			LoadFiles();
		}

		#endregion

		#region Implement IGestionLog

		public void SelectFile(string pathFile)
		{
			try
			{
				AllTextLogSelected = File.ReadAllLines(pathFile);
			}
			catch (Exception ex)
			{
				AllTextLogSelected = new string[] { ex.Message };
			}
		}

		#endregion

		#region Private methods

		private void LoadFiles()
		{
			// Récupération des fichiers dans le répertoire
			var allFiles = Directory.GetFiles(_pathBaseLog);

			foreach (var filePath in allFiles)
			{
				string fileName = Path.GetFileNameWithoutExtension(filePath);

				FileLog fileLog = new FileLog(fileName, filePath);
				FilesLog.Add(fileLog);
			}
		}

		#endregion
	}
}
