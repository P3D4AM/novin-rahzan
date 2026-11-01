using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using EntityCache.Business.Attendance;
using FormCustom;
using FormCustom.Cache;
using FormCustom.UC;
using PacketParser;
using PacketParser.BusinessLayer.Models.Attendance;
using PacketParser.Entities;
using TimeAttendanceDAL.Settings;

namespace TimeAttendanceUI.Forms.Service;

public class frmBioDeviceRelation : AccountingBaseForm
{
	private bool _isDisposed;

	private IEnumerable<BioDeviceRelationModel> _list;

	private IEnumerable<BioDeviceRelationModel> _filteredList;

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _searchToken;

	private CancellationTokenSource _initToken;

	private IContainer components;

	private DataGridView DGrid;

	private BindingSource DeviceRelationBindingSource;

	private UcSearchBox txtSearch;

	private DataGridViewTextBoxColumn personNameDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn hardwareCodeDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn privilageForDisplayDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn modifiedForDisplayDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn FingerPrintCount;

	private DataGridViewCheckBoxColumn HasPalm;

	private DataGridViewCheckBoxColumn HasFace;

	private DataGridViewCheckBoxColumn HasCard;

	private DataGridViewCheckBoxColumn HasPassword;

	private DataGridViewTextBoxColumn deviceGuidDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn bioGuidDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn modifiedDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn privilageDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn deviceNameDataGridViewTextBoxColumn;

	public Guid DeviceGuid { get; private set; }

	public string SearchString { get; private set; }

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

	private async Task InitializeAsync(CancellationToken token)
	{
		try
		{
			TimeAttendanceSettings setting = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(((CacheBaseForm)this).ContextGuid, token);
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
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
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
				try
				{
					FormExtentions.Initialize(DGrid, setting.FontProgram, true);
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "InitializeAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\frmBioDeviceRelation.cs", 46);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitializeAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\frmBioDeviceRelation.cs", 49);
		}
	}

	private async Task LoadDataAsync(CancellationToken token)
	{
		try
		{
			_list = await BioDeviceRelationBusiness.GetAllAsync(((CacheBaseForm)this).ContextGuid, DeviceGuid, token, "");
			if (!IsDisposed && !token.IsCancellationRequested)
			{
				_searchToken?.Cancel();
				_searchToken = new CancellationTokenSource(60000);
				CancellationToken seToken = CancellationTokenSource.CreateLinkedTokenSource(_searchToken.Token, token).Token;
				Task.Run(() => SearchAsync(seToken));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\frmBioDeviceRelation.cs", 63);
		}
	}

	private async Task SearchAsync(CancellationToken token)
	{
		try
		{
			_filteredList = _list;
			if (token.IsCancellationRequested || IsDisposed)
			{
				return;
			}
			List<string> list = Text.SplitString(SearchString, " /\\\r\n\t)(-_=+%$#@*|><,`~'\".?؟،;:");
			if (list != null && list.Count > 0)
			{
				foreach (string item in list)
				{
					if (token.IsCancellationRequested || IsDisposed)
					{
						return;
					}
					if (!string.IsNullOrEmpty(item))
					{
						_filteredList = _filteredList?.Where((BioDeviceRelationModel x) => Text.StringComparisons(x.DeviceName, item, StringComparison.OrdinalIgnoreCase) || Text.StringComparisons(x.PersonName, item, StringComparison.OrdinalIgnoreCase) || Text.StringComparisons(x.PrivilageForDisplay, item, StringComparison.OrdinalIgnoreCase) || Text.StringComparisons(x.HardwareCode, item, StringComparison.OrdinalIgnoreCase));
					}
				}
			}
			if (token.IsCancellationRequested || IsDisposed)
			{
				return;
			}
			while (!((Control)this).IsHandleCreated)
			{
				await Task.Delay(100);
				if (token.IsCancellationRequested || IsDisposed)
				{
					return;
				}
			}
			if (token.IsCancellationRequested || IsDisposed)
			{
				return;
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				if (token.IsCancellationRequested || IsDisposed)
				{
					return;
				}
				try
				{
					if (!token.IsCancellationRequested && !IsDisposed)
					{
						BindingSource deviceRelationBindingSource = DeviceRelationBindingSource;
						IEnumerable<BioDeviceRelationModel> filteredList = _filteredList;
						object dataSource;
						if (filteredList == null)
						{
							dataSource = null;
						}
						else
						{
							IOrderedEnumerable<BioDeviceRelationModel> orderedEnumerable = filteredList.OrderByDescending((BioDeviceRelationModel q) => q.Modified);
							dataSource = ((orderedEnumerable != null) ? Utilities.ToSortableBindingList<BioDeviceRelationModel>((IEnumerable<BioDeviceRelationModel>)orderedEnumerable, token) : null);
						}
						deviceRelationBindingSource.DataSource = dataSource;
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "SearchAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\frmBioDeviceRelation.cs", 97);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SearchAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\frmBioDeviceRelation.cs", 100);
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
				_initToken?.Cancel();
				_initToken?.Dispose();
				_initToken = null;
				_searchToken?.Cancel();
				_searchToken?.Dispose();
				_searchToken = null;
				components?.Dispose();
				((BaseForm)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\frmBioDeviceRelation.cs", 124);
		}
	}

	public frmBioDeviceRelation(string contextGuid, Guid deviceGuid)
		: base(contextGuid)
	{
		frmBioDeviceRelation frmBioDeviceRelation2 = this;
		try
		{
			InitializeComponent();
			DeviceGuid = deviceGuid;
			_initToken = new CancellationTokenSource(60000);
			CancellationToken initToken = _initToken.Token;
			Task.Run(() => frmBioDeviceRelation2.InitializeAsync(initToken));
			_loadDataToken = new CancellationTokenSource(60000);
			CancellationToken token = _loadDataToken.Token;
			Task.Run(() => frmBioDeviceRelation2.LoadDataAsync(token));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\frmBioDeviceRelation.cs", 143);
		}
	}

	private void frmBioWriteDeviceLog_FormClosed(object sender, FormClosedEventArgs e)
	{
		((Component)(object)this).Dispose(disposing: true);
	}

	private void frmBioWriteDeviceLog_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode == 27)
			{
				if (!string.IsNullOrEmpty(((Control)txtSearch).Text))
				{
					((Control)txtSearch).Text = "";
					return;
				}
				((Form)this).DialogResult = (DialogResult)2;
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "frmBioWriteDeviceLog_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\frmBioDeviceRelation.cs", 164);
		}
	}

	private void txtSearch_OnKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode == 40 || (int)e.KeyCode == 13)
			{
				((Control)DGrid).Focus();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "txtSearch_OnKeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\frmBioDeviceRelation.cs", 173);
		}
	}

	private void txtSearch_OnTextChanged(object sender, EventArgs e)
	{
		try
		{
			SearchString = ((Control)txtSearch).Text;
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource(60000);
			CancellationToken token = _searchToken.Token;
			Task.Run(() => SearchAsync(token));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "txtSearch_OnTextChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\frmBioDeviceRelation.cs", 185);
		}
	}

	private void DGrid_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			if (Text.IsStringOrNumber(e.KeyChar))
			{
				((Control)txtSearch).Focus();
				((Control)txtSearch).Text = e.KeyChar.ToString();
				txtSearch.SelectionStart = 9999;
				txtSearch.SelectionLength = 0;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DGrid_KeyPress", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\frmBioDeviceRelation.cs", 197);
		}
	}

	private void InitializeComponent()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
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
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Expected O, but got Unknown
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Expected O, but got Unknown
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Expected O, but got Unknown
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Expected O, but got Unknown
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Expected O, but got Unknown
		//IL_0374: Unknown result type (might be due to invalid IL or missing references)
		//IL_037e: Expected O, but got Unknown
		//IL_0830: Unknown result type (might be due to invalid IL or missing references)
		//IL_083a: Expected O, but got Unknown
		//IL_083f: Unknown result type (might be due to invalid IL or missing references)
		//IL_088a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0894: Expected O, but got Unknown
		//IL_089c: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a6: Expected O, but got Unknown
		components = new Container();
		DGrid = new DataGridView();
		DeviceRelationBindingSource = new BindingSource(components);
		txtSearch = new UcSearchBox();
		personNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		hardwareCodeDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		privilageForDisplayDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		modifiedForDisplayDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		FingerPrintCount = new DataGridViewTextBoxColumn();
		HasPalm = new DataGridViewCheckBoxColumn();
		HasFace = new DataGridViewCheckBoxColumn();
		HasCard = new DataGridViewCheckBoxColumn();
		HasPassword = new DataGridViewCheckBoxColumn();
		deviceGuidDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		bioGuidDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		modifiedDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		privilageDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		deviceNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)DeviceRelationBindingSource).BeginInit();
		((Control)this).SuspendLayout();
		DGrid.AllowUserToAddRows = false;
		DGrid.AllowUserToDeleteRows = false;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		DGrid.AutoGenerateColumns = false;
		DGrid.BackgroundColor = SystemColors.ControlLightLight;
		DGrid.ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		DGrid.Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[14]
		{
			(DataGridViewColumn)personNameDataGridViewTextBoxColumn,
			(DataGridViewColumn)hardwareCodeDataGridViewTextBoxColumn,
			(DataGridViewColumn)privilageForDisplayDataGridViewTextBoxColumn,
			(DataGridViewColumn)modifiedForDisplayDataGridViewTextBoxColumn,
			(DataGridViewColumn)FingerPrintCount,
			(DataGridViewColumn)HasPalm,
			(DataGridViewColumn)HasFace,
			(DataGridViewColumn)HasCard,
			(DataGridViewColumn)HasPassword,
			(DataGridViewColumn)deviceGuidDataGridViewTextBoxColumn,
			(DataGridViewColumn)bioGuidDataGridViewTextBoxColumn,
			(DataGridViewColumn)modifiedDataGridViewTextBoxColumn,
			(DataGridViewColumn)privilageDataGridViewTextBoxColumn,
			(DataGridViewColumn)deviceNameDataGridViewTextBoxColumn
		});
		DGrid.DataSource = DeviceRelationBindingSource;
		((Control)DGrid).Location = new Point(0, 56);
		DGrid.MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		DGrid.ReadOnly = true;
		DGrid.RowHeadersWidth = 62;
		DGrid.SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(784, 502);
		((Control)DGrid).TabIndex = 22;
		((Control)DGrid).KeyPress += new KeyPressEventHandler(DGrid_KeyPress);
		DeviceRelationBindingSource.DataSource = typeof(BioDeviceRelationModel);
		((Control)txtSearch).Anchor = (AnchorStyles)13;
		((Control)txtSearch).BackColor = Color.Transparent;
		((Control)txtSearch).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		txtSearch.IsSaerchBox = true;
		((Control)txtSearch).Location = new Point(67, 8);
		((Control)txtSearch).Margin = new Padding(4, 5, 4, 5);
		((Control)txtSearch).Name = "txtSearch";
		((Control)txtSearch).RightToLeft = (RightToLeft)1;
		txtSearch.SelectionLength = 0;
		txtSearch.SelectionStart = 0;
		((Control)txtSearch).Size = new Size(614, 40);
		((Control)txtSearch).TabIndex = 23;
		txtSearch.OnTextChanged += new EventHandler(txtSearch_OnTextChanged);
		txtSearch.OnKeyDown += new KeyEventHandler(txtSearch_OnKeyDown);
		((DataGridViewColumn)personNameDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)personNameDataGridViewTextBoxColumn).DataPropertyName = "PersonName";
		((DataGridViewColumn)personNameDataGridViewTextBoxColumn).HeaderText = "[شخص]";
		((DataGridViewColumn)personNameDataGridViewTextBoxColumn).Name = "personNameDataGridViewTextBoxColumn";
		((DataGridViewBand)personNameDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)hardwareCodeDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)hardwareCodeDataGridViewTextBoxColumn).DataPropertyName = "HardwareCode";
		((DataGridViewColumn)hardwareCodeDataGridViewTextBoxColumn).HeaderText = "[کد سخت افزاری]";
		((DataGridViewColumn)hardwareCodeDataGridViewTextBoxColumn).Name = "hardwareCodeDataGridViewTextBoxColumn";
		((DataGridViewBand)hardwareCodeDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)hardwareCodeDataGridViewTextBoxColumn).Width = 123;
		((DataGridViewColumn)privilageForDisplayDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)privilageForDisplayDataGridViewTextBoxColumn).DataPropertyName = "PrivilageForDisplay";
		((DataGridViewColumn)privilageForDisplayDataGridViewTextBoxColumn).HeaderText = "[دسترسی]";
		((DataGridViewColumn)privilageForDisplayDataGridViewTextBoxColumn).Name = "privilageForDisplayDataGridViewTextBoxColumn";
		((DataGridViewBand)privilageForDisplayDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)privilageForDisplayDataGridViewTextBoxColumn).Width = 87;
		((DataGridViewColumn)modifiedForDisplayDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)modifiedForDisplayDataGridViewTextBoxColumn).DataPropertyName = "ModifiedForDisplay";
		((DataGridViewColumn)modifiedForDisplayDataGridViewTextBoxColumn).HeaderText = "[آخرین بروزرسانی]";
		((DataGridViewColumn)modifiedForDisplayDataGridViewTextBoxColumn).Name = "modifiedForDisplayDataGridViewTextBoxColumn";
		((DataGridViewBand)modifiedForDisplayDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)FingerPrintCount).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)FingerPrintCount).DataPropertyName = "FingerPrintCount";
		((DataGridViewColumn)FingerPrintCount).HeaderText = "[تعداد انگشت]";
		((DataGridViewColumn)FingerPrintCount).Name = "FingerPrintCount";
		((DataGridViewBand)FingerPrintCount).ReadOnly = true;
		((DataGridViewColumn)FingerPrintCount).Width = 109;
		((DataGridViewColumn)HasPalm).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)HasPalm).DataPropertyName = "HasPalm";
		((DataGridViewColumn)HasPalm).HeaderText = "[کف دست]";
		((DataGridViewColumn)HasPalm).Name = "HasPalm";
		((DataGridViewBand)HasPalm).ReadOnly = true;
		((DataGridViewColumn)HasPalm).Width = 69;
		((DataGridViewColumn)HasFace).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)HasFace).DataPropertyName = "HasFace";
		((DataGridViewColumn)HasFace).HeaderText = "[چهره]";
		((DataGridViewColumn)HasFace).Name = "HasFace";
		((DataGridViewBand)HasFace).ReadOnly = true;
		((DataGridViewColumn)HasFace).Width = 48;
		((DataGridViewColumn)HasCard).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)HasCard).DataPropertyName = "HasCard";
		((DataGridViewColumn)HasCard).HeaderText = "[کارت]";
		((DataGridViewColumn)HasCard).Name = "HasCard";
		((DataGridViewBand)HasCard).ReadOnly = true;
		((DataGridViewColumn)HasCard).Width = 49;
		((DataGridViewColumn)HasPassword).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)HasPassword).DataPropertyName = "HasPassword";
		((DataGridViewColumn)HasPassword).HeaderText = "[رمز]";
		((DataGridViewColumn)HasPassword).Name = "HasPassword";
		((DataGridViewBand)HasPassword).ReadOnly = true;
		((DataGridViewColumn)HasPassword).Width = 42;
		((DataGridViewColumn)deviceGuidDataGridViewTextBoxColumn).DataPropertyName = "DeviceGuid";
		((DataGridViewColumn)deviceGuidDataGridViewTextBoxColumn).HeaderText = "DeviceGuid";
		((DataGridViewColumn)deviceGuidDataGridViewTextBoxColumn).Name = "deviceGuidDataGridViewTextBoxColumn";
		((DataGridViewBand)deviceGuidDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)deviceGuidDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)bioGuidDataGridViewTextBoxColumn).DataPropertyName = "BioGuid";
		((DataGridViewColumn)bioGuidDataGridViewTextBoxColumn).HeaderText = "BioGuid";
		((DataGridViewColumn)bioGuidDataGridViewTextBoxColumn).Name = "bioGuidDataGridViewTextBoxColumn";
		((DataGridViewBand)bioGuidDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)bioGuidDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)modifiedDataGridViewTextBoxColumn).DataPropertyName = "Modified";
		((DataGridViewColumn)modifiedDataGridViewTextBoxColumn).HeaderText = "Modified";
		((DataGridViewColumn)modifiedDataGridViewTextBoxColumn).Name = "modifiedDataGridViewTextBoxColumn";
		((DataGridViewBand)modifiedDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)modifiedDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)privilageDataGridViewTextBoxColumn).DataPropertyName = "Privilage";
		((DataGridViewColumn)privilageDataGridViewTextBoxColumn).HeaderText = "Privilage";
		((DataGridViewColumn)privilageDataGridViewTextBoxColumn).Name = "privilageDataGridViewTextBoxColumn";
		((DataGridViewBand)privilageDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)privilageDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)deviceNameDataGridViewTextBoxColumn).DataPropertyName = "DeviceName";
		((DataGridViewColumn)deviceNameDataGridViewTextBoxColumn).HeaderText = "DeviceName";
		((DataGridViewColumn)deviceNameDataGridViewTextBoxColumn).Name = "deviceNameDataGridViewTextBoxColumn";
		((DataGridViewBand)deviceNameDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)deviceNameDataGridViewTextBoxColumn).Visible = false;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(784, 561);
		((Control)this).Controls.Add((Control)(object)txtSearch);
		((Control)this).Controls.Add((Control)(object)DGrid);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Form)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).MinimumSize = new Size(800, 600);
		((Control)this).Name = "frmBioDeviceRelation";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[سابقه اطلاعات دستگاه]";
		((Form)this).WindowState = (FormWindowState)2;
		((Form)this).FormClosed += new FormClosedEventHandler(frmBioWriteDeviceLog_FormClosed);
		((Control)this).KeyDown += new KeyEventHandler(frmBioWriteDeviceLog_KeyDown);
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)DeviceRelationBindingSource).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
