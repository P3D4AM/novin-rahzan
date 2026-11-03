using System;

namespace PacketParser.DepartmentApiModel;

public class Imagewoo
{
	public string Src { get; set; }

	public long id { get; set; }

	public DateTime date_created { get; set; }

	public DateTime date_created_gmt { get; set; }

	public DateTime date_modified { get; set; }

	public DateTime date_modified_gmt { get; set; }

	public string name { get; set; }

	public string alt { get; set; }
}
