using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Serilog.Sinks.Http.Private.IO;

namespace Serilog.Sinks.Http.Private.Durable;

public class FileSizeRolledBufferFiles : IBufferFiles
{
	private readonly DirectoryService directoryService;

	private readonly string logFolder;

	private readonly string candidateSearchPath;

	private readonly Regex fileNameMatcher;

	public string BookmarkFileName { get; }

	public FileSizeRolledBufferFiles(DirectoryService directoryService, string bufferBaseFilePath)
	{
		if (bufferBaseFilePath == null)
		{
			throw new ArgumentNullException("bufferBaseFilePath");
		}
		this.directoryService = directoryService ?? throw new ArgumentNullException("directoryService");
		string fullPath = Path.GetFullPath(bufferBaseFilePath);
		string text = Path.GetFileName(bufferBaseFilePath) ?? throw new Exception("Cannot get file name from buffer base file path");
		logFolder = Path.GetDirectoryName(fullPath) ?? throw new Exception("Cannot get directory of buffer base file path");
		candidateSearchPath = text + "-*.*";
		fileNameMatcher = new Regex("^" + Regex.Escape(text) + "-(?<date>\\d{8})(?<sequence>_[0-9]{3,}){0,1}\\.(?<extension>json|txt)$");
		BookmarkFileName = fullPath + ".bookmark";
	}

	public string[] Get()
	{
		return (from pair in (from filePath in directoryService.GetFiles(logFolder, candidateSearchPath)
				select new KeyValuePair<string, Match>(filePath, fileNameMatcher.Match(Path.GetFileName(filePath))) into pair
				where pair.Value.Success
				orderby (pair.Value.Groups["extension"].Value == "txt") ? 1 : 0
				select pair).ThenBy<KeyValuePair<string, Match>, string>((KeyValuePair<string, Match> pair) => pair.Value.Groups["date"].Value, StringComparer.OrdinalIgnoreCase).ThenBy((KeyValuePair<string, Match> pair) => int.Parse("0" + pair.Value.Groups["sequence"].Value.Replace("_", string.Empty)))
			select pair.Key).ToArray();
	}
}
