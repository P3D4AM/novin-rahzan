using System;
using PacketParser.Entities;

namespace PacketParser.Events;

public class SanadEventAggregator
{
	public bool HasEventHandler => this.SanadChanged != null;

	public event Action<Guid, Document, ENAction> SanadChanged;

	public void RaiseSanadChanged(Guid asGuid, Document asType, ENAction action)
	{
		this.SanadChanged?.Invoke(asGuid, asType, action);
	}
}
