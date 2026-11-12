using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Server;
using Stimulsoft.Controls;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design.Editors;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Helper;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Maps.Helpers;

namespace Stimulsoft.Report.Design.Dictionary;

public class StiResourceEditForm : StiFormUX
{
	private string cachedContentText;

	private bool isEditMode;

	private string baseName;

	private StiReport report;

	private string oldValue;

	private byte[] content;

	private IContainer components;

	private StiButtonUX buttonOk;

	private StiButtonUX buttonCancel;

	private ErrorProvider errorProvider;

	private StiButtonPanelUX groupLineButtons;

	private StiButtonUX buttonSaveCopy;

	private Panel panelResource;

	private StiLabelUX labelAlias;

	public StiTextBox textBoxAlias;

	private StiLabelUX labelName;

	public StiTextBox textBoxName;

	private Panel panelContent;

	private PictureBox imageFileType;

	private StiLabelUX labelContentInfo;

	private ImageList imageListImages;

	private StiButtonUX buttonView;

	private StiButtonUX buttonRemove;

	private StiButtonUX buttonOpen;

	private StiButtonUX buttonSave;

	private Panel panelFont;

	private Panel panelReport;

	private StiButtonUX buttonEdit;

	private RichTextBox textBoxRich;

	private ToolTip toolTip;

	public StiCheckBoxUX checkBoxAvailableInTheViewer;

	private bool IsImageType => ResourceType == StiResourceType.Image;

	private bool IsMapType => ResourceType == StiResourceType.Map;

	private bool IsTextType
	{
		get
		{
			if (ResourceType != StiResourceType.Txt && ResourceType != StiResourceType.Json && ResourceType != StiResourceType.Csv && ResourceType != StiResourceType.Xml)
			{
				return ResourceType == StiResourceType.Xsd;
			}
			return true;
		}
	}

	private bool IsRtfType => ResourceType == StiResourceType.Rtf;

	private bool IsDataType
	{
		get
		{
			if (ResourceType != StiResourceType.Json && ResourceType != StiResourceType.Csv && ResourceType != StiResourceType.Xml && ResourceType != StiResourceType.Dbf)
			{
				return ResourceType == StiResourceType.Excel;
			}
			return true;
		}
	}

	private bool IsFontType
	{
		get
		{
			if (ResourceType != StiResourceType.FontOtf && ResourceType != StiResourceType.FontTtc)
			{
				return ResourceType == StiResourceType.FontTtf;
			}
			return true;
		}
	}

	private bool IsReportType
	{
		get
		{
			if (ResourceType != StiResourceType.Report)
			{
				return ResourceType == StiResourceType.ReportSnapshot;
			}
			return true;
		}
	}

	public bool CopyModeActivated { get; private set; }

	public StiResourceType ResourceType { get; private set; }

	public byte[] Content
	{
		get
		{
			return content;
		}
		private set
		{
			content = value;
			((Control)buttonSave).Enabled = value != null;
		}
	}

	public StiResourceEditForm(StiReport report, bool isEditMode, StiResourceType resourceType, byte[] content)
	{
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Expected O, but got Unknown
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		DoTheming();
		DoScaling();
		double step = StiScale.Step;
		((Form)this).HelpButton = true;
		((Control)imageFileType).Size = new Size((int)Math.Ceiling(32.0 * step), (int)Math.Ceiling(32.0 * step));
		((Control)imageFileType).Location = new Point((((Control)panelContent).Height - ((Control)imageFileType).Height) / 2, (((Control)panelContent).Height - ((Control)imageFileType).Height) / 2);
		if (!((Component)(object)this).DesignMode)
		{
			ResourceType = StiResourceType.Image;
			this.report = report;
			this.isEditMode = isEditMode;
			Localize();
			((Form)this).ShowIcon = false;
			((Control)buttonSaveCopy).Visible = isEditMode;
			((Control)buttonSave).Visible = isEditMode;
			if (!isEditMode)
			{
				StiButtonUX stiButtonUX = buttonRemove;
				((Control)stiButtonUX).Top = ((Control)stiButtonUX).Top - StiScale.I(26);
				StiButtonUX stiButtonUX2 = buttonView;
				((Control)stiButtonUX2).Top = ((Control)stiButtonUX2).Top - StiScale.I(26);
				StiButtonUX stiButtonUX3 = buttonEdit;
				((Control)stiButtonUX3).Top = ((Control)stiButtonUX3).Top - StiScale.I(26);
			}
			StiFormSettings.Load((Form)(object)this, skipLocation: false, skipSize: true);
			SetContent(resourceType, content);
			((Form)this).FormClosed += new FormClosedEventHandler(This_FormClosed);
		}
	}

	private void Localize()
	{
		((Control)labelName).Text = Loc.GetCleaned("Report", "LabelName");
		((Control)labelAlias).Text = Loc.GetCleaned("Report", "LabelAlias");
		((Control)panelResource).Text = StiLocalization.Get("PropertyMain", "Resource");
		((Control)checkBoxAvailableInTheViewer).Text = StiLocalization.Get("PropertyMain", "AvailableInTheViewer");
		((Control)buttonSaveCopy).Text = StiLocalization.Get("Buttons", "SaveCopy");
		((Control)buttonOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		toolTip.SetToolTip((Control)(object)buttonOpen, StiLocalization.Get("Buttons", "Open"));
		toolTip.SetToolTip((Control)(object)buttonSave, StiLocalization.Get("Buttons", "Save"));
		toolTip.SetToolTip((Control)(object)buttonRemove, StiLocalization.Get("Buttons", "Remove"));
		toolTip.SetToolTip((Control)(object)buttonView, StiLocalization.Get("Cloud", "ButtonView"));
		toolTip.SetToolTip((Control)(object)buttonEdit, StiLocalization.Get("QueryBuilder", "Edit"));
		StiButtonSizeHelper.CheckSizeOfButton((Button)(object)buttonSaveCopy);
	}

	public static void SaveResource(StiResourceType resourceType, byte[] buffer, string name)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Invalid comparison between Unknown and I4
		if (buffer == null)
		{
			return;
		}
		string filter = StiResourceTypeHelper.GetFilter(resourceType);
		if (filter == null)
		{
			return;
		}
		SaveFileDialog val = new SaveFileDialog();
		try
		{
			((FileDialog)val).FileName = name;
			((FileDialog)val).Filter = filter;
			((FileDialog)val).RestoreDirectory = true;
			if ((int)((CommonDialog)val).ShowDialog() == 1)
			{
				try
				{
					File.WriteAllBytes(((FileDialog)val).FileName, buffer);
					return;
				}
				catch (Exception exception)
				{
					StiExceptionProvider.Show(exception);
					return;
				}
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public DataSet GetContentDataSet()
	{
		if (Content == null || !IsDataType)
		{
			return null;
		}
		return StiResourceArrayToDataSet.Get(ResourceType, Content);
	}

	public byte[] GetContentImage()
	{
		if (Content == null || ResourceType != 0)
		{
			return null;
		}
		return Content;
	}

	public string GetContentText()
	{
		if (Content == null || (!IsTextType && !IsRtfType && !IsMapType))
		{
			return null;
		}
		try
		{
			if (cachedContentText == null)
			{
				cachedContentText = StiGZipHelper.ConvertByteArrayToString(Content);
			}
			return cachedContentText;
		}
		catch
		{
		}
		return null;
	}

	public void SetContentText(string value)
	{
		if (Content == null || (!IsTextType && !IsRtfType && !IsMapType))
		{
			return;
		}
		try
		{
			SetContent(ResourceType, StiGZipHelper.ConvertStringToByteArray(value));
		}
		catch
		{
		}
	}

	internal void SetBaseName(string name)
	{
		StiTextBox stiTextBox = textBoxName;
		string text2 = (((Control)textBoxAlias).Text = name);
		string text4 = (((Control)stiTextBox).Text = text2);
		baseName = text4;
	}

	private void UpdateNameAndAlias(string name)
	{
		if (baseName == ((Control)textBoxName).Text && baseName == ((Control)textBoxAlias).Text)
		{
			SetBaseName(name);
		}
	}

	private void SetContent(StiResourceType type, byte[] content)
	{
		ResourceType = type;
		Content = content;
		cachedContentText = null;
		RefreshInfo();
	}

	private void ClearContent()
	{
		Content = null;
		cachedContentText = null;
		RefreshInfo();
	}

	private void RefreshInfo()
	{
		bool flag = Content != null;
		((Control)buttonRemove).Enabled = flag;
		((Control)buttonView).Enabled = flag && (IsDataType || IsReportType);
		((Control)buttonEdit).Enabled = flag && (IsTextType || IsRtfType || (IsMapType && StiAccountSettings.CurrentAccount != null));
		((Control)checkBoxAvailableInTheViewer).Enabled = flag && (ResourceType == StiResourceType.Pdf || ResourceType == StiResourceType.Rtf || ResourceType == StiResourceType.Word || ResourceType == StiResourceType.Excel || ResourceType == StiResourceType.Txt || ResourceType == StiResourceType.Image || ResourceType == StiResourceType.Report || ResourceType == StiResourceType.ReportSnapshot);
		Rectangle clientRectangle = ((Control)panelResource).ClientRectangle;
		clientRectangle.X = 0;
		clientRectangle.Y = 0;
		clientRectangle.Inflate(-StiScale.I4, -StiScale.I4);
		RichTextBox obj = textBoxRich;
		Panel obj2 = panelFont;
		Point point = (((Control)panelReport).Location = clientRectangle.Location);
		Point location2 = (((Control)obj2).Location = point);
		((Control)obj).Location = location2;
		RichTextBox obj3 = textBoxRich;
		Panel obj4 = panelFont;
		Size size2 = (((Control)panelReport).Size = clientRectangle.Size);
		Size size4 = (((Control)obj4).Size = size2);
		((Control)obj3).Size = size4;
		((Control)textBoxRich).Width = ((Control)buttonOpen).Left - StiScale.I4 - ((Control)textBoxRich).Left;
		if (IsFontType && flag)
		{
			((Control)panelFont).Visible = true;
			((Control)panelFont).Invalidate();
		}
		else
		{
			((Control)panelFont).Visible = false;
		}
		if (IsRtfType && flag)
		{
			((Control)textBoxRich).Visible = true;
			textBoxRich.Rtf = GetContentText();
		}
		else
		{
			((Control)textBoxRich).Visible = false;
		}
		if (IsReportType && flag)
		{
			((Control)panelReport).Visible = true;
			((Control)panelReport).Invalidate();
		}
		else
		{
			((Control)panelReport).Visible = false;
		}
		if (flag && !IsImageType && !IsFontType && !IsReportType)
		{
			((Control)panelContent).Visible = true;
			((Control)labelContentInfo).Text = GetFileInfo();
			imageFileType.Image = GetResourceIcon();
		}
		else
		{
			((Control)panelContent).Visible = false;
		}
		((Control)panelResource).Invalidate();
	}

	private Image GetResourceIcon()
	{
		if (Content == null)
		{
			return null;
		}
		return imageListImages.Images[ResourceType.ToString()];
	}

	private string GetFileInfo()
	{
		if (Content == null)
		{
			return string.Empty;
		}
		return $"{ResourceType}, {StiHumanReadableHelper.GetSize(Content.Length)}";
	}

	private void DoTheming()
	{
		if (StiUX.IsDark)
		{
			((Control)panelContent).BackColor = StiUX.ButtonBackground;
			((Control)panelResource).BackColor = StiUX.BackgroundDark;
		}
	}

	private void DoScaling()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		double step = StiScale.Step;
		if (step >= 1.5)
		{
			string bigStepName = StiScale.BigStepName;
			Bitmap image = StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.ResourceCsv" + bigStepName + ".png");
			imageListImages = new ImageList
			{
				TransparentColor = Color.Transparent,
				ImageSize = ((Image)image).Size
			};
			imageListImages.Images.Add("Csv", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.ResourceCsv" + bigStepName + ".png"));
			imageListImages.Images.Add("Dbf", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.ResourceDbf" + bigStepName + ".png"));
			imageListImages.Images.Add("Excel", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.ResourceExcel" + bigStepName + ".png"));
			imageListImages.Images.Add("Json", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.ResourceJson" + bigStepName + ".png"));
			imageListImages.Images.Add("Pdf", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.ResourcePdf" + bigStepName + ".png"));
			imageListImages.Images.Add("Template", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.ResourceReport" + bigStepName + ".png"));
			imageListImages.Images.Add("Rtf", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.ResourceRtf" + bigStepName + ".png"));
			imageListImages.Images.Add("Txt", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.ResourceTxt" + bigStepName + ".png"));
			imageListImages.Images.Add("Xml", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.ResourceXml" + bigStepName + ".png"));
			imageListImages.Images.Add("Xsd", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.ResourceXsd" + bigStepName + ".png"));
			imageListImages.Images.Add("Report", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.ResourceReport" + bigStepName + ".png"));
			imageListImages.Images.Add("Document", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.ResourceReportSnapshot" + bigStepName + ".png"));
			imageListImages.Images.Add("FontEot", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.ResourceFontEot" + bigStepName + ".png"));
			imageListImages.Images.Add("FontOtf", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.ResourceFontOtf" + bigStepName + ".png"));
			imageListImages.Images.Add("FontTtc", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.ResourceFontTtc" + bigStepName + ".png"));
			imageListImages.Images.Add("FontTtf", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.ResourceFontTtf" + bigStepName + ".png"));
			imageListImages.Images.Add("FontWoff", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.ResourceFontWoff" + bigStepName + ".png"));
			imageListImages.Images.Add("Map", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.ResourceMap" + bigStepName + ".png"));
		}
		((ButtonBase)buttonEdit).Image = StiReportDesignImages.Actions.Edit();
		((ButtonBase)buttonView).Image = StiReportDesignImages.Actions.View();
		((ButtonBase)buttonRemove).Image = StiReportDesignImages.Actions.Remove();
		((ButtonBase)buttonSave).Image = StiReportDesignImages.Actions.SaveFile();
		((ButtonBase)buttonOpen).Image = StiReportDesignImages.Actions.OpenFile();
		errorProvider.Icon = StiIconUtils.ResizeIcon(errorProvider.Icon, StiScale.I16, StiScale.I16);
	}

	private void This_Load(object sender, EventArgs e)
	{
		Task.Run(delegate
		{
			string cleaned = Loc.GetCleaned("FormDictionaryDesigner", isEditMode ? "ResourceEdit" : "ResourceNew");
			report.Designer.UndoRedoSave(cleaned);
		});
	}

	private void This_FormClosed(object sender, FormClosedEventArgs e)
	{
		StiFormSettings.Save((Form)(object)this);
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		if (((Control)textBoxName).Text.Length == 0)
		{
			errorProvider.SetError((Control)(object)textBoxName, StiFieldChecker.NameRequire);
			((Control)textBoxName).Focus();
		}
		else if (!isEditMode && StiNameCreation.IsResourceNameExists(report, ((Control)textBoxName).Text))
		{
			errorProvider.SetError((Control)(object)textBoxName, StiFieldChecker.NameExists(((Control)textBoxName).Text));
			((Control)textBoxName).Focus();
		}
		else
		{
			StiCustomMapFinder.Clear();
			((Form)this).DialogResult = (DialogResult)1;
		}
	}

	private void This_CancelClick(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)2;
	}

	private void This_SaveCopyClick(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)1;
		CopyModeActivated = true;
	}

	private void This_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			e.Handled = true;
			((Form)this).Close();
		}
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/index.html?data_dictionary_resources.htm");
	}

	private void PanelResource_DoubleClick(object sender, EventArgs e)
	{
		ButtonOpen_Click(sender, e);
	}

	private void ButtonOpen_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Invalid comparison between Unknown and I4
		try
		{
			OpenFileDialog val = new OpenFileDialog();
			try
			{
				((FileDialog)val).Filter = StiFileDialogHelper.GetAllResourceFilter();
				((FileDialog)val).RestoreDirectory = true;
				if ((int)((CommonDialog)val).ShowDialog() != 1)
				{
					return;
				}
				try
				{
					UpdateNameAndAlias(StiNameCreation.CreateResourceName(report, Path.GetFileNameWithoutExtension(((FileDialog)val).FileName).Normalize()));
					StiResourceType? typeFromExtension = StiResourceTypeHelper.GetTypeFromExtension(Path.GetExtension(((FileDialog)val).FileName).ToLowerInvariant());
					if (typeFromExtension.HasValue && StiMaximumResourceSizeMessageBox.Check((Form)(object)this, ((FileDialog)val).FileName))
					{
						SetContent(typeFromExtension.GetValueOrDefault(), File.ReadAllBytes(((FileDialog)val).FileName));
					}
				}
				catch (Exception exception)
				{
					StiExceptionProvider.Show(exception);
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch (Exception exception2)
		{
			StiExceptionProvider.Show(exception2);
		}
	}

	private void ButtonSave_Click(object sender, EventArgs e)
	{
		SaveResource(ResourceType, Content, ((Control)textBoxName).Text);
	}

	private void ButtonRemove_Click(object sender, EventArgs e)
	{
		ClearContent();
	}

	private void ButtonEdit_Click(object sender, EventArgs e)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Invalid comparison between Unknown and I4
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Invalid comparison between Unknown and I4
		try
		{
			string text = GetContentText();
			bool flag = false;
			if (IsTextType)
			{
				flag = (int)StiTextEditor.EditText(report, ref text, StiCodeEditorMode.EditorText, null, allowPackExpression: false) == 1;
			}
			else if (IsMapType)
			{
				IStiCustomMapEditorWindow customMapEditorWindow = StiAccountSettings.CurrentAccount.AccountCreater.GetCustomMapEditorWindow(text);
				flag = customMapEditorWindow.ShowDialog(((Control)this).Handle).GetValueOrDefault();
				if (flag)
				{
					text = customMapEditorWindow.ResultJson;
				}
			}
			else
			{
				flag = (int)StiRichTextEditor.EditText(report, ref text, showExpressionButton: false, isPacked: false, showOpenSaveButtons: false) == 1;
			}
			if (flag)
			{
				SetContentText(text);
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ButtonView_Click(object sender, EventArgs e)
	{
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (IsReportType)
			{
				try
				{
					StiReport stiReport = new StiReport();
					if (ResourceType == StiResourceType.ReportSnapshot)
					{
						stiReport.LoadDocument(Content);
					}
					else
					{
						stiReport.Load(Content);
					}
					stiReport.Show();
					return;
				}
				catch (Exception exception)
				{
					StiExceptionProvider.Show(exception);
					return;
				}
			}
			if (!IsDataType)
			{
				return;
			}
			try
			{
				DataSet contentDataSet = GetContentDataSet();
				StiViewDataSetForm stiViewDataSetForm = new StiViewDataSetForm(contentDataSet);
				try
				{
					report?.Designer?.AddOwnedForm((Form)(object)stiViewDataSetForm);
					((Control)stiViewDataSetForm).Text = StiLocalization.Get("FormDictionaryDesigner", "ViewData");
					((Form)stiViewDataSetForm).ShowDialog();
				}
				finally
				{
					((IDisposable)stiViewDataSetForm)?.Dispose();
				}
			}
			catch (Exception ex)
			{
				if (ResourceType == StiResourceType.Csv || ResourceType == StiResourceType.Json || ResourceType == StiResourceType.Xml)
				{
					string text = StiGZipHelper.ConvertByteArrayToString(Content);
					StiTextEditor.EditText(report, ref text, StiCodeEditorMode.EditorText | StiCodeEditorMode.OkButtonOnly, null, allowPackExpression: false);
					return;
				}
				throw ex;
			}
		}
		catch (Exception exception2)
		{
			StiExceptionProvider.Show(exception2);
		}
	}

	private void TextBoxName_Enter(object sender, EventArgs e)
	{
		oldValue = ((Control)textBoxName).Text;
	}

	private void TextBoxName_Leave(object sender, EventArgs e)
	{
		if (oldValue == ((Control)textBoxAlias).Text)
		{
			((Control)textBoxAlias).Text = ((Control)textBoxName).Text;
		}
	}

	private void PanelContent_Paint(object sender, PaintEventArgs e)
	{
		StiDrawing.DrawRectangle(e.Graphics, StiUX.IsDark ? StiUX.ButtonBorder : SystemColors.ControlDark, 0.0, 0.0, ((Control)panelContent).Width - 1, ((Control)panelContent).Height - 1);
	}

	private void PanelFont_Paint(object sender, PaintEventArgs e)
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		if (!IsFontType || Content == null)
		{
			return;
		}
		Font val = new Font(StiFontCollection.GetFontFamilyByContent(((Control)textBoxName).Text, Content, ResourceType.ToString().Substring(3), ((Control)textBoxAlias).Text), 14f);
		try
		{
			e.Graphics.DrawString(val.Name, val, Brushes.DimGray, (RectangleF)new Rectangle(0, 0, ((Control)panelFont).Width - 1, ((Control)panelFont).Height - 1));
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void PanelReport_Paint(object sender, PaintEventArgs e)
	{
		if (Content == null)
		{
			return;
		}
		try
		{
			StiReport stiReport = new StiReport();
			StiPage stiPage;
			if (ResourceType == StiResourceType.ReportSnapshot)
			{
				stiReport.LoadDocument(Content);
				stiPage = stiReport.RenderedPages[0];
			}
			else
			{
				stiReport.Load(Content);
				stiPage = stiReport.Pages[0];
			}
			Bitmap thumbnail = stiPage.GetThumbnail(((Control)panelReport).Width, ((Control)panelReport).Height);
			try
			{
				e.Graphics.DrawImage((Image)(object)thumbnail, (((Control)panelReport).Width - ((Image)thumbnail).Width) / 2, (((Control)panelReport).Height - ((Image)thumbnail).Height) / 2, ((Image)thumbnail).Width, ((Image)thumbnail).Height);
			}
			finally
			{
				((IDisposable)thumbnail)?.Dispose();
			}
		}
		catch
		{
		}
	}

	private void PanelResource_Paint(object sender, PaintEventArgs e)
	{
		StiImageDialogPainter.Paint((Control)(object)panelResource, e.Graphics, GetContentImage(), Content == null, drawBorder: true, isImage: false);
	}

	private void Control_DragOver(object sender, DragEventArgs e)
	{
		if (!e.Data.GetDataPresent(DataFormats.FileDrop, false))
		{
			return;
		}
		string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
		if (array != null)
		{
			foreach (string item in array.Where((string f) => f != null))
			{
				string ext = Path.GetExtension(item).ToLowerInvariant();
				if (StiFileDialogHelper.GetAllResourceExtensions().Any((string se) => ext.EndsWithInvariant(se)))
				{
					e.Effect = (DragDropEffects)(-2147483645);
					return;
				}
			}
		}
		e.Effect = (DragDropEffects)0;
	}

	private void Control_DragDrop(object sender, DragEventArgs e)
	{
		string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
		if (array == null)
		{
			return;
		}
		foreach (string item in array.Where((string f) => f != null))
		{
			string extension = Path.GetExtension(item).ToLowerInvariant();
			StiResourceType? typeFromExtension = StiResourceTypeHelper.GetTypeFromExtension(extension);
			if (typeFromExtension.HasValue)
			{
				UpdateNameAndAlias(StiNameCreation.CreateConnectionName(report, Path.GetFileNameWithoutExtension(item).Normalize()));
				if (StiMaximumResourceSizeMessageBox.Check((Form)(object)this, item))
				{
					SetContent(typeFromExtension.GetValueOrDefault(), File.ReadAllBytes(item));
				}
				break;
			}
		}
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
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Expected O, but got Unknown
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Expected O, but got Unknown
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Expected O, but got Unknown
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Expected O, but got Unknown
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Expected O, but got Unknown
		//IL_079d: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a7: Expected O, but got Unknown
		//IL_07b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07be: Expected O, but got Unknown
		//IL_07cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d5: Expected O, but got Unknown
		//IL_0816: Unknown result type (might be due to invalid IL or missing references)
		//IL_0820: Expected O, but got Unknown
		//IL_08b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bf: Expected O, but got Unknown
		//IL_0948: Unknown result type (might be due to invalid IL or missing references)
		//IL_0952: Expected O, but got Unknown
		//IL_09db: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e5: Expected O, but got Unknown
		//IL_0a6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a78: Expected O, but got Unknown
		//IL_0bbd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc7: Expected O, but got Unknown
		//IL_0bd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bde: Expected O, but got Unknown
		//IL_0beb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf5: Expected O, but got Unknown
		//IL_0c5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c69: Expected O, but got Unknown
		//IL_0c76: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c80: Expected O, but got Unknown
		//IL_0c8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c97: Expected O, but got Unknown
		//IL_0d21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d2b: Expected O, but got Unknown
		//IL_0d38: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d42: Expected O, but got Unknown
		//IL_0d4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d59: Expected O, but got Unknown
		//IL_1100: Unknown result type (might be due to invalid IL or missing references)
		//IL_110a: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiResourceEditForm));
		errorProvider = new ErrorProvider(components);
		labelAlias = new StiLabelUX();
		textBoxAlias = new StiTextBox();
		labelName = new StiLabelUX();
		textBoxName = new StiTextBox();
		imageListImages = new ImageList(components);
		buttonOk = new StiButtonUX();
		buttonCancel = new StiButtonUX();
		panelResource = new Panel();
		buttonEdit = new StiButtonUX();
		buttonView = new StiButtonUX();
		buttonRemove = new StiButtonUX();
		buttonSave = new StiButtonUX();
		buttonOpen = new StiButtonUX();
		textBoxRich = new RichTextBox();
		panelReport = new Panel();
		panelFont = new Panel();
		panelContent = new Panel();
		labelContentInfo = new StiLabelUX();
		imageFileType = new PictureBox();
		groupLineButtons = new StiButtonPanelUX();
		buttonSaveCopy = new StiButtonUX();
		toolTip = new ToolTip(components);
		checkBoxAvailableInTheViewer = new StiCheckBoxUX();
		((ISupportInitialize)errorProvider).BeginInit();
		((Control)panelResource).SuspendLayout();
		((Control)panelContent).SuspendLayout();
		((ISupportInitialize)imageFileType).BeginInit();
		((Control)groupLineButtons).SuspendLayout();
		((Control)this).SuspendLayout();
		errorProvider.ContainerControl = (ContainerControl)(object)this;
		errorProvider.DataMember = "";
		errorProvider.Icon = (Icon)componentResourceManager.GetObject("errorProvider.Icon");
		((Control)labelAlias).Location = new Point(12, 44);
		((Control)labelAlias).Name = "labelAlias";
		((Control)labelAlias).Size = new Size(130, 20);
		((Control)labelAlias).TabIndex = 11;
		((Control)labelAlias).Text = "Alias";
		((Label)labelAlias).TextAlign = (ContentAlignment)16;
		((Control)textBoxAlias).Anchor = (AnchorStyles)13;
		((Control)textBoxAlias).Location = new Point(144, 44);
		((Control)textBoxAlias).Name = "textBoxAlias";
		((Control)textBoxAlias).Size = new Size(202, 21);
		((Control)textBoxAlias).TabIndex = 1;
		((Control)labelName).Location = new Point(12, 12);
		((Control)labelName).Name = "labelName";
		((Control)labelName).Size = new Size(130, 20);
		((Control)labelName).TabIndex = 10;
		((Control)labelName).Text = "Name";
		((Label)labelName).TextAlign = (ContentAlignment)16;
		((Control)textBoxName).Anchor = (AnchorStyles)13;
		((Control)textBoxName).Location = new Point(144, 12);
		((Control)textBoxName).Name = "textBoxName";
		((Control)textBoxName).Size = new Size(202, 21);
		((Control)textBoxName).TabIndex = 0;
		((Control)textBoxName).Enter += TextBoxName_Enter;
		((Control)textBoxName).Leave += TextBoxName_Leave;
		imageListImages.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("imageListImages.ImageStream");
		imageListImages.TransparentColor = Color.Transparent;
		imageListImages.Images.SetKeyName(0, "Csv");
		imageListImages.Images.SetKeyName(1, "Dbf");
		imageListImages.Images.SetKeyName(2, "Excel");
		imageListImages.Images.SetKeyName(3, "Word");
		imageListImages.Images.SetKeyName(4, "Image");
		imageListImages.Images.SetKeyName(5, "Json");
		imageListImages.Images.SetKeyName(6, "Pdf");
		imageListImages.Images.SetKeyName(7, "Template");
		imageListImages.Images.SetKeyName(8, "Rtf");
		imageListImages.Images.SetKeyName(9, "Txt");
		imageListImages.Images.SetKeyName(10, "Xml");
		imageListImages.Images.SetKeyName(11, "Xsd");
		imageListImages.Images.SetKeyName(12, "Report");
		imageListImages.Images.SetKeyName(13, "Document");
		imageListImages.Images.SetKeyName(14, "FontEot");
		imageListImages.Images.SetKeyName(15, "FontOtf");
		imageListImages.Images.SetKeyName(16, "FontTtc");
		imageListImages.Images.SetKeyName(17, "FontTtf");
		imageListImages.Images.SetKeyName(18, "FontWoff");
		imageListImages.Images.SetKeyName(19, "Map");
		((Control)buttonOk).Anchor = (AnchorStyles)10;
		((Control)buttonOk).Location = new Point(311, 12);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(75, 24);
		((Control)buttonOk).TabIndex = 1;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Control)buttonOk).Click += This_OkClick;
		((Control)buttonCancel).Anchor = (AnchorStyles)10;
		((Button)buttonCancel).DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(391, 12);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(75, 24);
		((Control)buttonCancel).TabIndex = 2;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)buttonCancel).Click += This_CancelClick;
		((Control)panelResource).AllowDrop = true;
		((Control)panelResource).Anchor = (AnchorStyles)15;
		((Control)panelResource).Controls.Add((Control)(object)buttonEdit);
		((Control)panelResource).Controls.Add((Control)(object)buttonView);
		((Control)panelResource).Controls.Add((Control)(object)buttonRemove);
		((Control)panelResource).Controls.Add((Control)(object)buttonSave);
		((Control)panelResource).Controls.Add((Control)(object)buttonOpen);
		((Control)panelResource).Controls.Add((Control)(object)textBoxRich);
		((Control)panelResource).Controls.Add((Control)(object)panelReport);
		((Control)panelResource).Controls.Add((Control)(object)panelFont);
		((Control)panelResource).Controls.Add((Control)(object)panelContent);
		((Control)panelResource).Location = new Point(12, 79);
		((Control)panelResource).Name = "panelResource";
		((Control)panelResource).Size = new Size(454, 253);
		((Control)panelResource).TabIndex = 2;
		((Control)panelResource).Text = "File";
		((Control)panelResource).DragDrop += new DragEventHandler(Control_DragDrop);
		((Control)panelResource).DragOver += new DragEventHandler(Control_DragOver);
		((Control)panelResource).Paint += new PaintEventHandler(PanelResource_Paint);
		((Control)panelResource).DoubleClick += PanelResource_DoubleClick;
		((Control)buttonEdit).Anchor = (AnchorStyles)9;
		((Control)buttonEdit).Enabled = false;
		((ButtonBase)buttonEdit).Image = (Image)componentResourceManager.GetObject("buttonEdit.Image");
		((Control)buttonEdit).Location = new Point(424, 109);
		((Control)buttonEdit).Name = "buttonEdit";
		((Control)buttonEdit).Size = new Size(24, 24);
		((Control)buttonEdit).TabIndex = 19;
		((ButtonBase)buttonEdit).UseVisualStyleBackColor = true;
		((Control)buttonEdit).Click += ButtonEdit_Click;
		((Control)buttonView).Anchor = (AnchorStyles)9;
		((Control)buttonView).Enabled = false;
		((ButtonBase)buttonView).Image = (Image)componentResourceManager.GetObject("buttonView.Image");
		((Control)buttonView).Location = new Point(424, 83);
		((Control)buttonView).Name = "buttonView";
		((Control)buttonView).Size = new Size(24, 24);
		((Control)buttonView).TabIndex = 19;
		((ButtonBase)buttonView).UseVisualStyleBackColor = true;
		((Control)buttonView).Click += ButtonView_Click;
		((Control)buttonRemove).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonRemove).Image = (Image)componentResourceManager.GetObject("buttonRemove.Image");
		((Control)buttonRemove).Location = new Point(424, 57);
		((Control)buttonRemove).Name = "buttonRemove";
		((Control)buttonRemove).Size = new Size(24, 24);
		((Control)buttonRemove).TabIndex = 18;
		((ButtonBase)buttonRemove).UseVisualStyleBackColor = true;
		((Control)buttonRemove).Click += ButtonRemove_Click;
		((Control)buttonSave).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonSave).Image = (Image)componentResourceManager.GetObject("buttonSave.Image");
		((Control)buttonSave).Location = new Point(424, 31);
		((Control)buttonSave).Name = "buttonSave";
		((Control)buttonSave).Size = new Size(24, 24);
		((Control)buttonSave).TabIndex = 17;
		((ButtonBase)buttonSave).UseVisualStyleBackColor = true;
		((Control)buttonSave).Click += ButtonSave_Click;
		((Control)buttonOpen).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonOpen).Image = (Image)componentResourceManager.GetObject("buttonOpen.Image");
		((Control)buttonOpen).Location = new Point(424, 5);
		((Control)buttonOpen).Name = "buttonOpen";
		((Control)buttonOpen).Size = new Size(24, 24);
		((Control)buttonOpen).TabIndex = 17;
		((ButtonBase)buttonOpen).UseVisualStyleBackColor = true;
		((Control)buttonOpen).Click += ButtonOpen_Click;
		((TextBoxBase)textBoxRich).BorderStyle = (BorderStyle)0;
		((Control)textBoxRich).Location = new Point(129, 53);
		((Control)textBoxRich).Name = "textBoxRich";
		((TextBoxBase)textBoxRich).ReadOnly = true;
		((Control)textBoxRich).Size = new Size(54, 49);
		((Control)textBoxRich).TabIndex = 26;
		((Control)textBoxRich).Text = "";
		((Control)panelReport).AllowDrop = true;
		((Control)panelReport).Anchor = (AnchorStyles)15;
		((Control)panelReport).Location = new Point(68, 53);
		((Control)panelReport).Name = "panelReport";
		((Control)panelReport).Size = new Size(48, 37);
		((Control)panelReport).TabIndex = 25;
		((Control)panelReport).DragDrop += new DragEventHandler(Control_DragDrop);
		((Control)panelReport).DragOver += new DragEventHandler(Control_DragOver);
		((Control)panelReport).Paint += new PaintEventHandler(PanelReport_Paint);
		((Control)panelFont).AllowDrop = true;
		((Control)panelFont).Anchor = (AnchorStyles)15;
		((Control)panelFont).Location = new Point(6, 53);
		((Control)panelFont).Name = "panelFont";
		((Control)panelFont).Size = new Size(50, 37);
		((Control)panelFont).TabIndex = 24;
		((Control)panelFont).DragDrop += new DragEventHandler(Control_DragDrop);
		((Control)panelFont).DragOver += new DragEventHandler(Control_DragOver);
		((Control)panelFont).Paint += new PaintEventHandler(PanelFont_Paint);
		((Control)panelContent).BackgroundImageLayout = (ImageLayout)2;
		((Control)panelContent).Controls.Add((Control)(object)labelContentInfo);
		((Control)panelContent).Controls.Add((Control)(object)imageFileType);
		((Control)panelContent).Location = new Point(6, 7);
		((Control)panelContent).Name = "panelContent";
		((Control)panelContent).Size = new Size(197, 40);
		((Control)panelContent).TabIndex = 1;
		((Control)panelContent).DragDrop += new DragEventHandler(Control_DragDrop);
		((Control)panelContent).DragOver += new DragEventHandler(Control_DragOver);
		((Control)panelContent).Paint += new PaintEventHandler(PanelContent_Paint);
		((Control)labelContentInfo).AutoSize = true;
		((Control)labelContentInfo).Location = new Point(42, 14);
		((Control)labelContentInfo).Name = "labelContentInfo";
		((Control)labelContentInfo).Size = new Size(84, 13);
		((Control)labelContentInfo).TabIndex = 1;
		((Control)labelContentInfo).Text = "labelContentInfo";
		((Control)imageFileType).Location = new Point(3, 3);
		((Control)imageFileType).Name = "imageFileType";
		((Control)imageFileType).Size = new Size(32, 32);
		imageFileType.SizeMode = (PictureBoxSizeMode)2;
		imageFileType.TabIndex = 0;
		imageFileType.TabStop = false;
		((Control)groupLineButtons).Controls.Add((Control)(object)buttonSaveCopy);
		((Control)groupLineButtons).Controls.Add((Control)(object)buttonCancel);
		((Control)groupLineButtons).Controls.Add((Control)(object)buttonOk);
		((Control)groupLineButtons).Dock = (DockStyle)2;
		groupLineButtons.LinePosition = StiLinePosition.Top;
		groupLineButtons.LineStyle = StiLineStyle.Dot;
		((Control)groupLineButtons).Location = new Point(0, 368);
		((Control)groupLineButtons).Name = "groupLineButtons";
		((Control)groupLineButtons).Size = new Size(477, 46);
		((Control)groupLineButtons).TabIndex = 3;
		((Control)buttonSaveCopy).Anchor = (AnchorStyles)6;
		((Control)buttonSaveCopy).Location = new Point(12, 12);
		((Control)buttonSaveCopy).Name = "buttonSaveCopy";
		((Control)buttonSaveCopy).Size = new Size(93, 24);
		((Control)buttonSaveCopy).TabIndex = 0;
		((Control)buttonSaveCopy).Text = "Save a Copy";
		((ButtonBase)buttonSaveCopy).UseVisualStyleBackColor = true;
		((Control)buttonSaveCopy).Visible = false;
		((Control)buttonSaveCopy).Click += This_SaveCopyClick;
		((Control)checkBoxAvailableInTheViewer).AutoSize = true;
		((Control)checkBoxAvailableInTheViewer).BackColor = Color.Transparent;
		((Control)checkBoxAvailableInTheViewer).Location = new Point(12, 343);
		((Control)checkBoxAvailableInTheViewer).Name = "checkBoxAvailableInTheViewer";
		((Control)checkBoxAvailableInTheViewer).Size = new Size(133, 17);
		((Control)checkBoxAvailableInTheViewer).TabIndex = 14;
		((Control)checkBoxAvailableInTheViewer).Text = "Available in the Viewer";
		((ButtonBase)checkBoxAvailableInTheViewer).UseVisualStyleBackColor = false;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(477, 414);
		((Control)this).Controls.Add((Control)(object)checkBoxAvailableInTheViewer);
		((Control)this).Controls.Add((Control)(object)labelAlias);
		((Control)this).Controls.Add((Control)(object)panelResource);
		((Control)this).Controls.Add((Control)(object)textBoxAlias);
		((Control)this).Controls.Add((Control)(object)groupLineButtons);
		((Control)this).Controls.Add((Control)(object)labelName);
		((Control)this).Controls.Add((Control)(object)textBoxName);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiResourceEditForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Edit resource";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Form)this).Load += This_Load;
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((ISupportInitialize)errorProvider).EndInit();
		((Control)panelResource).ResumeLayout(false);
		((Control)panelContent).ResumeLayout(false);
		((Control)panelContent).PerformLayout();
		((ISupportInitialize)imageFileType).EndInit();
		((Control)groupLineButtons).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
