using System;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Business.OrderDepot;

public class AbadDetBusiness : IAbadDet, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid, IHasContext
{
	public Guid Guid { get; set; } = Guid.NewGuid();


	public DateTime Modified { get; set; } = DateTime.Now;


	public Guid MasterGuid { get; set; }

	public decimal X { get; set; }

	public string XForDisplay => ((decimal)(double)Math.Round(X, DigitForRound)).ToString().Replace(".", "/");

	public decimal Y { get; set; }

	public string YForDisplay => ((decimal)(double)Math.Round(Y, DigitForRound)).ToString().Replace(".", "/");

	public decimal Z { get; set; }

	public string ZForDisplay => ((decimal)(double)Math.Round(Z, DigitForRound)).ToString().Replace(".", "/");

	public decimal Count { get; set; }

	public string CountForDisplay => ((decimal)(double)Math.Round(Count, DigitForRound)).ToString().Replace(".", "/");

	public Shape ShapeType { get; set; }

	public ShapeInfo ShapeInformation { get; set; }

	[Obsolete]
	public int DetailID { get; set; }

	public bool Status { get; set; } = true;


	public string ContextGuid { get; set; }

	public decimal Mohit
	{
		get
		{
			using (new JaegerMonitor("Mohit", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\AbadDetBusiness.cs"))
			{
				try
				{
					switch (ShapeType)
					{
					case Shape.Mostatil:
						return (X + Y) * 2m;
					case Shape.Dayere:
						return 2m * (3.14159265358979m * X);
					case Shape.MokabMostatil:
						return X * Y * 2m + X * Z * 2m + Y * Z * 2m;
					}
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "Mohit", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\AbadDetBusiness.cs", 43);
				}
				return 0m;
			}
		}
	}

	public decimal Masahat
	{
		get
		{
			using (new JaegerMonitor("Masahat", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\AbadDetBusiness.cs"))
			{
				try
				{
					return ShapeType switch
					{
						Shape.Mostatil => X * Y, 
						Shape.Dayere => 3.14159265358979m * X * X, 
						Shape.MokabMostatil => X * Y * Z, 
						_ => 0m, 
					};
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "Masahat", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\AbadDetBusiness.cs", 62);
				}
				return 0m;
			}
		}
	}

	public decimal TotalMasahat => Masahat * Count;

	public decimal TotalMohit => Mohit * Count;

	[JsonIgnore]
	public short DigitForRound { get; }

	public AbadDetBusiness(string contextGuid)
	{
		try
		{
			ContextGuid = contextGuid;
			DigitForRound = (ClsCache.GetCache(ContextGuid)?.MultiLanguageConfig?.DigitForRound).GetValueOrDefault(3);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\AbadDetBusiness.cs", 77);
		}
	}

	public string GetDescription()
	{
		using (new JaegerMonitor("GetDescription", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\AbadDetBusiness.cs"))
		{
			try
			{
				string text = "";
				switch (ShapeType)
				{
				case Shape.Mostatil:
					text = "ابعاد : ".GetTranslate() + Y.ToString().Replace(".", "/") + "×" + X.ToString().Replace(".", "/");
					break;
				case Shape.Dayere:
					text = "شعاع : ".GetTranslate() + X.ToString().Replace(".", "/");
					break;
				case Shape.MokabMostatil:
					text = "ابعاد : ".GetTranslate() + Z.ToString().Replace(".", "/") + "×" + Y.ToString().Replace(".", "/") + "×" + X.ToString().Replace(".", "/");
					break;
				}
				if (ShapeInformation == ShapeInfo.Mohit)
				{
					text = text + ((text.Trim() != "") ? " - " : "").ToString() + "محیط ".GetTranslate() + ((decimal)(double)Math.Round(Mohit, DigitForRound)).ToString().Replace(".", "/").Trim();
				}
				else if (ShapeInformation == ShapeInfo.Masahat)
				{
					text = text + ((text.Trim() != "") ? " - " : "").ToString() + "مساحت ".GetTranslate() + ((decimal)(double)Math.Round(Masahat, DigitForRound)).ToString().Replace(".", "/").Trim();
				}
				else if (ShapeInformation == ShapeInfo.Mohit_Masahat)
				{
					text = text + ((text.Trim() != "") ? " - " : "").ToString() + "محیط ".GetTranslate() + ((decimal)(double)Math.Round(Mohit, DigitForRound)).ToString().Replace(".", "/").Trim() + " مساحت " + ((decimal)(double)Math.Round(Masahat, 3)).ToString().Replace(".", "/").Trim();
				}
				if (ShapeType == Shape.MokabMostatil)
				{
					if (ShapeInformation == ShapeInfo.Mohit || ShapeInformation == ShapeInfo.Mohit_Masahat)
					{
						text = text + ((text.Trim() != "") ? " - " : "").ToString() + "سطح ".GetTranslate() + Masahat.ToString().Replace(".", "/").Trim();
					}
					else if (ShapeInformation == ShapeInfo.Masahat || ShapeInformation == ShapeInfo.Mohit_Masahat)
					{
						text = text + ((text.Trim() != "") ? " - " : "").ToString() + "مساحت ".GetTranslate() + Masahat.ToString().Replace(".", "/").Trim();
					}
				}
				return text + ((text.Trim() != "") ? " - " : "").ToString() + "تعداد ".GetTranslate() + Count.ToString().Replace(".", "/").Trim();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetDescription", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\AbadDetBusiness.cs", 114);
				throw;
			}
		}
	}
}
