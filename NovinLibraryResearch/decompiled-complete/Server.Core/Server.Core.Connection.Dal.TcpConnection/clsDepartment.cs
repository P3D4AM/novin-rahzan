using System;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace Server.Core.Connection.Dal.TcpConnection;

public class clsDepartment : IDalDepartment
{
	private class NestedDepartment
	{
		internal static readonly clsDepartment Instance;

		static NestedDepartment()
		{
			Instance = new clsDepartment();
		}
	}

	public static clsDepartment Instance => NestedDepartment.Instance;

	public Task<WebDepartment> GetAsync(string userName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfoWithValue<short>> GetDepartmentIdAsync(string userName)
	{
		throw new NotImplementedException();
	}

	public Task<WebDepartment> LoadDataAsync(short depId)
	{
		return Task.FromResult(GetDefaultData());
	}

	public Task<WebDepartment> LoadDataAsync(string userName, string password)
	{
		throw new NotImplementedException();
	}

	public Task<WebDepartment> LoadDataAsync(Guid tafsilGuid)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SaveAsync(WebDepartment item)
	{
		throw new NotImplementedException();
	}

	private WebDepartment GetDefaultData()
	{
		return new WebDepartment
		{
			Status = true,
			DateExpire = DateTime.Now.AddYears(10)
		};
	}
}
