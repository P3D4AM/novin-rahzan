using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace Server.Core;

public interface IDalUser
{
	Task<ReturnedSaveFuncInfo> SaveAsync(short DepId, WebUser user, string callerInfo);

	Task<WebUser> LoadDataAsync(string Username, string Password);

	Task<IEnumerable<WebUser>> GetAllAsync(ENSource source);

	Task<ReturnedSaveFuncInfo> SetAndroidImeAsync(int Id, long AndroidIme);

	Task<ReturnedSaveFuncInfo> SetDBGuidAsync(int Id, Guid guid);

	Task<WebUser> LoadDataAsync(int userId);

	Task<ReturnedSaveFuncInfo> SetPcSerialAsync(short id, string cpuSerial);

	Task<ReturnedSaveFuncInfo> SetLastLoginAsync(int id, DateTime now);

	Task<WebUser> LoadDataByTaxClientIdAsync(short depid, string taxClientId);

	Task<WebUser> GetByUserOfDepartmentAsync(int depId, ENSource source);
}
