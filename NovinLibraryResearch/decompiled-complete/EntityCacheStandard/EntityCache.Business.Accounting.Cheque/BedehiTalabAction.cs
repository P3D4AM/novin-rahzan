using System;
using System.Threading.Tasks;
using EntityCache.EventArguments;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace EntityCache.Business.Accounting.Cheque;

public class BedehiTalabAction
{
	public EnChequeOperation Operation { get; private set; }

	public BedehiTalabParams Params { get; private set; }

	public Func<BedehiTalabEventArgs, Task<ReturnedSaveFuncInfo>> Action { get; private set; }

	public BedehiTalabAction(EnChequeOperation operation, BedehiTalabParams param, Func<BedehiTalabEventArgs, Task<ReturnedSaveFuncInfo>> action)
	{
		Operation = operation;
		Action = action;
		Params = param;
	}
}
