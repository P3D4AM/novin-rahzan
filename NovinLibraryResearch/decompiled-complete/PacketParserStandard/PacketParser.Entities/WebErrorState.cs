using System;
using System.Collections.Generic;
using System.Linq;

namespace PacketParser.Entities;

[Serializable]
public class WebErrorState
{
	private List<short> listNextStates = new List<short>();

	public short Id { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public bool Status { get; set; }

	public short? IconIndex { get; set; }

	public List<short> NextStates
	{
		get
		{
			return listNextStates?.ToList();
		}
		set
		{
			listNextStates = value;
		}
	}
}
