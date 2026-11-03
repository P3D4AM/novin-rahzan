using System.ComponentModel.DataAnnotations;
using PacketParser.Entities;

namespace PacketParser.DepartmentApiModel.ReportGenerator;

public class ReportGeneratorFilter
{
	[Required]
	public ENSource Source;

	[Required]
	public string Version = "1.00";

	public bool LoadDetail { get; set; } = true;


	public bool Status { get; set; } = true;


	public string ApplicationVersion { get; set; } = "";

}
