using System;
using System.Runtime.InteropServices;
using RealSvrOcxTcpLib;

namespace AxRealSvrOcxTcpLib;

[ClassInterface(ClassInterfaceType.None)]
public class AxRealSvrOcxTcpEventMulticaster : _DRealSvrOcxTcpEvents
{
	private AxRealSvrOcxTcp parent;

	public AxRealSvrOcxTcpEventMulticaster(AxRealSvrOcxTcp parent)
	{
		this.parent = parent;
	}

	public virtual void OnReceiveGLogData(string astrDeviceIP, int anDevicePort, int anDeviceID, int anSEnrollNumber, int anVerifyMode, int anInOutMode, DateTime anLogDate, string astrSerialNo)
	{
		_DRealSvrOcxTcpEvents_OnReceiveGLogDataEvent e = new _DRealSvrOcxTcpEvents_OnReceiveGLogDataEvent(astrDeviceIP, anDevicePort, anDeviceID, anSEnrollNumber, anVerifyMode, anInOutMode, anLogDate, astrSerialNo);
		parent.RaiseOnOnReceiveGLogData(parent, e);
	}

	public virtual void OnReceiveGLogDataExtend(string astrRootIP, string astrDeviceIP, int anDevicePort, int anDeviceID, int anSEnrollNumber, int anVerifyMode, int anInOutMode, DateTime anLogDate, string astrSerialNo)
	{
		_DRealSvrOcxTcpEvents_OnReceiveGLogDataExtendEvent e = new _DRealSvrOcxTcpEvents_OnReceiveGLogDataExtendEvent(astrRootIP, astrDeviceIP, anDevicePort, anDeviceID, anSEnrollNumber, anVerifyMode, anInOutMode, anLogDate, astrSerialNo);
		parent.RaiseOnOnReceiveGLogDataExtend(parent, e);
	}

	public virtual void OnReceiveGLogTextAndImage(string astrClientIP, int anClientPort, string astrLogText, string astrLogImage)
	{
		_DRealSvrOcxTcpEvents_OnReceiveGLogTextAndImageEvent e = new _DRealSvrOcxTcpEvents_OnReceiveGLogTextAndImageEvent(astrClientIP, anClientPort, astrLogText, astrLogImage);
		parent.RaiseOnOnReceiveGLogTextAndImage(parent, e);
	}

	public virtual void OnReceiveGLogText(string astrClientIP, int anClientPort, string astrLogText)
	{
		_DRealSvrOcxTcpEvents_OnReceiveGLogTextEvent e = new _DRealSvrOcxTcpEvents_OnReceiveGLogTextEvent(astrClientIP, anClientPort, astrLogText);
		parent.RaiseOnOnReceiveGLogText(parent, e);
	}

	public virtual void OnReceiveGLogTextOnDoorOpen(string astrClientIP, int anClientPort, string astrLogText, string astrLogImage)
	{
		_DRealSvrOcxTcpEvents_OnReceiveGLogTextOnDoorOpenEvent e = new _DRealSvrOcxTcpEvents_OnReceiveGLogTextOnDoorOpenEvent(astrClientIP, anClientPort, astrLogText, astrLogImage);
		parent.RaiseOnOnReceiveGLogTextOnDoorOpen(parent, e);
	}
}
