using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface INews : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Category { get; set; }

	DateTime DateM { get; set; }

	string FullContent { get; set; }

	string KeyWords { get; set; }

	string MetaDescription { get; set; }

	string PicUrl { get; set; }

	string QuickText { get; set; }

	string Title { get; set; }

	NewsType Type { get; set; }
}
