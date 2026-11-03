using System;
using PacketParser.Scale.Entities.Interfaces;

namespace PacketParser.Scale.Entities;

public class BaseScaleMemory : IScaleMemory
{
	public string ContextGuid { get; set; }

	public string CurrentName { get; set; }

	public decimal CurrentPrice { get; set; }

	public bool CurrentVazni { get; set; }

	public DateTime DateM { get; set; }

	public bool IsSaved { get; set; }

	public int MemoryNumber { get; set; }

	public Guid PrdGuid { get; set; }

	public int ScaleId { get; set; }

	public int RowVersion { get; set; }

	public bool Exists
	{
		get
		{
			if (ScaleId > 0 && PrdGuid != Guid.Empty)
			{
				return true;
			}
			return false;
		}
	}
}
