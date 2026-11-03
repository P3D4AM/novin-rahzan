using System;
using PacketParser.Entities;
using PacketParser.Scale.Entities.Interfaces;

namespace PacketParser.Scale.Entities;

public class BaseScaleMessage_ReadSale : IScaleMessage_ReadSale
{
	public short Count { get; set; }

	public DateTime DateM { get; set; }

	public int Id { get; set; }

	public int MemId { get; set; }

	public short Priority { get; set; }

	public int ScaleId { get; set; }

	public BaseScaleMessage_ReadSale()
	{
	}

	public BaseScaleMessage_ReadSale(int memId, DateTime dateM, short count, string scaleNo, short priority, int x)
	{
		try
		{
			MemId = memId;
			DateM = dateM;
			Priority = priority;
			Count = count;
			ScaleId = short.Parse(scaleNo);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Scale\\Entities\\BaseScaleMessage_ReadSale.cs", 28);
		}
	}
}
