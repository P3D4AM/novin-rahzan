using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using FormCustom.Cache;
using PacketParser.Entities;
using Stimulsoft.Report;
using Stimulsoft.Report.Render;
using Stimulsoft.Report.Viewer;

namespace TimeAttendancePrint.UI;

public class FrmPrintPreview : CacheBaseForm
{
	private StiReport _sti;

	private StiPreviewControl _stiV;

	private Container components;

	public FrmPrintPreview(StiReport sti, string contextGuid)
		: base(contextGuid)
	{
		InitializeComponent();
		_sti = sti;
	}

	private void STIPrintPreview_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode != 27)
			{
				if ((int)keyCode != 80)
				{
					if ((int)keyCode == 116)
					{
						_sti.Print();
					}
				}
				else if (e.Control)
				{
					_sti.Print();
				}
			}
			else
			{
				((Form)this).DialogResult = (DialogResult)2;
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "STIPrintPreview_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendancePrint\\UI\\FRMPrintPreview.cs", 42);
		}
	}

	public void FRMPrintPreview_Load(object sender, EventArgs e)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Expected O, but got Unknown
		try
		{
			if (_sti != null)
			{
				_sti.Show(true);
				((Form)this).Close();
			}
			return;
		}
		catch (Exception ex)
		{
			if (ex.Message.ToLower().Contains("are you missing a using directive or an assembly reference".ToLower()))
			{
				AggregatorExtention.ShowError((CacheBaseForm)(object)this, "[یکی از فایلهای استیمول سافت پیدا نشد]", "خطا");
				return;
			}
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FRMPrintPreview_Load", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendancePrint\\UI\\FRMPrintPreview.cs", 63);
		}
		if (_sti == null)
		{
			return;
		}
		_stiV = new StiPreviewControl(_sti.CompiledReport);
		try
		{
			((Control)this).Controls.Add((Control)(object)_stiV);
			((Control)_stiV).Height = ((Control)this).Height - 20;
			((Control)_stiV).Width = ((Control)this).Width - 20;
			((Control)_stiV).Top = 0;
			((Control)_stiV).Left = 0;
			((Control)_stiV).Anchor = (AnchorStyles)15;
			_sti.Render();
			((StiViewerControl)_stiV).Report = _sti;
			((StiViewerControl)_stiV).Report.Render(true);
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "FRMPrintPreview_Load", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendancePrint\\UI\\FRMPrintPreview.cs", 83);
			try
			{
				_sti.Show(true);
			}
			catch (Exception ex3)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex3, "", "FRMPrintPreview_Load", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendancePrint\\UI\\FRMPrintPreview.cs", 90);
			}
		}
	}

	[DebuggerNonUserCode]
	protected override void Dispose(bool disposing)
	{
		try
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
		}
		finally
		{
			((CacheBaseForm)this).Dispose(disposing);
		}
	}

	[DebuggerStepThrough]
	private void InitializeComponent()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		((Control)this).SuspendLayout();
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((Control)this).KeyDown += new KeyEventHandler(STIPrintPreview_KeyDown);
		((Form)this).Load += FRMPrintPreview_Load;
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(534, 262);
		((Control)this).Name = "FrmPrintPreview";
		((Control)this).Text = "پیش نمایش چاپ";
		((Form)this).WindowState = (FormWindowState)2;
		((Control)this).ResumeLayout(false);
	}
}
