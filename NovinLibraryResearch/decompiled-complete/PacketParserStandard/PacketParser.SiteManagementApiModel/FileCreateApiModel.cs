using System;
using PacketParser.Entities;

namespace PacketParser.SiteManagementApiModel;

public class FileCreateApiModel
{
	public FileManagement_Parts Part { get; set; }

	public string FileName { get; set; }

	public byte[] FileBytes { get; set; }

	public string SubDirectory { get; set; }

	public Guid FileGuid { get; set; }
}
