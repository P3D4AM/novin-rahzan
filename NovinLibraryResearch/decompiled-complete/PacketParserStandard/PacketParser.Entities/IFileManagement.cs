using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface IFileManagement : IHasIntId
{
	long CRC { get; set; }

	DateTime DateM { get; set; }

	string DateSh { get; }

	string Description { get; set; }

	string FileName { get; set; }

	string FullURL { get; set; }

	string PageURL { get; set; }

	int PartID { get; set; }

	string Title { get; set; }

	int Version { get; set; }

	string VersionTXT { get; set; }
}
