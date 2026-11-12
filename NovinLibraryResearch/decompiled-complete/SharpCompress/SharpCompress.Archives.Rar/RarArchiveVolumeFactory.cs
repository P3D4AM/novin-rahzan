using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SharpCompress.Common.Rar;
using SharpCompress.Common.Rar.Headers;
using SharpCompress.Readers;

namespace SharpCompress.Archives.Rar;

internal static class RarArchiveVolumeFactory
{
	internal static IEnumerable<RarVolume> GetParts(IEnumerable<Stream> streams, ReaderOptions options)
	{
		foreach (Stream stream in streams)
		{
			if (!stream.CanRead || !stream.CanSeek)
			{
				throw new ArgumentException("Stream is not readable and seekable");
			}
			yield return new StreamRarArchiveVolume(stream, options);
		}
	}

	internal static IEnumerable<RarVolume> GetParts(FileInfo fileInfo, ReaderOptions options)
	{
		FileInfoRarArchiveVolume part2 = new FileInfoRarArchiveVolume(fileInfo, options);
		yield return part2;
		ArchiveHeader ah = part2.ArchiveHeader;
		if (ah.IsVolume)
		{
			fileInfo = GetNextFileInfo(ah, part2.FileParts.FirstOrDefault() as FileInfoRarFilePart);
			while (fileInfo != null && fileInfo.Exists)
			{
				part2 = new FileInfoRarArchiveVolume(fileInfo, options);
				fileInfo = GetNextFileInfo(ah, part2.FileParts.FirstOrDefault() as FileInfoRarFilePart);
				yield return part2;
			}
		}
	}

	private static FileInfo? GetNextFileInfo(ArchiveHeader ah, FileInfoRarFilePart? currentFilePart)
	{
		if (currentFilePart == null)
		{
			return null;
		}
		if (ah.OldNumberingFormat || currentFilePart.MarkHeader.OldNumberingFormat)
		{
			return FindNextFileWithOldNumbering(currentFilePart.FileInfo);
		}
		return FindNextFileWithNewNumbering(currentFilePart.FileInfo);
	}

	private static FileInfo FindNextFileWithOldNumbering(FileInfo currentFileInfo)
	{
		string extension = currentFileInfo.Extension;
		StringBuilder stringBuilder = new StringBuilder(currentFileInfo.FullName.Length);
		stringBuilder.Append(currentFileInfo.FullName.Substring(0, currentFileInfo.FullName.Length - extension.Length));
		int result;
		if (string.Compare(extension, ".rar", StringComparison.OrdinalIgnoreCase) == 0)
		{
			stringBuilder.Append(".r00");
		}
		else if (int.TryParse(extension.Substring(2, 2), out result))
		{
			result++;
			stringBuilder.Append(".r");
			if (result < 10)
			{
				stringBuilder.Append('0');
			}
			stringBuilder.Append(result);
		}
		else
		{
			ThrowInvalidFileName(currentFileInfo);
		}
		return new FileInfo(stringBuilder.ToString());
	}

	private static FileInfo FindNextFileWithNewNumbering(FileInfo currentFileInfo)
	{
		if (string.Compare(currentFileInfo.Extension, ".rar", StringComparison.OrdinalIgnoreCase) != 0)
		{
			throw new ArgumentException("Invalid extension, expected 'rar': " + currentFileInfo.FullName);
		}
		int num = currentFileInfo.FullName.LastIndexOf(".part");
		if (num < 0)
		{
			ThrowInvalidFileName(currentFileInfo);
		}
		StringBuilder stringBuilder = new StringBuilder(currentFileInfo.FullName.Length);
		stringBuilder.Append(currentFileInfo.FullName, 0, num);
		string text = currentFileInfo.FullName.Substring(num + 5, currentFileInfo.FullName.IndexOf('.', num + 5) - num - 5);
		stringBuilder.Append(".part");
		if (int.TryParse(text, out var result))
		{
			result++;
			for (int i = 0; i < text.Length - result.ToString().Length; i++)
			{
				stringBuilder.Append('0');
			}
			stringBuilder.Append(result);
		}
		else
		{
			ThrowInvalidFileName(currentFileInfo);
		}
		stringBuilder.Append(".rar");
		return new FileInfo(stringBuilder.ToString());
	}

	private static void ThrowInvalidFileName(FileInfo fileInfo)
	{
		throw new ArgumentException("Filename invalid or next archive could not be found:" + fileInfo.FullName);
	}
}
