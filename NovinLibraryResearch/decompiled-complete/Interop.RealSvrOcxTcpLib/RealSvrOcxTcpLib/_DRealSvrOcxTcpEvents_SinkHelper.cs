using System;
using System.Runtime.InteropServices;

namespace RealSvrOcxTcpLib;

[TypeLibType(TypeLibTypeFlags.FHidden)]
[ClassInterface(ClassInterfaceType.None)]
public sealed class _DRealSvrOcxTcpEvents_SinkHelper : _DRealSvrOcxTcpEvents
{
	public _DRealSvrOcxTcpEvents_OnReceiveGLogDataEventHandler m_OnReceiveGLogDataDelegate;

	public _DRealSvrOcxTcpEvents_OnReceiveGLogDataExtendEventHandler m_OnReceiveGLogDataExtendDelegate;

	public _DRealSvrOcxTcpEvents_OnReceiveGLogTextAndImageEventHandler m_OnReceiveGLogTextAndImageDelegate;

	public _DRealSvrOcxTcpEvents_OnReceiveGLogTextEventHandler m_OnReceiveGLogTextDelegate;

	public _DRealSvrOcxTcpEvents_OnReceiveGLogTextOnDoorOpenEventHandler m_OnReceiveGLogTextOnDoorOpenDelegate;

	public int m_dwCookie;

	public void OnReceiveGLogData(string P_0, int P_1, int P_2, int P_3, int P_4, int P_5, DateTime P_6, string P_7)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_OnReceiveGLogDataDelegate != null)
		{
			m_OnReceiveGLogDataDelegate(P_0, P_1, P_2, P_3, P_4, P_5, P_6, P_7);
		}
	}

	public void OnReceiveGLogDataExtend(string P_0, string P_1, int P_2, int P_3, int P_4, int P_5, int P_6, DateTime P_7, string P_8)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_OnReceiveGLogDataExtendDelegate != null)
		{
			m_OnReceiveGLogDataExtendDelegate(P_0, P_1, P_2, P_3, P_4, P_5, P_6, P_7, P_8);
		}
	}

	public void OnReceiveGLogTextAndImage(string P_0, int P_1, string P_2, string P_3)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_OnReceiveGLogTextAndImageDelegate != null)
		{
			m_OnReceiveGLogTextAndImageDelegate(P_0, P_1, P_2, P_3);
		}
	}

	public void OnReceiveGLogText(string P_0, int P_1, string P_2)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_OnReceiveGLogTextDelegate != null)
		{
			m_OnReceiveGLogTextDelegate(P_0, P_1, P_2);
		}
	}

	public void OnReceiveGLogTextOnDoorOpen(string P_0, int P_1, string P_2, string P_3)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_OnReceiveGLogTextOnDoorOpenDelegate != null)
		{
			m_OnReceiveGLogTextOnDoorOpenDelegate(P_0, P_1, P_2, P_3);
		}
	}

	internal _DRealSvrOcxTcpEvents_SinkHelper()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		m_dwCookie = 0;
		m_OnReceiveGLogDataDelegate = null;
		m_OnReceiveGLogDataExtendDelegate = null;
		m_OnReceiveGLogTextAndImageDelegate = null;
		m_OnReceiveGLogTextDelegate = null;
		m_OnReceiveGLogTextOnDoorOpenDelegate = null;
	}
}
