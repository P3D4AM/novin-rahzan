using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using AxSBXPCLib;
using Serilog;

namespace TimeAttendanceDevices;

public class ActivexFormHidden : Form
{
	public dynamic SbxpcControl;

	private const int DATASIZE = 354;

	private const int DATASIZE_FACE = 6917;

	private int[] _gTemplngEnrollData = new int[6917];

	private byte[] _gbytEnrollData = new byte[34585];

	private int _gMachineNumber = 1;

	private readonly int _Password;

	private string _IP = "";

	private int _Port;

	private IContainer components;

	public bool Conected { get; private set; }

	public ActivexFormHidden(int deviceNum, int password, string ip, int port)
	{
		AxSBXPC xx = CreateAxSbxpcControl();
		((Control)this).Controls.Add((Control)(object)xx);
		((Form)this).Load += delegate
		{
			((Control)xx).CreateControl();
		};
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).ShowInTaskbar = false;
		((Control)this).Visible = false;
		_gMachineNumber = deviceNum;
		_Password = password;
		_IP = ip;
		_Port = port;
	}

	private AxSBXPC CreateAxSbxpcControl()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		AxSBXPC val = new AxSBXPC();
		try
		{
			((ISupportInitialize)val).BeginInit();
			((Control)this).SuspendLayout();
			((AxHost)val).Enabled = true;
			((Control)val).Location = new Point(12, 211);
			((Control)val).Name = "axSBXPC1";
			((Control)val).Size = new Size(100, 50);
			((Control)val).TabIndex = 0;
			((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
			((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
			((Form)this).ClientSize = new Size(292, 273);
			((Control)this).Controls.Add((Control)(object)val);
			((Control)this).Name = "Ebio";
			((Control)this).Text = "E";
			((ISupportInitialize)val).EndInit();
			((Control)this).ResumeLayout(false);
		}
		catch (Exception)
		{
		}
		return val;
	}

	public bool Connect()
	{
		try
		{
			if (Conected)
			{
				Conected = Disconnect();
			}
			Conected = SbxpcControl.ConnectTcpip(_gMachineNumber, ref _IP, _Port, _Password);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return Conected;
	}

	public bool Disconnect()
	{
		try
		{
			SbxpcControl.Disconnect();
			Conected = false;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return Conected;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		((Control)this).SuspendLayout();
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(800, 450);
		((Control)this).Name = "ActivexFormHidden";
		((Control)this).Text = "ActivexFormHidden";
		((Control)this).ResumeLayout(false);
	}
}
