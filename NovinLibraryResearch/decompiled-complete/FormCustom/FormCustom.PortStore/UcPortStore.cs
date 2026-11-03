using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using FormCustom.Properties;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace FormCustom.PortStore;

public class UcPortStore : UserControl, IHasContext
{
	private IContainer components;

	private PictureBox picPortStore;

	public string ContextGuid { get; private set; }

	public int Port { get; private set; }

	public Font Font { get; private set; }

	public EnPortType Type { get; private set; }

	public event Action<int> OnPortChanged;

	public void Init(string contextGuid, EnPortType type, Font font)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ContextGuid = contextGuid;
			Font = font;
			Type = type;
			new ToolTip().SetToolTip((Control)(object)picPortStore, "راهنمای پورت ها");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Init", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\PortStore\\UcPortStore.cs", 29);
		}
	}

	public UcPortStore()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	private async void picPortStore_Click(object sender, EventArgs e)
	{
		try
		{
			frmPortStore frm = new frmPortStore(ContextGuid, Type, Font);
			if ((int)(await frm.ShowAsync((IWin32Window)(object)((Control)this).FindForm())) == 1)
			{
				Port = frm.Port;
				this.OnPortChanged?.Invoke(Port);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "picPortStore_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\PortStore\\UcPortStore.cs", 47);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		picPortStore = new PictureBox();
		((ISupportInitialize)picPortStore).BeginInit();
		((Control)this).SuspendLayout();
		((Control)picPortStore).Cursor = Cursors.Hand;
		((Control)picPortStore).Dock = (DockStyle)5;
		picPortStore.Image = (Image)(object)FormCustom.Properties.Resources.help;
		((Control)picPortStore).Location = new Point(0, 0);
		((Control)picPortStore).Name = "picPortStore";
		((Control)picPortStore).Size = new Size(35, 31);
		picPortStore.SizeMode = (PictureBoxSizeMode)1;
		picPortStore.TabIndex = 0;
		picPortStore.TabStop = false;
		((Control)picPortStore).Click += picPortStore_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)picPortStore);
		((Control)this).Name = "UcPortStore";
		((Control)this).Size = new Size(35, 31);
		((ISupportInitialize)picPortStore).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
