using System;
using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models;

public class SaveTempViewModel
{
	public Guid Guid { get; set; }

	public Guid AsGuid { get; set; }

	public Document AsType { get; set; }

	public string AsTypeStr => AsType.GetDisplay().GetTranslate();

	public DateTime DateM { get; set; }

	public string DateSh => DateM.MiladiToCalture();

	public string Time => $"{DateM.Hour}:{DateM.Minute}";

	public int Id2 { get; set; }

	public string Description { get; set; }

	public bool Status { get; set; }
}
