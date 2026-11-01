using System;
using System.Collections.Generic;
using System.Linq;
using EntityCache.Business;
using PacketParser;
using PacketParser.Entities;
using SmartAssembly.Attributes;

namespace TimeAttendanceDAL.Dal.FLRSanad;

[Serializable]
[DoNotPruneType]
public class SanadDet
{
	public string Description { get; set; } = "";


	public long CodeMoein { get; set; }

	public long CodeTafsil { get; set; }

	public decimal Credit { get; set; }

	public decimal Debit { get; set; }

	public static SanadDet FromString(string val)
	{
		try
		{
			List<string> list = val.Split(new char[1] { '\t' }).ToList();
			if (list.Count != 5)
			{
				return null;
			}
			return new SanadDet
			{
				CodeMoein = long.Parse(list[0]),
				CodeTafsil = long.Parse(list[1]),
				Debit = Utilities.ParseToDecimal(list[2], 0m, 0),
				Credit = Utilities.ParseToDecimal(list[3], 0m, 0),
				Description = list[4]
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FromString", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\FLRSanad\\SanadDet.cs", 38);
			return null;
		}
	}

	public override string ToString()
	{
		try
		{
			return CodeMoein + "\t" + CodeTafsil + "\t" + Debit + "\t" + Credit + "\t" + Description;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ToString", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\FLRSanad\\SanadDet.cs", 51);
			return "";
		}
	}

	public Sanad_DetBusiness ToBusiness(string contextGuid, SanadBusiness parent)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Expected O, but got Unknown
		return new Sanad_DetBusiness(contextGuid, (EnSystem)2, parent.Guid, (Document)50, 1)
		{
			RowDesc = Description,
			Guid = Guid.NewGuid(),
			CodeMoeinForUi = CodeMoein.ToString(),
			CodeTafsilForUi = CodeTafsil.ToString(),
			ContextGuid = contextGuid,
			Credit = Credit,
			Debit = Debit,
			Modified = DateTime.Now,
			Status = true,
			DecimalGuid = null,
			MasterGuid = parent.Guid
		};
	}
}
