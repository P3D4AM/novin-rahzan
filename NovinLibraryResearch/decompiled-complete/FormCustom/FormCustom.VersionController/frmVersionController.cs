using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using FormCustom.Cache;
using PacketParser.Entities;

namespace FormCustom.VersionController;

public class frmVersionController : CacheBaseForm
{
	private IContainer components;

	private FlowLayoutPanel flowLayoutPanel1;

	private async Task LoadDataAsync(ENSource source, string version)
	{
		try
		{
			List<SoftwarePackage> list = await SoftwarePackage.GetListFromApiAsync(source);
			string[] array = version.Split(new char[1] { '.' });
			string value = array[0] + array[1] + array[2];
			int criteria = Convert.ToInt32(value);
			list = list?.Where((SoftwarePackage x) => Convert.ToInt32(x.Version.Split(new char[1] { '.' })[0] + x.Version.Split(new char[1] { '.' })[1] + x.Version.Split(new char[1] { '.' })[2]) > criteria).Take(10).ToList();
			if (!(list?.Any() ?? false))
			{
				return;
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				((Control)flowLayoutPanel1).Controls.Clear();
				foreach (SoftwarePackage item in list)
				{
					UcVersionController ucVersionController = new UcVersionController();
					((Control)ucVersionController).Width = ((Control)flowLayoutPanel1).Width - 30;
					((Control)ucVersionController).Anchor = (AnchorStyles)13;
					UcVersionController ucVersionController2 = ucVersionController;
					ucVersionController2.UpdateInfo(item);
					((Control)flowLayoutPanel1).Controls.Add((Control)(object)ucVersionController2);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\VersionController\\frmVersionController.cs", 39);
		}
	}

	public frmVersionController(string contextGuid, ENSource source, string version)
		: base(contextGuid)
	{
		frmVersionController frmVersionController2 = this;
		try
		{
			InitializeComponent();
			Task.Run(() => frmVersionController2.LoadDataAsync(source, version));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\VersionController\\frmVersionController.cs", 48);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		flowLayoutPanel1 = new FlowLayoutPanel();
		((Control)this).SuspendLayout();
		((Control)flowLayoutPanel1).Anchor = (AnchorStyles)15;
		((ScrollableControl)flowLayoutPanel1).AutoScroll = true;
		flowLayoutPanel1.FlowDirection = (FlowDirection)1;
		((Control)flowLayoutPanel1).Location = new Point(72, 184);
		((Control)flowLayoutPanel1).Name = "flowLayoutPanel1";
		((Control)flowLayoutPanel1).RightToLeft = (RightToLeft)1;
		((Control)flowLayoutPanel1).Size = new Size(641, 336);
		((Control)flowLayoutPanel1).TabIndex = 0;
		flowLayoutPanel1.WrapContents = false;
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)0;
		((ScrollableControl)this).AutoScroll = true;
		((Control)this).AutoSize = true;
		((Form)this).ClientSize = new Size(784, 561);
		((Control)this).Controls.Add((Control)(object)flowLayoutPanel1);
		((Form)this).FormBorderStyle = (FormBorderStyle)5;
		((Control)this).MinimumSize = new Size(800, 600);
		((Control)this).Name = "Form1";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).ShowIcon = false;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Form)this).WindowState = (FormWindowState)2;
		((Control)this).ResumeLayout(false);
	}
}
