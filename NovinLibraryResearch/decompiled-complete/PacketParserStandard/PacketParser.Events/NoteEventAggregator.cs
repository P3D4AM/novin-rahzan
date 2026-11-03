using System;

namespace PacketParser.Events;

public class NoteEventAggregator
{
	public event EventHandler NoteSaved;

	public void RaiseNoteSaved(object sender, System.EventArgs e)
	{
		this.NoteSaved?.Invoke(sender, e);
	}
}
