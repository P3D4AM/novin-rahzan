using System;

namespace PacketParser.Entities;

[Serializable]
public class WebErrorStateChanges
{
	public DateTime DateM { get; set; }

	public int UserId { get; set; }

	public short OldState { get; set; }

	public short NewState { get; set; }

	public string Description { get; set; }

	public int? ReferedUser { get; set; }

	public Priority Priority { get; set; }
}
