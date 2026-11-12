using System;
using System.Collections.Generic;
using System.IO;
using ExcelDataReader.Exceptions;

namespace ExcelDataReader.Core.CompoundFormat;

internal class CompoundStream : Stream
{
	public List<uint> SectorChain { get; }

	public List<uint> RootSectorChain { get; }

	public override bool CanRead => true;

	public override bool CanSeek => false;

	public override bool CanWrite => false;

	public override long Length { get; }

	public override long Position
	{
		get
		{
			return Offset - SectorBytes.Length + SectorOffset;
		}
		set
		{
			Seek(value, SeekOrigin.Begin);
		}
	}

	private Stream BaseStream { get; set; }

	private CompoundDocument Document { get; }

	private bool IsMini { get; }

	private bool LeaveOpen { get; }

	private int SectorChainOffset { get; set; }

	private int Offset { get; set; }

	private int SectorOffset { get; set; }

	private byte[] SectorBytes { get; set; }

	public CompoundStream(CompoundDocument document, Stream baseStream, List<uint> sectorChain, int length, bool leaveOpen)
	{
		Document = document;
		BaseStream = baseStream;
		IsMini = false;
		LeaveOpen = leaveOpen;
		Length = length;
		SectorChain = sectorChain;
		ReadSector();
	}

	public CompoundStream(CompoundDocument document, Stream baseStream, uint baseSector, int length, bool isMini, bool leaveOpen)
	{
		Document = document;
		BaseStream = baseStream;
		IsMini = isMini;
		Length = length;
		LeaveOpen = leaveOpen;
		if (IsMini)
		{
			SectorChain = Document.GetSectorChain(baseSector, Document.MiniSectorTable);
			RootSectorChain = Document.GetSectorChain(Document.RootEntry.StreamFirstSector, Document.SectorTable);
		}
		else
		{
			SectorChain = Document.GetSectorChain(baseSector, Document.SectorTable);
		}
		ReadSector();
	}

	public override void Flush()
	{
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		int num = 0;
		while (num < count && Position < Length)
		{
			if (SectorOffset == SectorBytes.Length)
			{
				ReadSector();
				SectorOffset = 0;
			}
			int num2 = Math.Min(count - num, SectorBytes.Length - SectorOffset);
			Array.Copy(SectorBytes, SectorOffset, buffer, offset + num, num2);
			num += num2;
			SectorOffset += num2;
		}
		return num;
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		int num = (IsMini ? Document.Header.MiniSectorSize : Document.Header.SectorSize);
		switch (origin)
		{
		case SeekOrigin.Begin:
			SectorChainOffset = (int)(offset / num);
			Offset = SectorChainOffset * num;
			SectorOffset = (int)(offset % num);
			if (Offset < Length)
			{
				ReadSector();
			}
			return Position;
		case SeekOrigin.Current:
			return Seek(Position + offset, SeekOrigin.Begin);
		case SeekOrigin.End:
			return Seek(Length + offset, SeekOrigin.Begin);
		default:
			return Offset;
		}
	}

	public override void SetLength(long value)
	{
		throw new NotImplementedException();
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		throw new NotImplementedException();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && !LeaveOpen)
		{
			BaseStream?.Dispose();
			BaseStream = null;
		}
		base.Dispose(disposing);
	}

	private void ReadSector()
	{
		if (IsMini)
		{
			ReadMiniSector();
		}
		else
		{
			ReadRegularSector();
		}
	}

	private void ReadMiniSector()
	{
		uint sector = SectorChain[SectorChainOffset];
		int num = (int)Document.GetMiniSectorOffset(sector);
		int num2 = num / Document.Header.SectorSize;
		if (num2 >= RootSectorChain.Count)
		{
			throw new CompoundDocumentException("The excel file may be corrupt or truncated. We've read past the end of the file.");
		}
		uint sector2 = RootSectorChain[num2];
		int num3 = num % Document.Header.SectorSize;
		BaseStream.Seek(Document.GetSectorOffset(sector2) + num3, SeekOrigin.Begin);
		int num4 = (int)Math.Min(Length - Offset, Document.Header.MiniSectorSize);
		SectorBytes = new byte[num4];
		if (BaseStream.Read(SectorBytes, 0, num4) < num4)
		{
			throw new CompoundDocumentException("The excel file may be corrupt or truncated. We've read past the end of the file.");
		}
		Offset += num4;
		SectorChainOffset++;
	}

	private void ReadRegularSector()
	{
		uint sector = SectorChain[SectorChainOffset];
		BaseStream.Seek(Document.GetSectorOffset(sector), SeekOrigin.Begin);
		int num = (int)Math.Min(Length - Offset, Document.Header.SectorSize);
		SectorBytes = new byte[num];
		if (BaseStream.Read(SectorBytes, 0, num) < num)
		{
			throw new CompoundDocumentException("The excel file may be corrupt or truncated. We've read past the end of the file.");
		}
		Offset += num;
		SectorChainOffset++;
	}
}
