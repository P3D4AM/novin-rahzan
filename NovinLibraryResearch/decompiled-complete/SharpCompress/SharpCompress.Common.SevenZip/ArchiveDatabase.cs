using System;
using System.Collections.Generic;
using System.IO;
using SharpCompress.Compressors.LZMA;
using SharpCompress.Compressors.LZMA.Utilites;

namespace SharpCompress.Common.SevenZip;

internal class ArchiveDatabase
{
	internal byte _majorVersion;

	internal byte _minorVersion;

	internal long _startPositionAfterHeader;

	internal long _dataStartPosition;

	internal List<long> _packSizes = new List<long>();

	internal List<uint?> _packCrCs = new List<uint?>();

	internal List<CFolder> _folders = new List<CFolder>();

	internal List<int> _numUnpackStreamsVector;

	internal List<CFileItem> _files = new List<CFileItem>();

	internal List<long> _packStreamStartPositions = new List<long>();

	internal List<int> _folderStartFileIndex = new List<int>();

	internal List<int> _fileIndexToFolderIndexMap = new List<int>();

	internal IPasswordProvider PasswordProvider { get; }

	public ArchiveDatabase(IPasswordProvider passwordProvider)
	{
		PasswordProvider = passwordProvider;
	}

	internal void Clear()
	{
		_packSizes.Clear();
		_packCrCs.Clear();
		_folders.Clear();
		_numUnpackStreamsVector = null;
		_files.Clear();
		_packStreamStartPositions.Clear();
		_folderStartFileIndex.Clear();
		_fileIndexToFolderIndexMap.Clear();
	}

	internal bool IsEmpty()
	{
		if (_packSizes.Count == 0 && _packCrCs.Count == 0 && _folders.Count == 0 && _numUnpackStreamsVector.Count == 0)
		{
			return _files.Count == 0;
		}
		return false;
	}

	private void FillStartPos()
	{
		_packStreamStartPositions.Clear();
		long num = 0L;
		for (int i = 0; i < _packSizes.Count; i++)
		{
			_packStreamStartPositions.Add(num);
			num += _packSizes[i];
		}
	}

	private void FillFolderStartFileIndex()
	{
		_folderStartFileIndex.Clear();
		_fileIndexToFolderIndexMap.Clear();
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < _files.Count; i++)
		{
			bool flag = !_files[i].HasStream;
			if (flag && num2 == 0)
			{
				_fileIndexToFolderIndexMap.Add(-1);
				continue;
			}
			if (num2 == 0)
			{
				while (true)
				{
					if (num >= _folders.Count)
					{
						throw new InvalidOperationException();
					}
					_folderStartFileIndex.Add(i);
					if (_numUnpackStreamsVector[num] != 0)
					{
						break;
					}
					num++;
				}
			}
			_fileIndexToFolderIndexMap.Add(num);
			if (!flag)
			{
				num2++;
				if (num2 >= _numUnpackStreamsVector[num])
				{
					num++;
					num2 = 0;
				}
			}
		}
	}

	public void Fill()
	{
		FillStartPos();
		FillFolderStartFileIndex();
	}

	internal long GetFolderStreamPos(CFolder folder, int indexInFolder)
	{
		int index = folder._firstPackStreamId + indexInFolder;
		return _dataStartPosition + _packStreamStartPositions[index];
	}

	internal long GetFolderFullPackSize(int folderIndex)
	{
		int firstPackStreamId = _folders[folderIndex]._firstPackStreamId;
		CFolder cFolder = _folders[folderIndex];
		long num = 0L;
		for (int i = 0; i < cFolder._packStreams.Count; i++)
		{
			num += _packSizes[firstPackStreamId + i];
		}
		return num;
	}

	internal Stream GetFolderStream(Stream stream, CFolder folder, IPasswordProvider pw)
	{
		int firstPackStreamId = folder._firstPackStreamId;
		long folderStreamPos = GetFolderStreamPos(folder, 0);
		int count = folder._packStreams.Count;
		long[] array = new long[count];
		for (int i = 0; i < count; i++)
		{
			array[i] = _packSizes[firstPackStreamId + i];
		}
		return DecoderStreamHelper.CreateDecoderStream(stream, folderStreamPos, array, folder, pw);
	}

	private long GetFolderPackStreamSize(int folderIndex, int streamIndex)
	{
		return _packSizes[_folders[folderIndex]._firstPackStreamId + streamIndex];
	}

	private long GetFilePackSize(int fileIndex)
	{
		int num = _fileIndexToFolderIndexMap[fileIndex];
		if (num != -1 && _folderStartFileIndex[num] == fileIndex)
		{
			return GetFolderFullPackSize(num);
		}
		return 0L;
	}
}
