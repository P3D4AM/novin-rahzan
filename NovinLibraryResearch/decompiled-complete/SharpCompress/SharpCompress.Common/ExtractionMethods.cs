using System;
using System.IO;

namespace SharpCompress.Common;

internal static class ExtractionMethods
{
	public static void WriteEntryToDirectory(IEntry entry, string destinationDirectory, ExtractionOptions? options, Action<string, ExtractionOptions?> write)
	{
		string text = Path.GetFullPath(destinationDirectory);
		if (text[text.Length - 1] != Path.DirectorySeparatorChar)
		{
			string text2 = text;
			char directorySeparatorChar = Path.DirectorySeparatorChar;
			text = text2 + directorySeparatorChar;
		}
		if (!Directory.Exists(text))
		{
			throw new ExtractionException("Directory does not exist to extract to: " + text);
		}
		if (options == null)
		{
			options = new ExtractionOptions
			{
				Overwrite = true
			};
		}
		string fileName = Path.GetFileName(entry.Key);
		string path;
		if (options.ExtractFullPath)
		{
			string directoryName = Path.GetDirectoryName(entry.Key);
			string fullPath = Path.GetFullPath(Path.Combine(text, directoryName));
			if (!Directory.Exists(fullPath))
			{
				if (!fullPath.StartsWith(text, StringComparison.Ordinal))
				{
					throw new ExtractionException("Entry is trying to create a directory outside of the destination directory.");
				}
				Directory.CreateDirectory(fullPath);
			}
			path = Path.Combine(fullPath, fileName);
		}
		else
		{
			path = Path.Combine(text, fileName);
		}
		if (!entry.IsDirectory)
		{
			path = Path.GetFullPath(path);
			if (!path.StartsWith(text, StringComparison.Ordinal))
			{
				throw new ExtractionException("Entry is trying to write a file outside of the destination directory.");
			}
			write(path, options);
		}
		else if (options.ExtractFullPath && !Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}
	}

	public static void WriteEntryToFile(IEntry entry, string destinationFileName, ExtractionOptions? options, Action<string, FileMode> openAndWrite)
	{
		if (entry.LinkTarget != null)
		{
			if (options?.WriteSymbolicLink == null)
			{
				throw new ExtractionException("Entry is a symbolic link but ExtractionOptions.WriteSymbolicLink delegate is null");
			}
			options.WriteSymbolicLink(destinationFileName, entry.LinkTarget);
			return;
		}
		FileMode arg = FileMode.Create;
		if (options == null)
		{
			options = new ExtractionOptions
			{
				Overwrite = true
			};
		}
		if (!options.Overwrite)
		{
			arg = FileMode.CreateNew;
		}
		openAndWrite(destinationFileName, arg);
		entry.PreserveExtractionOptions(destinationFileName, options);
	}
}
