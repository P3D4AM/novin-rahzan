using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace Server.Core.Connection.Dal.TcpConnection;

public class clsUser : IDalUser
{
	private class NestedUser
	{
		internal static readonly clsUser Instance;

		static NestedUser()
		{
			Instance = new clsUser();
		}
	}

	public static clsUser Instance => NestedUser.Instance;

	public Task<IEnumerable<WebUser>> GetAllAsync(ENSource source)
	{
		throw new NotImplementedException();
	}

	public Task<WebUser> GetByUserOfDepartmentAsync(int depId, ENSource source)
	{
		throw new NotImplementedException();
	}

	public Task<WebUser> LoadDataAsync(string username, string password)
	{
		return Task.FromResult(GenerateDefauktUser());
	}

	public Task<WebUser> LoadDataAsync(int userId)
	{
		return Task.FromResult(GenerateDefauktUser());
	}

	public Task<WebUser> LoadDataByTaxClientIdAsync(short depId, string taxClientId)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SaveAsync(short DepId, WebUser user, string callerInfo)
	{
		return Task.FromResult(new ReturnedSaveFuncInfo());
	}

	public Task<ReturnedSaveFuncInfo> SetAndroidImeAsync(int Id, long AndroidIme)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SetDBGuidAsync(int Id, Guid guid)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SetLastLoginAsync(int id, DateTime now)
	{
		return Task.FromResult(new ReturnedSaveFuncInfo());
	}

	public Task<ReturnedSaveFuncInfo> SetPcSerialAsync(short id, string cpuSerial)
	{
		return Task.FromResult(new ReturnedSaveFuncInfo());
	}

	private WebUser GenerateDefauktUser()
	{
		return new WebUser
		{
			DepId = 5000,
			Status = true,
			Id = 5000,
			CashierGuid = Guid.NewGuid()
		};
	}
}
