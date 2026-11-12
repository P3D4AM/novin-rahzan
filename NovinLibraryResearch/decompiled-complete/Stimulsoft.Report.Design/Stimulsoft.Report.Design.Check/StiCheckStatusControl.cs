using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.DesktopIntegration;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Controls.Win.DotNetBar.Controls;
using Stimulsoft.Report.Check;
using Stimulsoft.Report.Design.Components;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.Design.Check;

[ToolboxItem(false)]
public class StiCheckStatusControl : UserControl
{
	private int range = 50;

	private int pageCount;

	private int pageIndex;

	private bool lockButton;

	private List<StiCheck> errorsList;

	private List<StiCheck> warningsList;

	private List<StiCheck> informationMessagesList;

	private List<StiCheck> reportRenderingMessagesList;

	private StiReport report;

	private IContainer components;

	internal SuperTooltip superTooltip;

	internal ToolTip toolTip;

	internal StiPanelUX listBoxItems;

	internal Bar toolStripError;

	internal ButtonItem buttonErrorsItem;

	internal ButtonItem buttonWarningsItem;

	internal ButtonItem buttonInformationMessagesItem;

	internal ButtonItem buttonReportRenderingMessagesItem;

	private StiGroupPanelUX panelButtons;

	private StiPanelUX panelButtons1;

	private StiLabelUX textBlockRange;

	private StiLabelUX textBlockInfo;

	private ButtonItem buttonSettings;

	private ButtonItem buttonShowSettings;

	private ButtonItem buttonCheck;

	private StiButtonUX firstButton;

	private StiButtonUX lastButton;

	private StiButtonUX nextButton;

	private StiButtonUX prevButton;

	internal bool IsReportChanged { get; set; }

	public event EventHandler Process;

	public event StiCheckStatusEventHandler Checked;

	public StiCheckStatusControl(bool showSettings)
	{
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Expected O, but got Unknown
		InitializeComponent();
		DoTheming();
		DoScaling();
		if (!((Component)this).DesignMode)
		{
			Localize();
			toolStripError.Visible = false;
			StiSettings.Load();
			buttonErrorsItem.Checked = StiSettings.GetBool("CheckSettings", "ShowErrors", nullValue: true);
			buttonWarningsItem.Checked = StiSettings.GetBool("CheckSettings", "ShowWarnings", nullValue: true);
			buttonInformationMessagesItem.Checked = StiSettings.GetBool("CheckSettings", "ShowInformationMessages", nullValue: true);
			buttonReportRenderingMessagesItem.Checked = StiSettings.GetBool("CheckSettings", "ReportRenderingMessages", nullValue: true);
			((Control)textBlockInfo).Visible = true;
			((Control)listBoxItems).Visible = false;
			lockButton = true;
			buttonSettings.Visible = showSettings;
			((Control)textBlockInfo).Paint += new PaintEventHandler(TextBlockInfo_Paint);
		}
	}

	private void TextBlockInfo_Paint(object sender, PaintEventArgs e)
	{
		Image val = StiReportDesignImages.Check.NoErrors();
		Label val2 = (Label)((sender is Label) ? sender : null);
		e.Graphics.DrawImage(val, new PointF(((Control)val2).Width / 2 - val.Width / 2, ((Control)val2).Height / 2 - val.Height - StiScale.I10));
	}

	private void InvokeProcess()
	{
		this.Process?.Invoke(this, EventArgs.Empty);
	}

	private void InvokeChecked()
	{
		this.Checked?.Invoke(this, new StiCheckStatusEventArgs(errorsList.Count, warningsList.Count, informationMessagesList.Count, reportRenderingMessagesList.Count));
	}

	private void DoTheming()
	{
		if (StiUX.IsDark)
		{
			((Control)toolStripError).BackColor = StiUX.Background;
		}
	}

	private void DoScaling()
	{
		((ButtonBase)firstButton).Image = StiReportDesignImages.Check.FirstPage();
		((ButtonBase)prevButton).Image = StiReportDesignImages.Check.PrevPage();
		((ButtonBase)nextButton).Image = StiReportDesignImages.Check.NextPage();
		((ButtonBase)lastButton).Image = StiReportDesignImages.Check.LastPage();
		buttonErrorsItem.Image = StiReportDesignImages.Actions.Error();
		buttonWarningsItem.Image = StiReportDesignImages.Actions.Warning();
		buttonInformationMessagesItem.Image = StiReportDesignImages.Actions.Information();
		buttonReportRenderingMessagesItem.Image = StiReportDesignImages.Actions.RenderingMessage();
		buttonSettings.Image = StiReportDesignImages.Actions.Properties();
		buttonShowSettings.Image = StiReportDesignImages.Actions.Settings();
	}

	public void BuildChecks(StiReport report, bool compileReport)
	{
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		((Control)textBlockInfo).Text = StiLocalization.Get("Report", "Checking");
		Windows7Taskbar.ShowTaskbarProgress(IntPtr.Zero, Windows7Taskbar.ThumbnailProgressState.Indeterminate);
		this.report = report;
		InvokeProcess();
		CrearResults();
		string text = null;
		if (compileReport && report.CalculationMode == StiCalculationMode.Compilation)
		{
			try
			{
				report.Compile();
			}
			catch (Exception ex)
			{
				if (ex is TargetInvocationException ex2)
				{
					string arg = string.Empty;
					if (ex2.InnerException.TargetSite != null)
					{
						arg = "TargetSite: " + ex2.InnerException.TargetSite.Name;
					}
					text = $"{ex2.Message} {ex2.InnerException.Message} ({arg})";
				}
			}
		}
		StiCheckEngine stiCheckEngine = new StiCheckEngine();
		List<StiCheck> list = stiCheckEngine.CheckReport(report);
		if (text != null)
		{
			StiCompilationErrorCheck item = new StiCompilationErrorCheck
			{
				Element = report,
				Error = new CompilerError("", 0, 0, "", $"TargetInvocation exception: '{text}'")
			};
			list.Add(item);
		}
		if (report.Designer != null)
		{
			StiCheckHelper checkHelper = ((StiDesignerControl)report.Designer.DesignerControl).CheckHelper;
			if (checkHelper != null && checkHelper.ReportRenderingMessagesCount > 0 && checkHelper.ReportRenderingMessagesChecks != null)
			{
				foreach (StiCheck reportRenderingMessagesCheck in checkHelper.ReportRenderingMessagesChecks)
				{
					list.Add(reportRenderingMessagesCheck);
				}
			}
			if (!compileReport && checkHelper != null && checkHelper.ErrorsCount > 0 && checkHelper.Checks != null)
			{
				foreach (StiCheck check2 in checkHelper.Checks)
				{
					if (check2 is StiCompilationErrorCheck && list.All((StiCheck check1) => check1.LongMessage != check2.LongMessage))
					{
						list.Add(check2);
					}
				}
			}
		}
		errorsList = new List<StiCheck>();
		warningsList = new List<StiCheck>();
		informationMessagesList = new List<StiCheck>();
		reportRenderingMessagesList = new List<StiCheck>();
		foreach (StiCheck item2 in list)
		{
			switch (item2.Status)
			{
			case StiCheckStatus.Error:
				errorsList.Add(item2);
				break;
			case StiCheckStatus.Warning:
				warningsList.Add(item2);
				break;
			case StiCheckStatus.Information:
				informationMessagesList.Add(item2);
				break;
			case StiCheckStatus.ReportRenderingMessage:
				reportRenderingMessagesList.Add(item2);
				break;
			}
		}
		BuildVisibleList();
		Windows7Taskbar.HideTaskbarProgress();
		((Control)textBlockInfo).Text = StiLocalization.Get("Report", "NoIssues").Replace("&", string.Empty).Replace("...", string.Empty);
		InvokeChecked();
	}

	private void BuildVisibleList()
	{
		List<StiCheck> list = new List<StiCheck>();
		if (buttonErrorsItem.Checked)
		{
			list.AddRange(errorsList);
		}
		if (buttonWarningsItem.Checked)
		{
			list.AddRange(warningsList);
		}
		if (buttonInformationMessagesItem.Checked)
		{
			list.AddRange(informationMessagesList);
		}
		if (buttonReportRenderingMessagesItem.Checked)
		{
			list.AddRange(reportRenderingMessagesList);
		}
		List<Control> list2 = new List<Control>();
		pageCount = (int)Math.Floor((float)list.Count / (float)range);
		int num = pageIndex * range + 1;
		int num2 = pageIndex * range + range;
		if (num2 > list.Count)
		{
			num2 = list.Count;
		}
		if (num > list.Count && list.Count != 0)
		{
			if (list.Count % range == 0)
			{
				pageIndex = list.Count / range - 1;
			}
			else
			{
				pageIndex = list.Count / range;
			}
			num = pageIndex * range + 1;
		}
		int num3 = (int)Math.Ceiling(6.0 * StiScale.Y);
		int num4 = 0;
		for (int i = num; i <= num2; i++)
		{
			StiCheck check = list[i - 1];
			StiCheckControl stiCheckControl = new StiCheckControl(report, check, i, this, ((Control)listBoxItems).Width - num3 - SystemInformation.VerticalScrollBarWidth);
			((Control)stiCheckControl).Location = new Point(0, num4);
			num4 += ((Control)stiCheckControl).Height + num3;
			list2.Add((Control)(object)stiCheckControl);
			Line obj = new Line
			{
				DashStyle = (DashStyle)(!StiUX.IsDark)
			};
			((Control)obj).ForeColor = (StiUX.IsDark ? StiUX.Separator : Color.LightGray);
			((Control)obj).Location = new Point(SystemInformation.VerticalScrollBarWidth, num4 - num3 / 2);
			((Control)obj).Size = new Size(((Control)stiCheckControl).Width - SystemInformation.VerticalScrollBarWidth, 1);
			list2.Add((Control)(object)obj);
		}
		((Control)listBoxItems).SuspendLayout();
		((Control)listBoxItems).Controls.Clear();
		((ScrollProperties)((ScrollableControl)listBoxItems).VerticalScroll).Value = 0;
		((Control)listBoxItems).PerformLayout();
		foreach (Control item in list2)
		{
			((Control)listBoxItems).Controls.Add(item);
		}
		((Control)listBoxItems).ResumeLayout();
		((Control)listBoxItems).Visible = list2.Count > 0;
		((Control)textBlockInfo).Visible = list2.Count == 0;
		((Control)panelButtons).Visible = list.Count > range;
		((Control)panelButtons1).Visible = ((Control)panelButtons).Visible;
		((Control)textBlockRange).Text = $"{num} - {num2}";
		StiButtonUX stiButtonUX = firstButton;
		bool enabled = (((Control)prevButton).Enabled = pageIndex > 0);
		((Control)stiButtonUX).Enabled = enabled;
		StiButtonUX stiButtonUX2 = lastButton;
		enabled = (((Control)nextButton).Enabled = num2 < list.Count);
		((Control)stiButtonUX2).Enabled = enabled;
		toolStripError.Visible = true;
	}

	public void CrearResults()
	{
		errorsList = null;
		warningsList = null;
		informationMessagesList = null;
		reportRenderingMessagesList = null;
	}

	private void Localize()
	{
		((Control)textBlockInfo).Text = StiLocalization.Get("Report", "Checking");
		buttonErrorsItem.Text = StiLocalization.Get("Report", "Errors");
		buttonWarningsItem.Text = StiLocalization.Get("Report", "Warnings");
		buttonInformationMessagesItem.Text = StiLocalization.Get("Report", "InformationMessages");
		buttonReportRenderingMessagesItem.Text = StiLocalization.Get("Report", "ReportRenderingMessages");
		buttonSettings.Text = StiLocalization.Get("Export", "Settings");
	}

	private void SetSettings(ButtonItem button, string key)
	{
		StiSettings.Load();
		StiSettings.Set("CheckSettings", key, button.Checked);
		StiSettings.Save();
	}

	internal void RemoveCheckControl(StiCheckControl control, StiCheck check)
	{
		if (errorsList != null && errorsList.Contains(check))
		{
			errorsList.Remove(check);
		}
		if (warningsList != null && warningsList.Contains(check))
		{
			warningsList.Remove(check);
		}
		if (informationMessagesList != null && informationMessagesList.Contains(check))
		{
			informationMessagesList.Remove(check);
		}
		if (reportRenderingMessagesList != null && reportRenderingMessagesList.Contains(check))
		{
			reportRenderingMessagesList.Remove(check);
		}
		((Control)control).Enabled = false;
	}

	protected override void OnResize(EventArgs e)
	{
		((UserControl)this).OnResize(e);
		((Control)listBoxItems).Width = ((Control)this).Width;
	}

	private void ButtonErrors_CheckedChanged(object sender, EventArgs e)
	{
		if (lockButton)
		{
			SetSettings(buttonErrorsItem, "ShowErrors");
			if (errorsList.Count != 0)
			{
				BuildVisibleList();
			}
		}
	}

	private void ButtonWarnings_CheckedChanged(object sender, EventArgs e)
	{
		if (lockButton)
		{
			SetSettings(buttonWarningsItem, "ShowWarnings");
			if (warningsList.Count != 0)
			{
				BuildVisibleList();
			}
		}
	}

	private void ButtonInformationMessages_CheckedChanged(object sender, EventArgs e)
	{
		if (lockButton)
		{
			SetSettings(buttonInformationMessagesItem, "ShowInformationMessages");
			if (informationMessagesList.Count != 0)
			{
				BuildVisibleList();
			}
		}
	}

	private void ButtonReportRenderingMessages_CheckedChanged(object sender, EventArgs e)
	{
		if (lockButton)
		{
			SetSettings(buttonReportRenderingMessagesItem, "ReportRenderingMessages");
			if (reportRenderingMessagesList.Count != 0)
			{
				BuildVisibleList();
			}
		}
	}

	private void ButtonFirst_Click(object sender, EventArgs e)
	{
		pageIndex = 0;
		BuildVisibleList();
	}

	private void ButtonLast_Click(object sender, EventArgs e)
	{
		pageIndex = pageCount;
		BuildVisibleList();
	}

	private void ButtonPrevious_Click(object sender, EventArgs e)
	{
		pageIndex--;
		BuildVisibleList();
	}

	private void ButtonNext_Click(object sender, EventArgs e)
	{
		pageIndex++;
		BuildVisibleList();
	}

	private void ButtonSettings_Click(object sender, EventArgs e)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		StiCheckSettingsForm stiCheckSettingsForm = new StiCheckSettingsForm();
		try
		{
			if (((Control)this).Parent is GroupPanel groupPanel && ((Control)groupPanel).Parent is StiPopupForm stiPopupForm)
			{
				stiPopupForm.ClosePopupForm();
			}
			((Form)stiCheckSettingsForm).Owner = ((ContainerControl)this).ParentForm;
			((Form)stiCheckSettingsForm).ShowDialog();
		}
		finally
		{
			((IDisposable)stiCheckSettingsForm)?.Dispose();
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Expected O, but got Unknown
		//IL_09a2: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		toolTip = new ToolTip(components);
		superTooltip = new SuperTooltip();
		listBoxItems = new StiPanelUX();
		textBlockInfo = new StiLabelUX();
		toolStripError = new Bar();
		buttonShowSettings = new ButtonItem();
		buttonErrorsItem = new ButtonItem();
		buttonWarningsItem = new ButtonItem();
		buttonInformationMessagesItem = new ButtonItem();
		buttonReportRenderingMessagesItem = new ButtonItem();
		buttonSettings = new ButtonItem();
		buttonCheck = new ButtonItem();
		panelButtons = new StiGroupPanelUX();
		lastButton = new StiButtonUX();
		nextButton = new StiButtonUX();
		prevButton = new StiButtonUX();
		firstButton = new StiButtonUX();
		textBlockRange = new StiLabelUX();
		panelButtons1 = new StiPanelUX();
		((ISupportInitialize)toolStripError).BeginInit();
		((Control)panelButtons).SuspendLayout();
		((Control)this).SuspendLayout();
		((ScrollableControl)listBoxItems).AutoScroll = true;
		((Control)listBoxItems).Dock = (DockStyle)5;
		((Control)listBoxItems).Location = new Point(0, 41);
		((Control)listBoxItems).Name = "listBoxItems";
		((Control)listBoxItems).Size = new Size(686, 421);
		((Control)listBoxItems).TabIndex = 8;
		((Control)listBoxItems).Visible = false;
		((Control)textBlockInfo).Dock = (DockStyle)5;
		((Control)textBlockInfo).Location = new Point(0, 41);
		((Control)textBlockInfo).Name = "textBlockInfo";
		((Control)textBlockInfo).Size = new Size(686, 421);
		((Control)textBlockInfo).TabIndex = 2;
		((Control)textBlockInfo).Text = "No Issues";
		((Label)textBlockInfo).TextAlign = (ContentAlignment)32;
		((Control)toolStripError).AccessibleDescription = "toolStrip1 (toolStripError)";
		((Control)toolStripError).AccessibleName = "toolStrip1";
		((Control)toolStripError).AccessibleRole = (AccessibleRole)22;
		toolStripError.BarType = eBarType.MenuBar;
		((Control)toolStripError).Dock = (DockStyle)1;
		toolStripError.DockTabStripHeight = 50;
		((Control)toolStripError).Font = new Font("Segoe UI", 9f);
		toolStripError.Items.AddRange(new BaseItem[1] { buttonShowSettings });
		toolStripError.Location = new Point(0, 0);
		toolStripError.Name = "toolStripError";
		toolStripError.PaddingBottom = 6;
		toolStripError.PaddingLeft = 6;
		toolStripError.PaddingRight = 6;
		toolStripError.PaddingTop = 6;
		toolStripError.RoundCorners = false;
		toolStripError.Size = new Size(686, 41);
		toolStripError.Stretch = true;
		toolStripError.Style = eDotNetBarStyle.Office2010;
		toolStripError.TabIndex = 15;
		toolStripError.TabStop = false;
		((Control)toolStripError).Text = "toolStrip1";
		buttonShowSettings.AccessibleRole = (AccessibleRole)43;
		buttonShowSettings.AutoExpandOnClick = true;
		buttonShowSettings.ImagePaddingHorizontal = 26;
		buttonShowSettings.ImagePaddingVertical = 12;
		buttonShowSettings.ItemAlignment = eItemAlignment.Far;
		buttonShowSettings.Name = "buttonShowSettings";
		buttonShowSettings.SubItems.AddRange(new BaseItem[5] { buttonErrorsItem, buttonWarningsItem, buttonInformationMessagesItem, buttonReportRenderingMessagesItem, buttonSettings });
		buttonErrorsItem.AutoCheckOnClick = true;
		buttonErrorsItem.GlobalItem = false;
		buttonErrorsItem.ImagePaddingHorizontal = 16;
		buttonErrorsItem.ImagePaddingVertical = 12;
		buttonErrorsItem.Name = "buttonErrorsItem";
		buttonErrorsItem.Text = "Errors";
		buttonErrorsItem.CheckedChanged += ButtonErrors_CheckedChanged;
		buttonWarningsItem.AutoCheckOnClick = true;
		buttonWarningsItem.GlobalItem = false;
		buttonWarningsItem.ImagePaddingHorizontal = 16;
		buttonWarningsItem.ImagePaddingVertical = 12;
		buttonWarningsItem.Name = "buttonWarningsItem";
		buttonWarningsItem.Text = "Warnings";
		buttonWarningsItem.CheckedChanged += ButtonWarnings_CheckedChanged;
		buttonInformationMessagesItem.AutoCheckOnClick = true;
		buttonInformationMessagesItem.GlobalItem = false;
		buttonInformationMessagesItem.ImagePaddingHorizontal = 16;
		buttonInformationMessagesItem.ImagePaddingVertical = 12;
		buttonInformationMessagesItem.Name = "buttonInformationMessagesItem";
		buttonInformationMessagesItem.Text = "InformationMessages";
		buttonInformationMessagesItem.CheckedChanged += ButtonInformationMessages_CheckedChanged;
		buttonReportRenderingMessagesItem.AutoCheckOnClick = true;
		buttonReportRenderingMessagesItem.GlobalItem = false;
		buttonReportRenderingMessagesItem.ImagePaddingHorizontal = 16;
		buttonReportRenderingMessagesItem.ImagePaddingVertical = 12;
		buttonReportRenderingMessagesItem.Name = "buttonReportRenderingMessagesItem";
		buttonReportRenderingMessagesItem.Text = "ReportRenderingMessages";
		buttonReportRenderingMessagesItem.CheckedChanged += ButtonReportRenderingMessages_CheckedChanged;
		buttonSettings.BeginGroup = true;
		buttonSettings.ImagePaddingHorizontal = 16;
		buttonSettings.ImagePaddingVertical = 12;
		buttonSettings.ItemAlignment = eItemAlignment.Far;
		buttonSettings.Name = "buttonSettings";
		buttonSettings.Text = "Settings";
		buttonSettings.Click += ButtonSettings_Click;
		buttonCheck.Name = "buttonCheck";
		((Control)panelButtons).Controls.Add((Control)(object)lastButton);
		((Control)panelButtons).Controls.Add((Control)(object)nextButton);
		((Control)panelButtons).Controls.Add((Control)(object)prevButton);
		((Control)panelButtons).Controls.Add((Control)(object)firstButton);
		((Control)panelButtons).Controls.Add((Control)(object)textBlockRange);
		((Control)panelButtons).Dock = (DockStyle)2;
		panelButtons.LinePosition = StiLinePosition.Top;
		panelButtons.LineStyle = StiLineStyle.Dot;
		((Control)panelButtons).Location = new Point(0, 465);
		((Control)panelButtons).Name = "panelButtons";
		((Control)panelButtons).Size = new Size(686, 36);
		((Control)panelButtons).TabIndex = 17;
		((Control)panelButtons).Visible = false;
		((Control)lastButton).Anchor = (AnchorStyles)9;
		((Control)lastButton).Location = new Point(427, 7);
		((Control)lastButton).Name = "lastButton";
		((Control)lastButton).Size = new Size(21, 23);
		((Control)lastButton).TabIndex = 6;
		((Control)lastButton).Click += ButtonLast_Click;
		((Control)nextButton).Anchor = (AnchorStyles)9;
		((Control)nextButton).Location = new Point(400, 7);
		((Control)nextButton).Name = "nextButton";
		((Control)nextButton).Size = new Size(21, 23);
		((Control)nextButton).TabIndex = 5;
		((Control)nextButton).Click += ButtonNext_Click;
		((Control)prevButton).Location = new Point(265, 7);
		((Control)prevButton).Name = "prevButton";
		((Control)prevButton).Size = new Size(21, 23);
		((Control)prevButton).TabIndex = 3;
		((Control)prevButton).Click += ButtonPrevious_Click;
		((Control)firstButton).Location = new Point(238, 7);
		((Control)firstButton).Name = "firstButton";
		((Control)firstButton).Size = new Size(21, 23);
		((Control)firstButton).TabIndex = 2;
		((Control)firstButton).Click += ButtonFirst_Click;
		((Control)textBlockRange).Anchor = (AnchorStyles)13;
		((Control)textBlockRange).BackColor = Color.White;
		((Control)textBlockRange).Location = new Point(292, 7);
		((Control)textBlockRange).Name = "textBlockRange";
		((Control)textBlockRange).Size = new Size(102, 23);
		((Control)textBlockRange).TabIndex = 1;
		((Control)textBlockRange).Text = "1-100";
		((Label)textBlockRange).TextAlign = (ContentAlignment)32;
		((Control)panelButtons1).Dock = (DockStyle)2;
		((Control)panelButtons1).Location = new Point(0, 462);
		((Control)panelButtons1).Name = "panelButtons1";
		((Control)panelButtons1).Size = new Size(686, 3);
		((Control)panelButtons1).TabIndex = 18;
		((Control)panelButtons1).Visible = false;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)textBlockInfo);
		((Control)this).Controls.Add((Control)(object)listBoxItems);
		((Control)this).Controls.Add((Control)(object)panelButtons1);
		((Control)this).Controls.Add((Control)(object)panelButtons);
		((Control)this).Controls.Add((Control)(object)toolStripError);
		((Control)this).Margin = new Padding(0);
		((Control)this).Name = "StiCheckStatusControl";
		((Control)this).Size = new Size(686, 501);
		((ISupportInitialize)toolStripError).EndInit();
		((Control)panelButtons).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
