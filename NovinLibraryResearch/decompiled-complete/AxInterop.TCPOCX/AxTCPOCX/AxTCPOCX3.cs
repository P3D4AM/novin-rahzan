using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TCPOCX;

namespace AxTCPOCX;

[DefaultEvent("ReceiveSalePacket")]
[Clsid("{c3639507-6596-44c9-ad5a-d0dd97f42cd4}")]
[DesignTimeVisible(true)]
public class AxTCPOCX3 : AxHost
{
	private _TCPOCX3 ocx;

	private AxTCPOCX3EventMulticaster eventMulticaster;

	private ConnectionPointCookie cookie;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(1745027078)]
	public virtual TypeEnum ConnectionType
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("ConnectionType", (ActiveXInvokeKind)1);
			}
			return ocx.ConnectionType;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("ConnectionType", (ActiveXInvokeKind)2);
			}
			ocx.ConnectionType = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(1745027077)]
	public virtual short MaxScaleNO
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("MaxScaleNO", (ActiveXInvokeKind)1);
			}
			return ocx.MaxScaleNO;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(1745027076)]
	public virtual bool NetWork
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("NetWork", (ActiveXInvokeKind)1);
			}
			return ocx.NetWork;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("NetWork", (ActiveXInvokeKind)2);
			}
			ocx.NetWork = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(1745027074)]
	public virtual ModEnum Model
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("Model", (ActiveXInvokeKind)1);
			}
			return ocx.Model;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("Model", (ActiveXInvokeKind)2);
			}
			ocx.Model = value;
		}
	}

	[DispId(1745027073)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual byte ErrNumber
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("ErrNumber", (ActiveXInvokeKind)1);
			}
			return ocx.ErrNumber;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(1745027072)]
	public virtual byte SaleNumber
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("SaleNumber", (ActiveXInvokeKind)1);
			}
			return ocx.SaleNumber;
		}
	}

	public event __TCPOCX3_ReceiveSalePacketEventHandler ReceiveSalePacket;

	public AxTCPOCX3()
		: base("c3639507-6596-44c9-ad5a-d0dd97f42cd4")
	{
	}

	public virtual bool ReadFactorNumber(string iPStr)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ReadFactorNumber", (ActiveXInvokeKind)0);
		}
		return ocx.ReadFactorNumber(iPStr);
	}

	public virtual bool ReadManagerLock(string iPStr)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ReadManagerLock", (ActiveXInvokeKind)0);
		}
		return ocx.ReadManagerLock(iPStr);
	}

	public virtual bool WritePavilionNumber(byte iDNo, byte pavilionNo, string iPStr)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("WritePavilionNumber", (ActiveXInvokeKind)0);
		}
		return ocx.WritePavilionNumber(iDNo, pavilionNo, iPStr);
	}

	public virtual bool ReadWeight(string iPStr)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ReadWeight", (ActiveXInvokeKind)0);
		}
		return ocx.ReadWeight(iPStr);
	}

	public virtual bool WriteDirectMemory(short memoryNo, short memoryType, string memoryName, float untiPrice, double gPrice, double gWeight, bool changG, string iPStr)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("WriteDirectMemory", (ActiveXInvokeKind)0);
		}
		return ocx.WriteDirectMemory(memoryNo, memoryType, memoryName, untiPrice, gPrice, gWeight, changG, iPStr);
	}

	public virtual bool ReadDirectMemory(short memoryNo, string iPStr)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ReadDirectMemory", (ActiveXInvokeKind)0);
		}
		return ocx.ReadDirectMemory(memoryNo, iPStr);
	}

	public virtual bool ReadSaleMemory(bool beforSale, string iPStr)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ReadSaleMemory", (ActiveXInvokeKind)0);
		}
		return ocx.ReadSaleMemory(beforSale, iPStr);
	}

	public virtual bool ReadDisplay(string iPStr)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ReadDisplay", (ActiveXInvokeKind)0);
		}
		return ocx.ReadDisplay(iPStr);
	}

	public virtual bool ReadVersion(string iPStr)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ReadVersion", (ActiveXInvokeKind)0);
		}
		return ocx.ReadVersion(iPStr);
	}

	public virtual double ISExistNewSalePacket()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ISExistNewSalePacket", (ActiveXInvokeKind)0);
		}
		return ocx.ISExistNewSalePacket();
	}

	public virtual string ReadSalePacket(double index)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ReadSalePacket", (ActiveXInvokeKind)0);
		}
		return ocx.ReadSalePacket(index);
	}

	public virtual void ChangeToReadState(double index)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ChangeToReadState", (ActiveXInvokeKind)0);
		}
		ocx.ChangeToReadState(index);
	}

	public virtual short Convert(string msg)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Convert", (ActiveXInvokeKind)0);
		}
		return ocx.Convert(msg);
	}

	public virtual string SaleConvert(string msg)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SaleConvert", (ActiveXInvokeKind)0);
		}
		return ocx.SaleConvert(msg);
	}

	public virtual void BufferProcessing(string inputStr)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("BufferProcessing", (ActiveXInvokeKind)0);
		}
		ocx.BufferProcessing(inputStr);
	}

	public virtual void ManageEVRecieve()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ManageEVRecieve", (ActiveXInvokeKind)0);
		}
		ocx.ManageEVRecieve();
	}

	public virtual object Answer(byte nom)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Answer", (ActiveXInvokeKind)0);
		}
		return ocx.Answer(nom);
	}

	public virtual object SaleAnswer(byte nom)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SaleAnswer", (ActiveXInvokeKind)0);
		}
		return ocx.SaleAnswer(nom);
	}

	public virtual bool ReadCalibInfo(byte deviceID)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ReadCalibInfo", (ActiveXInvokeKind)0);
		}
		return ocx.ReadCalibInfo(deviceID);
	}

	public virtual bool SetScaleID(byte deviceID, byte newID)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetScaleID", (ActiveXInvokeKind)0);
		}
		return ocx.SetScaleID(deviceID, newID);
	}

	public virtual bool ActivateScale(string iPStr, bool beforSale)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ActivateScale", (ActiveXInvokeKind)0);
		}
		return ocx.ActivateScale(iPStr, beforSale);
	}

	public virtual bool ActivateScale2(string iPStr, bool beforSale)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ActivateScale2", (ActiveXInvokeKind)0);
		}
		return ocx.ActivateScale2(iPStr, beforSale);
	}

	public virtual void ActiveTimer()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ActiveTimer", (ActiveXInvokeKind)0);
		}
		ocx.ActiveTimer();
	}

	public virtual void DeActiveTimer()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("DeActiveTimer", (ActiveXInvokeKind)0);
		}
		ocx.DeActiveTimer();
	}

	public virtual void OpenConnection(short newPort)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("OpenConnection", (ActiveXInvokeKind)0);
		}
		ocx.OpenConnection(newPort);
	}

	public virtual string ReadIP(short index)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ReadIP", (ActiveXInvokeKind)0);
		}
		return ocx.ReadIP(index);
	}

	public virtual bool ReadOffScale()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ReadOffScale", (ActiveXInvokeKind)0);
		}
		return ocx.ReadOffScale();
	}

	public virtual void CloseConnection()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("CloseConnection", (ActiveXInvokeKind)0);
		}
		ocx.CloseConnection();
	}

	public virtual ComEnum get_CommPort()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("get_CommPort", (ActiveXInvokeKind)0);
		}
		return ocx.CommPort;
	}

	public virtual void set_CommPort(ref ComEnum param0)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("set_CommPort", (ActiveXInvokeKind)0);
		}
		ocx.CommPort = ref param0;
	}

	public virtual bool ReadScaleInfo(string iPStr)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ReadScaleInfo", (ActiveXInvokeKind)0);
		}
		return ocx.ReadScaleInfo(iPStr);
	}

	public virtual bool WriteScaleInfo(byte modelID, byte verionID, string serialNumber, string iPStr)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("WriteScaleInfo", (ActiveXInvokeKind)0);
		}
		return ocx.WriteScaleInfo(modelID, verionID, serialNumber, iPStr);
	}

	public virtual bool EraseE2prom(string iPStr)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("EraseE2prom", (ActiveXInvokeKind)0);
		}
		return ocx.EraseE2prom(iPStr);
	}

	public virtual bool DefaultEErom(string iPStr)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("DefaultEErom", (ActiveXInvokeKind)0);
		}
		return ocx.DefaultEErom(iPStr);
	}

	public virtual bool WriteLockTimerPass(string serialNo, string iPStr)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("WriteLockTimerPass", (ActiveXInvokeKind)0);
		}
		return ocx.WriteLockTimerPass(serialNo, iPStr);
	}

	public virtual bool ReadLockTimerPass(string iPStr)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ReadLockTimerPass", (ActiveXInvokeKind)0);
		}
		return ocx.ReadLockTimerPass(iPStr);
	}

	public virtual bool WriteLockTimer(ref float lockTimerValue, string iPStr)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("WriteLockTimer", (ActiveXInvokeKind)0);
		}
		return ocx.WriteLockTimer(ref lockTimerValue, iPStr);
	}

	public virtual bool ReadLockTimer(string iPStr)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ReadLockTimer", (ActiveXInvokeKind)0);
		}
		return ocx.ReadLockTimer(iPStr);
	}

	public virtual bool WriteDescription(string shopName, string endmessage, string iPStr)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("WriteDescription", (ActiveXInvokeKind)0);
		}
		return ocx.WriteDescription(shopName, endmessage, iPStr);
	}

	public virtual bool ReadDescription(string iPStr)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ReadDescription", (ActiveXInvokeKind)0);
		}
		return ocx.ReadDescription(iPStr);
	}

	public virtual bool ManagerLockOn(string iPStr)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ManagerLockOn", (ActiveXInvokeKind)0);
		}
		return ocx.ManagerLockOn(iPStr);
	}

	public virtual bool ManagerLockOff(string iPStr)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ManagerLockOff", (ActiveXInvokeKind)0);
		}
		return ocx.ManagerLockOff(iPStr);
	}

	public virtual bool SetLockFlag(byte bitLock, string iPStr)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetLockFlag", (ActiveXInvokeKind)0);
		}
		return ocx.SetLockFlag(bitLock, iPStr);
	}

	public virtual bool WriteClockDate(byte currentMin, byte currentHour, byte currentDay, byte currentMonth, short currentYear, string iPStr)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("WriteClockDate", (ActiveXInvokeKind)0);
		}
		return ocx.WriteClockDate(currentMin, currentHour, currentDay, currentMonth, currentYear, iPStr);
	}

	public virtual bool ReadClockDate(string iPStr)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ReadClockDate", (ActiveXInvokeKind)0);
		}
		return ocx.ReadClockDate(iPStr);
	}

	public virtual bool WriteAccessCode(byte iDAcc, byte boudRate, byte renameMemoryAcc, byte renameShopNameAcc, object changeClockDateAcc, ref object changeDivisionAcc, string iPStr)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("WriteAccessCode", (ActiveXInvokeKind)0);
		}
		return ocx.WriteAccessCode(iDAcc, boudRate, renameMemoryAcc, renameShopNameAcc, changeClockDateAcc, ref changeDivisionAcc, iPStr);
	}

	public virtual bool WriteFactorNumber(float factorNumber, string iPStr)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("WriteFactorNumber", (ActiveXInvokeKind)0);
		}
		return ocx.WriteFactorNumber(factorNumber, iPStr);
	}

	protected override void CreateSink()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		try
		{
			eventMulticaster = new AxTCPOCX3EventMulticaster(this);
			cookie = new ConnectionPointCookie((object)ocx, (object)eventMulticaster, typeof(__TCPOCX3));
		}
		catch (Exception)
		{
		}
	}

	protected override void DetachSink()
	{
		try
		{
			cookie.Disconnect();
		}
		catch (Exception)
		{
		}
	}

	protected override void AttachInterfaces()
	{
		try
		{
			ocx = (_TCPOCX3)((AxHost)this).GetOcx();
		}
		catch (Exception)
		{
		}
	}

	internal void RaiseOnReceiveSalePacket(object sender, __TCPOCX3_ReceiveSalePacketEvent e)
	{
		if (this.ReceiveSalePacket != null)
		{
			this.ReceiveSalePacket(sender, e);
		}
	}
}
