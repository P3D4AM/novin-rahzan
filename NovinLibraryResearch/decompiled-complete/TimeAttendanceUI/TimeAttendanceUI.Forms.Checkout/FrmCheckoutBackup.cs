using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using BackUpDLL.Assistance;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom;
using FormCustom.BackUp;
using FormCustom.Cache;
using PacketParser;
using PacketParser.Assistance.MultiLingual;
using PacketParser.Attendance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using Serilog;
using TimeAttendanceDAL.Dal.FLRCheckout;
using TimeAttendanceDAL.Utility;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Forms.Checkout;

public class FrmCheckoutBackup : FormExtention, ICheckout
{
	private Font _font;

	private readonly TimeAttendanceDAL.Dal.FLRCheckout.Checkout _checkout;

	private ICheckout _nextPage;

	private IContainer components;

	public PanelEx PanelEx1;

	private GroupPanel groupPanel4;

	internal LabelX labelX1;

	private ButtonX BtnBackup;

	public FrmCheckoutBackup(string contextGuid, TimeAttendanceDAL.Dal.FLRCheckout.Checkout checkout, Font font)
		: base(contextGuid, true, true)
	{
		try
		{
			InitializeComponent();
			_checkout = checkout;
			_font = font;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public void FillCheckout()
	{
		throw new NotImplementedException();
	}

	public ICheckout NextPage(bool fillCheckout)
	{
		return _nextPage ?? (_nextPage = new FrmCheckOutVacation(((CacheBaseForm)this).ContextGuid, _checkout, _font));
	}

	protected override void OnLoad(EventArgs e)
	{
		((FormExtention)this).OnLoad(e);
		((Control)base.statusStrip1).Visible = false;
		((Control)BtnBackup).Focus();
	}

	private async void BtnBackup_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			ReturnedSaveFuncInfo val = await DataBase.BackUpStartAsync(((CacheBaseForm)this).ContextGuid, (IWin32Window)(object)this, new BackUpConfig(VersionManagment.Version)
			{
				ConnectionString = Extensions.GetSqlConnectionString(((CacheBaseForm)this).ContextGuid),
				Source = (ENSource)5,
				StartUpPath = Application.StartupPath
			}, default(CancellationToken), (IBackUpLog_GetUserName)null);
			if (val.HasError)
			{
				((object)val).ToString();
				await ((CacheBaseForm)new FrmShowErrorMessage(((CacheBaseForm)this).ContextGuid, val, "خطا در پشتیبان گیری از اطلاعات", (MessageBoxButtons)0, false, (LanguageTypes)0, "BtnBackup_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Checkout\\FrmCheckoutBackup.cs")).ShowAsync((IWin32Window)(object)this);
			}
			else
			{
				_ = "پشتیبانگیری از اطلاعات انجام شد، در تاریخ  : " + LanguageAndCalendar.Instance.CaltureCalender.MiladiToCalendar(DateTime.Now);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnBackup_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Checkout\\FrmCheckoutBackup.cs", 74);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((FormExtention)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Expected O, but got Unknown
		//IL_0455: Unknown result type (might be due to invalid IL or missing references)
		//IL_051c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0526: Expected O, but got Unknown
		PanelEx1 = new PanelEx();
		groupPanel4 = new GroupPanel();
		BtnBackup = new ButtonX();
		labelX1 = new LabelX();
		((Control)PanelEx1).SuspendLayout();
		((Control)groupPanel4).SuspendLayout();
		((Control)this).SuspendLayout();
		PanelEx1.CanvasColor = SystemColors.Control;
		PanelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelEx1).Controls.Add((Control)(object)groupPanel4);
		((Control)PanelEx1).Dock = (DockStyle)5;
		((Control)PanelEx1).Font = new Font("B Homa", 9.75f);
		((Control)PanelEx1).Location = new Point(0, 0);
		((Control)PanelEx1).Margin = new Padding(3, 4, 3, 4);
		((Control)PanelEx1).Name = "PanelEx1";
		((Control)PanelEx1).RightToLeft = (RightToLeft)1;
		((Control)PanelEx1).Size = new Size(582, 478);
		PanelEx1.Style.Alignment = (StringAlignment)1;
		PanelEx1.Style.BackColor1.Color = Color.LightSteelBlue;
		PanelEx1.Style.BackColor2.Color = Color.LightSteelBlue;
		PanelEx1.Style.Border = (eBorderType)1;
		PanelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx1.Style.GradientAngle = 90;
		((Control)PanelEx1).TabIndex = 0;
		((PanelControl)groupPanel4).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel4).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel4).Controls.Add((Control)(object)BtnBackup);
		((Control)groupPanel4).Controls.Add((Control)(object)labelX1);
		((Control)groupPanel4).Dock = (DockStyle)5;
		((Control)groupPanel4).Location = new Point(0, 0);
		((Control)groupPanel4).Name = "groupPanel4";
		((Control)groupPanel4).Size = new Size(582, 478);
		((PanelControl)groupPanel4).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanel4).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanel4).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanel4).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanel4).Style.BorderBottomWidth = 1;
		((PanelControl)groupPanel4).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanel4).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanel4).Style.BorderLeftWidth = 1;
		((PanelControl)groupPanel4).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanel4).Style.BorderRightWidth = 1;
		((PanelControl)groupPanel4).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanel4).Style.BorderTopWidth = 1;
		((PanelControl)groupPanel4).Style.Class = "";
		((PanelControl)groupPanel4).Style.CornerDiameter = 4;
		((PanelControl)groupPanel4).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel4).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanel4).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel4).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel4).StyleMouseDown.Class = "";
		((PanelControl)groupPanel4).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel4).StyleMouseOver.Class = "";
		((PanelControl)groupPanel4).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel4).TabIndex = 5;
		((Control)groupPanel4).Text = "پشتیبان گیری";
		((Control)BtnBackup).AccessibleRole = (AccessibleRole)43;
		BtnBackup.ColorTable = (eButtonColor)6;
		((Control)BtnBackup).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)BtnBackup).ForeColor = Color.Black;
		BtnBackup.Image = (Image)(object)Resources.backup;
		BtnBackup.ImagePosition = (eImagePosition)1;
		((Control)BtnBackup).Location = new Point(203, 191);
		((Control)BtnBackup).Margin = new Padding(2);
		((Control)BtnBackup).Name = "BtnBackup";
		((Control)BtnBackup).RightToLeft = (RightToLeft)1;
		((Control)BtnBackup).Size = new Size(157, 53);
		((PopupItemControl)BtnBackup).Style = (eDotNetBarStyle)9;
		((Control)BtnBackup).TabIndex = 0;
		((Control)BtnBackup).Text = "پشتیبان گیری";
		((Control)BtnBackup).Click += BtnBackup_Click;
		((Control)labelX1).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX1).BackgroundStyle.Class = "";
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX1).ForeColor = Color.Firebrick;
		((Control)labelX1).Location = new Point(10, 117);
		((Control)labelX1).Name = "labelX1";
		labelX1.PaddingBottom = 5;
		labelX1.PaddingLeft = 5;
		labelX1.PaddingRight = 5;
		labelX1.PaddingTop = 5;
		((Control)labelX1).Size = new Size(557, 34);
		((Control)labelX1).TabIndex = 7;
		((Control)labelX1).Text = "توجه داشته باشید قبل از شروع عملیات، از اطلاعات نرم افزار پشتیبان گیری نمایید.";
		labelX1.TextAlignment = (StringAlignment)1;
		labelX1.WordWrap = true;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(582, 500);
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Control)this).Name = "FrmCheckoutBackup";
		((Control)this).Text = "FrmCheckoutBackup";
		((Control)this).Controls.SetChildIndex((Control)(object)PanelEx1, 0);
		((Control)PanelEx1).ResumeLayout(false);
		((Control)groupPanel4).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}

	bool ICheckout.get_TopLevel()
	{
		return ((Form)this).TopLevel;
	}

	void ICheckout.set_TopLevel(bool value)
	{
		((Form)this).TopLevel = value;
	}

	void ICheckout.Show(IWin32Window owner)
	{
		((Form)this).Show(owner);
	}
}
