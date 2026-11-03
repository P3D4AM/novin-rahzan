using System;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters.Configs;

namespace EntityCache.Business;

public class IndexEntityBusiness : IIndexEntity, IHasContext
{
	public string ContextGuid { get; set; }

	public int WordId { get; set; }

	public Guid EntityGuid { get; set; }

	public IndexEntityBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task IndexProductsAsync(string ContextGuid, object sender, Func<object, UpdateUiParams, Task> UiUpdateAction, CancellationToken token = default(CancellationToken))
	{
	}
}
