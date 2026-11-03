using System;
using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models;

public class AreaViewModel
{
	public Guid Guid { get; set; }

	public string Name { get; set; }

	public Guid MasterGuid { get; set; }

	public EnArea Type { get; set; }

	public bool IsChecked { get; set; }
}
