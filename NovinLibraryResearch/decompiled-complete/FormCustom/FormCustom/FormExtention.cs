using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom.Forms;
using PacketParser;
using PacketParser.Entities;

namespace FormCustom;

public class FormExtention : AccountingBaseForm
{
	public delegate void CrmEventHandler(object sender, EventArgs e);

	private bool _isDisposed;

	protected List<Control> objsCanEmpty = new List<Control>();

	protected TabControl TabControlExt;

	protected Control objDGrid;

	private IContainer components;

	protected StatusStrip statusStrip1;

	protected ToolStripStatusLabel StatusLabelOK;

	protected ToolStripStatusLabel StatusLabelCancel;

	protected ToolStripStatusLabel StatusLabelMessage;

	protected ToolStripStatusLabel StatusLabel1;

	protected ToolStripStatusLabel StatusLabel2;

	public bool IsHandleKeyDown { get; private set; }

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

	public event CrmEventHandler OK_Click;

	public event CrmEventHandler Cancel_Click;

	public event CrmEventHandler Insert_Click;

	public event CrmEventHandler Edit_Click;

	public event CrmEventHandler Other_Click;

	public event CrmEventHandler Delete_Click;

	public FormExtention(string context, bool enableStatusBar = true, bool isHandelKeyDown = true)
		: base(context)
	{
		if (enableStatusBar)
		{
			InitializeComponent();
		}
		IsHandleKeyDown = isHandelKeyDown;
	}

	protected override void OnLoad(EventArgs e)
	{
		try
		{
			base.OnLoad(e);
			if (objDGrid != null)
			{
				objDGrid.DoubleClick += DoubleClick_1;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnLoad", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FormExtention.cs", 68);
		}
	}

	private void DoubleClick_1(object sender, EventArgs e)
	{
		try
		{
			if (((ContainerControl)this).ActiveControl.Name == "DGrid")
			{
				BtnEditClick(null, e);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DoubleClick_1", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FormExtention.cs", 79);
		}
	}

	protected void ResetControlChanged()
	{
		try
		{
			base.IsFormContentsChanged = false;
			if (((Control)this).Text.Length > 0 && ((Control)this).Text.Substring(((Control)this).Text.Length - 1, 1) == "*")
			{
				((Control)this).Text = ((Control)this).Text.Substring(0, ((Control)this).Text.Length - 1);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ResetControlChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FormExtention.cs", 93);
		}
	}

	protected override void OnFormClosing(FormClosingEventArgs e)
	{
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Invalid comparison between Unknown and I4
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Invalid comparison between Unknown and I4
		int num = 0;
		try
		{
			if (IsDisposed)
			{
				return;
			}
			num = 1;
			if (base.NoCheckFormContentsChanged || !base.IsFormContentsChanged)
			{
				return;
			}
			num = 2;
			((CancelEventArgs)(object)e).Cancel = true;
			num = 3;
			string text = "[اطلاعات تغییر کرده است .آیا میخواهید اطلاعات ذخیره شود؟]".GetTranslate();
			num = 4;
			if (text == "[اطلاعات تغییر کرده است .آیا میخواهید اطلاعات ذخیره شود؟]")
			{
				text = "اطلاعات تغییر کرده است .آیا مایلید اطلاعات را ذخیره نمایید؟";
			}
			num = 5;
			string text2 = "[Warning]".GetTranslate();
			num = 6;
			if (text2 == "[Warning]")
			{
				text2 = "توجه !";
			}
			num = 7;
			DialogResult val = MessageBox.Show((IWin32Window)(object)this, text, text2, (MessageBoxButtons)3, (MessageBoxIcon)64, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288);
			num = 8;
			if ((int)val != 6)
			{
				if ((int)val == 7)
				{
					num = 11;
					((CancelEventArgs)(object)e).Cancel = false;
					num = 12;
					base.IsFormContentsChanged = false;
				}
			}
			else
			{
				num = 9;
				BtnOkExt_Click(null, (EventArgs)(object)e);
				num = 10;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"#Line_{num}", "OnFormClosing", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FormExtention.cs", 134);
		}
	}

	protected void BtnOkExt_Click(object sender, EventArgs e)
	{
		this.OK_Click?.Invoke(sender, e);
	}

	protected void BtnCancel_Click(object sender, EventArgs e)
	{
		try
		{
			if (this.Cancel_Click != null)
			{
				this.Cancel_Click(sender, e);
				return;
			}
			((Form)this).DialogResult = (DialogResult)2;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BtnCancel_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FormExtention.cs", 155);
		}
	}

	private void BtnInsertClick(object sender, EventArgs e)
	{
		try
		{
			this.Insert_Click?.Invoke(sender, e);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BtnInsertClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FormExtention.cs", 164);
		}
	}

	private void BtnEditClick(object sender, EventArgs e)
	{
		try
		{
			this.Edit_Click?.Invoke(sender, e);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BtnEditClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FormExtention.cs", 173);
		}
	}

	private void BtnOtherClick(object sender, EventArgs e)
	{
		this.Other_Click?.Invoke(sender, e);
	}

	private void BtnDeleteClick(object sender, EventArgs e)
	{
		try
		{
			this.Delete_Click?.Invoke(sender, e);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BtnDeleteClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FormExtention.cs", 186);
		}
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Invalid comparison between Unknown and I4
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Invalid comparison between Unknown and I4
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Invalid comparison between Unknown and I4
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Invalid comparison between Unknown and I4
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Invalid comparison between Unknown and I4
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Invalid comparison between Unknown and I4
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Invalid comparison between Unknown and I4
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Expected I4, but got Unknown
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Invalid comparison between Unknown and I4
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Invalid comparison between Unknown and I4
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Invalid comparison between Unknown and I4
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Invalid comparison between Unknown and I4
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Invalid comparison between Unknown and I4
		//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			base.OnKeyDown(e);
			if (!IsHandleKeyDown)
			{
				return;
			}
			Control activeControl = ((ContainerControl)this).ActiveControl;
			if (activeControl == null)
			{
				return;
			}
			string text = activeControl.Text;
			string name = activeControl.Name;
			Type type = ((object)activeControl).GetType();
			if (StatusLabelMessage != null)
			{
				((ToolStripItem)StatusLabelMessage).Text = "";
			}
			if (text == "" && objsCanEmpty.IndexOf(activeControl) >= 0)
			{
				text = " ";
			}
			Keys keyData = e.KeyData;
			if ((int)keyData <= 38)
			{
				if ((int)keyData <= 27)
				{
					if ((int)keyData != 13)
					{
						if ((int)keyData == 27)
						{
							((Form)this).Close();
						}
						return;
					}
					if (type == typeof(DataGridView) || type == typeof(DataGridViewX))
					{
						BtnEditClick(null, (EventArgs)(object)e);
						e.Handled = true;
						return;
					}
					if (text != "")
					{
						if (activeControl.TabStop)
						{
							((Control)this).SelectNextControl(activeControl, true, true, true, true);
						}
						if (((ContainerControl)this).ActiveControl.Name == "BtnOk")
						{
							type = ((object)((ContainerControl)this).ActiveControl).GetType();
							if (type == typeof(Button))
							{
								((Button)((ContainerControl)this).ActiveControl).PerformClick();
							}
							if (type == typeof(ButtonX))
							{
								((ButtonX)(object)((ContainerControl)this).ActiveControl).PerformClick();
							}
						}
						else if (((ContainerControl)this).ActiveControl.Name == "TXTDescription")
						{
							return;
						}
					}
					else
					{
						e.Handled = true;
					}
					e.SuppressKeyPress = true;
					return;
				}
				if ((int)keyData == 33)
				{
					if (TabControlExt != null)
					{
						if (TabControlExt.SelectedTabIndex <= 0)
						{
							TabControlExt.SelectedTabIndex = ((ArrangedElementCollection)((Control)TabControlExt).Controls).Count - 2;
						}
						else
						{
							TabControlExt.SelectedTabIndex--;
						}
					}
					return;
				}
				if ((int)keyData == 34)
				{
					if (TabControlExt != null)
					{
						if (TabControlExt.SelectedTabIndex < ((ArrangedElementCollection)((Control)TabControlExt).Controls).Count - 2)
						{
							TabControlExt.SelectedTabIndex++;
						}
						else
						{
							TabControlExt.SelectedTabIndex = 0;
						}
					}
					return;
				}
				if ((int)keyData != 38 || !(name == "TxtSearch"))
				{
					return;
				}
			}
			else
			{
				if ((int)keyData > 45)
				{
					if ((int)keyData != 46)
					{
						switch (keyData - 116)
						{
						case 2:
							if (type == typeof(DataGridView) || type == typeof(DataGridViewX))
							{
								BtnEditClick(null, (EventArgs)(object)e);
								e.Handled = true;
							}
							return;
						case 1:
							if (type == typeof(DataGridView) || type == typeof(DataGridViewX))
							{
								BtnOtherClick(null, (EventArgs)(object)e);
								e.Handled = true;
							}
							return;
						case 0:
							e.Handled = true;
							return;
						}
						if ((int)keyData == 123)
						{
							e.Handled = true;
							FrmCalc frmCalc = new FrmCalc(base.ContextGuid);
							((Form)(object)this).CheckVisibility("OnKeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FormExtention.cs");
							((Form)frmCalc).ShowDialog((IWin32Window)(object)this);
							((Component)(object)frmCalc).Dispose();
						}
					}
					else if (type == typeof(DataGridView) || type == typeof(DataGridViewX))
					{
						BtnDeleteClick(null, (EventArgs)(object)e);
						e.Handled = true;
					}
					return;
				}
				if ((int)keyData != 40)
				{
					if ((int)keyData == 45 && (type == typeof(DataGridView) || type == typeof(DataGridViewX)))
					{
						BtnInsertClick(null, (EventArgs)(object)e);
						e.Handled = true;
					}
					return;
				}
				if (!(name == "TxtSearch"))
				{
					return;
				}
			}
			objDGrid.Select();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnKeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FormExtention.cs", 293);
		}
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				if (disposing)
				{
					components?.Dispose();
				}
				base.Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FormExtention.cs", 307);
		}
	}

	private void FormExtention_FormClosed(object sender, FormClosedEventArgs e)
	{
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Expected O, but got Unknown
		statusStrip1 = new StatusStrip();
		StatusLabelOK = new ToolStripStatusLabel();
		StatusLabelCancel = new ToolStripStatusLabel();
		StatusLabel2 = new ToolStripStatusLabel();
		StatusLabel1 = new ToolStripStatusLabel();
		StatusLabelMessage = new ToolStripStatusLabel();
		((Control)statusStrip1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)statusStrip1).Font = new Font("Tahoma", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((ToolStrip)statusStrip1).Items.AddRange((ToolStripItem[])(object)new ToolStripItem[5]
		{
			(ToolStripItem)StatusLabelOK,
			(ToolStripItem)StatusLabelCancel,
			(ToolStripItem)StatusLabel2,
			(ToolStripItem)StatusLabel1,
			(ToolStripItem)StatusLabelMessage
		});
		((Control)statusStrip1).Location = new Point(0, 387);
		((Control)statusStrip1).Name = "statusStrip1";
		((Control)statusStrip1).Size = new Size(784, 22);
		((Control)statusStrip1).TabIndex = 2;
		((Control)statusStrip1).Text = "statusStrip1";
		((ToolStripItem)StatusLabelOK).Name = "StatusLabelOK";
		((ToolStripItem)StatusLabelOK).Size = new Size(85, 17);
		((ToolStripItem)StatusLabelOK).Text = "[تائید] = F5 ";
		((ToolStripItem)StatusLabelCancel).Name = "StatusLabelCancel";
		((ToolStripItem)StatusLabelCancel).Size = new Size(100, 17);
		((ToolStripItem)StatusLabelCancel).Text = "[لغو] = Esc ";
		((ToolStripItem)StatusLabel2).Name = "StatusLabel2";
		((ToolStripItem)StatusLabel2).Size = new Size(12, 17);
		((ToolStripItem)StatusLabel2).Text = " ";
		((ToolStripItem)StatusLabel1).Name = "StatusLabel1";
		((ToolStripItem)StatusLabel1).Size = new Size(12, 17);
		((ToolStripItem)StatusLabel1).Text = " ";
		((ToolStripItem)StatusLabelMessage).ForeColor = Color.Red;
		((ToolStripItem)StatusLabelMessage).Name = "StatusLabelMessage";
		((ToolStripItem)StatusLabelMessage).Size = new Size(108, 17);
		((ToolStripItem)StatusLabelMessage).Text = "                         ";
		((Form)this).ClientSize = new Size(784, 409);
		((Control)this).Controls.Add((Control)(object)statusStrip1);
		((Control)this).Name = "FormExtention";
		((Form)this).FormClosed += new FormClosedEventHandler(FormExtention_FormClosed);
		((Control)statusStrip1).ResumeLayout(false);
		((Control)statusStrip1).PerformLayout();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
