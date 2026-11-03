using System;
using EntityCache.Business.OrderDepot;
using PacketParser.Entities;

namespace EntityCache.Ui;

public class AbadDetBusinessForDisplay : AbadDetBusiness
{
	public string Description => GetDescription();

	public new decimal Mohit => base.Mohit;

	public new decimal Masahat => base.Masahat;

	public new decimal TotalMohit => base.TotalMohit;

	public new decimal TotalMasahat => base.TotalMasahat;

	public AbadDetBusinessForDisplay(string contextGuid)
		: base(contextGuid)
	{
		base.ContextGuid = contextGuid;
	}

	public static AbadDetBusinessForDisplay AdpterForDisplay(AbadDetBusiness Source)
	{
		try
		{
			if (Source == null)
			{
				return null;
			}
			AbadDetBusinessForDisplay abadDetBusinessForDisplay = new AbadDetBusinessForDisplay(Source.ContextGuid);
			abadDetBusinessForDisplay.X = Source.X;
			abadDetBusinessForDisplay.Y = Source.Y;
			abadDetBusinessForDisplay.Z = Source.Z;
			abadDetBusinessForDisplay.Count = Source.Count;
			abadDetBusinessForDisplay.DetailID = Source.DetailID;
			abadDetBusinessForDisplay.Guid = Source.Guid;
			abadDetBusinessForDisplay.MasterGuid = Source.MasterGuid;
			abadDetBusinessForDisplay.Modified = Source.Modified;
			abadDetBusinessForDisplay.MasterGuid = Source.MasterGuid;
			abadDetBusinessForDisplay.ShapeInformation = Source.ShapeInformation;
			abadDetBusinessForDisplay.ShapeType = Source.ShapeType;
			abadDetBusinessForDisplay.Status = Source.Status;
			return abadDetBusinessForDisplay;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AdpterForDisplay", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Ui\\AbadDetBusinessForDisplay.cs", 43);
			return null;
		}
	}

	public static AbadDetBusiness AdpterToAbadDet(AbadDetBusinessForDisplay Source)
	{
		try
		{
			if (Source == null)
			{
				return null;
			}
			AbadDetBusinessForDisplay abadDetBusinessForDisplay = new AbadDetBusinessForDisplay(Source.ContextGuid);
			abadDetBusinessForDisplay.X = Source.X;
			abadDetBusinessForDisplay.Y = Source.Y;
			abadDetBusinessForDisplay.Z = Source.Z;
			abadDetBusinessForDisplay.Count = Source.Count;
			abadDetBusinessForDisplay.DetailID = Source.DetailID;
			abadDetBusinessForDisplay.Guid = Source.Guid;
			abadDetBusinessForDisplay.MasterGuid = Source.MasterGuid;
			abadDetBusinessForDisplay.Modified = Source.Modified;
			abadDetBusinessForDisplay.MasterGuid = Source.MasterGuid;
			abadDetBusinessForDisplay.ShapeInformation = Source.ShapeInformation;
			abadDetBusinessForDisplay.ShapeType = Source.ShapeType;
			abadDetBusinessForDisplay.Status = Source.Status;
			return abadDetBusinessForDisplay;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AdpterToAbadDet", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Ui\\AbadDetBusinessForDisplay.cs", 72);
			return null;
		}
	}
}
