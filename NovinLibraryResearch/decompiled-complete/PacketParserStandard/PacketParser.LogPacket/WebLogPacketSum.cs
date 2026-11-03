using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.Entities;

namespace PacketParser.LogPacket;

public class WebLogPacketSum
{
	public int DepId { get; set; }

	public int HashId { get; set; }

	public DateTime StartDate { get; set; }

	public string StartDateString => StartDate.MiladiToCalture();

	public string StartTimeString => StartDate.Hour + ":" + StartDate.Minute + ":" + StartDate.Second;

	public string ConnectionDurationTimeString { get; set; }

	public string LenDateString
	{
		get
		{
			try
			{
				TimeSpan timeSpan = EndDate.Subtract(StartDate);
				if (timeSpan > new TimeSpan(0, 0, 1))
				{
					return default(DateTime).Add(timeSpan).ToLongTimeString();
				}
				return "00:00:00";
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "LenDateString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\LogPacket\\WebLogPacketSum.cs", 33);
				return "";
			}
		}
	}

	public DateTime EndDate { get; set; }

	public string EndDateString => EndDate.MiladiToCalture() + "  " + EndDate.Second + ":" + EndDate.Minute + ":" + EndDate.Hour;

	public long SumSentClientPacketLengh { get; set; }

	public string SumSentClientPacketLenghString => $"{SumSentClientPacketLengh / 1024 / 1024:n2}" + "  مگابایت  ";

	public int ClientSentPacketCount { get; set; }

	public long SumServerReceivedPacketLengh { get; set; }

	public string SumServerReceivedPacketLenghString => $"{SumServerReceivedPacketLengh / 1024 / 1024:n2}" + "  مگابایت  ";

	public int ServerReceivedPacketCount { get; set; }

	public string ConnectionUniqueId { get; set; }

	public PacketType PacketType { get; set; }

	public PacketAction PacketActgion { get; set; }

	public string PacketTypeString => PacketType.GetDisplay();

	public short PacketAction { get; set; }

	public string PacketActionString => GetStringPacketAction();

	[JsonIgnore]
	[XmlIgnore]
	public string HexColor { get; set; }

	private string GetStringPacketAction()
	{
		return Packet.GetPacketActionName(PacketType, PacketAction);
	}

	public static List<string> HexColorList()
	{
		return new List<string> { "#008000", "#00FF00", "#00BFFF", "#FF0000", "#DC143C", "#DB7093", "#D2691E", "#FF7F50", "#DAA520" };
	}
}
