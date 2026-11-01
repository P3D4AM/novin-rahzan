using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom;
using FormCustom.Cache;
using PacketParser;
using PacketParser.Entities;
using Serilog;
using TimeAttendanceDAL.Dal.Taradod;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.Utility;
using TimeAttendanceDAL.Utility.FarsiMessageBox;
using TimeAttendanceUI.Forms.Other;

namespace TimeAttendanceUI.Forms.Taradod;

public class DefMorakhasiTypeMain : FrmExtList
{
	private bool _isDisposed;

	private CancellationTokenSource _searchToken;

	private CancellationTokenSource _loadDataToken;

	private List<TypeMorakhasi> _dGridLists = new List<TypeMorakhasi>();

	private List<TypeMorakhasi> _dGridListsForDisplay = new List<TypeMorakhasi>();

	private string Frm_Text = "[لیست عملکرد کاربران]";

	private string trCountRecords = "[تعداد رکوردها]";

	private Font _font;

	private IContainer components;

	private DataGridViewX DGrid;

	private LabelX LblRowCount;

	private DataGridViewTextBoxColumn iDDGrid;

	private DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;

	private DataGridViewCheckBoxColumn bimehDataGridViewCheckBoxColumn;

	private DataGridViewCheckBoxColumn ezafatDataGridViewCheckBoxColumn;

	private DataGridViewCheckBoxColumn kosoratDataGridViewCheckBoxColumn;

	private DataGridViewCheckBoxColumn hoghoghDataGridViewCheckBoxColumn;

	private DataGridViewCheckBoxColumn maliyatDataGridViewCheckBoxColumn;

	private BindingSource typeMorakhasiBindingSource;

	public TypeMorakhasi SelectedItem { get; private set; }

	public bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return ((FrmExtList)this).IsDisposed;
			}
			return true;
		}
	}

	public DefMorakhasiTypeMain(string contextGuid, Font font)
		: base(contextGuid)
	{
		try
		{
			_font = font;
			InitializeComponent();
			((Control)DGrid).Focus();
			((Control)DGrid).Font = font;
			Frm_Text = "[لیست انواع مرخصی]";
			((FrmExtList)this).Search();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\DefMorakhasiTypeMain.cs", 43);
		}
	}

	public DefMorakhasiTypeMain(string contextGuid, Font font, bool pIsSelect)
		: base(contextGuid)
	{
		try
		{
			_font = font;
			InitializeComponent();
			((Control)DGrid).Focus();
			((Control)DGrid).Font = font;
			((Control)base.BtnSelect).Visible = pIsSelect;
			Frm_Text = "[لیست انواع مرخصی]";
			((FrmExtList)this).Search();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\DefMorakhasiTypeMain.cs", 59);
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		try
		{
			base.objDGrid = (Control)(object)DGrid;
			((FrmExtList)this).OnLoad(e);
			((FrmExtList)this).Add_Click += new CRMEventHandler(BtnAdd_Click);
			((FrmExtList)this).Edit_Click += new CRMEventHandler(BtnEdit_Click);
			((FrmExtList)this).Delete_Click += new CRMEventHandler(BtnDelete_Click);
			((Control)DGrid).Focus();
			if (!UserAttendance.SelectedUser.AccessLevelMerg.Vacation.VacationMain)
			{
				((Form)this).Close();
				return;
			}
			((Control)base.BtnAdd).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Vacation.VacationInsert;
			((Control)base.BtnDelete).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Vacation.VacationDelete;
			((Control)base.BtnEdit).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Vacation.VacationUpdate;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnLoad", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\DefMorakhasiTypeMain.cs", 82);
		}
	}

	protected override void Search()
	{
		try
		{
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource();
			CancellationToken token = _searchToken.Token;
			Task.Run(() => SearchAsync(token));
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Search", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\DefMorakhasiTypeMain.cs", 97);
		}
	}

	private async Task SearchAsync(CancellationToken token)
	{
		try
		{
			if (_dGridLists == null || _dGridLists.Count <= 0)
			{
				_dGridLists = await TypeMorakhasi.GetAllAsync(((CacheBaseForm)this).ContextGuid, token);
			}
			string text = Text.FixString(((Control)base.TxtSearch).Text).Trim();
			if (text == "")
			{
				do
				{
					await Task.Delay(100);
					if (IsDisposed || token.IsCancellationRequested)
					{
						return;
					}
				}
				while (!((Control)this).IsHandleCreated);
				((Control)this).BeginInvoke((Delegate)(Action)delegate
				{
					try
					{
						if (!token.IsCancellationRequested && !IsDisposed)
						{
							typeMorakhasiBindingSource.DataSource = _dGridLists;
							((Control)LblRowCount).Text = "تعداد رکوردها : " + _dGridLists.Count.ToString("N0");
						}
					}
					catch (TaskCanceledException)
					{
					}
					catch (Exception ex5)
					{
						Log.Error(ex5, "");
					}
				});
				return;
			}
			string[] array = text.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			_dGridListsForDisplay = _dGridLists.Select((TypeMorakhasi e1) => e1).ToList();
			string[] array2 = array;
			foreach (string word in array2)
			{
				_dGridListsForDisplay = _dGridListsForDisplay.Where((TypeMorakhasi e1) => e1.Name.Contains(word)).ToList();
			}
			do
			{
				await Task.Delay(100);
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			while (!((Control)this).IsHandleCreated);
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (!token.IsCancellationRequested && !IsDisposed)
					{
						typeMorakhasiBindingSource.DataSource = _dGridListsForDisplay;
						((Control)LblRowCount).Text = "تعداد رکوردها : " + _dGridListsForDisplay.Count.ToString("N0");
					}
				}
				catch (TaskCanceledException)
				{
				}
				catch (Exception ex3)
				{
					Log.Error(ex3, "");
				}
			});
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void BtnAdd_Click(object sender, EventArgs e)
	{
		((Control)DGrid).Focus();
		if (!((Control)base.BtnAdd).Enabled || !((Control)base.BtnAdd).Visible)
		{
			return;
		}
		try
		{
			DefMorakhasiType frm = new DefMorakhasiType(((CacheBaseForm)this).ContextGuid);
			if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				_dGridLists = await TypeMorakhasi.GetAllAsync(((CacheBaseForm)this).ContextGuid);
				((FrmExtList)this).Search();
			}
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAdd_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\DefMorakhasiTypeMain.cs", 171);
		}
	}

	private async void BtnEdit_Click(object sender, EventArgs e)
	{
		((Control)DGrid).Focus();
		if (!((Control)base.BtnEdit).Enabled || !((Control)base.BtnEdit).Visible)
		{
			return;
		}
		try
		{
			if (SelectedItem == null)
			{
				return;
			}
			if (SelectedItem.Id <= 5)
			{
				Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)5, "[مجاز به اصلاح این نوع مرخصی نمیباشید مگر با رمز مدیریت]", "", 3);
				if ((int)(await ((CacheBaseForm)new FrmGetPass(((CacheBaseForm)this).ContextGuid)).ShowAsync((IWin32Window)(object)this)) != 1)
				{
					return;
				}
			}
			DefMorakhasiType frm = new DefMorakhasiType(((CacheBaseForm)this).ContextGuid, SelectedItem);
			if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				_dGridLists = await TypeMorakhasi.GetAllAsync(((CacheBaseForm)this).ContextGuid);
				((FrmExtList)this).Search();
			}
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnEdit_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\DefMorakhasiTypeMain.cs", 197);
		}
	}

	private async void BtnDelete_Click(object sender, EventArgs e)
	{
		_ = 2;
		try
		{
			((Control)DGrid).Focus();
			if (((Control)base.BtnDelete).Enabled && ((Control)base.BtnDelete).Visible && SelectedItem != null && (SelectedItem.Id > 5 || (int)(await ((CacheBaseForm)new FrmGetPass(((CacheBaseForm)this).ContextGuid)).ShowAsync((IWin32Window)(object)this)) == 1) && (int)FMessageBox.Show(Utilities.GetTranslate("[مایل به حذف  نوع مرخصی انتخاب شده هستید ؟]"), Utilities.GetTranslate("[هشدار]"), FMessageBox.FMessageBoxButtons.YesNo, FMessageBox.FMessageBoxIcon.Exclamation, FMessageBox.FMessageBoxDefaultButton.Button2) == 6)
			{
				SelectedItem.ST = false;
				if (await SelectedItem.UpdateAsync())
				{
					_dGridLists = await TypeMorakhasi.GetAllAsync(((CacheBaseForm)this).ContextGuid);
					((FrmExtList)this).Search();
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\DefMorakhasiTypeMain.cs", 223);
		}
	}

	private void typeMorakhasiBindingSource_CurrentChanged(object sender, EventArgs e)
	{
		SelectedItem = (TypeMorakhasi)typeMorakhasiBindingSource.Current;
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_searchToken?.Cancel();
				_searchToken?.Dispose();
				_searchToken = null;
				_loadDataToken?.Cancel();
				_loadDataToken?.Dispose();
				_loadDataToken = null;
				components.Dispose();
				((FrmExtList)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void InitializeComponent()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		//IL_0425: Unknown result type (might be due to invalid IL or missing references)
		//IL_042f: Expected O, but got Unknown
		//IL_07ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f9: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DGrid = new DataGridViewX();
		iDDGrid = new DataGridViewTextBoxColumn();
		nameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		bimehDataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
		ezafatDataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
		kosoratDataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
		hoghoghDataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
		maliyatDataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
		typeMorakhasiBindingSource = new BindingSource(components);
		LblRowCount = new LabelX();
		((Control)base.PanelEx1).SuspendLayout();
		((Control)base.GroupPanel1).SuspendLayout();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)typeMorakhasiBindingSource).BeginInit();
		((Control)this).SuspendLayout();
		((Control)base.PanelEx1).Controls.Add((Control)(object)LblRowCount);
		((Control)base.PanelEx1).Controls.Add((Control)(object)DGrid);
		((Control)base.PanelEx1).Size = new Size(784, 561);
		base.PanelEx1.Style.Alignment = (StringAlignment)1;
		base.PanelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		base.PanelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		base.PanelEx1.Style.Border = (eBorderType)1;
		base.PanelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		base.PanelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		base.PanelEx1.Style.GradientAngle = 90;
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.BtnSelect, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.GroupPanel1, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.BtnEdit, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.BtnAdd, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.BtnDelete, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.BtnOther, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)DGrid, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)LblRowCount, 0);
		((PanelControl)base.GroupPanel1).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)base.GroupPanel1).Style.BackColorGradientAngle = 90;
		((PanelControl)base.GroupPanel1).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)base.GroupPanel1).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)base.GroupPanel1).Style.BorderBottomWidth = 1;
		((PanelControl)base.GroupPanel1).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)base.GroupPanel1).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)base.GroupPanel1).Style.BorderLeftWidth = 1;
		((PanelControl)base.GroupPanel1).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)base.GroupPanel1).Style.BorderRightWidth = 1;
		((PanelControl)base.GroupPanel1).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)base.GroupPanel1).Style.BorderTopWidth = 1;
		((PanelControl)base.GroupPanel1).Style.CornerDiameter = 4;
		((PanelControl)base.GroupPanel1).Style.CornerType = (eCornerType)2;
		((PanelControl)base.GroupPanel1).Style.TextAlignment = (eStyleTextAlignment)2;
		((PanelControl)base.GroupPanel1).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)base.GroupPanel1).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)base.GroupPanel1).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)base.GroupPanel1).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)base.LabelX1).Location = new Point(676, 3);
		base.TxtSearch.Border.Class = "TextBoxBorder";
		base.TxtSearch.Border.CornerType = (eCornerType)1;
		((Control)base.TxtSearch).Size = new Size(664, 21);
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		val.Alignment = (DataGridViewContentAlignment)16;
		val.BackColor = SystemColors.Window;
		val.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		val.ForeColor = SystemColors.ControlText;
		val.SelectionBackColor = SystemColors.Highlight;
		val.SelectionForeColor = SystemColors.ControlText;
		val.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).AlternatingRowsDefaultCellStyle = val;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[7]
		{
			(DataGridViewColumn)iDDGrid,
			(DataGridViewColumn)nameDataGridViewTextBoxColumn,
			(DataGridViewColumn)bimehDataGridViewCheckBoxColumn,
			(DataGridViewColumn)ezafatDataGridViewCheckBoxColumn,
			(DataGridViewColumn)kosoratDataGridViewCheckBoxColumn,
			(DataGridViewColumn)hoghoghDataGridViewCheckBoxColumn,
			(DataGridViewColumn)maliyatDataGridViewCheckBoxColumn
		});
		((DataGridView)DGrid).DataSource = typeMorakhasiBindingSource;
		((DataGridView)DGrid).DefaultCellStyle = val;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(147, 77);
		((DataGridView)DGrid).MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(624, 459);
		((Control)DGrid).TabIndex = 24;
		((DataGridViewColumn)iDDGrid).DataPropertyName = "Id";
		((DataGridViewColumn)iDDGrid).HeaderText = "[شناسه]";
		((DataGridViewColumn)iDDGrid).Name = "iDDGrid";
		((DataGridViewBand)iDDGrid).ReadOnly = true;
		((DataGridViewBand)iDDGrid).Visible = false;
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).DataPropertyName = "Name";
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).HeaderText = "[نام]";
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).MinimumWidth = 150;
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).Name = "nameDataGridViewTextBoxColumn";
		((DataGridViewBand)nameDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)bimehDataGridViewCheckBoxColumn).DataPropertyName = "Bimeh";
		((DataGridViewColumn)bimehDataGridViewCheckBoxColumn).HeaderText = "[بیمه]";
		((DataGridViewColumn)bimehDataGridViewCheckBoxColumn).Name = "bimehDataGridViewCheckBoxColumn";
		((DataGridViewBand)bimehDataGridViewCheckBoxColumn).ReadOnly = true;
		((DataGridViewBand)bimehDataGridViewCheckBoxColumn).Visible = false;
		((DataGridViewColumn)ezafatDataGridViewCheckBoxColumn).DataPropertyName = "Ezafat";
		((DataGridViewColumn)ezafatDataGridViewCheckBoxColumn).HeaderText = "[مزایا]";
		((DataGridViewColumn)ezafatDataGridViewCheckBoxColumn).Name = "ezafatDataGridViewCheckBoxColumn";
		((DataGridViewBand)ezafatDataGridViewCheckBoxColumn).ReadOnly = true;
		((DataGridViewBand)ezafatDataGridViewCheckBoxColumn).Visible = false;
		((DataGridViewColumn)kosoratDataGridViewCheckBoxColumn).DataPropertyName = "Kosorat";
		((DataGridViewColumn)kosoratDataGridViewCheckBoxColumn).HeaderText = "[کسورات]";
		((DataGridViewColumn)kosoratDataGridViewCheckBoxColumn).Name = "kosoratDataGridViewCheckBoxColumn";
		((DataGridViewBand)kosoratDataGridViewCheckBoxColumn).ReadOnly = true;
		((DataGridViewBand)kosoratDataGridViewCheckBoxColumn).Visible = false;
		((DataGridViewColumn)hoghoghDataGridViewCheckBoxColumn).DataPropertyName = "Hoghogh";
		((DataGridViewColumn)hoghoghDataGridViewCheckBoxColumn).HeaderText = "[مشمول حقوق]";
		((DataGridViewColumn)hoghoghDataGridViewCheckBoxColumn).MinimumWidth = 100;
		((DataGridViewColumn)hoghoghDataGridViewCheckBoxColumn).Name = "hoghoghDataGridViewCheckBoxColumn";
		((DataGridViewBand)hoghoghDataGridViewCheckBoxColumn).ReadOnly = true;
		((DataGridViewColumn)maliyatDataGridViewCheckBoxColumn).DataPropertyName = "Maliyat";
		((DataGridViewColumn)maliyatDataGridViewCheckBoxColumn).HeaderText = "[مالیات]";
		((DataGridViewColumn)maliyatDataGridViewCheckBoxColumn).Name = "maliyatDataGridViewCheckBoxColumn";
		((DataGridViewBand)maliyatDataGridViewCheckBoxColumn).ReadOnly = true;
		((DataGridViewBand)maliyatDataGridViewCheckBoxColumn).Visible = false;
		typeMorakhasiBindingSource.DataSource = typeof(TypeMorakhasi);
		typeMorakhasiBindingSource.CurrentChanged += typeMorakhasiBindingSource_CurrentChanged;
		((BaseItemControl)LblRowCount).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblRowCount).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)LblRowCount).Location = new Point(13, 514);
		((Control)LblRowCount).Name = "LblRowCount";
		((Control)LblRowCount).Size = new Size(127, 23);
		((Control)LblRowCount).TabIndex = 25;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((Form)this).ClientSize = new Size(784, 561);
		((Form)this).MaximizeBox = false;
		((Control)this).MaximumSize = new Size(800, 600);
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(800, 600);
		((Control)this).Name = "DefMorakhasiTypeMain";
		((Control)this).Controls.SetChildIndex((Control)(object)base.PanelEx1, 0);
		((Control)base.PanelEx1).ResumeLayout(false);
		((Control)base.PanelEx1).PerformLayout();
		((Control)base.GroupPanel1).ResumeLayout(false);
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)typeMorakhasiBindingSource).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
