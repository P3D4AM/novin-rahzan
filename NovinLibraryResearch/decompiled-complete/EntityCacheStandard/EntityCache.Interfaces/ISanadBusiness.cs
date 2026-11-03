using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;

namespace EntityCache.Interfaces;

public interface ISanadBusiness
{
	decimal Debit { get; }

	decimal Credit { get; }

	string Description { get; set; }

	int SanadNo { get; set; }

	IReadOnlyList<Sanad_DetBusiness> ListSanad_Det { get; }

	event Func<object, EventArgs, Task> OnSumChanged_static;

	Task<Sanad_DetBusiness> AddToListSanadAsync(Guid asGuid, Document asType, EnSystem system, CancellationToken token);

	Task<Sanad_DetBusiness> AddToListSanadAsync(Sanad_DetBusiness item, EnSystem system, CancellationToken token);

	Task AddRangeToListSanadAsync(List<Sanad_DetBusiness> Sanaddet, EnSystem system, bool appendCurrentItems = false, CancellationToken toke = default(CancellationToken));

	Task RemoveFromListSanad(Guid Guid, EnSystem system, CancellationToken token);
}
