using System;
using System.IO.Ports;
using System.Threading;
using System.Timers;
using PacketParser.PosToPc.Sadad.Classes.EventArguments;

namespace PacketParser.PosToPc.Sadad.Classes;

internal class MagicBusiness
{
	private static System.Timers.Timer aTimer;

	private SerialPort comport = new SerialPort();

	private string Curr_Amount;

	private string Curr_BillNo;

	private string Curr_MerchantNo;

	private string Curr_TerminalNo;

	private bool Synch_DataRcv_Flag;

	private bool Synch_Timeout_Flag;

	private PCPOSEventArg SynchSendTransResult = new PCPOSEventArg();

	public event EventHandler<PCPOSEventArg> Data_Received;

	private void OnTimedEvent(object source, ElapsedEventArgs e)
	{
		aTimer.Enabled = false;
		PCPOSEventArg pCPOSEventArg = new PCPOSEventArg
		{
			PosRes = "99",
			SwitchRes = "01",
			TerminalNo = Curr_TerminalNo,
			MerchantNo = Curr_MerchantNo,
			BillNo = Curr_BillNo,
			SwitchDate = "00000000",
			SwitchTime = "000000",
			TraceNo = "000000",
			RefNo = "000000000000",
			CardNo = "0000000000000000",
			Amount = Curr_Amount
		};
		SynchSendTransResult = new PCPOSEventArg
		{
			TerminalNo = pCPOSEventArg.TerminalNo,
			MerchantNo = pCPOSEventArg.MerchantNo,
			BillNo = pCPOSEventArg.BillNo,
			Amount = pCPOSEventArg.Amount,
			SwitchDate = pCPOSEventArg.SwitchDate,
			SwitchTime = pCPOSEventArg.SwitchTime,
			PosRes = pCPOSEventArg.PosRes,
			SwitchRes = pCPOSEventArg.SwitchRes,
			TraceNo = pCPOSEventArg.TraceNo,
			RefNo = pCPOSEventArg.RefNo,
			CardNo = pCPOSEventArg.CardNo,
			TransCode = pCPOSEventArg.TransCode
		};
		Synch_Timeout_Flag = true;
		if (comport.IsOpen)
		{
			try
			{
				comport.DiscardInBuffer();
				comport.DiscardOutBuffer();
				comport.Close();
			}
			catch (Exception)
			{
			}
		}
		if (this.Data_Received != null)
		{
			try
			{
				this.Data_Received(this, pCPOSEventArg);
			}
			catch (Exception)
			{
			}
		}
	}

	private int OpenPort(string ComPortName)
	{
		if (ComPortName == "")
		{
			return -82;
		}
		try
		{
			comport.BaudRate = 9600;
			comport.Parity = (Parity)0;
			comport.DataBits = 8;
			comport.StopBits = (StopBits)1;
			if (!comport.PortName.Equals(ComPortName, StringComparison.InvariantCultureIgnoreCase))
			{
				comport.PortName = ComPortName;
			}
			if (!comport.IsOpen)
			{
				comport.Open();
				return 0;
			}
			return -81;
		}
		catch (Exception)
		{
			return -84;
		}
	}

	public int SendTransSynch(string pReqId, string pComPortName, string pTerminalNo, string pMerchantNo, string pBillNo, string pAmount, string pTransDate, string pTransTime, int pTimeout, ref PCPOSEventArg pSendTransResult)
	{
		Synch_DataRcv_Flag = false;
		Synch_Timeout_Flag = false;
		pSendTransResult = new PCPOSEventArg
		{
			RequestId = pReqId,
			TerminalNo = pTerminalNo,
			MerchantNo = pMerchantNo,
			BillNo = pBillNo,
			Amount = pAmount,
			SwitchDate = pTransDate,
			SwitchTime = pTransTime,
			PosRes = "",
			SwitchRes = "",
			TraceNo = "",
			RefNo = "",
			CardNo = "",
			TransCode = MagicConsts.PCPOS_TRANS_TRCOD
		};
		int num = OpenPort(pComPortName.Trim());
		Curr_TerminalNo = pTerminalNo;
		Curr_MerchantNo = pMerchantNo;
		Curr_BillNo = pBillNo;
		Curr_Amount = pAmount;
		if (num != 0)
		{
			pSendTransResult = new PCPOSEventArg
			{
				RequestId = pReqId,
				TerminalNo = pTerminalNo,
				MerchantNo = pMerchantNo,
				BillNo = pBillNo,
				Amount = pAmount,
				SwitchDate = Utility.GetPersianPcDate(),
				SwitchTime = DateTime.Now.ToString("HH:mm:ss"),
				PosRes = string.Empty,
				SwitchRes = string.Empty,
				TraceNo = string.Empty,
				RefNo = string.Empty,
				CardNo = string.Empty,
				TransCode = string.Empty
			};
		}
		else
		{
			aTimer = new System.Timers.Timer
			{
				Enabled = false,
				Interval = pTimeout
			};
			aTimer.Elapsed += OnTimedEvent;
			aTimer.Enabled = true;
			do
			{
				Thread.Sleep(0);
			}
			while (!Synch_DataRcv_Flag && !Synch_Timeout_Flag);
			pSendTransResult = new PCPOSEventArg
			{
				RequestId = pReqId,
				TerminalNo = SynchSendTransResult.TerminalNo,
				MerchantNo = SynchSendTransResult.MerchantNo,
				BillNo = SynchSendTransResult.BillNo,
				Amount = SynchSendTransResult.Amount,
				SwitchDate = SynchSendTransResult.SwitchDate,
				SwitchTime = SynchSendTransResult.SwitchTime,
				PosRes = SynchSendTransResult.PosRes,
				SwitchRes = SynchSendTransResult.SwitchRes,
				TraceNo = SynchSendTransResult.TraceNo,
				RefNo = SynchSendTransResult.RefNo,
				CardNo = SynchSendTransResult.CardNo,
				TransCode = SynchSendTransResult.TransCode
			};
			if (SynchSendTransResult.PosRes.Length > 0)
			{
				num = Convert.ToInt32(SynchSendTransResult.PosRes.Trim());
			}
		}
		return num;
	}
}
