using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace Serilog.Sinks.File;

internal class PathRoller
{
	private const string PeriodMatchGroup = "period";

	private const string SequenceNumberMatchGroup = "sequence";

	private readonly string _directory;

	private readonly string _filenamePrefix;

	private readonly string _filenameSuffix;

	private readonly Regex _filenameMatcher;

	private readonly RollingInterval _interval;

	private readonly string _periodFormat;

	public string LogFileDirectory => _directory;

	public string DirectorySearchPattern { get; }

	public PathRoller(string path, RollingInterval interval)
	{
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		_interval = interval;
		_periodFormat = interval.GetFormat();
		string text = Path.GetDirectoryName(path);
		if (string.IsNullOrEmpty(text))
		{
			text = Directory.GetCurrentDirectory();
		}
		_directory = Path.GetFullPath(text);
		_filenamePrefix = Path.GetFileNameWithoutExtension(path);
		_filenameSuffix = Path.GetExtension(path);
		_filenameMatcher = new Regex("^" + Regex.Escape(_filenamePrefix) + "(?<period>\\d{" + _periodFormat.Length + "})(?<sequence>_[0-9]{3,}){0,1}" + Regex.Escape(_filenameSuffix) + "$", RegexOptions.Compiled);
		DirectorySearchPattern = _filenamePrefix + "*" + _filenameSuffix;
	}

	public void GetLogFilePath(DateTime date, int? sequenceNumber, out string path)
	{
		string text = GetCurrentCheckpoint(date)?.ToString(_periodFormat, CultureInfo.InvariantCulture) ?? "";
		if (sequenceNumber.HasValue)
		{
			text = text + "_" + sequenceNumber.Value.ToString("000", CultureInfo.InvariantCulture);
		}
		path = Path.Combine(_directory, _filenamePrefix + text + _filenameSuffix);
	}

	public IEnumerable<RollingLogFile> SelectMatches(IEnumerable<string> filenames)
	{
		foreach (string filename in filenames)
		{
			Match match = _filenameMatcher.Match(filename);
			if (match.Success)
			{
				int? sequenceNumber = null;
				Group group = match.Groups["sequence"];
				if (group.Captures.Count != 0)
				{
					string s = group.Captures[0].Value.Substring(1);
					sequenceNumber = int.Parse(s, CultureInfo.InvariantCulture);
				}
				DateTime? dateTime = null;
				Group group2 = match.Groups["period"];
				if (group2.Captures.Count != 0 && DateTime.TryParseExact(group2.Captures[0].Value, _periodFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
				{
					dateTime = result;
				}
				yield return new RollingLogFile(filename, dateTime, sequenceNumber);
			}
		}
	}

	public DateTime? GetCurrentCheckpoint(DateTime instant)
	{
		return _interval.GetCurrentCheckpoint(instant);
	}

	public DateTime? GetNextCheckpoint(DateTime instant)
	{
		return _interval.GetNextCheckpoint(instant);
	}
}
