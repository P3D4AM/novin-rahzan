using System;

namespace PacketParser.Entities;

public class WebServiceRequest<T> : Serializable<T>
{
	public Guid Token { get; set; }

	public T ConnectionData { get; set; }
}
