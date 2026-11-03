using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using FormCustom.Cache;
using PacketParser.Assistance.MultiLingual;
using PacketParser.Entities;

namespace FormCustom;

public class frmShowErrorDetails : CacheBaseForm
{
	private bool _isDisposed;

	private IContainer components;

	private ButtonX btnClose;

	private TextBox textBox1;

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

	private void ShowUrl(string msg)
	{
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Invalid comparison between Unknown and I4
		try
		{
			if (!msg.Contains("http"))
			{
				return;
			}
			MatchCollection matchCollection = new Regex("(http|ftp|https)://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?", RegexOptions.IgnoreCase).Matches(msg);
			string text = "";
			foreach (Match item in matchCollection)
			{
				text = item.Value;
			}
			if ((int)MessageBox.Show((IWin32Window)(object)this, "Do you want to open url: " + text + " ?", text ?? "", (MessageBoxButtons)4, (MessageBoxIcon)32) == 6)
			{
				Process.Start(text);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ShowUrl", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\frmShowErrorDetails.cs", 34);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\frmShowErrorDetails.cs", 45);
		}
	}

	public frmShowErrorDetails(LanguageTypes language, string text, FormWindowState state, string context)
		: base(context)
	{
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			InitializeComponent();
			switch (language)
			{
			case LanguageTypes.Farsi:
				((Control)btnClose).Text = "بستن";
				break;
			case LanguageTypes.Arbic:
				((Control)btnClose).Text = "لإغلاق";
				break;
			case LanguageTypes.English:
				((Control)btnClose).Text = "Close";
				break;
			default:
				throw new ArgumentOutOfRangeException("language", language, null);
			case LanguageTypes.Spanish:
			case LanguageTypes.Turkish:
			case LanguageTypes.EnterNational:
				break;
			}
			((Control)textBox1).Text = text;
			((Form)this).WindowState = state;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\frmShowErrorDetails.cs", 66);
		}
	}

	private void textBox1_Click(object sender, EventArgs e)
	{
		ShowUrl(((Control)textBox1).Text);
	}

	private void frmShowErrorDetails_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			((Form)this).Close();
		}
	}

	private void frmShowErrorDetails_FormClosed(object sender, FormClosedEventArgs e)
	{
		((Component)(object)this).Dispose(disposing: true);
	}

	private void InitializeComponent()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Expected O, but got Unknown
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Expected O, but got Unknown
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Expected O, but got Unknown
		btnClose = new ButtonX();
		textBox1 = new TextBox();
		((Control)this).SuspendLayout();
		((Control)btnClose).AccessibleRole = (AccessibleRole)43;
		((Control)btnClose).Anchor = (AnchorStyles)14;
		btnClose.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)btnClose).Cursor = Cursors.Hand;
		btnClose.DialogResult = (DialogResult)2;
		((Control)btnClose).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)btnClose).Location = new Point(74, 427);
		((Control)btnClose).Name = "btnClose";
		btnClose.Shape = new RoundRectangleShapeDescriptor(10);
		((Control)btnClose).Size = new Size(278, 28);
		btnClose.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)btnClose).TabIndex = 3;
		((Control)btnClose).Text = "Close";
		((Control)textBox1).Anchor = (AnchorStyles)15;
		((Control)textBox1).Location = new Point(1, 0);
		((TextBoxBase)textBox1).Multiline = true;
		((Control)textBox1).Name = "textBox1";
		textBox1.ScrollBars = (ScrollBars)2;
		((Control)textBox1).Size = new Size(422, 422);
		((Control)textBox1).TabIndex = 4;
		((TextBoxBase)textBox1).Click += textBox1_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 16f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(424, 460);
		((Control)this).Controls.Add((Control)(object)textBox1);
		((Control)this).Controls.Add((Control)(object)btnClose);
		((Control)this).Font = new Font("Tahoma", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Form)this).KeyPreview = true;
		((Form)this).Margin = new Padding(3, 4, 3, 4);
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "frmShowErrorDetails";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Form)this).FormClosed += new FormClosedEventHandler(frmShowErrorDetails_FormClosed);
		((Control)this).KeyDown += new KeyEventHandler(frmShowErrorDetails_KeyDown);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
