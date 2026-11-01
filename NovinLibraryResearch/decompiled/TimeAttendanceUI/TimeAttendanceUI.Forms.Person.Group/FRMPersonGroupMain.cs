using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using FormCustom.Waiting;
using PacketParser;
using PacketParser.Entities;
using Serilog;
using TimeAttendanceDAL.Dal.FLRPerson;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.Utility;
using TimeAttendanceDAL.Utility.FarsiMessageBox;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Forms.Person.Group;

public class FRMPersonGroupMain : FrmExtList
{
	private PersonGroup _cls;

	private List<PersonGroup> _listData;

	private SortableBindingList<PersonGroup> _listForDisplay;

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _searchToken;

	private string _searchString = "";

	private bool _isDisposed;

	private IContainer components;

	private LabelX LblRowCount;

	private DataGridViewX DGrid;

	private DataGridViewTextBoxColumn DGID;

	private DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;

	private BindingSource persongroupBindingSource;

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
			if (((DataGridView)DGrid).Rows.Count == 0 || ((DataGridView)DGrid).CurrentRow == null)
			{
				return 0;
			}
			return short.Parse(((DataGridView)DGrid)[((DataGridViewBand)DGID).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Value.ToString());
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

	public FRMPersonGroupMain(string contextGuid, Font font, bool btnSelect = true)
		: base(contextGuid)
	{
		try
		{
			InitializeComponent();
			try
			{
				((Control)base.BtnSelect).Visible = btnSelect;
				((Control)DGrid).Focus();
				((Control)DGrid).Font = font;
				((Control)this).Text = "لیست گروه کارکنان";
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\Group\\FRMPersonGroupMain.cs", 70);
			}
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\Group\\FRMPersonGroupMain.cs", 75);
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
			new Waiter("جستجو در لیست", (Form)(object)this, SearchAsync(token), false, (EnWaiterType)0, (Control)null, "Search", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\Group\\FRMPersonGroupMain.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Search", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\Group\\FRMPersonGroupMain.cs", 100);
		}
	}

	private async Task SearchAsync(CancellationToken token)
	{
		try
		{
			if (_listData == null)
			{
				_listData = await PersonGroup.SelectAllAsync(((CacheBaseForm)this).ContextGuid, containsAllGroups: false, token);
			}
			if (_listData != null)
			{
				_listForDisplay = null;
			}
			if (string.IsNullOrEmpty(_searchString))
			{
				FRMPersonGroupMain fRMPersonGroupMain = this;
				List<PersonGroup> listData = _listData;
				fRMPersonGroupMain._listForDisplay = ((listData != null) ? Utilities.ToSortableBindingList<PersonGroup>((IEnumerable<PersonGroup>)listData, token) : null);
			}
			else
			{
				IEnumerable<PersonGroup> enumerable = _listData;
				foreach (string word in Text.SplitString(_searchString, " /\\\r\n\t)(-_=+%$#@*|><,`~'\".?؟،;:"))
				{
					if (token.IsCancellationRequested)
					{
						return;
					}
					enumerable = enumerable.Where((PersonGroup p) => p.Name.Contains(word));
				}
				_listForDisplay = ((enumerable != null) ? Utilities.ToSortableBindingList<PersonGroup>(enumerable, token) : null);
			}
			FRMPersonGroupMain fRMPersonGroupMain2 = this;
			SortableBindingList<PersonGroup> listForDisplay = _listForDisplay;
			fRMPersonGroupMain2._listForDisplay = ((listForDisplay != null) ? Utilities.ToSortableBindingList<PersonGroup>((IEnumerable<PersonGroup>)((IEnumerable<PersonGroup>)listForDisplay).OrderBy((PersonGroup p) => p.Name), default(CancellationToken)) : null);
			string countRecordTr = "تعداد ردیف";
			int count = ((Collection<PersonGroup>)(object)_listForDisplay).Count;
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
						persongroupBindingSource.DataSource = _listForDisplay;
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
				if (!UserAttendance.SelectedUser.AccessLevelMerg.Personnel.CanInsertPersonGroup(UserAttendance.SelectedUser.AccessLevelMerg, persongroupBindingSource.Count + 1))
				{
					Utility.MessageBoxShowError("با توجه به سطح دسترسی نرم افزار، شما اجازه ندارید گروه پرسنلی ایجاد نمائید", ((CacheBaseForm)this).ContextGuid);
				}
				else if ((int)(await ((CacheBaseForm)new FRMPersonGroup(((CacheBaseForm)this).ContextGuid)).ShowAsync((IWin32Window)(object)this)) == 1)
				{
					_listData = null;
					((FrmExtList)this).Search();
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAdd_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\Group\\FRMPersonGroupMain.cs", 168);
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
			if (ID > 0 && (int)(await ((CacheBaseForm)new FRMPersonGroup(((CacheBaseForm)this).ContextGuid, ID)).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				_listData = null;
				((FrmExtList)this).Search();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnEdit_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\Group\\FRMPersonGroupMain.cs", 185);
		}
	}

	private async void BtnDelete_Click(object sender, EventArgs e)
	{
		if (!((Control)base.BtnDelete).Visible || !((Control)base.BtnDelete).Enabled)
		{
			return;
		}
		try
		{
			((Control)DGrid).Focus();
			if (ID <= 0)
			{
				return;
			}
			if (((DataGridView)DGrid).RowCount == 1)
			{
				Utility.MessageBoxShowError("حداقل يك گروه  بايد در سيستم وجود داشته باشد", ((CacheBaseForm)this).ContextGuid);
				return;
			}
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			int num = await PersonGroup.CountGroupIDAsync(((CacheBaseForm)this).ContextGuid, ID, token);
			if (num > 0)
			{
				Utility.MessageBoxShow("به تعداد " + num + " شخص دراین گروه وجود دارد ، جهت حذف گروه ابتدا باید گروه این اشخاص را تغییر دهید سپس اقدام به حذف نمایید", ((CacheBaseForm)this).ContextGuid);
			}
			else if ((int)FMessageBox.Show("براي حذف کردن مطمئن هستيد؟", "هشدار", FMessageBox.FMessageBoxButtons.YesNo, FMessageBox.FMessageBoxIcon.Error, FMessageBox.FMessageBoxDefaultButton.Button2) != 7)
			{
				_cls = await PersonGroup.GetAsync(((CacheBaseForm)this).ContextGuid, ID, token);
				_cls.St = false;
				if (await _cls.SaveAsync(token))
				{
					_listData = null;
					((FrmExtList)this).Search();
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\Group\\FRMPersonGroupMain.cs", 219);
		}
	}

	private void FRMPersonGroupMain_Load(object sender, EventArgs e)
	{
		((FrmExtList)this).Search();
	}

	private void InitializeComponent()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_056e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0578: Expected O, but got Unknown
		//IL_060d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0617: Expected O, but got Unknown
		//IL_06c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ca: Expected O, but got Unknown
		//IL_0738: Unknown result type (might be due to invalid IL or missing references)
		//IL_0742: Expected O, but got Unknown
		//IL_07dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e6: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		DataGridViewCellStyle val3 = new DataGridViewCellStyle();
		LblRowCount = new LabelX();
		DGrid = new DataGridViewX();
		DGID = new DataGridViewTextBoxColumn();
		nameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		persongroupBindingSource = new BindingSource(components);
		((Control)base.PanelEx1).SuspendLayout();
		((Control)base.GroupPanel1).SuspendLayout();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)persongroupBindingSource).BeginInit();
		((Control)this).SuspendLayout();
		((Control)base.PanelEx1).Controls.Add((Control)(object)LblRowCount);
		((Control)base.PanelEx1).Controls.Add((Control)(object)DGrid);
		((Control)base.PanelEx1).Size = new Size(534, 362);
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
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)DGrid, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)LblRowCount, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.BtnSelect, 0);
		((Control)base.GroupPanel1).Size = new Size(508, 60);
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
		((Control)base.LabelX1).Location = new Point(426, 3);
		((Control)base.LabelX1).RightToLeft = (RightToLeft)1;
		((Control)base.LabelX1).Text = "[جستجو] :";
		base.TxtSearch.Border.Class = "TextBoxBorder";
		base.TxtSearch.Border.CornerType = (eCornerType)1;
		((Control)base.TxtSearch).Size = new Size(414, 21);
		((Control)base.BtnDelete).Location = new Point(14, 201);
		((Control)base.BtnDelete).RightToLeft = (RightToLeft)1;
		((Control)base.BtnDelete).Size = new Size(126, 36);
		((Control)base.BtnDelete).Text = "   حذف";
		base.BtnDelete.TextAlignment = (eButtonTextAlignment)0;
		((Control)base.BtnDelete).Click += BtnDelete_Click;
		((Control)base.BtnAdd).Location = new Point(14, 121);
		((Control)base.BtnAdd).RightToLeft = (RightToLeft)1;
		((Control)base.BtnAdd).Size = new Size(126, 36);
		((Control)base.BtnAdd).Text = "   اضافه";
		base.BtnAdd.TextAlignment = (eButtonTextAlignment)0;
		((Control)base.BtnAdd).Click += BtnAdd_Click;
		((Control)base.BtnEdit).Location = new Point(14, 161);
		((Control)base.BtnEdit).Size = new Size(126, 36);
		((Control)base.BtnEdit).Text = "   ویرایش";
		base.BtnEdit.TextAlignment = (eButtonTextAlignment)0;
		((Control)base.BtnEdit).Click += BtnEdit_Click;
		base.BtnSelect.Image = (Image)(object)Resources.accept1;
		base.BtnSelect.ImageFixedSize = new Size(25, 25);
		base.BtnSelect.ImagePosition = (eImagePosition)1;
		((Control)base.BtnSelect).Location = new Point(14, 81);
		((Control)base.BtnSelect).RightToLeft = (RightToLeft)1;
		base.BtnSelect.Shape = (ShapeDescriptor)new RoundRectangleShapeDescriptor(2);
		((Control)base.BtnSelect).Size = new Size(126, 36);
		((Control)base.BtnSelect).Text = "   انتخاب";
		base.BtnSelect.TextAlignment = (eButtonTextAlignment)0;
		((Control)base.BtnSelect).Visible = true;
		((Control)base.BtnOther).Location = new Point(14, 241);
		((Control)base.BtnOther).Size = new Size(126, 36);
		((Control)LblRowCount).Anchor = (AnchorStyles)6;
		((BaseItemControl)LblRowCount).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblRowCount).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)LblRowCount).Location = new Point(13, 314);
		((Control)LblRowCount).Name = "LblRowCount";
		((Control)LblRowCount).RightToLeft = (RightToLeft)1;
		((Control)LblRowCount).Size = new Size(127, 23);
		((Control)LblRowCount).TabIndex = 28;
		((Control)LblRowCount).Text = "---";
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
		((Control)DGrid).Anchor = (AnchorStyles)13;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		val2.Alignment = (DataGridViewContentAlignment)32;
		val2.BackColor = SystemColors.Control;
		val2.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		val2.ForeColor = SystemColors.WindowText;
		val2.SelectionBackColor = SystemColors.Highlight;
		val2.SelectionForeColor = SystemColors.HighlightText;
		val2.WrapMode = (DataGridViewTriState)1;
		((DataGridView)DGrid).ColumnHeadersDefaultCellStyle = val2;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[2]
		{
			(DataGridViewColumn)DGID,
			(DataGridViewColumn)nameDataGridViewTextBoxColumn
		});
		((DataGridView)DGrid).DataSource = persongroupBindingSource;
		val3.Alignment = (DataGridViewContentAlignment)16;
		val3.BackColor = SystemColors.Window;
		val3.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		val3.ForeColor = SystemColors.ControlText;
		val3.SelectionBackColor = SystemColors.Highlight;
		val3.SelectionForeColor = SystemColors.ControlText;
		val3.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).DefaultCellStyle = val3;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(147, 77);
		((DataGridView)DGrid).MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((Control)DGrid).RightToLeft = (RightToLeft)1;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(374, 260);
		((Control)DGrid).TabIndex = 27;
		((DataGridViewColumn)DGID).DataPropertyName = "Id";
		((DataGridViewColumn)DGID).HeaderText = "Id";
		((DataGridViewColumn)DGID).Name = "DGID";
		((DataGridViewBand)DGID).ReadOnly = true;
		((DataGridViewBand)DGID).Visible = false;
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).DataPropertyName = "Name";
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).HeaderText = "عنوان";
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).MinimumWidth = 150;
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).Name = "nameDataGridViewTextBoxColumn";
		((DataGridViewBand)nameDataGridViewTextBoxColumn).ReadOnly = true;
		persongroupBindingSource.DataSource = typeof(PersonGroup);
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(534, 362);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FRMPersonGroupMain";
		((Control)this).Text = "FRMPersonGroupMain";
		((Form)this).Load += FRMPersonGroupMain_Load;
		((Control)this).Controls.SetChildIndex((Control)(object)base.PanelEx1, 0);
		((Control)base.PanelEx1).ResumeLayout(false);
		((Control)base.PanelEx1).PerformLayout();
		((Control)base.GroupPanel1).ResumeLayout(false);
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)persongroupBindingSource).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
