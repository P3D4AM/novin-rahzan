using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Check;
using Stimulsoft.Report.Design.Check.Images;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Design.Check;

[ToolboxItem(false)]
public class StiCheckControl : UserControl
{
	private int gap = 8;

	private PictureBox gridView;

	private StiReport report;

	private StiCheckStatusControl checksControl;

	private SuperTooltipInfo info;

	private bool locked;

	private StiCheck check;

	private IContainer components;

	private Panel panelImage;

	private PictureBox pictureBoxMessage;

	private Panel panelActions;

	private Panel panelText;

	private Label textBlockShortMessage;

	private Label textBlockLongMessage;

	private CheckBox cbHide;

	private void DoTheming()
	{
		((Control)this).BackColor = StiUX.Background;
	}

	private void ReloceControlsPanelActions()
	{
		int num = ((Control)panelActions).Width - StiScale.XXI(12.0);
		for (int i = 1; i < ((ArrangedElementCollection)((Control)panelActions).Controls).Count; i++)
		{
			Control val = ((Control)panelActions).Controls[i];
			num -= val.Width + StiScale.XXI(6.0);
			val.Location = new Point(num, 0);
		}
		((Control)cbHide).Location = new Point(gap, 3);
	}

	private static void CreateImage(StiCheck check, out Bitmap pixelElement, out Bitmap pixelHighlightedElement)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		pixelElement = null;
		pixelHighlightedElement = null;
		if (check.PreviewVisible)
		{
			check.CreatePreviewImage(out var elementImage, out var highlightedElementImage);
			if (elementImage != null)
			{
				pixelElement = new Bitmap(elementImage);
			}
			if (highlightedElementImage != null)
			{
				pixelHighlightedElement = new Bitmap(highlightedElementImage);
			}
		}
	}

	internal void SetCheckNumber(int number)
	{
		int num = ((Control)textBlockShortMessage).Text.IndexOf(".");
		if (num != -1)
		{
			((Control)textBlockShortMessage).Text = $"{number}{((Control)textBlockShortMessage).Text.Remove(0, num)}";
		}
	}

	private string GetActionDescription(Stimulsoft.Report.Check.StiAction action, StiCheck check)
	{
		if (action is StiApplyGeneralTextFormat)
		{
			return string.Format(action.Description, check.ElementName);
		}
		return action.Description;
	}

	protected override void OnResize(EventArgs e)
	{
		((UserControl)this).OnResize(e);
		ReloceControlsPanelActions();
	}

	private void PanelActions_ControlAdded(object sender, ControlEventArgs e)
	{
		ReloceControlsPanelActions();
	}

	private void cbEnabled_CheckedChanged(object sender, EventArgs e)
	{
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Expected O, but got Unknown
		if (locked)
		{
			return;
		}
		locked = true;
		if (checksControl != null)
		{
			Type type = check.GetType();
			StiSettings.Load();
			check.Enabled = !cbHide.Checked;
			StiSettings.Set("CheckSettings", type.Name, !cbHide.Checked);
			StiSettings.Save();
			foreach (Control item in (ArrangedElementCollection)((Control)checksControl.listBoxItems).Controls)
			{
				Control val = item;
				StiCheckControl stiCheckControl = val as StiCheckControl;
				if (stiCheckControl?.check.GetType() == type)
				{
					stiCheckControl.cbHide.Checked = cbHide.Checked;
				}
			}
		}
		locked = false;
	}

	private void This_MouseDoubleClick(object sender, MouseEventArgs e)
	{
		Button_Click(null, new EventArgs());
	}

	private void Button_Click(object sender, EventArgs e)
	{
		CompilerError error = ((StiCompilationErrorCheck)check).Error;
		StiDesignerControl stiDesignerControl = report.Designer?.DesignerControl as StiDesignerControl;
		if (error != null && stiDesignerControl != null)
		{
			bool flag = true;
			if (stiDesignerControl.Services.GetService(typeof(StiDesignerConfigService)) is StiDesignerConfigService stiDesignerConfigService)
			{
				flag = stiDesignerConfigService.CodeTabVisible;
			}
			if (stiDesignerControl.TabControl.SelectedTabIndex != stiDesignerControl.PageCodeIndex && flag)
			{
				stiDesignerControl.TabControl.SelectedTabIndex = stiDesignerControl.PageCodeIndex;
			}
			stiDesignerControl.GotoEditor(error.Line - 1, error.Column - 1);
		}
		StiReportChecker.InvokeGoToCode();
	}

	private void SuperTooltip_BeforeTooltipDisplay(object sender, SuperTooltipEventArgs e)
	{
		if (e.TooltipInfo != info || info.BodyImage != null)
		{
			return;
		}
		((Control)this).Cursor = Cursors.WaitCursor;
		CreateImage(check, out var pixelElement, out var pixelHighlightedElement);
		if (pixelHighlightedElement != null)
		{
			info.BodyImage = (Image)(object)pixelHighlightedElement;
			if (e.Location.Y + ((Image)pixelHighlightedElement).Height > Screen.FromControl((Control)(object)this).Bounds.Height)
			{
				e.Location.Y -= ((Image)pixelHighlightedElement).Height + StiScale.I(45);
				if (e.Location.Y < 0)
				{
					e.Location.Y = 0;
				}
			}
		}
		else if (pixelElement != null)
		{
			info.BodyImage = (Image)(object)pixelElement;
			if (e.Location.Y + ((Image)pixelElement).Height > Screen.FromControl((Control)(object)this).Bounds.Height)
			{
				e.Location.Y -= ((Image)pixelElement).Height + StiScale.I(45);
				if (e.Location.Y < 0)
				{
					e.Location.Y = 0;
				}
			}
		}
		((Control)this).Cursor = Cursors.Default;
	}

	internal StiCheckControl(StiReport report, StiCheck check, int number, StiCheckStatusControl checkStatusControl, int width)
	{
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Unknown result type (might be due to invalid IL or missing references)
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Expected O, but got Unknown
		//IL_0423: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0500: Expected O, but got Unknown
		//IL_0641: Unknown result type (might be due to invalid IL or missing references)
		//IL_064b: Expected O, but got Unknown
		//IL_0658: Unknown result type (might be due to invalid IL or missing references)
		//IL_0662: Expected O, but got Unknown
		//IL_066f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0679: Expected O, but got Unknown
		//IL_0686: Unknown result type (might be due to invalid IL or missing references)
		//IL_0690: Expected O, but got Unknown
		//IL_069d: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a7: Expected O, but got Unknown
		StiCheckControl stiCheckControl = this;
		InitializeComponent();
		DoTheming();
		gap = (int)Math.Ceiling((double)gap * StiScale.X);
		double step = StiScale.Step;
		((Control)pictureBoxMessage).Size = new Size((int)Math.Ceiling(32.0 * step), (int)Math.Ceiling(32.0 * step));
		this.check = check;
		this.report = report;
		checksControl = checkStatusControl;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(number + ". ");
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiCheckControlOffice2013Resource));
		string stepName = StiScale.StepName;
		switch (check.Status)
		{
		case StiCheckStatus.Error:
			pictureBoxMessage.Image = StiReportDesignImages.Actions.Error(StiImageSize.Double);
			stringBuilder.Append(StiLocalizationExt.Get("CheckGlobal", "Error"));
			break;
		case StiCheckStatus.Information:
			pictureBoxMessage.Image = StiReportDesignImages.Actions.Information(StiImageSize.Double);
			stringBuilder.Append(StiLocalizationExt.Get("CheckGlobal", "Information"));
			break;
		case StiCheckStatus.Warning:
			pictureBoxMessage.Image = StiReportDesignImages.Actions.Warning(StiImageSize.Double);
			stringBuilder.Append(StiLocalizationExt.Get("CheckGlobal", "Warning"));
			break;
		case StiCheckStatus.ReportRenderingMessage:
			pictureBoxMessage.Image = StiReportDesignImages.Actions.RenderingMessage(StiImageSize.Double);
			stringBuilder.Append(StiLocalizationExt.Get("CheckGlobal", "RenderingMessage"));
			break;
		}
		stringBuilder.Append(check.ShortMessage);
		((Control)textBlockShortMessage).Text = stringBuilder.ToString();
		((Control)textBlockLongMessage).Text = check.LongMessage;
		((Control)panelActions).Width = width - ((Control)panelImage).Width - 1;
		((Control)this).Width = width;
		((Control)textBlockShortMessage).MaximumSize = new Size(((Control)this).Width - ((Control)panelImage).Width - gap * 2, 0);
		((Control)textBlockLongMessage).MaximumSize = new Size(((Control)this).Width - ((Control)panelImage).Width - gap * 2, 0);
		((Control)textBlockShortMessage).Location = new Point(gap, gap);
		((Control)textBlockLongMessage).Location = new Point(gap, ((Control)textBlockShortMessage).Height + gap * 2);
		((Control)this).Height = ((Control)textBlockLongMessage).Bottom + ((Control)panelActions).Height + gap * 2;
		((Control)pictureBoxMessage).Location = new Point((((Control)panelImage).Width - ((Control)pictureBoxMessage).Width) / 2 + gap, gap);
		if (check.PreviewVisible)
		{
			gridView = new PictureBox
			{
				SizeMode = (PictureBoxSizeMode)3,
				Image = StiReportDesignImages.Actions.View(),
				Location = new Point(0, StiScale.I3),
				Name = "gridView",
				Size = new Size(StiScale.I(23), StiScale.I(23))
			};
			((Control)panelActions).Controls.Add((Control)(object)gridView);
			info = new SuperTooltipInfo(string.Empty, string.Empty, string.Empty, null, null, eTooltipColor.Gray);
			checkStatusControl.superTooltip.SetSuperTooltip((IComponent)gridView, info);
			checkStatusControl.superTooltip.BeforeTooltipDisplay += SuperTooltip_BeforeTooltipDisplay;
		}
		if (!(check is StiLicenseTrialCheck))
		{
			StiButtonUX stiButtonUX = new StiButtonUX();
			((Control)stiButtonUX).Size = new Size((int)Math.Ceiling(23.0 * StiScale.X), (int)Math.Ceiling(23.0 * step));
			((Control)stiButtonUX).Margin = new Padding((int)Math.Ceiling(2.0 * StiScale.X));
			((ButtonBase)stiButtonUX).UseVisualStyleBackColor = true;
			((ButtonBase)stiButtonUX).Image = StiReportDesignImages.Editor.ClipboardCopy();
			StiButtonUX stiButtonUX2 = stiButtonUX;
			checkStatusControl.toolTip.SetToolTip((Control)(object)stiButtonUX2, StiLocalization.Get("HelpDesigner", "CopyToClipboard"));
			((Control)panelActions).Controls.Add((Control)(object)stiButtonUX2);
			((Control)stiButtonUX2).Click += delegate
			{
				Clipboard.SetDataObject((object)(stiCheckControl.check.ShortMessage + "\r\n" + stiCheckControl.check.LongMessage), true);
			};
		}
		bool flag = false;
		for (int num = check.Actions.Count - 1; num >= 0; num--)
		{
			Stimulsoft.Report.Check.StiAction stiAction = check.Actions[num];
			StiButtonUX stiButtonUX3 = new StiButtonUX();
			((ButtonBase)stiButtonUX3).UseVisualStyleBackColor = true;
			((Control)stiButtonUX3).Text = stiAction.Name;
			((Control)stiButtonUX3).Tag = stiAction;
			((Control)stiButtonUX3).Margin = new Padding(StiScale.I2);
			StiButtonUX stiButtonUX4 = stiButtonUX3;
			Font val = new Font(((Control)stiButtonUX4).Font.FontFamily, 9f);
			try
			{
				((Control)stiButtonUX4).Size = new Size((int)StiReport.GlobalMeasureGraphics.MeasureString(stiAction.Name, val).Width + StiScale.XXI(12.0), StiScale.I(23));
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			if (stiAction is StiGoToCodeAction && check is StiCompilationErrorCheck)
			{
				if (!StiOptions.Designer.IsCodeTabVisible)
				{
					continue;
				}
				if (report.Designer != null && report.Designer.DesignerControl != null)
				{
					flag = true;
					((Control)stiButtonUX4).Click += Button_Click;
				}
			}
			else if (stiAction is StiEditPropertyAction)
			{
				((Control)stiButtonUX4).Click += delegate(object sender, EventArgs e)
				{
					Button val3 = (Button)((sender is Button) ? sender : null);
					Stimulsoft.Report.Check.StiAction stiAction3 = ((Control)val3).Tag as Stimulsoft.Report.Check.StiAction;
					if (check is StiExpressionElementCheck)
					{
						stiAction3.Invoke(report, (check as StiExpressionElementCheck).ElementName, (check as StiExpressionElementCheck).Expression);
					}
					if (check is StiCompilationErrorCheck)
					{
						stiAction3.Invoke(report, (check as StiCompilationErrorCheck).ComponentName, (check as StiCompilationErrorCheck).PropertyName);
					}
					if (check is StiComponentExpressionCheck)
					{
						stiAction3.Invoke(report, (check as StiComponentExpressionCheck).ComponentName, (check as StiComponentExpressionCheck).PropertyName);
					}
					if (check is StiCalculatedColumnRecursionCheck)
					{
						stiAction3.Invoke(report, ((check as StiCalculatedColumnRecursionCheck).Element as StiCalcDataColumn).DataSource.Name, (check as StiCalculatedColumnRecursionCheck).ElementName);
					}
					if (check is StiVariableRecursionCheck)
					{
						stiAction3.Invoke(report, ((check as StiVariableRecursionCheck).Element as StiVariable).Name, (check as StiVariableRecursionCheck).ElementName);
					}
					if (check is StiVariableInitializationCheck)
					{
						stiAction3.Invoke(report, ((check as StiVariableInitializationCheck).Element as StiVariable).Name, (check as StiVariableInitializationCheck).ElementName);
					}
					if (check is StiVariableTypeCheck)
					{
						stiAction3.Invoke(report, ((check as StiVariableTypeCheck).Element as StiVariable).Name, (check as StiVariableTypeCheck).ElementName);
					}
					stiCheckControl.checksControl.RemoveCheckControl(stiCheckControl, check);
				};
			}
			else
			{
				((Control)stiButtonUX4).Click += delegate(object sender, EventArgs e)
				{
					Button val2 = (Button)((sender is Button) ? sender : null);
					Stimulsoft.Report.Check.StiAction stiAction2 = ((Control)val2).Tag as Stimulsoft.Report.Check.StiAction;
					stiAction2.Invoke(report, check.Element, check.ElementName);
					stiCheckControl.checksControl.RemoveCheckControl(stiCheckControl, check);
				};
			}
			checkStatusControl.toolTip.SetToolTip((Control)(object)stiButtonUX4, GetActionDescription(stiAction, check));
			((Control)panelActions).Controls.Add((Control)(object)stiButtonUX4);
			if (flag)
			{
				((Control)panelText).MouseDoubleClick += new MouseEventHandler(This_MouseDoubleClick);
				((Control)panelActions).MouseDoubleClick += new MouseEventHandler(This_MouseDoubleClick);
				((Control)panelImage).MouseDoubleClick += new MouseEventHandler(This_MouseDoubleClick);
				((Control)textBlockShortMessage).MouseDoubleClick += new MouseEventHandler(This_MouseDoubleClick);
				((Control)textBlockLongMessage).MouseDoubleClick += new MouseEventHandler(This_MouseDoubleClick);
			}
		}
		if (check.Status != StiCheckStatus.Error && check.Status != 0 && !(check is StiLicenseTrialCheck))
		{
			((Control)cbHide).Text = StiLocalization.Get("PropertyEnum", "HotkeyPrefixHide");
			checksControl.toolTip.SetToolTip((Control)(object)cbHide, StiLocalizationExt.Get("CheckActions", "Hide"));
		}
		else
		{
			((Control)cbHide).Visible = false;
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
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Expected O, but got Unknown
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Expected O, but got Unknown
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Expected O, but got Unknown
		panelImage = new Panel();
		pictureBoxMessage = new PictureBox();
		panelActions = new Panel();
		cbHide = new CheckBox();
		panelText = new Panel();
		textBlockShortMessage = new Label();
		textBlockLongMessage = new Label();
		((Control)panelImage).SuspendLayout();
		((ISupportInitialize)pictureBoxMessage).BeginInit();
		((Control)panelActions).SuspendLayout();
		((Control)panelText).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)panelImage).Controls.Add((Control)(object)pictureBoxMessage);
		((Control)panelImage).Dock = (DockStyle)3;
		((Control)panelImage).Name = "panelImage";
		((Control)panelImage).Size = new Size(66, 0);
		((Control)panelImage).TabIndex = 0;
		((Control)pictureBoxMessage).Name = "pictureBoxMessage";
		((Control)pictureBoxMessage).Size = new Size(32, 32);
		pictureBoxMessage.TabIndex = 0;
		pictureBoxMessage.TabStop = false;
		((Control)panelActions).ControlAdded += new ControlEventHandler(PanelActions_ControlAdded);
		((Control)panelActions).Controls.Add((Control)(object)cbHide);
		((Control)panelActions).Dock = (DockStyle)2;
		((Control)panelActions).Name = "panelActions";
		((Control)panelActions).Size = new Size(0, 43);
		((Control)panelActions).TabIndex = 8;
		((Control)cbHide).AutoSize = true;
		((Control)cbHide).Name = "cbHide";
		((Control)cbHide).Size = new Size(48, 17);
		((Control)cbHide).TabIndex = 0;
		((Control)cbHide).Text = "Hide";
		((ButtonBase)cbHide).UseVisualStyleBackColor = true;
		cbHide.CheckedChanged += cbEnabled_CheckedChanged;
		((Control)panelText).AutoSize = true;
		((Control)panelText).Controls.Add((Control)(object)textBlockShortMessage);
		((Control)panelText).Controls.Add((Control)(object)textBlockLongMessage);
		((Control)panelText).Dock = (DockStyle)5;
		((Control)panelText).Name = "panelText";
		((Control)panelText).TabIndex = 9;
		((Control)textBlockShortMessage).AutoSize = true;
		((Control)textBlockShortMessage).Font = new Font("Segoe UI", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)204);
		((Control)textBlockShortMessage).Name = "textBlockShortMessage";
		((Control)textBlockShortMessage).TabIndex = 2;
		((Control)textBlockShortMessage).BackColor = Color.Transparent;
		((Control)textBlockShortMessage).Text = "1.   Short Message";
		((Control)textBlockLongMessage).AutoSize = true;
		((Control)textBlockLongMessage).Font = new Font("Segoe UI", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)textBlockLongMessage).Name = "textBlockLongMessage";
		((Control)textBlockLongMessage).TabIndex = 3;
		((Control)textBlockLongMessage).BackColor = Color.Transparent;
		((Control)textBlockLongMessage).Text = "It's a very long description";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).AutoSize = true;
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)panelText);
		((Control)this).Controls.Add((Control)(object)panelActions);
		((Control)this).Controls.Add((Control)(object)panelImage);
		((Control)this).Name = "StiCheckControl";
		((Control)panelImage).ResumeLayout(false);
		((ISupportInitialize)pictureBoxMessage).EndInit();
		((Control)panelActions).ResumeLayout(false);
		((Control)panelActions).PerformLayout();
		((Control)panelText).ResumeLayout(false);
		((Control)panelText).PerformLayout();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
