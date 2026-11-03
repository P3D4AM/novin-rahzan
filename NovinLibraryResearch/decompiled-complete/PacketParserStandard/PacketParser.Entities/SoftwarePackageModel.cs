using System;
using System.Collections.Generic;

namespace PacketParser.Entities;

public class SoftwarePackageModel
{
	public string Version { get; set; }

	public DateTime DateM { get; set; }

	public string DateSh => DateM.MiladiToCalture();

	public List<string> Options { get; set; }
}
