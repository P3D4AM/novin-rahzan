using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClientHub.AccountingHubs;
using FormCustom;
using PacketParser.BusinessLayer.Interfaces;
using PacketParser.Entities;
using PacketParser.Payamak.Entities;
using PayamakUi.Classes.Entities;
using PayamakUi.Forms.FRMOprate;

namespace PayamakUi;

public class MessageSenderCacheBaseForm : FormExtention, IMessageSender
{
	public bool InvokeRequiredForSend { get; private set; }

	public MessageSenderCacheBaseForm(string contectGuid, bool isHandleKeyDown = true, bool invokeRequired = true)
		: base(contectGuid, enableStatusBar: false, isHandleKeyDown)
	{
		InvokeRequiredForSend = invokeRequired;
	}

	public async Task SendSmsAsync(PayamakSendStatus sendStatus, int account_Id, string message, string mobiles, CancellationToken token = default(CancellationToken))
	{
		try
		{
			switch (sendStatus)
			{
			case PayamakSendStatus.ImedSend:
			{
				PayamakClientHubInstances instance = PayamakClientHubInstances.GetInstance(base.ContextGuid);
				if (instance != null)
				{
					BaseSmsNetAccount baseSmsNetAccount = (await instance.SmsNetAccount.GetByIdAsync(account_Id))?.value;
					if (baseSmsNetAccount == null)
					{
						throw new NullReferenceException("smsNetAccount");
					}
					SmsOutBox item = new SmsOutBox(message, mobiles, baseSmsNetAccount.SenderNumber, 0, account_Id);
					await instance.SmsOutBox.SaveAsync(item, token);
				}
				break;
			}
			case PayamakSendStatus.ShowSendForm:
				while (!((Control)this).IsHandleCreated)
				{
					await Task.Delay(100);
					if (base.IsDisposed || token.IsCancellationRequested)
					{
						return;
					}
				}
				if (InvokeRequiredForSend)
				{
					((Control)this).BeginInvoke((Delegate)(Action)delegate
					{
						//IL_0035: Unknown result type (might be due to invalid IL or missing references)
						try
						{
							FRMSend obj2 = new FRMSend(base.ContextGuid)
							{
								Message = message,
								Mobiles = mobiles
							};
							((Form)obj2).TopMost = true;
							((Form)obj2).ShowDialog((IWin32Window)(object)this);
						}
						catch (Exception ex2)
						{
							WebErrorLog.Instance.StartLog(ex2, "", "SendSmsAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\MessageSenderBaseForm.cs", 75);
						}
					});
				}
				else
				{
					FRMSend obj = new FRMSend(base.ContextGuid)
					{
						Message = message,
						Mobiles = mobiles
					};
					((Form)obj).TopMost = true;
					((Form)obj).ShowDialog((IWin32Window)(object)this);
				}
				break;
			default:
				throw new NotImplementedException();
			case PayamakSendStatus.Disable:
				break;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendSmsAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\MessageSenderBaseForm.cs", 85);
		}
	}
}
