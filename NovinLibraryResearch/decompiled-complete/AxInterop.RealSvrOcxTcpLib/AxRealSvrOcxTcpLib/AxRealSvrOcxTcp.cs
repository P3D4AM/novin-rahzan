using System;
using System.ComponentModel;
using System.Windows.Forms;
using RealSvrOcxTcpLib;

namespace AxRealSvrOcxTcpLib;

[DefaultEvent("OnReceiveGLogData")]
[DesignTimeVisible(true)]
[Clsid("{fe364b34-c0f4-42fe-8fa1-c8fe62208946}")]
public class AxRealSvrOcxTcp : AxHost
{
	private _DRealSvrOcxTcp ocx;

	private AxRealSvrOcxTcpEventMulticaster eventMulticaster;

	private ConnectionPointCookie cookie;

	public event _DRealSvrOcxTcpEvents_OnReceiveGLogDataEventHandler OnReceiveGLogData;

	public event _DRealSvrOcxTcpEvents_OnReceiveGLogDataExtendEventHandler OnReceiveGLogDataExtend;

	public event _DRealSvrOcxTcpEvents_OnReceiveGLogTextAndImageEventHandler OnReceiveGLogTextAndImage;

	public event _DRealSvrOcxTcpEvents_OnReceiveGLogTextEventHandler OnReceiveGLogText;

	public event _DRealSvrOcxTcpEvents_OnReceiveGLogTextOnDoorOpenEventHandler OnReceiveGLogTextOnDoorOpen;

	public AxRealSvrOcxTcp()
		: base("fe364b34-c0f4-42fe-8fa1-c8fe62208946")
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		((AxHost)this).SetAboutBoxDelegate(new AboutBoxDelegate(AboutBox));
	}

	public virtual int OpenNetwork(int anHostPort)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("OpenNetwork", (ActiveXInvokeKind)0);
		}
		return ocx.OpenNetwork(anHostPort);
	}

	public virtual int CloseNetwork(int anHostPort)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("CloseNetwork", (ActiveXInvokeKind)0);
		}
		return ocx.CloseNetwork(anHostPort);
	}

	public virtual int SendResponse(string astrClientIP, int anClientPort, string astrResponse)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SendResponse", (ActiveXInvokeKind)0);
		}
		return ocx.SendResponse(astrClientIP, anClientPort, astrResponse);
	}

	public virtual int SendRtLogResponseV1(string astrClientIP, int anClientPort, string astrResponse)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SendRtLogResponseV1", (ActiveXInvokeKind)0);
		}
		return ocx.SendRtLogResponseV1(astrClientIP, anClientPort, astrResponse);
	}

	public virtual int SendRtLogResponseV3(string astrClientIP, int anClientPort, string astrResponse)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SendRtLogResponseV3", (ActiveXInvokeKind)0);
		}
		return ocx.SendRtLogResponseV3(astrClientIP, anClientPort, astrResponse);
	}

	public virtual void AboutBox()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("AboutBox", (ActiveXInvokeKind)0);
		}
		ocx.AboutBox();
	}

	protected override void CreateSink()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		try
		{
			eventMulticaster = new AxRealSvrOcxTcpEventMulticaster(this);
			cookie = new ConnectionPointCookie((object)ocx, (object)eventMulticaster, typeof(_DRealSvrOcxTcpEvents));
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
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		try
		{
			ocx = (_DRealSvrOcxTcp)((AxHost)this).GetOcx();
		}
		catch (Exception)
		{
		}
	}

	internal void RaiseOnOnReceiveGLogData(object sender, _DRealSvrOcxTcpEvents_OnReceiveGLogDataEvent e)
	{
		if (this.OnReceiveGLogData != null)
		{
			this.OnReceiveGLogData(sender, e);
		}
	}

	internal void RaiseOnOnReceiveGLogDataExtend(object sender, _DRealSvrOcxTcpEvents_OnReceiveGLogDataExtendEvent e)
	{
		if (this.OnReceiveGLogDataExtend != null)
		{
			this.OnReceiveGLogDataExtend(sender, e);
		}
	}

	internal void RaiseOnOnReceiveGLogTextAndImage(object sender, _DRealSvrOcxTcpEvents_OnReceiveGLogTextAndImageEvent e)
	{
		if (this.OnReceiveGLogTextAndImage != null)
		{
			this.OnReceiveGLogTextAndImage(sender, e);
		}
	}

	internal void RaiseOnOnReceiveGLogText(object sender, _DRealSvrOcxTcpEvents_OnReceiveGLogTextEvent e)
	{
		if (this.OnReceiveGLogText != null)
		{
			this.OnReceiveGLogText(sender, e);
		}
	}

	internal void RaiseOnOnReceiveGLogTextOnDoorOpen(object sender, _DRealSvrOcxTcpEvents_OnReceiveGLogTextOnDoorOpenEvent e)
	{
		if (this.OnReceiveGLogTextOnDoorOpen != null)
		{
			this.OnReceiveGLogTextOnDoorOpen(sender, e);
		}
	}
}
