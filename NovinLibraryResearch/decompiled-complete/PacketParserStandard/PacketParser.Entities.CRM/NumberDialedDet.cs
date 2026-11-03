using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PacketParser.Entities.CRM;

public class NumberDialedDet : Serializable<NumberDialedDet>
{
	private CancellationTokenSource _token = new CancellationTokenSource();

	public string serialNumber { get; set; }

	public string ConnectionUniqueId { get; set; }

	public Guid Guid { get; set; }

	public ConnectionData connection { get; set; }

	public bool isTcp { get; set; }

	public short LineNumber { get; set; }

	public short DeviceNumber { get; set; }

	public string FullDialedNumber => string.Join("", DialedNumber?.OrderBy((Tuple<short, DateTime> x) => x.Item2)?.Select((Tuple<short, DateTime> x) => x.Item1));

	public List<Tuple<short, DateTime>> DialedNumber { get; set; }

	public DateTime DateM { get; set; } = DateTime.Now;


	public bool swIsActive { get; set; }

	public event Func<object, NumberDialedDet, Task> OnNumber;

	public void ResetToken()
	{
		_token.Cancel();
		_token = new CancellationTokenSource();
		CancellationToken tkn = _token.Token;
		Task.Run(() => WaitForNumberTimeOut(10000, tkn));
	}

	private async Task WaitForNumberTimeOut(int delay, CancellationToken token)
	{
		try
		{
			await Task.Delay(delay);
			if (!token.IsCancellationRequested)
			{
				this.OnNumber?.Invoke(this, this);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "WaitForNumberTimeOut", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\NumberDialedDet.cs", 40);
		}
	}
}
