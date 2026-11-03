using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom.Properties;
using PacketParser;
using PacketParser.Entities;

namespace FormCustom;

public class FrmExtList : BaseForm
{
	public delegate void CRMEventHandler(object sender, EventArgs e);

	private bool _isDisposed;

	protected Control objDGrid;

	public ContextMenuStrip contextMenuStrip1;

	public ToolStripMenuItem ToolItemSelect;

	public ToolStripMenuItem ToolItemModify;

	public ToolStripMenuItem ToolItemAdd;

	public ToolStripMenuItem ToolItemDelete;

	public ToolStripMenuItem ToolItemCancelSearch;

	protected StatusStrip statusStrip1;

	protected ToolStripStatusLabel StatusLabelIns;

	protected ToolStripStatusLabel StatusLabelModiFy;

	protected ToolStripStatusLabel StatusLabelDel;

	protected ToolStripStatusLabel StatusLabel2;

	protected ToolStripStatusLabel StatusLabel1;

	protected ToolStripStatusLabel StatusLabelMessage;

	protected PanelEx PanelEx1;

	protected GroupPanel GroupPanel1;

	protected Label LabelX1;

	protected TextBoxX TxtSearch;

	protected ButtonX BtnDelete;

	protected ButtonX BtnAdd;

	protected ButtonX BtnEdit;

	protected ButtonX BtnSelect;

	protected ButtonX BtnOther;

	private IContainer components;

	public new bool IsDisposed
	{
		get
		{
			if (!base.IsDisposed)
			{
				return _isDisposed;
			}
			return true;
		}
	}

	public event CRMEventHandler Add_Click;

	public event CRMEventHandler Edit_Click;

	public event CRMEventHandler Delete_Click;

	public event CRMEventHandler Other_Click;

	public event CRMEventHandler Select_Click;

	public FrmExtList(string context)
		: base(context)
	{
		try
		{
			BaseInitializeComponent();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmExtList.cs", 28);
		}
	}

	protected virtual void Search()
	{
	}

	protected override void OnLoad(EventArgs e)
	{
		try
		{
			base.OnLoad(e);
			if (objDGrid != null)
			{
				objDGrid.DoubleClick += DoubleClickInGrid;
				objDGrid.ContextMenuStrip = contextMenuStrip1;
			}
			_ = new CancellationTokenSource(60000).Token;
			SetLables();
			((ToolStripItem)ToolItemSelect).Visible = ((Control)BtnSelect).Visible;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnLoad", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmExtList.cs", 70);
		}
	}

	private void txtSearch_TextChanged(object sender, EventArgs e)
	{
		try
		{
			Search();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "txtSearch_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmExtList.cs", 81);
		}
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Invalid comparison between Unknown and I4
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Invalid comparison between Unknown and I4
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Invalid comparison between Unknown and I4
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Invalid comparison between Unknown and I4
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Invalid comparison between Unknown and I4
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Invalid comparison between Unknown and I4
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Invalid comparison between Unknown and I4
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Invalid comparison between Unknown and I4
		try
		{
			base.OnKeyDown(e);
			string name = ((ContainerControl)this).ActiveControl.Name;
			if ((int)e.KeyData == 27)
			{
				if (((Control)TxtSearch).Text != "")
				{
					((Control)TxtSearch).Text = "";
				}
				else
				{
					((Form)this).Close();
				}
			}
			else if (!(name == "TxtSearch"))
			{
				if (!(name == "DGrid"))
				{
					return;
				}
				Keys keyCode = e.KeyCode;
				if ((int)keyCode <= 45)
				{
					if ((int)keyCode != 13)
					{
						if ((int)keyCode == 45)
						{
							e.Handled = true;
							e.SuppressKeyPress = true;
							BtnAdd.PerformClick();
						}
					}
					else
					{
						e.Handled = true;
						e.SuppressKeyPress = true;
						if (((Control)BtnSelect).Visible)
						{
							BtnSelect.PerformClick();
						}
						else
						{
							BtnEdit.PerformClick();
						}
					}
				}
				else if ((int)keyCode != 46)
				{
					if ((int)keyCode == 118)
					{
						e.Handled = true;
						e.SuppressKeyPress = true;
						BtnEdit.PerformClick();
					}
				}
				else
				{
					e.Handled = true;
					e.SuppressKeyPress = true;
					BtnDelete.PerformClick();
				}
			}
			else
			{
				Keys keyCode = e.KeyData;
				if (((int)keyCode == 38 || (int)keyCode == 40) ? true : false)
				{
					objDGrid.Select();
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnKeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmExtList.cs", 135);
		}
	}

	protected override void OnKeyPress(KeyPressEventArgs e)
	{
		try
		{
			if (((ContainerControl)this).ActiveControl.Name == "DGrid" && Text.IsStringOrNumber(e.KeyChar))
			{
				((Control)TxtSearch).Text = e.KeyChar.ToString();
				((TextBoxBase)TxtSearch).SelectionStart = ((Control)TxtSearch).Text.Length;
				((Control)TxtSearch).Select();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnKeyPress", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmExtList.cs", 155);
		}
	}

	private void ToolItemCancelSearch_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)TxtSearch).Text = "";
			Search();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ToolItemCancelSearch_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmExtList.cs", 167);
		}
	}

	protected void BtnSelect_Click(object sender, EventArgs e)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (this.Select_Click == null)
			{
				DataGridView val = (DataGridView)objDGrid;
				if ((int)val != 0 && val.RowCount <= 0)
				{
					MessageBox.Show((IWin32Window)(object)this, "[NoItemForSelect]".GetTranslate(), "[Warning]".GetTranslate(), (MessageBoxButtons)0, (MessageBoxIcon)64, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288);
					return;
				}
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
			}
			else
			{
				this.Select_Click(sender, e);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BtnSelect_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmExtList.cs", 189);
		}
	}

	protected void DoubleClickInGrid(object sender, EventArgs e)
	{
		try
		{
			if (((ContainerControl)this).ActiveControl.Name == "DGrid")
			{
				if (((Control)BtnSelect).Visible)
				{
					BtnSelect.PerformClick();
				}
				else
				{
					BtnEdit.PerformClick();
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DoubleClickInGrid", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmExtList.cs", 207);
		}
	}

	private void BtnAdd_Click(object sender, EventArgs e)
	{
		try
		{
			this.Add_Click?.Invoke(sender, e);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BtnAdd_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmExtList.cs", 221);
		}
	}

	private void BtnEdit_Click(object sender, EventArgs e)
	{
		try
		{
			this.Edit_Click?.Invoke(sender, e);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BtnEdit_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmExtList.cs", 232);
		}
	}

	private void BtnDelete_Click(object sender, EventArgs e)
	{
		try
		{
			this.Delete_Click?.Invoke(sender, e);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BtnDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmExtList.cs", 243);
		}
	}

	private void BtnOther_Click(object sender, EventArgs e)
	{
		try
		{
			if (this.Other_Click != null)
			{
				this.Other_Click(sender, e);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BtnOther_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmExtList.cs", 255);
		}
	}

	private async void FrmExtList_Load(object sender, EventArgs e)
	{
	}

	private void SetLables()
	{
		try
		{
			string translate = "[انتخاب]".GetTranslate();
			string translate2 = "[ویرایش]".GetTranslate();
			string translate3 = "[اضافه]".GetTranslate();
			string translate4 = "[حذف]".GetTranslate();
			string translate5 = "[تمامی رکوردها نمایش داده شود]".GetTranslate();
			((ToolStripItem)ToolItemSelect).Text = translate;
			((ToolStripItem)ToolItemModify).Text = translate2;
			((ToolStripItem)ToolItemAdd).Text = translate3;
			((ToolStripItem)ToolItemDelete).Text = translate4;
			((ToolStripItem)ToolItemCancelSearch).Text = translate5;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetLables", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmExtList.cs", 283);
		}
	}

	private void InitializeComponent()
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		((Control)this).SuspendLayout();
		((Form)this).ClientSize = new Size(915, 561);
		((Control)this).Name = "FrmExtList";
		((Form)this).FormClosed += new FormClosedEventHandler(FrmExtList_FormClosed);
		((Control)this).ResumeLayout(false);
	}

	private void FrmExtList_FormClosed(object sender, FormClosedEventArgs e)
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
				components?.Dispose();
				base.Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmExtList.cs", 314);
		}
	}

	protected void BaseInitializeComponent()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Expected O, but got Unknown
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected O, but got Unknown
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected O, but got Unknown
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Expected O, but got Unknown
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Expected O, but got Unknown
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Expected O, but got Unknown
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Expected O, but got Unknown
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0300: Expected O, but got Unknown
		//IL_034f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Expected O, but got Unknown
		//IL_0476: Unknown result type (might be due to invalid IL or missing references)
		//IL_0572: Unknown result type (might be due to invalid IL or missing references)
		//IL_057c: Expected O, but got Unknown
		//IL_059a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0647: Unknown result type (might be due to invalid IL or missing references)
		//IL_0651: Expected O, but got Unknown
		//IL_067e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0725: Unknown result type (might be due to invalid IL or missing references)
		//IL_072f: Expected O, but got Unknown
		//IL_076c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0817: Unknown result type (might be due to invalid IL or missing references)
		//IL_0821: Expected O, but got Unknown
		//IL_085b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0906: Unknown result type (might be due to invalid IL or missing references)
		//IL_0910: Expected O, but got Unknown
		//IL_094a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a44: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c55: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cfb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d81: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d8b: Expected O, but got Unknown
		//IL_0e08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fdd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe7: Expected O, but got Unknown
		//IL_0ff3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ffd: Expected O, but got Unknown
		//IL_1002: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FrmExtList));
		contextMenuStrip1 = new ContextMenuStrip(components);
		ToolItemSelect = new ToolStripMenuItem();
		ToolItemModify = new ToolStripMenuItem();
		ToolItemAdd = new ToolStripMenuItem();
		ToolItemDelete = new ToolStripMenuItem();
		ToolItemCancelSearch = new ToolStripMenuItem();
		PanelEx1 = new PanelEx();
		BtnSelect = new ButtonX();
		BtnOther = new ButtonX();
		BtnDelete = new ButtonX();
		BtnAdd = new ButtonX();
		BtnEdit = new ButtonX();
		GroupPanel1 = new GroupPanel();
		LabelX1 = new Label();
		TxtSearch = new TextBoxX();
		statusStrip1 = new StatusStrip();
		StatusLabelIns = new ToolStripStatusLabel();
		StatusLabelModiFy = new ToolStripStatusLabel();
		StatusLabelDel = new ToolStripStatusLabel();
		StatusLabel2 = new ToolStripStatusLabel();
		StatusLabel1 = new ToolStripStatusLabel();
		StatusLabelMessage = new ToolStripStatusLabel();
		((Control)contextMenuStrip1).SuspendLayout();
		((Control)PanelEx1).SuspendLayout();
		((Control)GroupPanel1).SuspendLayout();
		((Control)statusStrip1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)contextMenuStrip1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((ToolStrip)contextMenuStrip1).Items.AddRange((ToolStripItem[])(object)new ToolStripItem[5]
		{
			(ToolStripItem)ToolItemSelect,
			(ToolStripItem)ToolItemModify,
			(ToolStripItem)ToolItemAdd,
			(ToolStripItem)ToolItemDelete,
			(ToolStripItem)ToolItemCancelSearch
		});
		((Control)contextMenuStrip1).Name = "contextMenuStrip1";
		((Control)contextMenuStrip1).RightToLeft = (RightToLeft)1;
		((Control)contextMenuStrip1).Size = new Size(243, 114);
		((ToolStripItem)ToolItemSelect).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((ToolStripItem)ToolItemSelect).Name = "ToolItemSelect";
		((ToolStripItem)ToolItemSelect).Size = new Size(242, 22);
		((ToolStripItem)ToolItemSelect).Click += BtnSelect_Click;
		((ToolStripItem)ToolItemModify).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((ToolStripItem)ToolItemModify).Name = "ToolItemModify";
		((ToolStripItem)ToolItemModify).Size = new Size(242, 22);
		((ToolStripItem)ToolItemModify).Click += BtnEdit_Click;
		((ToolStripItem)ToolItemAdd).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((ToolStripItem)ToolItemAdd).Name = "ToolItemAdd";
		((ToolStripItem)ToolItemAdd).Size = new Size(242, 22);
		((ToolStripItem)ToolItemAdd).Click += BtnAdd_Click;
		((ToolStripItem)ToolItemDelete).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((ToolStripItem)ToolItemDelete).Name = "ToolItemDelete";
		((ToolStripItem)ToolItemDelete).Size = new Size(242, 22);
		((ToolStripItem)ToolItemDelete).Click += BtnDelete_Click;
		((ToolStripItem)ToolItemCancelSearch).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((ToolStripItem)ToolItemCancelSearch).Name = "ToolItemCancelSearch";
		((ToolStripItem)ToolItemCancelSearch).Size = new Size(242, 22);
		((ToolStripItem)ToolItemCancelSearch).Click += ToolItemCancelSearch_Click;
		PanelEx1.CanvasColor = SystemColors.Control;
		PanelEx1.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
		((Control)PanelEx1).Controls.Add((Control)(object)BtnSelect);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnOther);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnDelete);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnAdd);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnEdit);
		((Control)PanelEx1).Controls.Add((Control)(object)GroupPanel1);
		((Control)PanelEx1).Controls.Add((Control)(object)statusStrip1);
		((Control)PanelEx1).Dock = (DockStyle)5;
		((Control)PanelEx1).Location = new Point(0, 0);
		((Control)PanelEx1).Margin = new Padding(3, 2, 3, 2);
		((Control)PanelEx1).Name = "PanelEx1";
		((Control)PanelEx1).Size = new Size(784, 362);
		PanelEx1.Style.Alignment = (StringAlignment)1;
		PanelEx1.Style.BackColor1.ColorSchemePart = eColorSchemePart.PanelBackground;
		PanelEx1.Style.BackColor2.ColorSchemePart = eColorSchemePart.PanelBackground2;
		PanelEx1.Style.Border = eBorderType.SingleLine;
		PanelEx1.Style.BorderColor.ColorSchemePart = eColorSchemePart.PanelBorder;
		PanelEx1.Style.ForeColor.ColorSchemePart = eColorSchemePart.PanelText;
		PanelEx1.Style.GradientAngle = 90;
		((Control)PanelEx1).TabIndex = 0;
		((Control)BtnSelect).AccessibleRole = (AccessibleRole)43;
		BtnSelect.ColorTable = eButtonColor.Office2007WithBackground;
		((Control)BtnSelect).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)BtnSelect).Location = new Point(13, 77);
		((Control)BtnSelect).Margin = new Padding(4, 3, 4, 3);
		((Control)BtnSelect).Name = "BtnSelect";
		((Control)BtnSelect).Size = new Size(127, 150);
		((Control)BtnSelect).TabIndex = 18;
		((Control)BtnSelect).TabStop = false;
		((Control)BtnSelect).Text = "[انتخاب] \r\n    \r\n    (Enter)";
		((Control)BtnSelect).Visible = false;
		((Control)BtnSelect).Click += BtnSelect_Click;
		((Control)BtnOther).AccessibleRole = (AccessibleRole)43;
		BtnOther.ColorTable = eButtonColor.Office2007WithBackground;
		((Control)BtnOther).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnOther.ImagePosition = eImagePosition.Right;
		((Control)BtnOther).Location = new Point(14, 191);
		((Control)BtnOther).Margin = new Padding(4, 3, 4, 3);
		((Control)BtnOther).Name = "BtnOther";
		((Control)BtnOther).Size = new Size(126, 32);
		BtnOther.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnOther).TabIndex = 11;
		((Control)BtnOther).TabStop = false;
		((Control)BtnOther).Visible = false;
		((Control)BtnOther).Click += BtnOther_Click;
		((Control)BtnDelete).AccessibleRole = (AccessibleRole)43;
		BtnDelete.ColorTable = eButtonColor.Office2007WithBackground;
		((Control)BtnDelete).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnDelete.Image = (Image)(object)FormCustom.Properties.Resources.edit_delete;
		BtnDelete.ImagePosition = eImagePosition.Right;
		((Control)BtnDelete).Location = new Point(13, 153);
		((Control)BtnDelete).Margin = new Padding(4, 3, 4, 3);
		((Control)BtnDelete).Name = "BtnDelete";
		((Control)BtnDelete).Size = new Size(126, 32);
		BtnDelete.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnDelete).TabIndex = 11;
		((Control)BtnDelete).TabStop = false;
		((Control)BtnDelete).Text = "[حذف] (Delete)    ";
		((Control)BtnDelete).Click += BtnDelete_Click;
		((Control)BtnAdd).AccessibleRole = (AccessibleRole)43;
		BtnAdd.ColorTable = eButtonColor.Office2007WithBackground;
		((Control)BtnAdd).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnAdd.Image = (Image)(object)FormCustom.Properties.Resources.edit_add1;
		BtnAdd.ImagePosition = eImagePosition.Right;
		((Control)BtnAdd).Location = new Point(13, 77);
		((Control)BtnAdd).Margin = new Padding(4, 3, 4, 3);
		((Control)BtnAdd).Name = "BtnAdd";
		((Control)BtnAdd).Size = new Size(126, 32);
		BtnAdd.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnAdd).TabIndex = 9;
		((Control)BtnAdd).TabStop = false;
		((Control)BtnAdd).Text = "[اضافه](Add)";
		((Control)BtnAdd).Click += BtnAdd_Click;
		((Control)BtnEdit).AccessibleRole = (AccessibleRole)43;
		BtnEdit.ColorTable = eButtonColor.Office2007WithBackground;
		((Control)BtnEdit).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnEdit.Image = (Image)(object)FormCustom.Properties.Resources.edit2;
		BtnEdit.ImagePosition = eImagePosition.Right;
		((Control)BtnEdit).Location = new Point(14, 115);
		((Control)BtnEdit).Margin = new Padding(4, 3, 4, 3);
		((Control)BtnEdit).Name = "BtnEdit";
		((Control)BtnEdit).RightToLeft = (RightToLeft)1;
		((Control)BtnEdit).Size = new Size(126, 32);
		BtnEdit.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnEdit).TabIndex = 10;
		((Control)BtnEdit).TabStop = false;
		((Control)BtnEdit).Text = "    [ویرایش](F7)";
		((Control)BtnEdit).Click += BtnEdit_Click;
		((Control)GroupPanel1).Anchor = (AnchorStyles)13;
		GroupPanel1.CanvasColor = SystemColors.Control;
		GroupPanel1.ColorSchemeStyle = eDotNetBarStyle.Office2007;
		((Control)GroupPanel1).Controls.Add((Control)(object)LabelX1);
		((Control)GroupPanel1).Controls.Add((Control)(object)TxtSearch);
		((Control)GroupPanel1).Location = new Point(13, 11);
		((Control)GroupPanel1).Margin = new Padding(4, 3, 4, 3);
		((Control)GroupPanel1).Name = "GroupPanel1";
		((Control)GroupPanel1).Size = new Size(758, 60);
		GroupPanel1.Style.BackColor2SchemePart = eColorSchemePart.PanelBackground2;
		GroupPanel1.Style.BackColorGradientAngle = 90;
		GroupPanel1.Style.BackColorSchemePart = eColorSchemePart.PanelBackground;
		GroupPanel1.Style.BorderBottom = eStyleBorderType.Solid;
		GroupPanel1.Style.BorderBottomWidth = 1;
		GroupPanel1.Style.BorderColorSchemePart = eColorSchemePart.PanelBorder;
		GroupPanel1.Style.BorderLeft = eStyleBorderType.Solid;
		GroupPanel1.Style.BorderLeftWidth = 1;
		GroupPanel1.Style.BorderRight = eStyleBorderType.Solid;
		GroupPanel1.Style.BorderRightWidth = 1;
		GroupPanel1.Style.BorderTop = eStyleBorderType.Solid;
		GroupPanel1.Style.BorderTopWidth = 1;
		GroupPanel1.Style.Class = "";
		GroupPanel1.Style.CornerDiameter = 4;
		GroupPanel1.Style.CornerType = eCornerType.Rounded;
		GroupPanel1.Style.TextAlignment = eStyleTextAlignment.Far;
		GroupPanel1.Style.TextColorSchemePart = eColorSchemePart.PanelText;
		GroupPanel1.Style.TextLineAlignment = eStyleTextAlignment.Near;
		GroupPanel1.StyleMouseDown.Class = "";
		GroupPanel1.StyleMouseDown.CornerType = eCornerType.Square;
		GroupPanel1.StyleMouseOver.Class = "";
		GroupPanel1.StyleMouseOver.CornerType = eCornerType.Square;
		((Control)GroupPanel1).TabIndex = 8;
		((Control)GroupPanel1).Text = "[جستجو]";
		GroupPanel1.TitleImage = (Image)(object)FormCustom.Properties.Resources.search2;
		((Control)LabelX1).Anchor = (AnchorStyles)9;
		((Control)LabelX1).Location = new Point(682, 3);
		((Control)LabelX1).Margin = new Padding(4, 3, 4, 3);
		((Control)LabelX1).Name = "LabelX1";
		((Control)LabelX1).Size = new Size(66, 23);
		((Control)LabelX1).TabIndex = 0;
		((Control)LabelX1).Text = "[جستجو]  :";
		LabelX1.TextAlign = (ContentAlignment)16;
		((Control)TxtSearch).Anchor = (AnchorStyles)13;
		TxtSearch.Border.Class = "TextBoxBorder";
		TxtSearch.Border.CornerType = eCornerType.Square;
		((Control)TxtSearch).Location = new Point(4, 3);
		((Control)TxtSearch).Margin = new Padding(4, 3, 4, 3);
		((Control)TxtSearch).Name = "TxtSearch";
		((Control)TxtSearch).Size = new Size(670, 21);
		((Control)TxtSearch).TabIndex = 1;
		((Control)TxtSearch).TextChanged += txtSearch_TextChanged;
		((ToolStrip)statusStrip1).BackColor = Color.FromArgb(227, 239, 255);
		((Control)statusStrip1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((ToolStrip)statusStrip1).Items.AddRange((ToolStripItem[])(object)new ToolStripItem[6]
		{
			(ToolStripItem)StatusLabelIns,
			(ToolStripItem)StatusLabelModiFy,
			(ToolStripItem)StatusLabelDel,
			(ToolStripItem)StatusLabel2,
			(ToolStripItem)StatusLabel1,
			(ToolStripItem)StatusLabelMessage
		});
		((Control)statusStrip1).Location = new Point(0, 340);
		((Control)statusStrip1).Name = "statusStrip1";
		statusStrip1.Padding = new Padding(10, 0, 1, 0);
		((Control)statusStrip1).Size = new Size(784, 22);
		((Control)statusStrip1).TabIndex = 7;
		((Control)statusStrip1).Text = "statusStrip1";
		((ToolStripItem)StatusLabelIns).Name = "StatusLabelIns";
		((ToolStripItem)StatusLabelIns).Size = new Size(71, 17);
		((ToolStripItem)StatusLabelIns).Text = "[اضافه] Ins";
		((ToolStripItem)StatusLabelModiFy).Name = "StatusLabelModiFy";
		((ToolStripItem)StatusLabelModiFy).Size = new Size(81, 17);
		((ToolStripItem)StatusLabelModiFy).Text = ", [ویرایش] F7";
		((ToolStripItem)StatusLabelDel).Name = "StatusLabelDel";
		((ToolStripItem)StatusLabelDel).Size = new Size(71, 17);
		((ToolStripItem)StatusLabelDel).Text = ", [حذف] Del";
		((ToolStripItem)StatusLabel2).Name = "StatusLabel2";
		((ToolStripItem)StatusLabel2).Size = new Size(10, 17);
		((ToolStripItem)StatusLabel2).Text = " ";
		((ToolStripItem)StatusLabel1).Name = "StatusLabel1";
		((ToolStripItem)StatusLabel1).Size = new Size(10, 17);
		((ToolStripItem)StatusLabel1).Text = " ";
		((ToolStripItem)StatusLabelMessage).ForeColor = Color.Red;
		((ToolStripItem)StatusLabelMessage).Name = "StatusLabelMessage";
		((ToolStripItem)StatusLabelMessage).Size = new Size(82, 17);
		((ToolStripItem)StatusLabelMessage).Text = "                         ";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(784, 362);
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).Margin = new Padding(4, 3, 4, 3);
		((Control)this).Name = "FrmExtList";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[AnavinList]";
		((Control)contextMenuStrip1).ResumeLayout(false);
		((Control)PanelEx1).ResumeLayout(false);
		((Control)PanelEx1).PerformLayout();
		((Control)GroupPanel1).ResumeLayout(false);
		((Control)statusStrip1).ResumeLayout(false);
		((Control)statusStrip1).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
