using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using FormCustom.Properties;
using PacketParser.Entities;

namespace FormCustom.BackUp.Forms;

public class FRMShowSqlServiceErrors : AccountingBaseForm
{
	private List<string> messages;

	private string connectionString;

	private bool _isDisposed;

	private IContainer components;

	private ListView LstMessages;

	private Label label1;

	private ButtonX BtnRetry;

	private ButtonX BtnOk;

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

	public FRMShowSqlServiceErrors(List<string> messages, string connectionString, string context)
		: base(context)
	{
		InitializeComponent();
		this.messages = messages;
		this.connectionString = connectionString;
	}

	private void BtnOk_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void BtnRetry_Click(object sender, EventArgs e)
	{
	}

	private void FRMShowSqlServiceErrors_Load(object sender, EventArgs e)
	{
		if (messages == null || !messages.Any())
		{
			return;
		}
		foreach (string message in messages)
		{
			LstMessages.Items.Add(message);
		}
	}

	private void FRMShowSqlServiceErrors_FormClosed(object sender, FormClosedEventArgs e)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\FRMShowSqlServiceErrors.cs", 60);
		}
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0362: Unknown result type (might be due to invalid IL or missing references)
		//IL_036c: Expected O, but got Unknown
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Expected O, but got Unknown
		LstMessages = new ListView();
		label1 = new Label();
		BtnOk = new ButtonX();
		BtnRetry = new ButtonX();
		((Control)this).SuspendLayout();
		LstMessages.HideSelection = false;
		((Control)LstMessages).Location = new Point(12, 29);
		((Control)LstMessages).Margin = new Padding(3, 4, 3, 4);
		((Control)LstMessages).Name = "LstMessages";
		((Control)LstMessages).Size = new Size(300, 287);
		((Control)LstMessages).TabIndex = 0;
		LstMessages.UseCompatibleStateImageBehavior = false;
		((Control)label1).Anchor = (AnchorStyles)13;
		((Control)label1).AutoSize = true;
		((Control)label1).Location = new Point(186, 9);
		((Control)label1).Name = "label1";
		((Control)label1).RightToLeft = (RightToLeft)1;
		((Control)label1).Size = new Size(126, 16);
		((Control)label1).TabIndex = 1;
		((Control)label1).Text = "[پیام های] SqlService:";
		((Control)BtnOk).AccessibleRole = (AccessibleRole)43;
		BtnOk.ColorTable = eButtonColor.OrangeWithBackground;
		BtnOk.Image = (Image)(object)FormCustom.Properties.Resources.accept;
		BtnOk.ImagePosition = eImagePosition.Right;
		((Control)BtnOk).Location = new Point(12, 323);
		((Control)BtnOk).Name = "BtnOk";
		((Control)BtnOk).RightToLeft = (RightToLeft)1;
		((Control)BtnOk).Size = new Size(99, 34);
		BtnOk.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnOk).TabIndex = 14;
		((Control)BtnOk).Text = "[تائید](F5)";
		((Control)BtnOk).Click += BtnOk_Click;
		((Control)BtnRetry).AccessibleRole = (AccessibleRole)43;
		BtnRetry.ColorTable = eButtonColor.OrangeWithBackground;
		BtnRetry.Image = (Image)(object)FormCustom.Properties.Resources.ic_refresh;
		BtnRetry.ImageFixedSize = new Size(30, 30);
		BtnRetry.ImagePosition = eImagePosition.Right;
		((Control)BtnRetry).Location = new Point(186, 323);
		((Control)BtnRetry).Name = "BtnRetry";
		((Control)BtnRetry).RightToLeft = (RightToLeft)1;
		((Control)BtnRetry).Size = new Size(126, 34);
		BtnRetry.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnRetry).TabIndex = 13;
		((Control)BtnRetry).Text = "[تلاش مجدد]";
		((Control)BtnRetry).Visible = false;
		((Control)BtnRetry).Click += BtnRetry_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 16f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.FromArgb(208, 228, 255);
		((Form)this).ClientSize = new Size(324, 366);
		((Control)this).Controls.Add((Control)(object)BtnOk);
		((Control)this).Controls.Add((Control)(object)BtnRetry);
		((Control)this).Controls.Add((Control)(object)label1);
		((Control)this).Controls.Add((Control)(object)LstMessages);
		((Control)this).Font = new Font("Tahoma", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Form)this).Margin = new Padding(3, 4, 3, 4);
		((Control)this).Name = "FRMShowSqlServiceErrors";
		((Form)this).FormClosed += new FormClosedEventHandler(FRMShowSqlServiceErrors_FormClosed);
		((Form)this).Load += FRMShowSqlServiceErrors_Load;
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
