using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom;
using FormCustom.Cache;
using FormCustom.Waiting;
using PacketParser;
using PacketParser.Entities;
using Serilog;
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceDAL.Settings;
using TimeAttendanceDAL.Utility;

namespace TimeAttendanceUI.Forms.Gharardads;

public class FrmGroupSanavatList : FrmExtList
{
	private List<GroupSanavat> _listData;

	private SortableBindingList<GroupSanavat> _listForDisplay;

	private Dictionary<string, string> trDictionary = new Dictionary<string, string>();

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _searchToken;

	private string _searchString = "";

	private bool _isDisposed;

	private IContainer components;

	private DataGridViewX DGrid;

	private BindingSource groupSanavatBindingSource;

	private LabelX LblRowCount;

	private DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn descDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn iDDGrid;

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

	public short ID
	{
		get
		{
			if (((DataGridView)DGrid).Rows.Count == 0 || ((DataGridView)DGrid).CurrentRow == null || iDDGrid == null)
			{
				return 0;
			}
			return short.Parse(((DataGridView)DGrid)[((DataGridViewBand)iDDGrid).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Value.ToString());
		}
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
				((FrmExtList)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void SetTrDictionary()
	{
		try
		{
			if (trDictionary == null)
			{
				trDictionary = new Dictionary<string, string>();
			}
			if (!trDictionary.TryGetValue("[گروه های سنواتی]", out var value))
			{
				trDictionary.Add("[گروه های سنواتی]", Utilities.GetTranslate("[گروه های سنواتی]"));
			}
			if (!trDictionary.TryGetValue("[انتخاب گروه های سنواتی]", out value))
			{
				trDictionary.Add("[انتخاب گروه های سنواتی]", Utilities.GetTranslate("[انتخاب گروه های سنواتی]"));
			}
			if (!trDictionary.TryGetValue("[تعداد رکوردها]", out value))
			{
				trDictionary.Add("[تعداد رکوردها]", Utilities.GetTranslate("[تعداد رکوردها]"));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetTrDictionary", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmGroupSanavatList.cs", 62);
		}
	}

	public FrmGroupSanavatList(string contextGuid)
		: base(contextGuid)
	{
		try
		{
			SetTrDictionary();
			InitializeComponent();
			((Control)DGrid).Focus();
			((Control)this).Text = "گروه های سنواتی";
			if (trDictionary.TryGetValue("[گروه های سنواتی]", out var value))
			{
				((Control)this).Text = value;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmGroupSanavatList.cs", 79);
		}
	}

	public FrmGroupSanavatList(string contextGuid, bool pIsSelect)
		: base(contextGuid)
	{
		try
		{
			SetTrDictionary();
			InitializeComponent();
			((Control)DGrid).Focus();
			((Control)base.BtnSelect).Visible = pIsSelect;
			((Control)this).Text = "انتخاب گروه های سنواتی";
			if (trDictionary.TryGetValue("[انتخاب گروه های سنواتی]", out var value))
			{
				((Control)this).Text = value;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmGroupSanavatList.cs", 94);
		}
	}

	protected override async void OnLoad(EventArgs e)
	{
		try
		{
			base.objDGrid = (Control)(object)DGrid;
			_003C_003En__0(e);
			((FrmExtList)this).Add_Click += new CRMEventHandler(BtnAdd_Click);
			((FrmExtList)this).Edit_Click += new CRMEventHandler(BtnEdit_Click);
			((FrmExtList)this).Delete_Click += new CRMEventHandler(BtnDelete_Click);
			DataGridViewX dGrid = DGrid;
			((Control)dGrid).Font = (await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(((CacheBaseForm)this).ContextGuid)).FontProgram;
			((Control)DGrid).Focus();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnLoad", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmGroupSanavatList.cs", 120);
		}
	}

	protected override void Search()
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			_searchString = Text.FixString(((Control)base.TxtSearch).Text).Trim();
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource();
			CancellationToken token = _searchToken.Token;
			new Waiter("جستجو در لیست", (Form)(object)this, SearchAsync(token), false, (EnWaiterType)0, (Control)null, "Search", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmGroupSanavatList.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Search", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmGroupSanavatList.cs", 136);
		}
	}

	private async Task SearchAsync(CancellationToken token)
	{
		try
		{
			if (_listData == null)
			{
				_listData = await GroupSanavat.SelectAllAsync(((CacheBaseForm)this).ContextGuid, "", token);
			}
			_listForDisplay = null;
			if (string.IsNullOrEmpty(_searchString))
			{
				FrmGroupSanavatList frmGroupSanavatList = this;
				List<GroupSanavat> listData = _listData;
				frmGroupSanavatList._listForDisplay = ((listData != null) ? Utilities.ToSortableBindingList<GroupSanavat>((IEnumerable<GroupSanavat>)listData, token) : null);
			}
			else
			{
				IEnumerable<GroupSanavat> enumerable = _listData;
				foreach (string word in Text.SplitString(_searchString, " /\\\r\n\t)(-_=+%$#@*|><,`~'\".?؟،;:"))
				{
					if (token.IsCancellationRequested)
					{
						return;
					}
					enumerable = enumerable.Where((GroupSanavat p) => p.Name.Contains(word) || p.Desc.Contains(word));
				}
				_listForDisplay = Utilities.ToSortableBindingList<GroupSanavat>(enumerable, token);
			}
			string countRecordTr = "تعداد ردیف";
			int count = ((Collection<GroupSanavat>)(object)_listForDisplay).Count;
			countRecordTr = Utilities.GetTranslate("[تعداد رکوردها]");
			while (!((Control)this).IsHandleCreated)
			{
				await Task.Delay(100);
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (!IsDisposed && !token.IsCancellationRequested)
					{
						groupSanavatBindingSource.DataSource = _listForDisplay;
						((Control)LblRowCount).Text = countRecordTr + " : " + count.ToString("N0");
					}
				}
				catch (OperationCanceledException)
				{
				}
				catch (Exception ex4)
				{
					Log.Error(ex4, "");
				}
			});
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
		}
	}

	private async void BtnAdd_Click(object sender, EventArgs e)
	{
		try
		{
			if (((Control)base.BtnAdd).Visible && ((Control)base.BtnAdd).Enabled)
			{
				((Control)DGrid).Focus();
				if ((int)(await ((CacheBaseForm)new FrmGroupSanavat(((CacheBaseForm)this).ContextGuid)).ShowAsync((IWin32Window)(object)this)) == 1)
				{
					_listData = null;
					((FrmExtList)this).Search();
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAdd_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmGroupSanavatList.cs", 202);
		}
	}

	private async void BtnEdit_Click(object sender, EventArgs e)
	{
		if (!((Control)base.BtnEdit).Visible || !((Control)base.BtnEdit).Enabled)
		{
			return;
		}
		try
		{
			((Control)DGrid).Focus();
			if (ID > 0 && (int)(await ((CacheBaseForm)new FrmGroupSanavat(((CacheBaseForm)this).ContextGuid, ID)).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				_listData = null;
				((FrmExtList)this).Search();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnEdit_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmGroupSanavatList.cs", 219);
		}
	}

	private void BtnDelete_Click(object sender, EventArgs e)
	{
		try
		{
			Utility.MessageBoxShowError("[امکان حذف آیتم وجود ندارد]", ((CacheBaseForm)this).ContextGuid);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmGroupSanavatList.cs", 232);
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
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		//IL_0430: Unknown result type (might be due to invalid IL or missing references)
		//IL_043a: Expected O, but got Unknown
		//IL_06b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bf: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DGrid = new DataGridViewX();
		nameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		descDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		iDDGrid = new DataGridViewTextBoxColumn();
		groupSanavatBindingSource = new BindingSource(components);
		LblRowCount = new LabelX();
		((Control)base.PanelEx1).SuspendLayout();
		((Control)base.GroupPanel1).SuspendLayout();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)groupSanavatBindingSource).BeginInit();
		((Control)this).SuspendLayout();
		((Control)base.PanelEx1).Controls.Add((Control)(object)LblRowCount);
		((Control)base.PanelEx1).Controls.Add((Control)(object)DGrid);
		((Control)base.PanelEx1).Size = new Size(800, 450);
		base.PanelEx1.Style.Alignment = (StringAlignment)1;
		base.PanelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		base.PanelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		base.PanelEx1.Style.Border = (eBorderType)1;
		base.PanelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		base.PanelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		base.PanelEx1.Style.GradientAngle = 90;
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.GroupPanel1, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.BtnEdit, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.BtnAdd, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.BtnDelete, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.BtnOther, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.BtnSelect, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)DGrid, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)LblRowCount, 0);
		((Control)base.GroupPanel1).Size = new Size(787, 60);
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
		((Control)base.LabelX1).Location = new Point(705, 3);
		((Control)base.LabelX1).Text = "[جستجو]:";
		base.TxtSearch.Border.Class = "TextBoxBorder";
		base.TxtSearch.Border.CornerType = (eCornerType)1;
		((Control)base.TxtSearch).Size = new Size(693, 21);
		((Control)base.BtnSelect).Text = "[انتخاب](Enter)";
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
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[3]
		{
			(DataGridViewColumn)nameDataGridViewTextBoxColumn,
			(DataGridViewColumn)descDataGridViewTextBoxColumn,
			(DataGridViewColumn)iDDGrid
		});
		((DataGridView)DGrid).DataSource = groupSanavatBindingSource;
		((DataGridView)DGrid).DefaultCellStyle = val;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(147, 77);
		((DataGridView)DGrid).MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(640, 348);
		((Control)DGrid).TabIndex = 24;
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).DataPropertyName = "Name";
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).HeaderText = "[عنوان]";
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).MinimumWidth = 100;
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).Name = "nameDataGridViewTextBoxColumn";
		((DataGridViewBand)nameDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)descDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)descDataGridViewTextBoxColumn).DataPropertyName = "Desc";
		((DataGridViewColumn)descDataGridViewTextBoxColumn).HeaderText = "[شرح]";
		((DataGridViewColumn)descDataGridViewTextBoxColumn).MinimumWidth = 100;
		((DataGridViewColumn)descDataGridViewTextBoxColumn).Name = "descDataGridViewTextBoxColumn";
		((DataGridViewBand)descDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)iDDGrid).DataPropertyName = "ID";
		((DataGridViewColumn)iDDGrid).HeaderText = "Id";
		((DataGridViewColumn)iDDGrid).Name = "iDDGrid";
		((DataGridViewBand)iDDGrid).ReadOnly = true;
		((DataGridViewBand)iDDGrid).Visible = false;
		groupSanavatBindingSource.DataSource = typeof(GroupSanavat);
		((Control)LblRowCount).Anchor = (AnchorStyles)6;
		((BaseItemControl)LblRowCount).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblRowCount).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)LblRowCount).Location = new Point(13, 402);
		((Control)LblRowCount).Name = "LblRowCount";
		((Control)LblRowCount).Size = new Size(127, 23);
		((Control)LblRowCount).TabIndex = 25;
		((Control)LblRowCount).Text = "labelX2";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(800, 450);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FrmGroupSanavatList";
		((Control)this).Text = "[گروه های سنواتی]*";
		((Control)this).Controls.SetChildIndex((Control)(object)base.PanelEx1, 0);
		((Control)base.PanelEx1).ResumeLayout(false);
		((Control)base.PanelEx1).PerformLayout();
		((Control)base.GroupPanel1).ResumeLayout(false);
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)groupSanavatBindingSource).EndInit();
		((Control)this).ResumeLayout(false);
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private void _003C_003En__0(EventArgs e)
	{
		((FrmExtList)this).OnLoad(e);
	}
}
