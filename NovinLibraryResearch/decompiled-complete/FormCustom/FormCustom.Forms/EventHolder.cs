using System;

namespace FormCustom.Forms;

internal class EventHolder
{
	public EventHandler c_ControlChanged { get; set; }

	public EventHandler c_OnLeave { get; set; }

	public EventHandler c_OnEnter { get; set; }
}
