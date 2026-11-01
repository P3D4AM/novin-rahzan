using System;
using System.Collections.Generic;
using PacketParser.Entities;

namespace TimeAttendanceDevices;

public class EnrollData
{
	public int PersonId { get; set; }

	public int EnrollNo { get; set; }

	public short DeviceId { get; set; }

	public short DeviceNo { get; set; }

	public List<EnrollDataDet> FingerDatas { get; set; } = new List<EnrollDataDet>();


	public static List<EnrollData> ConvertEnrollDatas(List<EnrollDataDet> pEnrollDataDets)
	{
		List<EnrollData> list = new List<EnrollData>();
		EnrollData enrollData = new EnrollData();
		try
		{
			foreach (EnrollDataDet pEnrollDataDet in pEnrollDataDets)
			{
				if (enrollData.DeviceNo != pEnrollDataDet.DeviceNo || enrollData.EnrollNo != pEnrollDataDet.EnrollNo)
				{
					if (enrollData.EnrollNo != 0)
					{
						enrollData = new EnrollData();
					}
					enrollData.DeviceNo = pEnrollDataDet.DeviceNo;
					enrollData.EnrollNo = pEnrollDataDet.EnrollNo;
					list.Add(enrollData);
				}
				enrollData.FingerDatas.Add(pEnrollDataDet);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ConvertEnrollDatas", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\EnrollData.cs", 35);
		}
		return list;
	}

	public static List<EnrollDataDet> ConvertEnrollDataToDets(List<EnrollData> pEnrollDatas)
	{
		List<EnrollDataDet> list = new List<EnrollDataDet>();
		try
		{
			foreach (EnrollData pEnrollData in pEnrollDatas)
			{
				foreach (EnrollDataDet fingerData in pEnrollData.FingerDatas)
				{
					list.Add(fingerData);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ConvertEnrollDataToDets", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\EnrollData.cs", 54);
		}
		return list;
	}
}
