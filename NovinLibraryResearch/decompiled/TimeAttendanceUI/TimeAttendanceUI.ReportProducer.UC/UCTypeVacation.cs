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
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Serilog;
using TimeAttendanceDAL.Dal.Taradod;
using TimeAttendanceDAL.Settings;

namespace TimeAttendanceUI.ReportProducer.UC;

public class UCTypeVacation : UserControl, IHasContext
{
	private bool _isDisposed;

	private CancellationTokenSource _loadDataToken;

	private Font _font;

	private List<TypeMorakhasi> _lstTypeMorakhasis;

	private IContainer components;

	private UCPanel ucPanel1;

	private PanelEx panelEx1;

	internal DataGridViewX DGrid;

	private TextBoxX TxtDesc;

	private LabelX labelX2;

	private BindingSource typeMorakhasiBindingSource;

	private DataGridViewCheckBoxColumn DGCheck;

	private DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;

	public string ContextGuid { get; private set; }

	public bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return ((Control)this).IsDisposed;
			}
			return true;
		}
	}

	private int ID
	{
		get
		{
			if (((DataGridView)DGrid).RowCount <= 0 || ((DataGridView)DGrid).CurrentRow == null)
			{
				return 0;
			}
			return int.Parse(((DataGridView)DGrid)[((DataGridViewBand)idDataGridViewTextBoxColumn).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Value.ToString());
		}
	}

	public List<int> ListID
	{
		get
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			List<int> list = new List<int>();
			try
			{
				if (((DataGridView)DGrid).RowCount > 0)
				{
					for (int i = 0; i < ((DataGridView)DGrid).RowCount; i++)
					{
						if (Convert.ToBoolean(((DataGridViewCell)(DataGridViewCheckBoxCell)((DataGridView)DGrid)[((DataGridViewBand)DGCheck).Index, i]).Value))
						{
							list.Add(int.Parse(((DataGridView)DGrid)[((DataGridViewBand)idDataGridViewTextBoxColumn).Index, i].Value.ToString()));
						}
					}
					if (list.Count > 0)
					{
						return list;
					}
					for (int j = 0; j < ((DataGridView)DGrid).RowCount; j++)
					{
						list.Add(int.Parse(((DataGridView)DGrid)[((DataGridViewBand)idDataGridViewTextBoxColumn).Index, j].Value.ToString()));
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ListID", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCTypeVacation.cs", 118);
			}
			return list;
		}
	}

	public bool EnalbleControl
	{
		get
		{
			return ucPanel1.Expand;
		}
		set
		{
			try
			{
				ucPanel1.Expand = value;
				RefreshForm();
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "EnalbleControl", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCTypeVacation.cs", 147);
			}
		}
	}

	internal void SetContextGuid(string contextGuid)
	{
		ContextGuid = contextGuid;
		_loadDataToken?.Cancel();
		_loadDataToken = new CancellationTokenSource();
		CancellationToken token = _loadDataToken.Token;
		LoadDataAsync(token);
	}

	public UCTypeVacation()
	{
		try
		{
			_font = TimeAttendanceSettings.appFont;
			InitializeComponent();
			((Control)DGrid).Focus();
			Font font = _font;
			if (font != null)
			{
				((Control)DGrid).Font = font;
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async Task LoadDataAsync(CancellationToken token)
	{
		try
		{
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			_lstTypeMorakhasis = await TypeMorakhasi.GetAllAsync(ContextGuid, token);
			TypeMorakhasi item = new TypeMorakhasi(ContextGuid)
			{
				Name = Utilities.GetTranslate("[مرخصی ساعتی]"),
				Hoghogh = true,
				Id = 999
			};
			_lstTypeMorakhasis.Add(item);
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
					if (!IsDisposed && !token.IsCancellationRequested)
					{
						((Control)ucPanel1.panelMain).Controls.Add((Control)(object)panelEx1);
						((Control)panelEx1).Top = 0;
						((Control)TxtDesc).Focus();
						typeMorakhasiBindingSource.DataSource = _lstTypeMorakhasis;
					}
				}
				catch (Exception ex3)
				{
					Log.Error(ex3, "");
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

	private void RefreshForm()
	{
		try
		{
			ucPanel1.HeaderText2 = (EnalbleControl ? "انواع مرخصی" : "انواع مرخصی - غیر فعال");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "RefreshForm", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCTypeVacation.cs", 132);
		}
	}

	private void ucPanel1_Resize(object sender, EventArgs e)
	{
		((Control)this).Size = ((Control)ucPanel1).Size;
		EnalbleControl = EnalbleControl;
	}

	private void DGrid_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		try
		{
			if (e.RowIndex >= 0 && e.ColumnIndex == ((DataGridViewBand)DGCheck).Index && ID >= 0)
			{
				DataGridViewCell currentCell = ((DataGridView)DGrid).CurrentCell;
				DataGridViewCheckBoxCell val = (DataGridViewCheckBoxCell)(object)((currentCell is DataGridViewCheckBoxCell) ? currentCell : null);
				if (val != null)
				{
					((DataGridViewCell)val).Value = !Convert.ToBoolean(((DataGridViewCell)val).Value);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_CellClick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCTypeVacation.cs", 167);
		}
	}

	private void DGrid_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode != 32)
			{
				return;
			}
			e.Handled = true;
			if (((Control)DGrid).Focused && ID >= 0 && ((DataGridView)DGrid).CurrentCell.ColumnIndex != ((DataGridViewBand)DGCheck).Index)
			{
				DataGridViewCell obj = ((DataGridView)DGrid).SelectedRows[0].Cells[((DataGridViewBand)DGCheck).Index];
				DataGridViewCheckBoxCell val = (DataGridViewCheckBoxCell)(object)((obj is DataGridViewCheckBoxCell) ? obj : null);
				if (val != null)
				{
					((DataGridViewCell)val).Value = !Convert.ToBoolean(((DataGridViewCell)val).Value);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCTypeVacation.cs", 184);
		}
	}

	private void TxtDesc_TextChanged(object sender, EventArgs e)
	{
		try
		{
			Search();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtDesc_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCTypeVacation.cs", 196);
		}
	}

	private void TxtPerson_Enter(object sender, EventArgs e)
	{
		((Control)TxtDesc).BackColor = Color.Yellow;
	}

	private void TxtPerson_Leave(object sender, EventArgs e)
	{
		((Control)TxtDesc).BackColor = Color.White;
	}

	private void Search()
	{
		try
		{
			string text = Text.FixString(((Control)TxtDesc).Text).Trim();
			if (text == "")
			{
				typeMorakhasiBindingSource.DataSource = _lstTypeMorakhasis;
				return;
			}
			string[] array = text.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			List<TypeMorakhasi> list = _lstTypeMorakhasis.ToList();
			string[] array2 = array;
			foreach (string word in array2)
			{
				list = list.Where((TypeMorakhasi e1) => e1.Name.Contains(word)).ToList();
			}
			typeMorakhasiBindingSource.DataSource = list;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Search", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCTypeVacation.cs", 225);
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
				components?.Dispose();
				((ContainerControl)this).Dispose(disposing);
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
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		//IL_044d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0457: Expected O, but got Unknown
		//IL_0530: Unknown result type (might be due to invalid IL or missing references)
		//IL_053a: Expected O, but got Unknown
		//IL_0547: Unknown result type (might be due to invalid IL or missing references)
		//IL_0551: Expected O, but got Unknown
		//IL_0679: Unknown result type (might be due to invalid IL or missing references)
		//IL_0683: Expected O, but got Unknown
		//IL_0752: Unknown result type (might be due to invalid IL or missing references)
		//IL_075c: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		panelEx1 = new PanelEx();
		labelX2 = new LabelX();
		TxtDesc = new TextBoxX();
		DGrid = new DataGridViewX();
		DGCheck = new DataGridViewCheckBoxColumn();
		idDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		nameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		typeMorakhasiBindingSource = new BindingSource(components);
		ucPanel1 = new UCPanel();
		((Control)panelEx1).SuspendLayout();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)typeMorakhasiBindingSource).BeginInit();
		((Control)this).SuspendLayout();
		((Control)panelEx1).Anchor = (AnchorStyles)15;
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)labelX2);
		((Control)panelEx1).Controls.Add((Control)(object)TxtDesc);
		((Control)panelEx1).Controls.Add((Control)(object)DGrid);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Location = new Point(2, 30);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).RightToLeft = (RightToLeft)1;
		((Control)panelEx1).Size = new Size(595, 267);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.Color = Color.FromArgb(142, 179, 231);
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 2;
		((Control)labelX2).Anchor = (AnchorStyles)9;
		((Control)labelX2).BackColor = Color.Transparent;
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Location = new Point(442, 6);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).RightToLeft = (RightToLeft)1;
		((Control)labelX2).Size = new Size(150, 21);
		((Control)labelX2).TabIndex = 9;
		((Control)labelX2).Text = "[شرح]";
		labelX2.TextAlignment = (StringAlignment)2;
		((Control)TxtDesc).Anchor = (AnchorStyles)13;
		TxtDesc.Border.Class = "TextBoxBorder";
		TxtDesc.Border.CornerType = (eCornerType)1;
		((Control)TxtDesc).Location = new Point(6, 8);
		((Control)TxtDesc).Name = "TxtDesc";
		((Control)TxtDesc).Size = new Size(417, 21);
		((Control)TxtDesc).TabIndex = 8;
		((Control)TxtDesc).TextChanged += TxtDesc_TextChanged;
		((Control)TxtDesc).Enter += TxtPerson_Enter;
		((Control)TxtDesc).Leave += TxtPerson_Leave;
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		val.BackColor = Color.FromArgb(255, 255, 192);
		((DataGridView)DGrid).AlternatingRowsDefaultCellStyle = val;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[3]
		{
			(DataGridViewColumn)DGCheck,
			(DataGridViewColumn)idDataGridViewTextBoxColumn,
			(DataGridViewColumn)nameDataGridViewTextBoxColumn
		});
		((DataGridView)DGrid).DataSource = typeMorakhasiBindingSource;
		val2.Alignment = (DataGridViewContentAlignment)16;
		val2.BackColor = SystemColors.Window;
		val2.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		val2.ForeColor = SystemColors.ControlText;
		val2.SelectionBackColor = SystemColors.Highlight;
		val2.SelectionForeColor = SystemColors.ControlText;
		val2.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).DefaultCellStyle = val2;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(6, 37);
		((DataGridView)DGrid).MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(586, 227);
		((Control)DGrid).TabIndex = 6;
		((DataGridView)DGrid).CellClick += new DataGridViewCellEventHandler(DGrid_CellClick);
		((Control)DGrid).KeyDown += new KeyEventHandler(DGrid_KeyDown);
		((DataGridViewColumn)DGCheck).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGCheck).HeaderText = "";
		((DataGridViewColumn)DGCheck).MinimumWidth = 30;
		((DataGridViewColumn)DGCheck).Name = "DGCheck";
		((DataGridViewBand)DGCheck).ReadOnly = true;
		((DataGridViewColumn)DGCheck).Width = 30;
		((DataGridViewColumn)idDataGridViewTextBoxColumn).DataPropertyName = "Id";
		((DataGridViewColumn)idDataGridViewTextBoxColumn).HeaderText = "Id";
		((DataGridViewColumn)idDataGridViewTextBoxColumn).Name = "idDataGridViewTextBoxColumn";
		((DataGridViewBand)idDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)idDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).DataPropertyName = "Name";
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).HeaderText = "[شرح]";
		((DataGridViewColumn)nameDataGridViewTextBoxColumn).Name = "nameDataGridViewTextBoxColumn";
		((DataGridViewBand)nameDataGridViewTextBoxColumn).ReadOnly = true;
		typeMorakhasiBindingSource.DataSource = typeof(TypeMorakhasi);
		((Control)ucPanel1).Anchor = (AnchorStyles)15;
		ucPanel1.Expand = true;
		((Control)ucPanel1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		ucPanel1.HeaderText = "";
		ucPanel1.HeaderText2 = "";
		((Control)ucPanel1).Location = new Point(0, 0);
		((Control)ucPanel1).Name = "ucPanel1";
		((Control)ucPanel1).Size = new Size(600, 300);
		((Control)ucPanel1).TabIndex = 1;
		((Control)ucPanel1).Resize += ucPanel1_Resize;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Controls.Add((Control)(object)ucPanel1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)this).MaximumSize = new Size(600, 300);
		((Control)this).MinimumSize = new Size(600, 10);
		((Control)this).Name = "UCTypeVacation";
		((Control)this).Size = new Size(600, 300);
		((Control)panelEx1).ResumeLayout(false);
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)typeMorakhasiBindingSource).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
