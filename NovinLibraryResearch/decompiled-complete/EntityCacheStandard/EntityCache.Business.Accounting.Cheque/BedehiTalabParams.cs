using System;
using PacketParser.Entities;

namespace EntityCache.Business.Accounting.Cheque;

public class BedehiTalabParams
{
	public bool IsTasfsilGuidRequered { get; private set; }

	public HesabType? TafsilType { get; private set; }

	public BedehiTalabParams(bool tafsilReq = false, HesabType? hType = null)
	{
		try
		{
			IsTasfsilGuidRequered = tafsilReq;
			TafsilType = hType;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabParams.cs", 18);
		}
	}
}
