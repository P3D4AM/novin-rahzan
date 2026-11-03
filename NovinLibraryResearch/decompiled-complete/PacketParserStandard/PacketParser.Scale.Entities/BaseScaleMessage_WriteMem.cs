using System;
using PacketParser.Entities;
using PacketParser.Scale.Entities.Interfaces;
using Serilog;

namespace PacketParser.Scale.Entities;

public class BaseScaleMessage_WriteMem : IScaleMessage_WriteMem
{
	private string barcode;

	public string Barcode
	{
		get
		{
			return barcode;
		}
		set
		{
			try
			{
				string text = value;
				value = value.RemoveNoNumbers();
				if (value.Length < 12)
				{
					barcode = "0000000000000";
					return;
				}
				string text2 = value.Substring(0, 12);
				string text3 = text2.Checksum_EAN13Barcode().ToString();
				barcode = text2 + text3;
				if (text.Length == 13 && text != barcode)
				{
					Log.Warning("Original(" + text + ") calculated barcode(" + barcode + ")");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Barcode", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Scale\\Entities\\BaseScaleMessage_WriteMem.cs", 32);
			}
		}
	}

	public string Code { get; set; }

	public DateTime DateM { get; set; }

	public string DateSh => DateM.MiladiToCalture();

	public DateTime ExpireDate { get; set; }

	public int Id { get; set; }

	public short MaxTryCount { get; set; }

	public int MemNo { get; set; }

	public string Name { get; set; }

	public DateTime NextTry { get; set; }

	public string Parameter1 { get; set; }

	public string Parameter2 { get; set; }

	public string Parameter3 { get; set; }

	public string Parameter4 { get; set; }

	public double Parsang { get; set; }

	public int ScaleId { get; set; }

	public short St { get; set; }

	public short TryCount { get; set; }

	public decimal UnitPrice { get; set; }

	public bool Vazni { get; set; }
}
