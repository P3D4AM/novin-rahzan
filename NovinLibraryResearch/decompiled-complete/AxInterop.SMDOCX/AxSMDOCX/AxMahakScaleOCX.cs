using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SMDOCX;

namespace AxSMDOCX;

[DesignTimeVisible(true)]
[Clsid("{c7c7ba84-df4d-47b2-90a8-a4b9fd6c8db0}")]
public class AxMahakScaleOCX : AxHost
{
	private _MahakScaleOCX ocx;

	private AxMahakScaleOCXEventMulticaster eventMulticaster;

	private ConnectionPointCookie cookie;

	[DispId(1745027076)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(1745027073)]
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

	public AxMahakScaleOCX()
		: base("c7c7ba84-df4d-47b2-90a8-a4b9fd6c8db0")
	{
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

	public virtual bool ReadScaleInfo(byte deviceID)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ReadScaleInfo", (ActiveXInvokeKind)0);
		}
		return ocx.ReadScaleInfo(deviceID);
	}

	public virtual bool WriteDescription(byte deviceID, string shopName, string endmessage)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("WriteDescription", (ActiveXInvokeKind)0);
		}
		return ocx.WriteDescription(deviceID, shopName, endmessage);
	}

	public virtual bool ReadDescription(byte deviceID)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ReadDescription", (ActiveXInvokeKind)0);
		}
		return ocx.ReadDescription(deviceID);
	}

	public virtual bool ManagerLockOn(byte deviceID)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ManagerLockOn", (ActiveXInvokeKind)0);
		}
		return ocx.ManagerLockOn(deviceID);
	}

	public virtual bool ManagerLockOff(byte deviceID)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ManagerLockOff", (ActiveXInvokeKind)0);
		}
		return ocx.ManagerLockOff(deviceID);
	}

	public virtual bool WriteClockDate(byte deviceID, byte currentMin, byte currentHour, byte currentDay, byte currentMonth, short currentYear)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("WriteClockDate", (ActiveXInvokeKind)0);
		}
		return ocx.WriteClockDate(deviceID, currentMin, currentHour, currentDay, currentMonth, currentYear);
	}

	public virtual bool ReadClockDate(byte deviceID)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ReadClockDate", (ActiveXInvokeKind)0);
		}
		return ocx.ReadClockDate(deviceID);
	}

	public virtual bool WriteAccessCode(byte deviceID, byte iDAcc, byte boudRate, byte renameMemoryAcc, byte renameShopNameAcc, object changeClockDateAcc, ref object changeDivisionAcc)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("WriteAccessCode", (ActiveXInvokeKind)0);
		}
		return ocx.WriteAccessCode(deviceID, iDAcc, boudRate, renameMemoryAcc, renameShopNameAcc, changeClockDateAcc, ref changeDivisionAcc);
	}

	public virtual bool WriteFactorNumber(byte deviceID, int factorNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("WriteFactorNumber", (ActiveXInvokeKind)0);
		}
		return ocx.WriteFactorNumber(deviceID, factorNumber);
	}

	public virtual bool ReadFactorNumber(byte deviceID)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ReadFactorNumber", (ActiveXInvokeKind)0);
		}
		return ocx.ReadFactorNumber(deviceID);
	}

	public virtual bool ReadManagerLock(byte deviceID)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ReadManagerLock", (ActiveXInvokeKind)0);
		}
		return ocx.ReadManagerLock(deviceID);
	}

	public virtual bool WritePavilionNumber(byte deviceID, byte iDNo, byte pavilionNo)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("WritePavilionNumber", (ActiveXInvokeKind)0);
		}
		return ocx.WritePavilionNumber(deviceID, iDNo, pavilionNo);
	}

	public virtual bool ReadWeight(byte deviceID)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ReadWeight", (ActiveXInvokeKind)0);
		}
		return ocx.ReadWeight(deviceID);
	}

	public virtual bool WriteDirectMemory(byte deviceID, short memoryNo, short memoryType, string memoryName, int untiPrice, double gPrice, double gWeight, bool changG)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("WriteDirectMemory", (ActiveXInvokeKind)0);
		}
		return ocx.WriteDirectMemory(deviceID, memoryNo, memoryType, memoryName, untiPrice, gPrice, gWeight, changG);
	}

	public virtual bool ReadDirectMemory(byte deviceID, short memoryNo)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ReadDirectMemory", (ActiveXInvokeKind)0);
		}
		return ocx.ReadDirectMemory(deviceID, memoryNo);
	}

	public virtual bool ReadSaleMemory(byte deviceID, bool beforSale)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ReadSaleMemory", (ActiveXInvokeKind)0);
		}
		return ocx.ReadSaleMemory(deviceID, beforSale);
	}

	public virtual bool ReadDisplay(byte deviceID)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ReadDisplay", (ActiveXInvokeKind)0);
		}
		return ocx.ReadDisplay(deviceID);
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

	public virtual bool EraseE2prom(byte deviceID)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("EraseE2prom", (ActiveXInvokeKind)0);
		}
		return ocx.EraseE2prom(deviceID);
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

	protected override void CreateSink()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		try
		{
			eventMulticaster = new AxMahakScaleOCXEventMulticaster(this);
			cookie = new ConnectionPointCookie((object)ocx, (object)eventMulticaster, typeof(__MahakScaleOCX));
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
			ocx = (_MahakScaleOCX)((AxHost)this).GetOcx();
		}
		catch (Exception)
		{
		}
	}
}
