using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface IPrdGroup : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	long Code { get; set; }

	string Description { get; set; }

	long FullCode { get; set; }

	string HtmlFull { get; set; }

	string HtmlQuick { get; set; }

	string Name { get; set; }

	string Pic { get; set; }

	bool Special { get; set; }

	string TechInfoData { get; set; }

	Guid? ParentGuid { get; set; }

	ServerStatusForClient ServerStatus { get; set; }

	int Level { get; set; }

	string Slug { get; set; }

	EnProductType Type { get; set; }

	string ShortName { get; set; }
}
