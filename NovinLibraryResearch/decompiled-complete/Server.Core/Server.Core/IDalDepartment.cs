using System;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace Server.Core;

public interface IDalDepartment
{
	Task<WebDepartment> LoadDataAsync(short DepId);

	Task<WebDepartment> LoadDataAsync(Guid tafsilGuid);

	Task<WebDepartment> LoadDataAsync(string userName, string password);

	Task<WebDepartment> GetAsync(string userName);

	Task<ReturnedSaveFuncInfoWithValue<short>> GetDepartmentIdAsync(string userName);

	Task<ReturnedSaveFuncInfo> SaveAsync(WebDepartment item);
}
