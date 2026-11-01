using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using FormCustom;
using FormCustom.Cache;
using FormCustom.Forms;
using FormCustom.Waiting;
using PacketParser.Entities;
using Serilog;
using TimeAttendanceDAL.Dal.Reports;
using TimeAttendanceUI.ReportProducer.UC;

namespace TimeAttendanceUI.ReportProducer;

public class FrmFilter : BaseForm
{
	private RPTGen _RPT;

	private bool _Show;

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _searchToken;

	private string _searchString = "";

	private bool _isDisposed;

	private int _reportId;

	private Font _font;

	private IContainer components;

	private PanelEx panelEx1;

	private UCDate ucDate1;

	private UCPerson ucPerson1;

	internal ButtonX BtnDesign;

	public bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return ((BaseForm)this).IsDisposed;
			}
			return true;
		}
	}

	public RPTGen RPT => _RPT;

	private void FrmFilter_FormClosed(object sender, FormClosedEventArgs e)
	{
		((Component)(object)this).Dispose(disposing: true);
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_loadDataToken?.Cancel();
				_loadDataToken?.Dispose();
				_loadDataToken = null;
				_searchToken?.Cancel();
				_searchToken?.Dispose();
				_searchToken = null;
				components?.Dispose();
				((BaseForm)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public FrmFilter(string contextGuid, RPTGen rpt, Font font)
		: base(contextGuid, (BaseFormConfig)null)
	{
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			_font = font;
			InitializeComponent();
			ucPerson1.SetContextGuid(contextGuid);
			ucPerson1.VisTab();
			_RPT = rpt;
			_Show = false;
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\FRMFilter.cs");
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public FrmFilter(string contextGuid, int ID, Font font)
		: base(contextGuid, (BaseFormConfig)null)
	{
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			_font = font;
			InitializeComponent();
			ucPerson1.SetContextGuid(contextGuid);
			ucPerson1.VisTab();
			_reportId = ID;
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\FRMFilter.cs");
			_Show = true;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async Task LoadDataAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			if (_RPT == null && _reportId > 0)
			{
				_RPT = await RPTGen.GetAsync(((CacheBaseForm)this).ContextGuid, _reportId, token);
			}
			await ReDesignAsync();
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
		}
	}

	private async Task ReDesignAsync()
	{
		try
		{
			((Control)ucDate1).Visible = false;
			((Control)ucPerson1).Visible = false;
			int pading = 10;
			int top2 = 10;
			if (_RPT == null)
			{
				return;
			}
			foreach (RPTGenDet item in await _RPT.GetLstRPTGenDetAsync())
			{
				if (((Control)ucDate1).Name == item.ParameterName)
				{
					((Control)ucDate1).Visible = true;
					((Control)ucDate1).Top = top2;
					top2 += ((Control)ucDate1).Height + pading;
				}
				if (!(((Control)ucPerson1).Name != item.ParameterName))
				{
					((Control)ucPerson1).Visible = true;
					ucPerson1.CheckKoli();
					((Control)ucPerson1).Top = top2;
					top2 += ((Control)ucPerson1).Height + pading;
				}
			}
			((Control)panelEx1).Height = ((Control)panelEx1).Height;
			top2 += ((Control)BtnDesign).Height + 5 * pading;
			((Control)this).Height = top2;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ReDesignAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\FRMFilter.cs", 135);
		}
	}

	private async void BtnDesign_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)panelEx1).Focus();
			if (((Control)ucDate1).Visible)
			{
				_RPT.DateM1 = ucDate1.DateM1;
				_RPT.DateM2 = ucDate1.DateM2;
			}
			if (((Control)ucPerson1).Visible)
			{
				_RPT.KhedmatID = ucPerson1.IdPerKhedmatPlace;
				_RPT.ListID = ucPerson1.LstSelected;
				_RPT.personID = ucPerson1.IdPerson;
				_RPT.GroupID = ucPerson1.IdGroup;
			}
			await _RPT.ShowAsync(_Show);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnDesign_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\FRMFilter.cs", 162);
		}
	}

	private async void ucDate1_Resize(object sender, EventArgs e)
	{
		await ReDesignAsync();
	}

	private async void ucPerson1_Resize(object sender, EventArgs e)
	{
		await ReDesignAsync();
	}

	private async void FRMFilter_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (((int)e.KeyCode >= 112) & ((int)e.KeyCode <= 123))
			{
				e.Handled = true;
				e.SuppressKeyPress = true;
			}
			ucDate1.GotoMonth(e);
			await ReDesignAsync();
			if ((int)e.KeyCode == 27)
			{
				((Form)this).DialogResult = (DialogResult)2;
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FRMFilter_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\FRMFilter.cs", 194);
		}
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Expected O, but got Unknown
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_0361: Expected O, but got Unknown
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Unknown result type (might be due to invalid IL or missing references)
		//IL_046d: Expected O, but got Unknown
		//IL_04dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e6: Expected O, but got Unknown
		//IL_04ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f8: Expected O, but got Unknown
		panelEx1 = new PanelEx();
		BtnDesign = new ButtonX();
		ucPerson1 = new UCPerson();
		ucDate1 = new UCDate();
		((Control)panelEx1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)panelEx1).Anchor = (AnchorStyles)15;
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)BtnDesign);
		((Control)panelEx1).Controls.Add((Control)(object)ucPerson1);
		((Control)panelEx1).Controls.Add((Control)(object)ucDate1);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).MinimumSize = new Size(630, 20);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(630, 545);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 1;
		((Control)BtnDesign).AccessibleRole = (AccessibleRole)43;
		((Control)BtnDesign).Anchor = (AnchorStyles)14;
		BtnDesign.ColorTable = (eButtonColor)6;
		((Control)BtnDesign).Location = new Point(15, 474);
		((Control)BtnDesign).Name = "BtnDesign";
		((Control)BtnDesign).Size = new Size(600, 59);
		((PopupItemControl)BtnDesign).Style = (eDotNetBarStyle)9;
		((Control)BtnDesign).TabIndex = 4;
		((Control)BtnDesign).Text = "[تهیه گزارش]";
		((Control)BtnDesign).Click += BtnDesign_Click;
		((Control)ucPerson1).Anchor = (AnchorStyles)7;
		ucPerson1.EnalbleControl = true;
		((Control)ucPerson1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)ucPerson1).Location = new Point(15, 41);
		((Control)ucPerson1).Margin = new Padding(0);
		((Control)ucPerson1).MaximumSize = new Size(600, 1000);
		((Control)ucPerson1).MinimumSize = new Size(600, 10);
		((Control)ucPerson1).Name = "ucPerson1";
		((Control)ucPerson1).RightToLeft = (RightToLeft)1;
		((Control)ucPerson1).Size = new Size(600, 430);
		((Control)ucPerson1).TabIndex = 2;
		((Control)ucPerson1).Resize += ucPerson1_Resize;
		ucDate1.EnalbleControl = true;
		((Control)ucDate1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)ucDate1).Location = new Point(15, 6);
		((Control)ucDate1).Margin = new Padding(0);
		((Control)ucDate1).MaximumSize = new Size(600, 200);
		((Control)ucDate1).MinimumSize = new Size(600, 10);
		((Control)ucDate1).Name = "ucDate1";
		((Control)ucDate1).RightToLeft = (RightToLeft)1;
		((Control)ucDate1).Size = new Size(600, 178);
		((Control)ucDate1).TabIndex = 3;
		((Control)ucDate1).Resize += ucDate1_Resize;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(96f, 96f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)2;
		((Form)this).ClientSize = new Size(624, 545);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Control)this).MaximumSize = new Size(640, 800);
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(640, 39);
		((Control)this).Name = "FrmFilter";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[فیلتر گزارش ساز پویا]";
		((Form)this).FormClosed += new FormClosedEventHandler(FrmFilter_FormClosed);
		((Control)this).KeyDown += new KeyEventHandler(FRMFilter_KeyDown);
		((Control)panelEx1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
