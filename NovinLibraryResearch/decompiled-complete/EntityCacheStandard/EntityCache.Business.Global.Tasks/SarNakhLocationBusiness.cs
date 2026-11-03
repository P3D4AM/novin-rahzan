using System;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Global.Tasks;

namespace EntityCache.Business.Global.Tasks;

public class SarNakhLocationBusiness : Serializable<SarNakhLocationBusiness>, IHasContext, ISarNakhLocation
{
	private bool _isDisposed;

	public string ContextGuid { get; set; }

	public Guid SarNakhGuid { get; set; }

	public int Ordinal { get; set; }

	public string State { get; set; }

	public string City { get; set; }

	public string Address { get; set; }

	public string Email { get; set; }

	public string WebSite { get; set; }

	public string FullAddress => State + " - " + City + " - " + Address;

	public event Func<object, EventArgs, Task> OnDisposed;

	private SarNakhLocationBusiness()
	{
	}

	public static Task<SarNakhLocationBusiness> GetFactoryAsync(string contextGuid, SarNakhBusiness master, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("GetFactoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\SarNakh\\SarNakhLocationBusiness.cs"))
		{
			SarNakhLocationBusiness sarNakhLocationBusiness = new SarNakhLocationBusiness();
			if (master == null)
			{
				throw new NullReferenceException("master");
			}
			try
			{
				sarNakhLocationBusiness.ContextGuid = contextGuid;
				sarNakhLocationBusiness.SarNakhGuid = master.Guid;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetFactoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\SarNakh\\SarNakhLocationBusiness.cs", 39);
			}
			return Task.FromResult(sarNakhLocationBusiness);
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
					PacketParser.Utilities.RaiseEvent(this.OnDisposed, this, new EventArgs(), default(CancellationToken), 60, "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\SarNakh\\SarNakhLocationBusiness.cs");
				}
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\SarNakh\\SarNakhLocationBusiness.cs", 55);
		}
	}
}
