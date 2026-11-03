using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PayamakUi.Classes.GSM;

namespace PayamakUi.UC.Others;

public class UCSignal : UserControl, IHasContext
{
	private short _Signal = 40;

	private IContainer components;

	internal PictureBox PictureBox1;

	internal Timer Timer1;

	public short Signal
	{
		get
		{
			return _Signal;
		}
		set
		{
		}
	}

	public string ContextGuid { get; set; }

	public UCSignal(string contextGuid_)
	{
		ContextGuid = contextGuid_;
		InitializeComponent();
	}

	public UCSignal()
	{
		InitializeComponent();
	}

	private void UCSignal_Load(object sender, EventArgs e)
	{
		try
		{
			SignalManagment.SignalStrength = 0;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UCSignal_Load", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\Others\\UCSignal.cs", 84);
		}
	}

	private void Timer1_Tick(object sender, EventArgs e)
	{
		try
		{
			Signal = SignalManagment.SignalStrength;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Timer1_Tick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\Others\\UCSignal.cs", 96);
		}
	}

	protected override void Dispose(bool disposing)
	{
		components?.Dispose();
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(UCSignal));
		PictureBox1 = new PictureBox();
		Timer1 = new Timer(components);
		((ISupportInitialize)PictureBox1).BeginInit();
		((Control)this).SuspendLayout();
		((Control)PictureBox1).BackColor = Color.LightSteelBlue;
		((Control)PictureBox1).Dock = (DockStyle)5;
		PictureBox1.Image = (Image)componentResourceManager.GetObject("PictureBox1.Image");
		((Control)PictureBox1).Location = new Point(0, 0);
		((Control)PictureBox1).Name = "PictureBox1";
		((Control)PictureBox1).Size = new Size(48, 48);
		PictureBox1.SizeMode = (PictureBoxSizeMode)4;
		PictureBox1.TabIndex = 0;
		PictureBox1.TabStop = false;
		Timer1.Enabled = true;
		Timer1.Interval = 500;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)PictureBox1);
		((Control)this).Name = "UCSignal";
		((Control)this).Size = new Size(48, 48);
		((ISupportInitialize)PictureBox1).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
