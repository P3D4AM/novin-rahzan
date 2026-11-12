using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;
using PacketParser.TaxService;
using Server.Core.Assistance;
using Server.Core.Connection;

namespace SharedServer.Core.Connection.Dal;

public class clsTaxService
{
	public ITaxConnectionManager TaxConnectionManager { get; private set; }

	public clsTaxService(ITaxConnectionManager taxManager)
	{
		TaxConnectionManager = taxManager;
	}

	public async Task HandleTaxClientConnectionAsync()
	{
		while (true)
		{
			try
			{
				_ = 2;
				try
				{
					IEnumerable<WebUser> enumerable = await Initializer.Dal.User.GetAllAsync(ENSource.TaxService);
					if (enumerable == null || !enumerable.Any())
					{
						continue;
					}
					foreach (WebUser item in enumerable.OrderBy((WebUser p) => Guid.NewGuid()).ToList())
					{
						await TaxConnectionManager.GetOrHandleConnectionAsync(item, "", handleLoginResponse: true);
						await Task.Delay(new Random().Next(60000));
					}
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "HandleTaxClientConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxService.cs", 32);
				}
			}
			finally
			{
				await Task.Delay(60000);
			}
		}
	}

	internal async Task TaxInstance_OnLoginRequestAsync(object sender, ConnectionEventArgs<WebTaxLogin> config)
	{
		using (new JaegerMonitor("TaxInstance_OnLoginRequestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxService.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				string clientId = config.ConnectionData.Single().ClientId;
				WebUser webUser = await Initializer.Dal.User.LoadDataByTaxClientIdAsync(config.Connection.DepId, config.ConnectionData.Single().ClientId);
				if (webUser != null)
				{
					if (webUser.DepId != config.Connection.DepId)
					{
						throw new InvalidOperationException($"tax clientId({clientId}) is not assinged to this department({config.Connection.DepId}) and assigned to another department({webUser.DepId})!");
					}
					await TaxConnectionManager.GetOrHandleConnectionAsync(config, handleLoginResponse: true);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "TaxInstance_OnLoginRequestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxService.cs", 52);
				ret.AddReturnedValue(ex, "", "TaxInstance_OnLoginRequestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxService.cs", 53);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> SendNotDeliveredAsync(ClsConnectionData connection)
	{
		if (connection == null)
		{
			throw new NullReferenceException("connection");
		}
		using (new JaegerMonitor("SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxService.cs"))
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
			int num = 0;
			short depId = connection.DepId;
			try
			{
				if (DateTime.Now.Month <= 4)
				{
					return returnedSaveFuncInfo;
				}
			}
			catch (OperationCanceledException exception)
			{
				returnedSaveFuncInfo.AddReturnedValue(exception, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxService.cs", 98);
			}
			catch (Exception ex)
			{
				returnedSaveFuncInfo.AddReturnedValue(ex, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxService.cs", 101);
				WebErrorLog.Instance.StartLog(ex, $"#Line_{num}\r\n" + $"#depId:{depId}", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxService.cs", 102);
			}
			return returnedSaveFuncInfo;
		}
	}
}
