using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace Server.Core;

public interface IDalPaymentGateway
{
	Task<WebPaymentGateway> GetAsync(Guid guid, CancellationToken token);

	Task<WebPaymentGateway> GetAsync(string wordpress_paymentMethodName, CancellationToken token);

	Task<List<WebPaymentGateway>> GetAllAsync(bool status, CancellationToken token);

	Task<ReturnedSaveFuncInfo> SaveAsync(WebPaymentGateway item, CancellationToken token);
}
