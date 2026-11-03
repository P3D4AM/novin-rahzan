using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Payamak;
using PacketParser.Payamak.Entities;

namespace PayamakUi.Classes.Entities;

public class SMSNet_Account : BaseSmsNetAccount, INotifyPropertyChanged
{
	private string _domain = "";

	private string _senderNumber = "";

	private decimal? _account;

	private CancellationTokenSource _accountTokenSource;

	public override decimal? Account
	{
		get
		{
			if (!_account.HasValue)
			{
				_accountTokenSource?.Cancel();
				_accountTokenSource = new CancellationTokenSource(60000);
				CancellationToken token = _accountTokenSource.Token;
				Task.Run(() => UpdateAccountAsync(token));
			}
			return _account;
		}
	}

	public string AccountForDisplay
	{
		get
		{
			if (Account.HasValue)
			{
				return Account.ToString();
			}
			return "---";
		}
	}

	public override string Domain
	{
		get
		{
			return _domain.ToLower().Replace(".com", "").Replace(".ir", "");
		}
		set
		{
			_domain = value ?? "";
		}
	}

	public override string SenderNumber
	{
		get
		{
			return _senderNumber?.Trim();
		}
		set
		{
			if (value != _senderNumber)
			{
				if (value != "0")
				{
					_senderNumber = value;
				}
				else
				{
					base.GSMSendedSmsCount = 0;
				}
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public async Task UpdateAccountAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			await Task.Delay(500);
			if (token.IsCancellationRequested || base.Type != 0)
			{
				return;
			}
			ReturnedSaveFuncInfoWithValue<decimal> returnedSaveFuncInfoWithValue = await WebSmsSender.GetCreditAsync(base.Username, base.Password, FullDomain, token);
			if (!token.IsCancellationRequested)
			{
				_account = returnedSaveFuncInfoWithValue.value;
				decimal? account = _account;
				decimal num = -26;
				if ((account.GetValueOrDefault() == num) & account.HasValue)
				{
					base.Title += " (خطای احراز هویت)";
				}
				if (!token.IsCancellationRequested)
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AccountForDisplay"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Title"));
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\Entities\\SMSNet_Account.cs", 63);
		}
	}
}
