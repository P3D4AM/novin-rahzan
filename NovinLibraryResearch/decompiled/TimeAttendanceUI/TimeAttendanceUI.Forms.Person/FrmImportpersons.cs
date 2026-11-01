using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using BehComponents;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using ExcelDataReader;
using Serilog;
using TimeAttendanceDAL.Dal.FLRPerson;

namespace TimeAttendanceUI.Forms.Person;

public class FrmImportpersons : Form
{
	private DataTable excelData;

	private List<string> excelColumns;

	private Dictionary<string, string> mapping = new Dictionary<string, string>();

	private IContainer components;

	private Label lblExcelFile;

	private DataGridViewX dgvMapping;

	private Panel panel1;

	private Label label1;

	private ProgressBar progressBar;

	private ButtonX btnSave;

	private DataGridViewX dgvStatus;

	private ButtonX btnSelectFile;

	private TextBoxX txtFilePath;

	private DataGridViewImageColumn dgServerStatus;

	private DataGridViewCheckBoxColumn DgChecked;

	private DataGridViewTextBoxColumn DGPicPath;

	private DataGridViewTextBoxColumn DGHardwareCode;

	private DataGridViewCheckBoxColumn St;

	private PanelEx panel2;

	public string ContextGuid { get; private set; }

	public FrmImportpersons(string contextGuid)
	{
		ContextGuid = contextGuid;
		InitializeComponent();
	}

	private void btnSelectFile_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Invalid comparison between Unknown and I4
		try
		{
			OpenFileDialog val = new OpenFileDialog
			{
				Filter = "Excel Files|*.xls;*.xlsx"
			};
			if ((int)((CommonDialog)val).ShowDialog() == 1)
			{
				((Control)txtFilePath).Text = ((FileDialog)val).FileName;
				LoadExcelFile(((FileDialog)val).FileName);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void LoadExcelFile(string filePath)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Expected O, but got Unknown
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
		using FileStream fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read);
		IExcelDataReader val = ExcelReaderFactory.CreateReader((Stream)fileStream, (ExcelReaderConfiguration)null);
		try
		{
			DataSet dataSet = ExcelDataReaderExtensions.AsDataSet(val, new ExcelDataSetConfiguration
			{
				ConfigureDataTable = (IExcelDataReader tableReader) => new ExcelDataTableConfiguration
				{
					UseHeaderRow = true
				}
			});
			excelData = dataSet.Tables[0];
			excelColumns = (from DataColumn c in excelData.Columns
				select c.ColumnName).ToList();
			if (excelColumns == null || excelColumns.Count <= 0)
			{
				MessageBoxFarsi.Show("فایل اکسل نامعتبر است\r\n سطر اول بایستی نام ستونها باشد\r\n و حداقل یک ستون برای نام خانوادگی بایستی موجود باشد");
				return;
			}
			((DataGridView)dgvMapping).Columns.Clear();
			((DataGridView)dgvStatus).Columns.Clear();
			SetupMappingGrid();
			FillMappingGrid();
			((Control)panel1).Enabled = true;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void SetupMappingGrid()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		((DataGridView)dgvMapping).Columns.Add("DBField", "Database Field");
		DataGridViewComboBoxColumn val = new DataGridViewComboBoxColumn
		{
			Name = "ExcelColumn",
			HeaderText = "Excel Column"
		};
		((DataGridView)dgvMapping).Columns.Add((DataGridViewColumn)(object)val);
		((DataGridView)dgvMapping).Rows.Add(new object[1] { "Name" });
		((DataGridView)dgvMapping).Rows.Add(new object[1] { "Family" });
		((DataGridView)dgvMapping).Rows.Add(new object[1] { "CodeMeli" });
		((DataGridView)dgvMapping).Rows.Add(new object[1] { "Mob1" });
		((DataGridView)dgvMapping).Rows.Add(new object[1] { "MadrakTahsili" });
		((DataGridView)dgvMapping).Rows.Add(new object[1] { "FatherName" });
		((DataGridView)dgvMapping).Rows.Add(new object[1] { "GroupName" });
		((DataGridView)dgvMapping).Rows.Add(new object[1] { "CodeTafsil" });
		((DataGridView)dgvMapping).Rows.Add(new object[1] { "CodeMoein" });
	}

	private void FillMappingGrid()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		foreach (DataGridViewRow item in (IEnumerable)((DataGridView)dgvMapping).Rows)
		{
			DataGridViewCell obj = item.Cells["ExcelColumn"];
			DataGridViewComboBoxCell val = (DataGridViewComboBoxCell)(object)((obj is DataGridViewComboBoxCell) ? obj : null);
			if (val != null)
			{
				val.Items.Clear();
				ObjectCollection items = val.Items;
				object[] array = excelColumns.ToArray();
				items.AddRange(array);
			}
		}
	}

	private async void btnSave_Click(object sender, EventArgs e)
	{
		_ = 4;
		try
		{
			if (excelData == null || excelData.Rows.Count <= 0)
			{
				MessageBoxFarsi.Show("داده ای برای ذخیره کردن وجود ندارد");
				return;
			}
			mapping.Clear();
			((DataGridView)dgvStatus).Columns.Add("rowNumber", "ردیف");
			((DataGridView)dgvStatus).Columns["rowNumber"].AutoSizeMode = (DataGridViewAutoSizeColumnMode)10;
			((DataGridView)dgvStatus).Columns.Add("status", "ذخیره");
			((DataGridView)dgvStatus).Columns["status"].AutoSizeMode = (DataGridViewAutoSizeColumnMode)10;
			foreach (DataGridViewRow item in (IEnumerable)((DataGridView)dgvMapping).Rows)
			{
				string text = item.Cells["DBField"].Value?.ToString();
				string value = item.Cells["ExcelColumn"].Value?.ToString();
				if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(value))
				{
					mapping.Add(text, value);
					((DataGridView)dgvStatus).Columns.Add(text, text);
					((DataGridView)dgvStatus).Columns[text].AutoSizeMode = (DataGridViewAutoSizeColumnMode)((text == "Name" || text == "Family") ? 16 : 6);
				}
			}
			((DataGridView)dgvStatus).Rows.Clear();
			progressBar.Value = 0;
			new TimeAttendanceDAL.Dal.FLRPerson.Person(ContextGuid);
			int imported = 0;
			object[] rowData = new object[((BaseCollection)((DataGridView)dgvStatus).Columns).Count];
			List<PersonGroup> groupList = await PersonGroup.SelectAllAsync(ContextGuid, containsAllGroups: true, default(CancellationToken));
			for (int i = 0; i < excelData.Rows.Count; i++)
			{
				DataRow dr = excelData.Rows[i];
				rowData[0] = i + 1;
				rowData[1] = "✖";
				int rdIndex = 2;
				TimeAttendanceDAL.Dal.FLRPerson.Person person = await TimeAttendanceDAL.Dal.FLRPerson.Person.FactoryAsync(ContextGuid);
				foreach (KeyValuePair<string, string> item2 in mapping)
				{
					rowData[rdIndex] = dr[item2.Value].ToString();
					rdIndex++;
					PropertyInfo prop = typeof(TimeAttendanceDAL.Dal.FLRPerson.Person).GetProperty(item2.Key);
					if ((object)prop == null && item2.Key == "GroupName")
					{
						prop = typeof(TimeAttendanceDAL.Dal.FLRPerson.Person).GetProperty("GroupId");
					}
					if (!(prop != null))
					{
						continue;
					}
					if (prop.Name == "GroupId")
					{
						string groupName = dr[item2.Value].ToString().Trim();
						if (string.IsNullOrEmpty(groupName))
						{
							groupName = "نامشخص";
						}
						PersonGroup personGroup = groupList.FirstOrDefault((PersonGroup p) => p.Name.Trim() == groupName);
						if (personGroup == null)
						{
							await new PersonGroup(ContextGuid)
							{
								Name = groupName
							}.SaveAsync();
							groupList = await PersonGroup.SelectAllAsync(ContextGuid, containsAllGroups: true, default(CancellationToken));
							personGroup = groupList.FirstOrDefault((PersonGroup p) => p.Name.Trim() == groupName);
						}
						if (personGroup != null)
						{
							prop.SetValue(person, personGroup.Id);
						}
					}
					else if (prop.PropertyType == typeof(DateTime))
					{
						DateTime.TryParse(dr[item2.Value].ToString(), out var result);
						prop.SetValue(person, result);
					}
					else if (prop.PropertyType == typeof(string))
					{
						prop.SetValue(person, dr[item2.Value].ToString());
					}
					else if (prop.PropertyType == typeof(int))
					{
						prop.SetValue(person, ExcelDataConverter.ToInt(dr[item2.Value].ToString()));
					}
					else if (prop.PropertyType == typeof(long))
					{
						prop.SetValue(person, ExcelDataConverter.ToLong(dr[item2.Value].ToString(), 0L));
					}
					else if (prop.PropertyType == typeof(double))
					{
						prop.SetValue(person, ExcelDataConverter.ToDouble(dr[item2.Value].ToString()));
					}
					else if (prop.PropertyType == typeof(decimal))
					{
						prop.SetValue(person, ExcelDataConverter.ToDecimal(dr[item2.Value].ToString()));
					}
					else if (prop.PropertyType == typeof(short))
					{
						prop.SetValue(person, ExcelDataConverter.ToShort(dr[item2.Value].ToString(), 0));
					}
					else if (prop.PropertyType == typeof(bool))
					{
						prop.SetValue(person, ExcelDataConverter.ToBool(dr[item2.Value].ToString()));
					}
				}
				rowData[1] = ((await person.SaveAsync()) ? "✔" : "✖");
				((DataGridView)dgvStatus).Rows.Add(rowData);
				imported++;
				progressBar.Value = imported * 100 / excelData.Rows.Count;
				Application.DoEvents();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		((Control)btnSave).Enabled = false;
	}

	private void dgvMapping_CellEndEdit(object sender, DataGridViewCellEventArgs e)
	{
		((Control)btnSave).Enabled = true;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Expected O, but got Unknown
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Expected O, but got Unknown
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Expected O, but got Unknown
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Expected O, but got Unknown
		//IL_0449: Unknown result type (might be due to invalid IL or missing references)
		//IL_0453: Expected O, but got Unknown
		//IL_06eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f5: Expected O, but got Unknown
		//IL_0abe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac8: Expected O, but got Unknown
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		DataGridViewCellStyle val3 = new DataGridViewCellStyle();
		DataGridViewCellStyle val4 = new DataGridViewCellStyle();
		lblExcelFile = new Label();
		txtFilePath = new TextBoxX();
		dgvMapping = new DataGridViewX();
		panel1 = new Panel();
		dgvStatus = new DataGridViewX();
		label1 = new Label();
		progressBar = new ProgressBar();
		btnSave = new ButtonX();
		btnSelectFile = new ButtonX();
		dgServerStatus = new DataGridViewImageColumn();
		DgChecked = new DataGridViewCheckBoxColumn();
		DGPicPath = new DataGridViewTextBoxColumn();
		DGHardwareCode = new DataGridViewTextBoxColumn();
		St = new DataGridViewCheckBoxColumn();
		panel2 = new PanelEx();
		((ISupportInitialize)dgvMapping).BeginInit();
		((Control)panel1).SuspendLayout();
		((ISupportInitialize)dgvStatus).BeginInit();
		((Control)panel2).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)lblExcelFile).Anchor = (AnchorStyles)9;
		((Control)lblExcelFile).AutoSize = true;
		((Control)lblExcelFile).ForeColor = Color.Blue;
		((Control)lblExcelFile).Location = new Point(489, 6);
		((Control)lblExcelFile).Name = "lblExcelFile";
		((Control)lblExcelFile).RightToLeft = (RightToLeft)1;
		((Control)lblExcelFile).Size = new Size(438, 13);
		((Control)lblExcelFile).TabIndex = 0;
		((Control)lblExcelFile).Text = "سطر اول فایل ،بایستی نام ستون ها باشد و داده ها از سطر دوم شروع شده باشند.";
		((Control)txtFilePath).Anchor = (AnchorStyles)13;
		txtFilePath.Border.Class = "TextBoxBorder";
		txtFilePath.Border.CornerType = (eCornerType)1;
		((Control)txtFilePath).Location = new Point(6, 22);
		((Control)txtFilePath).Name = "txtFilePath";
		((Control)txtFilePath).Size = new Size(918, 21);
		((Control)txtFilePath).TabIndex = 1;
		((Control)dgvMapping).Anchor = (AnchorStyles)9;
		((DataGridView)dgvMapping).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		val.Alignment = (DataGridViewContentAlignment)16;
		val.BackColor = SystemColors.Window;
		val.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		val.ForeColor = SystemColors.ControlText;
		val.SelectionBackColor = SystemColors.Highlight;
		val.SelectionForeColor = SystemColors.ControlText;
		val.WrapMode = (DataGridViewTriState)2;
		((DataGridView)dgvMapping).DefaultCellStyle = val;
		((DataGridView)dgvMapping).GridColor = Color.FromArgb(208, 215, 229);
		((Control)dgvMapping).Location = new Point(811, 3);
		((Control)dgvMapping).Name = "dgvMapping";
		((DataGridView)dgvMapping).RowTemplate.Height = 24;
		((Control)dgvMapping).Size = new Size(298, 495);
		((Control)dgvMapping).TabIndex = 3;
		((DataGridView)dgvMapping).CellEndEdit += new DataGridViewCellEventHandler(dgvMapping_CellEndEdit);
		((Control)panel1).Controls.Add((Control)(object)dgvStatus);
		((Control)panel1).Controls.Add((Control)(object)label1);
		((Control)panel1).Controls.Add((Control)(object)progressBar);
		((Control)panel1).Controls.Add((Control)(object)btnSave);
		((Control)panel1).Controls.Add((Control)(object)dgvMapping);
		((Control)panel1).Enabled = false;
		((Control)panel1).Location = new Point(0, 53);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(1112, 528);
		((Control)panel1).TabIndex = 8;
		((DataGridView)dgvStatus).AllowUserToAddRows = false;
		((DataGridView)dgvStatus).AllowUserToDeleteRows = false;
		((DataGridView)dgvStatus).AllowUserToResizeRows = false;
		val2.BackColor = Color.FromArgb(255, 255, 192);
		((DataGridView)dgvStatus).AlternatingRowsDefaultCellStyle = val2;
		((Control)dgvStatus).Anchor = (AnchorStyles)13;
		((DataGridView)dgvStatus).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		val3.Alignment = (DataGridViewContentAlignment)16;
		val3.BackColor = SystemColors.Window;
		val3.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		val3.ForeColor = SystemColors.ControlText;
		val3.SelectionBackColor = SystemColors.Highlight;
		val3.SelectionForeColor = SystemColors.ControlText;
		val3.WrapMode = (DataGridViewTriState)2;
		((DataGridView)dgvStatus).DefaultCellStyle = val3;
		((DataGridView)dgvStatus).GridColor = Color.FromArgb(208, 215, 229);
		((Control)dgvStatus).Location = new Point(3, 3);
		((DataGridView)dgvStatus).MultiSelect = false;
		((Control)dgvStatus).Name = "dgvStatus";
		((DataGridView)dgvStatus).ReadOnly = true;
		((DataGridView)dgvStatus).RowHeadersVisible = false;
		((DataGridView)dgvStatus).RowTemplate.Height = 24;
		((DataGridView)dgvStatus).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)dgvStatus).Size = new Size(802, 495);
		((Control)dgvStatus).TabIndex = 11;
		((Control)label1).Anchor = (AnchorStyles)10;
		((Control)label1).AutoSize = true;
		((Control)label1).ForeColor = Color.Blue;
		((Control)label1).Location = new Point(495, 506);
		((Control)label1).Name = "label1";
		((Control)label1).RightToLeft = (RightToLeft)1;
		((Control)label1).Size = new Size(607, 13);
		((Control)label1).TabIndex = 10;
		((Control)label1).Text = "ابتدا ستون های فایل اکسل را به ستون های جدول دیتابیس نرم افزار، نگاشت کنید. سپس روی دکمه ذخیره کلیک کنید";
		((Control)progressBar).Anchor = (AnchorStyles)14;
		((Control)progressBar).Location = new Point(3, 499);
		((Control)progressBar).Name = "progressBar";
		((Control)progressBar).Size = new Size(386, 26);
		((Control)progressBar).TabIndex = 9;
		((Control)btnSave).AccessibleRole = (AccessibleRole)43;
		((Control)btnSave).Anchor = (AnchorStyles)10;
		((Control)btnSave).Enabled = false;
		((Control)btnSave).Location = new Point(395, 499);
		((Control)btnSave).Name = "btnSave";
		((Control)btnSave).Size = new Size(94, 26);
		((Control)btnSave).TabIndex = 8;
		((Control)btnSave).Text = "ذخیره";
		((Control)btnSave).Click += btnSave_Click;
		((Control)btnSelectFile).AccessibleRole = (AccessibleRole)43;
		((Control)btnSelectFile).Anchor = (AnchorStyles)9;
		btnSelectFile.ColorTable = (eButtonColor)6;
		((Control)btnSelectFile).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)btnSelectFile).Location = new Point(941, 13);
		((Control)btnSelectFile).Name = "btnSelectFile";
		((Control)btnSelectFile).Size = new Size(168, 30);
		((Control)btnSelectFile).TabIndex = 2;
		((Control)btnSelectFile).Text = "انتخاب فایل اکسل پرسنل";
		((Control)btnSelectFile).Click += btnSelectFile_Click;
		((DataGridViewColumn)dgServerStatus).DataPropertyName = "ServerStatusImage";
		((DataGridViewColumn)dgServerStatus).HeaderText = "";
		((DataGridViewColumn)dgServerStatus).Name = "dgServerStatus";
		((DataGridViewBand)dgServerStatus).ReadOnly = true;
		((DataGridViewColumn)dgServerStatus).Width = 30;
		((DataGridViewColumn)DgChecked).DataPropertyName = "Checked";
		((DataGridViewColumn)DgChecked).HeaderText = "";
		((DataGridViewColumn)DgChecked).Name = "DgChecked";
		((DataGridViewBand)DgChecked).ReadOnly = true;
		DgChecked.ThreeState = true;
		((DataGridViewColumn)DgChecked).Width = 25;
		((DataGridViewColumn)DGPicPath).DataPropertyName = "PicPath";
		((DataGridViewColumn)DGPicPath).HeaderText = "PicPath";
		((DataGridViewColumn)DGPicPath).Name = "DGPicPath";
		((DataGridViewBand)DGPicPath).ReadOnly = true;
		((DataGridViewBand)DGPicPath).Visible = false;
		((DataGridViewColumn)DGHardwareCode).AutoSizeMode = (DataGridViewAutoSizeColumnMode)10;
		val4.NullValue = "...";
		((DataGridViewBand)DGHardwareCode).DefaultCellStyle = val4;
		((DataGridViewColumn)DGHardwareCode).HeaderText = "کد سخت افزاری";
		((DataGridViewColumn)DGHardwareCode).MinimumWidth = 30;
		((DataGridViewColumn)DGHardwareCode).Name = "DGHardwareCode";
		((DataGridViewBand)DGHardwareCode).ReadOnly = true;
		((DataGridViewColumn)St).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)St).DataPropertyName = "St";
		((DataGridViewColumn)St).HeaderText = "فعال";
		((DataGridViewColumn)St).MinimumWidth = 20;
		((DataGridViewColumn)St).Name = "St";
		((DataGridViewBand)St).ReadOnly = true;
		panel2.CanvasColor = SystemColors.Control;
		panel2.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panel2).Controls.Add((Control)(object)panel1);
		((Control)panel2).Controls.Add((Control)(object)btnSelectFile);
		((Control)panel2).Controls.Add((Control)(object)txtFilePath);
		((Control)panel2).Controls.Add((Control)(object)lblExcelFile);
		panel2.DisabledBackColor = Color.Empty;
		((Control)panel2).Dock = (DockStyle)5;
		((Control)panel2).Location = new Point(0, 0);
		((Control)panel2).Name = "panel2";
		((Control)panel2).Size = new Size(1115, 593);
		panel2.Style.Alignment = (StringAlignment)1;
		panel2.Style.BackColor1.Color = Color.LightSteelBlue;
		panel2.Style.BackColor2.Color = Color.LightSteelBlue;
		panel2.Style.Border = (eBorderType)1;
		panel2.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panel2.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panel2.Style.GradientAngle = 90;
		((Control)panel2).TabIndex = 9;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(1115, 593);
		((Control)this).Controls.Add((Control)(object)panel2);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Control)this).Name = "FrmImportpersons";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "دریافت اطلاعات پرسنل از فایل اکسل";
		((ISupportInitialize)dgvMapping).EndInit();
		((Control)panel1).ResumeLayout(false);
		((Control)panel1).PerformLayout();
		((ISupportInitialize)dgvStatus).EndInit();
		((Control)panel2).ResumeLayout(false);
		((Control)panel2).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
