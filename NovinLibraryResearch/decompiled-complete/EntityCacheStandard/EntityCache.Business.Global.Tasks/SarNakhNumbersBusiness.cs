using System;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Global.Tasks;

namespace EntityCache.Business.Global.Tasks;

public class SarNakhNumbersBusiness : Serializable<SarNakhNumbersBusiness>, IHasContext, ISarNakhNumbers
{
	private bool _isDisposed;

	public string ContextGuid { get; set; }

	public Guid SarNakhGuid { get; set; }

	public string Number { get; set; }

	public string Title { get; set; }

	public event Func<object, EventArgs, Task> OnDisposed;

	private SarNakhNumbersBusiness()
	{
	}

	public static Task<SarNakhNumbersBusiness> GetFactoryAsync(string contextGuid, SarNakhBusiness sarNakh, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("GetFactoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\SarNakh\\SarNakhNumbersBusiness.cs"))
		{
			SarNakhNumbersBusiness sarNakhNumbersBusiness = new SarNakhNumbersBusiness();
			if (sarNakh == null)
			{
				throw new NullReferenceException("sarNakh");
			}
			try
			{
				sarNakhNumbersBusiness.ContextGuid = contextGuid;
				sarNakhNumbersBusiness.SarNakhGuid = sarNakh.Guid;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetFactoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\SarNakh\\SarNakhNumbersBusiness.cs", 34);
			}
			return Task.FromResult(sarNakhNumbersBusiness);
		}
	}

	public override void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				if (this.OnDisposed != null)
				{
					PacketParser.Utilities.RaiseEvent(this.OnDisposed, this, new EventArgs(), default(CancellationToken), 60, "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\SarNakh\\SarNakhNumbersBusiness.cs");
				}
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\SarNakh\\SarNakhNumbersBusiness.cs", 49);
		}
	}
}
