using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface IFileInfo : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string FileName { get; set; }

	FileManagement_Parts Part { get; set; }

	string SubDirectory { get; set; }

	byte[] FileContent { get; set; }

	ENCompressedMode CompressMode { get; set; }

	Guid EntityGuid { get; set; }
}
