using System;

namespace PacketParser.Entities.Department;

public class Version
{
	public string Code { get; set; } = "";


	public DateTime PublishDateM { get; set; } = DateTime.Now;


	public string PublishDateSH => PublishDateM.MiladiToShamsi();

	public string StrVersion { get; set; } = "";


	public string FilePath { get; set; } = "";


	public string Description { get; set; } = "";


	public string DescriptionURL { get; set; } = "";


	public Guid Guid { get; set; }
}
