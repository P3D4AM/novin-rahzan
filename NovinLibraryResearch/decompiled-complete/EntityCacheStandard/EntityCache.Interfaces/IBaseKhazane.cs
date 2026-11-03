using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Business.Accounting;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Interfaces;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs.Sanad;

namespace EntityCache.Interfaces;

public interface IBaseKhazane : IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext, IHasSanadStatusReport, IHasTafsilGuid
{
	EnKhazaneMaster Type { get; set; }

	bool IsTemp { get; set; }

	decimal Sum { get; }

	string Description { get; set; }

	Guid MoeinGuid { get; set; }

	int NaghdCount { get; }

	decimal SumNaghd { get; }

	int BankCount { get; }

	int PossCount { get; }

	decimal SumBankHavale { get; }

	decimal SumPoss { get; }

	int CheckShCount { get; }

	decimal SumCheckSh { get; }

	int CheckMCount { get; }

	decimal SumCheckM { get; }

	int SanadNo { get; set; }

	DateTime DateM { get; set; }

	string Time { get; }

	ServerStatusForClient ServerStatus { get; set; }

	IReadOnlyCollection<BaseKhazaneDetail> DetailList { get; }

	event Func<object, EventArgs, Task> OnSumChanged;

	event Func<object, OrdinalChangedEventArgs, Task> OnOrdinalChanged;

	Task<IBaseKhazane> MapAsync(string contextGuid, IBaseKhazane source, IBaseKhazane destination, CancellationToken token);

	Task SetTafsilGuidAsync(Guid tafsilGuid, EnSystem system, CancellationToken token, string tranName = "");

	Task<int> GetId2Async(CancellationToken token, string tranName = "");

	void AddToDetList(BaseKhazaneDetail item, EnSystem system, CancellationToken token = default(CancellationToken));

	Task<decimal> GetBeforeAccountAsync(EnSystem system, CancellationToken token, string tranName = "");

	Task<decimal> GetAfterAccountAsync(EnSystem system, CancellationToken token, string tranName = "");

	Task<TafsilBusiness> GetTafsilAsync(EnSystem system, CancellationToken token, string tranName = "", bool useCache = true);

	new void SetId2(int value, CancellationToken token);

	Task<bool> CanUpdateAsync(EnSystem system, CancellationToken token, string tranName = "");

	Task<bool> CanDeleteAsync(CancellationToken token, string tranName = "");

	Task<bool> CanPrintAsync(CancellationToken token, string tranName = "");

	Task<string> GetUserNameAsync(CancellationToken token, string tranName = "");

	int NextOrdinal(CancellationToken token);

	Task<ReturnedSaveFuncInfo> CheckValidationAsync(EnSystem system, bool isDeleteAction, string tranName = "", Tuple<Document, Guid> relatedOrder = null, CancellationToken token = default(CancellationToken));

	Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, bool sendToServer, Priority priority = Priority.Low, bool awaitOnSemaphore = true, string CustomerUserName = "", string CustomerPassword = "", int timeout = 60000, bool await4send = false, short tryCount = 10, string tranName = "", CancellationToken token = default(CancellationToken), SanadBusiness orderSanad = null, IMessageSender messageSender = null);
}
