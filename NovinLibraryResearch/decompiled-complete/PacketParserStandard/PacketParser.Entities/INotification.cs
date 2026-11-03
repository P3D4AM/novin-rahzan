using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface INotification : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	short Action { get; set; }

	int? AppId { get; set; }

	DateTime DateM { get; set; }

	string DateSh { get; }

	short DepId { get; set; }

	Guid Id { get; set; }

	string Message { get; set; }

	string Param1 { get; set; }

	string Param2 { get; set; }

	string Title { get; set; }

	short? UserId { get; set; }
}
