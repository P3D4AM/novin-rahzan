using System;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Interfaces;
using PacketParser.Entities;

namespace FormCustom;

public class OrderGridActionAggregator : IOrderGridActionAggregator
{
	private Action _addAction;

	private Action _updateAction;

	public OrderGridActionAggregator(Action addAction, Action updateAction)
	{
		_addAction = addAction;
		_updateAction = updateAction;
	}

	public Task<ReturnedSaveFuncInfo> AddSync()
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			_addAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddSync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\OrderGridActionAggregator.cs", 30);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "AddSync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\OrderGridActionAggregator.cs", 31);
		}
		return Task.FromResult(returnedSaveFuncInfo);
	}

	public Task<ReturnedSaveFuncInfo> UpdateSync()
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			_updateAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateSync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\OrderGridActionAggregator.cs", 45);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "UpdateSync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\OrderGridActionAggregator.cs", 46);
		}
		return Task.FromResult(returnedSaveFuncInfo);
	}
}
