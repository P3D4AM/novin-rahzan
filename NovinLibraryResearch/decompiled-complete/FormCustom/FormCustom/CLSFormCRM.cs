using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom.Properties;
using PacketParser.Entities;

namespace FormCustom;

public class CLSFormCRM : BaseForm
{
	public delegate void CRMEventHandler(object sender, EventArgs e);

	protected new bool IsFormContentsChanged;

	protected new bool NoCheckFormContentsChanged;

	protected List<Control> ObjsCanEmpty = new List<Control>();

	private bool _isDisposed;

	private IContainer components;

	public StatusStrip statusStrip1;

	public ToolStripStatusLabel StatusLabelOK;

	public ToolStripStatusLabel StatusLabelCancel;

	public ToolStripStatusLabel StatusLabel2;

	public ToolStripStatusLabel StatusLabel1;

	public ToolStripStatusLabel StatusLabelMessage;

	public PanelEx panelEx1;

	public ButtonX BtnCancel;

	public ButtonX BtnOk;

	public GroupPanel groupPanel1;

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

	public event CRMEventHandler OK_Click;

	public event CRMEventHandler Cancel_Click;

	public CLSFormCRM(string contextGuid)
		: base(contextGuid)
	{
		try
		{
			InitializeComponent();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\CLSFormCRM.cs", 24);
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		try
		{
			base.OnLoad(e);
			AddControlChanged(((Control)this).Controls);
			IsFormContentsChanged = false;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnLoad", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\CLSFormCRM.cs", 34);
		}
	}

	private void AddControlChanged(ControlCollection pobj)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			foreach (Control item in (ArrangedElementCollection)pobj)
			{
				Control val = item;
				if (val is CheckBox)
				{
					((CheckBox)val).CheckedChanged += c_ControlChanged;
				}
				else
				{
					_ = val.Name;
					val.TextChanged += c_ControlChanged;
					Type type = ((object)val).GetType();
					if (type.Equals(typeof(TextBox)) || type.Equals(typeof(TextBoxX)) || type.Equals(typeof(ComboBox)) || type.Equals(typeof(ComboBoxEx)) || type.Equals(typeof(NumericUpDown)) || type.Equals(typeof(MaskedTextBoxAdv)))
					{
						val.Leave += c_OnLeave;
						val.Enter += c_OnEnter;
					}
				}
				AddControlChanged(val.Controls);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddControlChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\CLSFormCRM.cs", 73);
		}
	}

	private void c_ControlChanged(object sender, EventArgs e)
	{
		try
		{
			IsFormContentsChanged = true;
			if (!NoCheckFormContentsChanged && ((Control)this).Text.Substring(((Control)this).Text.Length - 1, 1) != "*")
			{
				((Control)this).Text = ((Control)this).Text + "*";
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "c_ControlChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\CLSFormCRM.cs", 86);
		}
	}

	protected void ResetControlChanged()
	{
		try
		{
			IsFormContentsChanged = false;
			if (((Control)this).Text.Substring(((Control)this).Text.Length - 1, 1) == "*")
			{
				((Control)this).Text = ((Control)this).Text.Substring(0, ((Control)this).Text.Length - 1);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ResetControlChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\CLSFormCRM.cs", 99);
		}
	}

	private void c_OnEnter(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		((Control)sender).BackColor = Color.Yellow;
	}

	private void c_OnLeave(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		((Control)sender).BackColor = Color.White;
	}

	protected override void OnFormClosing(FormClosingEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Invalid comparison between Unknown and I4
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Invalid comparison between Unknown and I4
		try
		{
			if (!NoCheckFormContentsChanged && IsFormContentsChanged)
			{
				((CancelEventArgs)(object)e).Cancel = true;
				DialogResult val = MessageBox.Show((IWin32Window)(object)this, "اطلاعات تغییر کرده است .آیا میخواهید اطلاعات ذخیره شود؟", "توجه", (MessageBoxButtons)3, (MessageBoxIcon)64, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288);
				if ((int)val == 6)
				{
					BtnOk.PerformClick();
					((CancelEventArgs)(object)e).Cancel = false;
				}
				else if ((int)val == 7)
				{
					((CancelEventArgs)(object)e).Cancel = false;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnFormClosing", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\CLSFormCRM.cs", 132);
		}
	}

	private void BtnOk_Click(object sender, EventArgs e)
	{
		try
		{
			if (this.OK_Click != null)
			{
				this.OK_Click(sender, e);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BtnOk_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\CLSFormCRM.cs", 144);
		}
	}

	private void BtnCancel_Click(object sender, EventArgs e)
	{
		try
		{
			if (this.Cancel_Click != null)
			{
				this.Cancel_Click(sender, e);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BtnCancel_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\CLSFormCRM.cs", 156);
		}
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Invalid comparison between Unknown and I4
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Invalid comparison between Unknown and I4
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			Control activeControl = ((ContainerControl)this).ActiveControl;
			string text = activeControl.Text;
			Type type = ((object)activeControl).GetType();
			bool flag = true;
			((ToolStripItem)StatusLabelMessage).Text = "";
			if ((type.Equals(typeof(TextBox)) || type.Equals(typeof(TextBoxX))) && ((TextBoxBase)(TextBox)activeControl).Multiline)
			{
				flag = false;
			}
			if (ObjsCanEmpty.IndexOf(activeControl) >= 0)
			{
				text = " ";
			}
			if (type.Equals(typeof(CheckBox)) || type.Equals(typeof(CheckBoxX)))
			{
				text = " ";
			}
			if ((int)e.KeyData != 13 && (int)e.KeyData != 13)
			{
				return;
			}
			if (text != "" && flag)
			{
				((Control)this).SelectNextControl(activeControl, true, true, true, true);
				if (((ContainerControl)this).ActiveControl.Name == "BtnOk")
				{
					type = ((object)((ContainerControl)this).ActiveControl).GetType();
					if (type.Equals(typeof(Button)))
					{
						((Button)((ContainerControl)this).ActiveControl).PerformClick();
					}
					if (type.Equals(typeof(ButtonX)))
					{
						((ButtonX)(object)((ContainerControl)this).ActiveControl).PerformClick();
					}
				}
			}
			else if (text == "")
			{
				((ToolStripItem)StatusLabelMessage).Text = "          مقدار نمیتواند خالی باشد          ";
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnKeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\CLSFormCRM.cs", 200);
		}
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\CLSFormCRM.cs", 219);
		}
	}

	private void CLSFormCRM_Load(object sender, EventArgs e)
	{
	}

	private void StatusLabelOK_Click(object sender, EventArgs e)
	{
	}

	private void CLSFormCRM_FormClosed(object sender, FormClosedEventArgs e)
	{
		((Component)(object)this).Dispose(disposing: true);
	}

	private void InitializeComponent()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
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
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0434: Unknown result type (might be due to invalid IL or missing references)
		//IL_0617: Unknown result type (might be due to invalid IL or missing references)
		//IL_0701: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f4: Expected O, but got Unknown
		//IL_0835: Unknown result type (might be due to invalid IL or missing references)
		//IL_083f: Expected O, but got Unknown
		panelEx1 = new PanelEx();
		groupPanel1 = new GroupPanel();
		statusStrip1 = new StatusStrip();
		StatusLabelOK = new ToolStripStatusLabel();
		StatusLabelCancel = new ToolStripStatusLabel();
		StatusLabel2 = new ToolStripStatusLabel();
		StatusLabel1 = new ToolStripStatusLabel();
		StatusLabelMessage = new ToolStripStatusLabel();
		BtnCancel = new ButtonX();
		BtnOk = new ButtonX();
		((Control)panelEx1).SuspendLayout();
		((Control)statusStrip1).SuspendLayout();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
		((Control)panelEx1).Controls.Add((Control)(object)groupPanel1);
		((Control)panelEx1).Controls.Add((Control)(object)statusStrip1);
		((Control)panelEx1).Controls.Add((Control)(object)BtnCancel);
		((Control)panelEx1).Controls.Add((Control)(object)BtnOk);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(424, 132);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = eColorSchemePart.PanelBackground;
		panelEx1.Style.Border = eBorderType.SingleLine;
		panelEx1.Style.BorderColor.ColorSchemePart = eColorSchemePart.PanelBorder;
		panelEx1.Style.ForeColor.ColorSchemePart = eColorSchemePart.PanelText;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 1;
		((Control)groupPanel1).Anchor = (AnchorStyles)15;
		((Control)groupPanel1).BackColor = Color.White;
		groupPanel1.CanvasColor = SystemColors.Control;
		groupPanel1.ColorSchemeStyle = eDotNetBarStyle.Office2007;
		groupPanel1.DisabledBackColor = Color.Empty;
		((Control)groupPanel1).Location = new Point(8, 6);
		((Control)groupPanel1).Margin = new Padding(2);
		((Control)groupPanel1).Name = "groupPanel1";
		((Control)groupPanel1).Size = new Size(407, 56);
		groupPanel1.Style.BackColor2SchemePart = eColorSchemePart.PanelBackground2;
		groupPanel1.Style.BackColorGradientAngle = 90;
		groupPanel1.Style.BackColorSchemePart = eColorSchemePart.PanelBackground;
		groupPanel1.Style.BorderBottom = eStyleBorderType.Solid;
		groupPanel1.Style.BorderBottomWidth = 1;
		groupPanel1.Style.BorderColorSchemePart = eColorSchemePart.PanelBorder;
		groupPanel1.Style.BorderLeft = eStyleBorderType.Solid;
		groupPanel1.Style.BorderLeftWidth = 1;
		groupPanel1.Style.BorderRight = eStyleBorderType.Solid;
		groupPanel1.Style.BorderRightWidth = 1;
		groupPanel1.Style.BorderTop = eStyleBorderType.Solid;
		groupPanel1.Style.BorderTopWidth = 1;
		groupPanel1.Style.CornerDiameter = 4;
		groupPanel1.Style.CornerType = eCornerType.Rounded;
		groupPanel1.Style.TextAlignment = eStyleTextAlignment.Center;
		groupPanel1.Style.TextColorSchemePart = eColorSchemePart.PanelText;
		groupPanel1.Style.TextLineAlignment = eStyleTextAlignment.Near;
		groupPanel1.StyleMouseDown.CornerType = eCornerType.Square;
		groupPanel1.StyleMouseOver.CornerType = eCornerType.Square;
		((Control)groupPanel1).TabIndex = 6;
		((ToolStrip)statusStrip1).Items.AddRange((ToolStripItem[])(object)new ToolStripItem[5]
		{
			(ToolStripItem)StatusLabelOK,
			(ToolStripItem)StatusLabelCancel,
			(ToolStripItem)StatusLabel2,
			(ToolStripItem)StatusLabel1,
			(ToolStripItem)StatusLabelMessage
		});
		((Control)statusStrip1).Location = new Point(0, 110);
		((Control)statusStrip1).Name = "statusStrip1";
		statusStrip1.Padding = new Padding(9, 0, 1, 0);
		((Control)statusStrip1).Size = new Size(424, 22);
		((Control)statusStrip1).TabIndex = 5;
		((Control)statusStrip1).Text = "statusStrip1";
		((ToolStripItem)StatusLabelOK).Name = "StatusLabelOK";
		((ToolStripItem)StatusLabelOK).Size = new Size(57, 17);
		((ToolStripItem)StatusLabelOK).Text = "[تائید] = F5 ";
		((ToolStripItem)StatusLabelOK).Click += StatusLabelOK_Click;
		((ToolStripItem)StatusLabelCancel).Name = "StatusLabelCancel";
		((ToolStripItem)StatusLabelCancel).Size = new Size(84, 17);
		((ToolStripItem)StatusLabelCancel).Text = ", [انصراف] = Esc ";
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
		((Control)BtnCancel).AccessibleRole = (AccessibleRole)43;
		((Control)BtnCancel).Anchor = (AnchorStyles)6;
		BtnCancel.ColorTable = eButtonColor.OrangeWithBackground;
		BtnCancel.DialogResult = (DialogResult)2;
		BtnCancel.Image = (Image)(object)FormCustom.Properties.Resources.stop;
		BtnCancel.ImageFixedSize = new Size(35, 35);
		BtnCancel.ImagePosition = eImagePosition.Right;
		((Control)BtnCancel).Location = new Point(8, 66);
		((Control)BtnCancel).Margin = new Padding(2);
		((Control)BtnCancel).Name = "BtnCancel";
		((Control)BtnCancel).Size = new Size(93, 42);
		BtnCancel.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnCancel).TabIndex = 4;
		((Control)BtnCancel).TabStop = false;
		((Control)BtnCancel).Text = "Cancel";
		((Control)BtnCancel).Click += BtnCancel_Click;
		((Control)BtnOk).AccessibleRole = (AccessibleRole)43;
		((Control)BtnOk).Anchor = (AnchorStyles)6;
		BtnOk.ColorTable = eButtonColor.OrangeWithBackground;
		BtnOk.Image = (Image)(object)FormCustom.Properties.Resources.accept;
		BtnOk.ImageFixedSize = new Size(35, 35);
		BtnOk.ImagePosition = eImagePosition.Right;
		((Control)BtnOk).Location = new Point(104, 66);
		((Control)BtnOk).Margin = new Padding(2);
		((Control)BtnOk).Name = "BtnOk";
		BtnOk.Shortcuts.Add(eShortcut.F5);
		((Control)BtnOk).Size = new Size(93, 42);
		BtnOk.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnOk).TabIndex = 3;
		((Control)BtnOk).TabStop = false;
		((Control)BtnOk).Text = "Accept";
		((Control)BtnOk).Click += BtnOk_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).CancelButton = (IButtonControl)(object)BtnCancel;
		((Form)this).ClientSize = new Size(424, 132);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "CLSFormCRM";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "CLSFormCRM";
		((Form)this).FormClosed += new FormClosedEventHandler(CLSFormCRM_FormClosed);
		((Form)this).Load += CLSFormCRM_Load;
		((Control)panelEx1).ResumeLayout(false);
		((Control)panelEx1).PerformLayout();
		((Control)statusStrip1).ResumeLayout(false);
		((Control)statusStrip1).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
