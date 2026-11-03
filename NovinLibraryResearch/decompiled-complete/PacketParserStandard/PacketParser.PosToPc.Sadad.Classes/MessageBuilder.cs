using System;
using System.Collections.Generic;

namespace PacketParser.PosToPc.Sadad.Classes;

internal abstract class MessageBuilder : IComparer<MessageBuilder>, IEquatable<MessageBuilder>
{
	public abstract int Length { get; }

	public int MaxLength { get; set; }

	public string Name { get; set; }

	public int Order { get; set; }

	public int Compare(MessageBuilder x, MessageBuilder y)
	{
		return x.Order.CompareTo(y.Order);
	}

	public bool Equals(MessageBuilder other)
	{
		return Order.Equals(other.Order);
	}
}
