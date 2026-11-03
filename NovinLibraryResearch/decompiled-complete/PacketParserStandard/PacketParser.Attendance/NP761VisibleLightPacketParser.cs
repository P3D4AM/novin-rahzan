using System;
using System.Threading.Tasks;
using PacketParser.Attendance.InterFaces;
using PacketParser.Attendance.SDK.LAN_VisibleLight;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using Serilog;

namespace PacketParser.Attendance;

public class NP761VisibleLightPacketParser
{
	private IDalSaveAttLogOnPerson _dal;

	public NP761VisibleLightPacketParser(IDalSaveAttLogOnPerson dal)
	{
		_dal = dal;
	}

	public async Task<ReturnedSaveFuncInfo> ParseAsync(string contextGuid, string jsonMessageTaradodDevice)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			VisibleLightLanSDK.Logdata logdata = VisibleLightLanSDK.ParseLogDataSingle(jsonMessageTaradodDevice);
			if (logdata != null)
			{
				WebAttLogOnPerson logonPersonel = new WebAttLogOnPerson
				{
					DeviceSerial_ = logdata.deviceKey,
					TrafficId = long.Parse(logdata.reqSeqNo),
					TypeTaradod = TypeTaradodEnumType.Vorod,
					PersonNo = logdata.accNo,
					DateTimeLogM = DateTime.Parse(logdata.passTime),
					IsVirtual = SavedTaradodEnumType.RealTaradod,
					TypeTaradodVacationOrMission = TypeTaradodEnumType.Vorod,
					DeviceId_ = 0
				};
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await _dal.SaveAsync(contextGuid, logonPersonel), ReturnedState.None, "ParseAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Attendance\\NP761VisibleLightPacketParser.cs", 39);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			res.AddError(ex.Message, "", "ParseAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Attendance\\NP761VisibleLightPacketParser.cs", 46);
		}
		return res;
	}
}
