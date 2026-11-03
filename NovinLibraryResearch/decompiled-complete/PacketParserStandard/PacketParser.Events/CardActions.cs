using System;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace PacketParser.Events;

public class CardActions
{
	public EnCardType Type { get; private set; }

	public Func<Guid, Task<ReturnedSaveFuncInfo>> Action { get; private set; }

	public CardActions(EnCardType type, Func<Guid, Task<ReturnedSaveFuncInfo>> action)
	{
		Type = type;
		Action = action;
	}
}
