using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.IO;

namespace Lab9.Models
{
	public class Node
	{
		private bool isHashed;
		public ObservableCollection<Node>? FilesAndFolders { get; set; }
		public string NodeName { get; }
		public string FullPath { get; }

		public Node(string _FullPath, bool isDisk)
		{
			FilesAndFolders = new ObservableCollection<Node>();
			FullPath = _FullPath;
			if (!isDisk)
				NodeName = Path.GetFileName(_FullPath);
			else
				NodeName = "Диск " + _FullPath;
			isHashed = false;
		}

		public void GetFilesAndFolders()
		{
			if (!isHashed)
			{
				try
				{
					IEnumerable<string> subdirs = Directory.EnumerateDirectories(FullPath, "*", SearchOption.TopDirectoryOnly);
					foreach (string dir in subdirs)
					{
						Node thisnode = new Node(dir, false);
						FilesAndFolders.Add(thisnode);
					}

					string[] allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
					IEnumerable<string> files = Directory.EnumerateFiles(FullPath)
						.Where(file => allowedExtensions.Any(file.EndsWith))
						.ToList();

					foreach (string file in files)
						FilesAndFolders.Add(new Node(file, false));

					isHashed = true;
				}
				catch { }
			}
		}
	}
}