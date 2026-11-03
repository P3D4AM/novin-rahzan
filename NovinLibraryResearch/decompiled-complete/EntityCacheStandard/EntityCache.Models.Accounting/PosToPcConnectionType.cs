using System;
using EntityCache.Utilities;
using PacketParser;
using PacketParser.Entities;

namespace EntityCache.Models.Accounting;

public class PosToPcConnectionType
{
	public EnumConnectionType Type { get; private set; }

	public string TypeForDisplay => Type.GetDisplay().GetTranslate();

	public int Port => Type switch
	{
		EnumConnectionType.None => 0, 
		EnumConnectionType.Parsian => 1362, 
		EnumConnectionType.FanapTCP => 7000, 
		EnumConnectionType.PardakhtNovin => 1362, 
		EnumConnectionType.SamanKishTCP => 1197, 
		EnumConnectionType.MabnaCart => 1, 
		EnumConnectionType.AsanPardakht => 17000, 
		EnumConnectionType.BehPardakhtTCPV3 => 1024, 
		EnumConnectionType.Sadad_BlueBird => 8888, 
		_ => throw new NotImplementedException(), 
	};

	public string IpTitle
	{
		get
		{
			switch (Type)
			{
			case EnumConnectionType.None:
			case EnumConnectionType.Parsian:
			case EnumConnectionType.FanapTCP:
			case EnumConnectionType.PardakhtNovin:
			case EnumConnectionType.MabnaCart:
			case EnumConnectionType.AsanPardakht:
			case EnumConnectionType.BehPardakhtTCPV3:
			case EnumConnectionType.Sadad_BlueBird:
				return "آدرس شبکه".GetTranslate() + ":";
			case EnumConnectionType.SamanKishTCP:
				return "آدرس کامپیوتر سرور:";
			default:
				throw new NotImplementedException();
			}
		}
	}

	public string PortForDisplay => "درگاه پیش فرض".GetTranslate() + ":" + $" {Port}";

	public byte[] Image => Type switch
	{
		EnumConnectionType.None => ImageResourceManager.PosToPcNone, 
		EnumConnectionType.Parsian => ImageResourceManager.PosToPcParsian, 
		EnumConnectionType.FanapTCP => ImageResourceManager.PosToPcFanap, 
		EnumConnectionType.PardakhtNovin => ImageResourceManager.PosToPcPardakhtNovin, 
		EnumConnectionType.SamanKishTCP => ImageResourceManager.PosToPcSamaKishTcp, 
		EnumConnectionType.MabnaCart => ImageResourceManager.PosToPcMabnaCard, 
		EnumConnectionType.AsanPardakht => ImageResourceManager.PosToPcAsanPardakht, 
		EnumConnectionType.BehPardakhtTCPV3 => ImageResourceManager.PosToPcBehPardakhtTcpv3, 
		EnumConnectionType.Sadad_BlueBird => ImageResourceManager.PosToPcSadad_BlueBird, 
		_ => throw new NotImplementedException(), 
	};

	public PosToPcConnectionType(EnumConnectionType type)
	{
		Type = type;
	}
}
