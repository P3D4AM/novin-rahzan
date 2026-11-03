using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FormCustom.Cache;
using FormCustom.LockService.Class;
using FormCustom.UC;
using PacketParser;
using PacketParser.Entities;
using PacketParser.LkService;

namespace FormCustom.LockService.Forms;

public class frmPackageResult : AccountingBaseForm
{
	private bool _isDisposed;

	private CancellationTokenSource _setDataToken;

	private IContainer components;

	private BindingSource AccountingPackageBindingSource;

	internal DataGridView DGrid;

	private DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;

	private Label lblTitle;

	private UcButtons btnCancel;

	private DataGridViewImageColumn Column1;

	private DataGridViewTextBoxColumn Ordinal;

	private DataGridViewTextBoxColumn facilityForDisplayDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn facilityDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn activationCodeDataGridViewTextBoxColumn;

	public new bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return base.IsDisposed;
			}
			return true;
		}
	}

	private async Task SetDataAsync(Level_Accounting level, List<PackageContainer> list, CancellationToken token)
	{
		try
		{
			Font _font = new Font(new FontFamily("B Titr"), "11.25".ParseTofloat(0f, 3), (FontStyle)1);
			List<PackageContainerForDisplay> data = list?.Select((PackageContainer q) => new PackageContainerForDisplay(q))?.OrderBy((PackageContainerForDisplay q) => q.FacilityForDisplay)?.ToList();
			short num = 0;
			if (data?.Any() ?? false)
			{
				using List<PackageContainerForDisplay>.Enumerator enumerator = data.GetEnumerator();
				while (enumerator.MoveNext())
				{
					num = (enumerator.Current.Ordinal = (short)(num + 1));
				}
			}
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
					DGrid.Initialize(_font);
					AccountingPackageBindingSource.DataSource = data;
					switch (level)
					{
					case Level_Accounting.ForoshgahiSade:
						((Control)lblTitle).Text = "فعال سازی نرم افزار حسابداری نسخه ساده با موفقیت انجام شد";
						break;
					case Level_Accounting.ForoshgahiMotavaset:
						((Control)lblTitle).Text = "فعال سازی نرم افزار حسابداری نسخه متوسط با موفقیت انجام شد";
						break;
					case Level_Accounting.ForoshgahiPishrafteh:
						((Control)lblTitle).Text = "فعال سازی نرم افزار حسابداری نسخه پیشرفته با موفقیت انجام شد";
						break;
					case Level_Accounting.Jame:
						((Control)lblTitle).Text = "فعال سازی نرم افزار حسابداری نسخه جامع با موفقیت انجام شد";
						break;
					default:
						((Control)lblTitle).Text = "خطا در فعالسازی نرم افزار";
						break;
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "SetDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmPackageResult.cs", 62);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmPackageResult.cs", 65);
		}
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_setDataToken?.Cancel();
				_setDataToken?.Dispose();
				_setDataToken = null;
				components?.Dispose();
				base.Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmPackageResult.cs", 81);
		}
	}

	public frmPackageResult(string contextGuid, Level_Accounting level, List<PackageContainer> list)
		: base(contextGuid)
	{
		frmPackageResult frmPackageResult2 = this;
		try
		{
			InitializeComponent();
			_setDataToken = new CancellationTokenSource(60000);
			CancellationToken token = _setDataToken.Token;
			Task.Run(() => frmPackageResult2.SetDataAsync(level, list, token));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmPackageResult.cs", 94);
		}
	}

	private void frmPackageResult_FormClosed(object sender, FormClosedEventArgs e)
	{
		((Component)(object)this).Dispose(disposing: true);
	}

	private void frmPackageResult_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode == 27)
			{
				btnCancel.PerformClick();
			}
			else if ((int)e.KeyCode == 13)
			{
				e.Handled = true;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "frmPackageResult_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmPackageResult.cs", 105);
		}
	}

	private void btnCancel_OnClick(object sender, EventArgs e)
	{
		try
		{
			((Form)this).DialogResult = (DialogResult)1;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btnCancel_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmPackageResult.cs", 114);
		}
	}

	private void InitializeComponent()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
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
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Expected O, but got Unknown
		//IL_0409: Unknown result type (might be due to invalid IL or missing references)
		//IL_0413: Expected O, but got Unknown
		//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0500: Expected O, but got Unknown
		//IL_0542: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ac: Expected O, but got Unknown
		//IL_065f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0669: Expected O, but got Unknown
		//IL_067c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0686: Expected O, but got Unknown
		//IL_0692: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e0: Expected O, but got Unknown
		//IL_06e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f2: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(frmPackageResult));
		AccountingPackageBindingSource = new BindingSource(components);
		DGrid = new DataGridView();
		Column1 = new DataGridViewImageColumn();
		Ordinal = new DataGridViewTextBoxColumn();
		facilityForDisplayDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		facilityDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		activationCodeDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		lblTitle = new Label();
		btnCancel = new UcButtons();
		((ISupportInitialize)AccountingPackageBindingSource).BeginInit();
		((ISupportInitialize)DGrid).BeginInit();
		((Control)this).SuspendLayout();
		AccountingPackageBindingSource.DataSource = typeof(PackageContainerForDisplay);
		DGrid.AllowUserToAddRows = false;
		DGrid.AllowUserToDeleteRows = false;
		DGrid.AllowUserToResizeRows = false;
		DGrid.AutoGenerateColumns = false;
		DGrid.ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		DGrid.Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[5]
		{
			(DataGridViewColumn)Column1,
			(DataGridViewColumn)Ordinal,
			(DataGridViewColumn)facilityForDisplayDataGridViewTextBoxColumn,
			(DataGridViewColumn)facilityDataGridViewTextBoxColumn,
			(DataGridViewColumn)activationCodeDataGridViewTextBoxColumn
		});
		DGrid.DataSource = AccountingPackageBindingSource;
		val.Alignment = (DataGridViewContentAlignment)16;
		val.BackColor = SystemColors.Window;
		val.Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		val.ForeColor = SystemColors.ControlText;
		val.SelectionBackColor = SystemColors.Highlight;
		val.SelectionForeColor = SystemColors.ControlText;
		val.WrapMode = (DataGridViewTriState)2;
		DGrid.DefaultCellStyle = val;
		DGrid.GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(2, 72);
		DGrid.MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		DGrid.ReadOnly = true;
		((Control)DGrid).RightToLeft = (RightToLeft)1;
		DGrid.RowHeadersVisible = false;
		DGrid.RowTemplate.Height = 29;
		DGrid.SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(531, 486);
		((Control)DGrid).TabIndex = 2;
		((DataGridViewColumn)Column1).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)Column1).DataPropertyName = "Image";
		((DataGridViewColumn)Column1).HeaderText = "";
		Column1.ImageLayout = (DataGridViewImageCellLayout)3;
		((DataGridViewColumn)Column1).Name = "Column1";
		((DataGridViewBand)Column1).ReadOnly = true;
		((DataGridViewColumn)Column1).Width = 5;
		((DataGridViewColumn)Ordinal).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)Ordinal).DataPropertyName = "Ordinal";
		((DataGridViewColumn)Ordinal).HeaderText = "[ردیف]";
		((DataGridViewColumn)Ordinal).Name = "Ordinal";
		((DataGridViewBand)Ordinal).ReadOnly = true;
		((DataGridViewColumn)Ordinal).Width = 60;
		((DataGridViewColumn)facilityForDisplayDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)facilityForDisplayDataGridViewTextBoxColumn).DataPropertyName = "FacilityForDisplay";
		((DataGridViewColumn)facilityForDisplayDataGridViewTextBoxColumn).HeaderText = "[عنوان زیر بسته]";
		((DataGridViewColumn)facilityForDisplayDataGridViewTextBoxColumn).Name = "facilityForDisplayDataGridViewTextBoxColumn";
		((DataGridViewBand)facilityForDisplayDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)facilityDataGridViewTextBoxColumn).DataPropertyName = "Facility";
		((DataGridViewColumn)facilityDataGridViewTextBoxColumn).HeaderText = "Facility";
		((DataGridViewColumn)facilityDataGridViewTextBoxColumn).Name = "facilityDataGridViewTextBoxColumn";
		((DataGridViewBand)facilityDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)facilityDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)activationCodeDataGridViewTextBoxColumn).DataPropertyName = "ActivationCode";
		((DataGridViewColumn)activationCodeDataGridViewTextBoxColumn).HeaderText = "ActivationCode";
		((DataGridViewColumn)activationCodeDataGridViewTextBoxColumn).Name = "activationCodeDataGridViewTextBoxColumn";
		((DataGridViewBand)activationCodeDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)activationCodeDataGridViewTextBoxColumn).Visible = false;
		((Control)lblTitle).Font = new Font("B Titr", 14.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)lblTitle).ForeColor = Color.Red;
		((Control)lblTitle).Location = new Point(34, 9);
		((Control)lblTitle).Name = "lblTitle";
		((Control)lblTitle).Size = new Size(472, 51);
		((Control)lblTitle).TabIndex = 3;
		((Control)lblTitle).Text = "---";
		lblTitle.TextAlign = (ContentAlignment)32;
		((Control)btnCancel).Anchor = (AnchorStyles)14;
		((Control)btnCancel).BackColor = Color.FromArgb(249, 250, 255);
		btnCancel.CornerType = EnButtonCornerType.Round;
		((Control)btnCancel).Cursor = Cursors.Hand;
		btnCancel.DisplayText = "[بستن و ادامه] (Esc)";
		((Control)btnCancel).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnCancel.Guid = new Guid("00000000-0000-0000-0000-000000000000");
		btnCancel.IsShowMoreDetails = false;
		((Control)btnCancel).Location = new Point(41, 564);
		((Control)btnCancel).Margin = new Padding(4, 5, 4, 5);
		((Control)btnCancel).Name = "btnCancel";
		((Control)btnCancel).RightToLeft = (RightToLeft)1;
		((Control)btnCancel).Size = new Size(447, 41);
		((Control)btnCancel).TabIndex = 4;
		btnCancel.TitleFont = new Font("B Yekan", 14.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		btnCancel.Type = EnButtonType.Normal;
		btnCancel.OnClick += btnCancel_OnClick;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.FromArgb(249, 250, 255);
		((Form)this).ClientSize = new Size(535, 611);
		((Control)this).Controls.Add((Control)(object)btnCancel);
		((Control)this).Controls.Add((Control)(object)lblTitle);
		((Control)this).Controls.Add((Control)(object)DGrid);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).KeyPreview = true;
		((Form)this).Margin = new Padding(4, 5, 4, 5);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "frmPackageResult";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[نتیجه فعالسازی قفل]";
		((Form)this).FormClosed += new FormClosedEventHandler(frmPackageResult_FormClosed);
		((Control)this).KeyDown += new KeyEventHandler(frmPackageResult_KeyDown);
		((ISupportInitialize)AccountingPackageBindingSource).EndInit();
		((ISupportInitialize)DGrid).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
