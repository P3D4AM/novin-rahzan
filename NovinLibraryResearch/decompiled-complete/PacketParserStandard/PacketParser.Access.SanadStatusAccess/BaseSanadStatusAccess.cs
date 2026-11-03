using System.ComponentModel;
using Newtonsoft.Json;

namespace PacketParser.Access.SanadStatusAccess;

public class BaseSanadStatusAccess
{
	public int StatusId { get; set; }

	[JsonIgnore]
	public string StatusName { get; set; }

	[DefaultValue(true)]
	public bool Add { get; set; } = true;


	[DefaultValue(true)]
	public bool Edit { get; set; } = true;


	[DefaultValue(true)]
	public bool Show { get; set; } = true;


	[DefaultValue(true)]
	public bool Delete { get; set; } = true;


	[DefaultValue(true)]
	public bool ChangeStatus { get; set; } = true;

}
