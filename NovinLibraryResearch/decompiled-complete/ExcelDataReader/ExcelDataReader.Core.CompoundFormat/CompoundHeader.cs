using System;
using System.Collections.Generic;

namespace ExcelDataReader.Core.CompoundFormat;

internal class CompoundHeader
{
	public ulong Signature { get; set; }

	public bool IsSignatureValid => Signature == 16220472316735377360uL;

	public Guid ClassId { get; set; }

	public ushort Version { get; set; }

	public ushort DllVersion { get; set; }

	public ushort ByteOrder { get; set; }

	public int SectorSizeInPot { get; set; }

	public int SectorSize => 1 << SectorSizeInPot;

	public int MiniSectorSizeInPot { get; set; }

	public int MiniSectorSize => 1 << MiniSectorSizeInPot;

	public int DirectorySectorCount { get; set; }

	public int FatSectorCount { get; set; }

	public uint RootDirectoryEntryStart { get; set; }

	public uint TransactionSignature { get; set; }

	public uint MiniStreamCutoff { get; set; }

	public uint MiniFatFirstSector { get; set; }

	public int MiniFatSectorCount { get; set; }

	public uint DifFirstSector { get; set; }

	public int DifSectorCount { get; set; }

	public List<uint> First109DifSectorChain { get; set; }
}
