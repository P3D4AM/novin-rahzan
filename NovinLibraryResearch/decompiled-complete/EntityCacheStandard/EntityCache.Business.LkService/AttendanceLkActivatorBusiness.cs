using System.Collections.Generic;
using EntityCache.Business.LkService.Interfaces;
using PacketParser.Entities;
using PacketParser.LkService;

namespace EntityCache.Business.LkService;

public class AttendanceLkActivatorBusiness : LkActivatorBaseBusiness
{
	private static Dictionary<string, AttendancePackageClass> _dic = new Dictionary<string, AttendancePackageClass>();

	private static bool _flag = false;

	public AttendanceLkActivatorBusiness(string contextGuid, ENSource source, ILkClientManager instance)
		: base(contextGuid, source, instance)
	{
	}
}
