using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Exceptions;
using Stimulsoft.Base.Licenses;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Base;

public class StiExceptionForm : Form, IComparer<AssemblyName>
{
	private IContainer components;

	private Button btCancel;

	private Button btSend;

	private TextBox textBoxMessage;

	private TabControl tabControlGeneral;

	private TabPage tabPageGeneral;

	private TabPage tabPageException;

	private PictureBox picGeneral;

	private Label labelInformation;

	private Label labelFramework;

	private Label labelUserName;

	private Label labelNumber;

	private TextBox textBoxInformation;

	private TextBox textBoxFramework;

	private TextBox textBoxUserName;

	private TextBox textBoxNumber;

	private Label labelVersion;

	private TextBox textBoxVersion;

	private Label labelApplication;

	private TextBox textBoxApplication;

	private TextBox textBoxStackTrace;

	private TextBox textBoxSource;

	private TextBox textBoxMessage2;

	private Label labelMessage1;

	private Label labelSource;

	private Label labelStackTrace;

	private TabPage tabPageAssemblies;

	private ListView listViewAssemblies;

	private Button buttonClipboard;

	private Button buttonSaveToFile;

	private ToolTip toolTip1;

	public int Compare(AssemblyName x, AssemblyName y)
	{
		return x.Name.CompareTo(y.Name);
	}

	public StiExceptionForm(Exception exception)
	{
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Expected O, but got Unknown
		InitializeComponent();
		Assembly assembly = Assembly.GetAssembly(((object)this).GetType());
		Version version = assembly.GetName().Version;
		string text = exception.Message;
		if (exception is ReflectionTypeLoadException ex && text.Contains("LoaderExceptions") && ex.LoaderExceptions != null && ex.LoaderExceptions.Length != 0)
		{
			int num = text.IndexOf(".", StringComparison.InvariantCulture);
			if (num > 0)
			{
				text = text.Substring(0, num) + ":\r\n" + ex.LoaderExceptions[0].Message;
			}
		}
		DateTime now = DateTime.Now;
		int num2 = new Random().Next(10000000, 99999999);
		((Control)textBoxNumber).Text = $"{now.Year}{now.Month}{now.Day}{num2}";
		((Control)textBoxMessage).Text = text;
		((Control)textBoxApplication).Text = Application.ProductName;
		((Control)textBoxFramework).Text = RuntimeEnvironment.GetSystemVersion();
		((Control)textBoxVersion).Text = string.Format("Version: {0}.{1}.{2} from {3:D}", version.Major, version.Minor, version.Build, "7 December 2021");
		((Control)textBoxMessage2).Text = exception.Message;
		((Control)textBoxSource).Text = exception.Source;
		((Control)textBoxStackTrace).Text = exception.StackTrace;
		((Control)btSend).Visible = StiDesignerAppStatus.IsRunning || !string.IsNullOrEmpty(StiExceptionProvider.ServerUrl);
		((ButtonBase)buttonClipboard).Image = StiExceptionsImages.Copy();
		((ButtonBase)buttonSaveToFile).Image = StiExceptionsImages.Save();
		StiLicenseKey stiLicenseKey = StiLicenseKey.Get(StiLicense.Key);
		if (stiLicenseKey != null && !string.IsNullOrEmpty(stiLicenseKey.UserName))
		{
			((Control)textBoxUserName).Text = stiLicenseKey.UserName;
			((TextBoxBase)textBoxUserName).ReadOnly = true;
		}
		else
		{
			((Control)btSend).Visible = false;
		}
		listViewAssemblies.Clear();
		listViewAssemblies.Columns.Add(StiLocalization.Get("PropertyMain", "Name"), 320, (HorizontalAlignment)0);
		listViewAssemblies.Columns.Add(StiLocalization.Get("PropertyMain", "Version"), 150, (HorizontalAlignment)0);
		Assembly assembly2 = Assembly.GetEntryAssembly();
		if (assembly2 == null)
		{
			assembly2 = Assembly.GetExecutingAssembly();
		}
		AssemblyName[] referencedAssemblies = assembly2.GetReferencedAssemblies();
		Array.Sort(referencedAssemblies, this);
		AssemblyName[] array = referencedAssemblies;
		foreach (AssemblyName assemblyName in array)
		{
			ListViewItem val = new ListViewItem();
			val.Text = assemblyName.Name;
			val.SubItems.Add(assemblyName.Version.ToString());
			listViewAssemblies.Items.Add(val);
		}
		Localize();
		((Form)this).ShowIcon = false;
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("ExceptionProvider", "ExceptionReport");
		toolTip1.SetToolTip((Control)(object)buttonSaveToFile, StiLocalization.Get("ExceptionProvider", "SaveToFile"));
		toolTip1.SetToolTip((Control)(object)buttonClipboard, StiLocalization.Get("HelpDesigner", "CopyToClipboard"));
		((Control)tabPageGeneral).Text = StiLocalization.Get("ExceptionProvider", "General");
		((Control)tabPageException).Text = StiLocalization.Get("ExceptionProvider", "Exception");
		((Control)tabPageAssemblies).Text = StiLocalization.Get("ExceptionProvider", "Assemblies");
		((Control)labelApplication).Text = StiLocalization.Get("PropertyEnum", "StiFontIconGroupWebApplicationIcons");
		((Control)labelVersion).Text = StiLocalization.Get("PropertyMain", "Version");
		((Control)labelFramework).Text = StiLocalization.Get("ExceptionProvider", "Framework");
		((Control)labelInformation).Text = StiLocalization.Get("ExceptionProvider", "PleaseEnterDetailedInformation");
		((Control)labelMessage1).Text = StiLocalization.Get("ExceptionProvider", "Message");
		((Control)labelSource).Text = StiLocalization.Get("ExceptionProvider", "Source");
		((Control)labelStackTrace).Text = StiLocalization.Get("ExceptionProvider", "StackTrace");
		((Control)btSend).Text = StiLocalization.Get("A_WebViewer", "ButtonSend");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Close");
		((Control)labelNumber).Text = StiLocalization.Get("ExceptionProvider", "Number");
		((Control)labelUserName).Text = StiLocalization.Get("Cloud", "labelUserName").Replace(":", "");
	}

	private string GetExceptionString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (!string.IsNullOrEmpty(((Control)textBoxInformation).Text))
		{
			stringBuilder.AppendLine("[Customer Explanation]").AppendLine(((Control)textBoxInformation).Text).AppendLine();
		}
		stringBuilder.AppendLine("[General Info]").AppendLine("Application: " + ((Control)textBoxApplication).Text).AppendLine("Framework:   " + ((Control)textBoxFramework).Text)
			.AppendLine("Version:     " + ((Control)textBoxVersion).Text)
			.AppendLine("MachineName: " + Environment.MachineName)
			.AppendLine("OSVersion:   " + Environment.OSVersion.VersionString)
			.AppendLine("UserName:    " + Environment.UserName)
			.AppendLine();
		stringBuilder.AppendLine("[Exception Info]").AppendLine("Message:     " + ((Control)textBoxMessage).Text).AppendLine()
			.AppendLine("Source:      " + ((Control)textBoxSource).Text)
			.AppendLine()
			.AppendLine("StackTrace:")
			.AppendLine(((Control)textBoxStackTrace).Text)
			.AppendLine();
		stringBuilder.AppendLine("[Assemblies]");
		AssemblyName[] referencedAssemblies = Assembly.GetEntryAssembly().GetReferencedAssemblies();
		Array.Sort(referencedAssemblies, this);
		AssemblyName[] array = referencedAssemblies;
		foreach (AssemblyName assemblyName in array)
		{
			stringBuilder.AppendLine($"{assemblyName.Name}, Version = {assemblyName.Version}");
		}
		return stringBuilder.ToString();
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

	private void ButtonSaveToFile_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Invalid comparison between Unknown and I4
		try
		{
			SaveFileDialog val = new SaveFileDialog();
			try
			{
				((FileDialog)val).Filter = "Text Files (*.txt)|*.txt|All files (*.*)|*.*";
				((FileDialog)val).FilterIndex = 1;
				((FileDialog)val).FileName = "Exception.txt";
				((FileDialog)val).RestoreDirectory = true;
				if ((int)((CommonDialog)val).ShowDialog() == 1)
				{
					using (StreamWriter streamWriter = new StreamWriter(((FileDialog)val).FileName))
					{
						streamWriter.Write(GetExceptionString());
						streamWriter.Flush();
						streamWriter.Close();
						return;
					}
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch
		{
		}
	}

	private void ButtonClipboard_Click(object sender, EventArgs e)
	{
		try
		{
			Clipboard.SetDataObject((object)GetExceptionString());
		}
		catch
		{
		}
	}

	private async void ButtonSend_Click(object sender, EventArgs e)
	{
		if (await StiExceptionProviderHelper.SendAsync(((Control)textBoxUserName).Text, ((Control)textBoxMessage).Text, GetExceptionString(), ((Control)textBoxNumber).Text))
		{
			MessageBox.Show((IWin32Window)(object)this, StiLocalization.Get("ExceptionProvider", "SendErrorSuccess"), "", (MessageBoxButtons)0);
			((Form)this).Close();
		}
		else
		{
			MessageBox.Show((IWin32Window)(object)this, StiLocalization.Get("ExceptionProvider", "SendErrorFailed"), "", (MessageBoxButtons)0);
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
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Expected O, but got Unknown
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Expected O, but got Unknown
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Expected O, but got Unknown
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Expected O, but got Unknown
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Expected O, but got Unknown
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Expected O, but got Unknown
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Expected O, but got Unknown
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Expected O, but got Unknown
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Expected O, but got Unknown
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Expected O, but got Unknown
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Expected O, but got Unknown
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected O, but got Unknown
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03df: Unknown result type (might be due to invalid IL or missing references)
		//IL_058c: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0621: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0739: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0869: Unknown result type (might be due to invalid IL or missing references)
		//IL_0902: Unknown result type (might be due to invalid IL or missing references)
		//IL_098c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a84: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b20: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c36: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c90: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c9a: Expected O, but got Unknown
		//IL_0cb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dbf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0de3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e54: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f69: Unknown result type (might be due to invalid IL or missing references)
		//IL_1002: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1134: Unknown result type (might be due to invalid IL or missing references)
		//IL_11cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_11f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1297: Unknown result type (might be due to invalid IL or missing references)
		//IL_131e: Unknown result type (might be due to invalid IL or missing references)
		//IL_13a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_14aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_14fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1506: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiExceptionForm));
		btCancel = new Button();
		btSend = new Button();
		textBoxMessage = new TextBox();
		tabControlGeneral = new TabControl();
		tabPageGeneral = new TabPage();
		labelInformation = new Label();
		labelFramework = new Label();
		labelUserName = new Label();
		labelNumber = new Label();
		textBoxInformation = new TextBox();
		textBoxFramework = new TextBox();
		textBoxUserName = new TextBox();
		textBoxNumber = new TextBox();
		labelVersion = new Label();
		textBoxVersion = new TextBox();
		labelApplication = new Label();
		textBoxApplication = new TextBox();
		picGeneral = new PictureBox();
		tabPageException = new TabPage();
		labelStackTrace = new Label();
		labelSource = new Label();
		labelMessage1 = new Label();
		textBoxStackTrace = new TextBox();
		textBoxSource = new TextBox();
		textBoxMessage2 = new TextBox();
		tabPageAssemblies = new TabPage();
		listViewAssemblies = new ListView();
		buttonClipboard = new Button();
		buttonSaveToFile = new Button();
		toolTip1 = new ToolTip(components);
		((Control)tabControlGeneral).SuspendLayout();
		((Control)tabPageGeneral).SuspendLayout();
		((ISupportInitialize)picGeneral).BeginInit();
		((Control)tabPageException).SuspendLayout();
		((Control)tabPageAssemblies).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)btCancel).Anchor = (AnchorStyles)10;
		btCancel.DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(1006, 833);
		((Control)btCancel).Margin = new Padding(6, 6, 6, 6);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(152, 46);
		((Control)btCancel).TabIndex = 1;
		((Control)btCancel).Text = "Close";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btSend).Anchor = (AnchorStyles)10;
		((Control)btSend).Location = new Point(842, 833);
		((Control)btSend).Margin = new Padding(6, 6, 6, 6);
		((Control)btSend).Name = "btSend";
		((Control)btSend).Size = new Size(152, 46);
		((Control)btSend).TabIndex = 1;
		((Control)btSend).Text = "Send";
		((ButtonBase)btSend).UseVisualStyleBackColor = true;
		((Control)btSend).Click += ButtonSend_Click;
		((Control)textBoxMessage).Anchor = (AnchorStyles)13;
		((Control)textBoxMessage).BackColor = SystemColors.Window;
		((Control)textBoxMessage).Location = new Point(176, 23);
		((Control)textBoxMessage).Margin = new Padding(6, 6, 6, 6);
		((TextBoxBase)textBoxMessage).Multiline = true;
		((Control)textBoxMessage).Name = "textBoxMessage";
		((TextBoxBase)textBoxMessage).ReadOnly = true;
		((Control)textBoxMessage).Size = new Size(916, 119);
		((Control)textBoxMessage).TabIndex = 0;
		((Control)tabControlGeneral).Anchor = (AnchorStyles)15;
		((Control)tabControlGeneral).Controls.Add((Control)(object)tabPageGeneral);
		((Control)tabControlGeneral).Controls.Add((Control)(object)tabPageException);
		((Control)tabControlGeneral).Controls.Add((Control)(object)tabPageAssemblies);
		((Control)tabControlGeneral).Location = new Point(24, 23);
		((Control)tabControlGeneral).Margin = new Padding(6, 6, 6, 6);
		((Control)tabControlGeneral).Name = "tabControlGeneral";
		tabControlGeneral.SelectedIndex = 0;
		((Control)tabControlGeneral).Size = new Size(1136, 785);
		((Control)tabControlGeneral).TabIndex = 0;
		((Control)tabPageGeneral).BackColor = Color.White;
		((Control)tabPageGeneral).Controls.Add((Control)(object)labelInformation);
		((Control)tabPageGeneral).Controls.Add((Control)(object)labelFramework);
		((Control)tabPageGeneral).Controls.Add((Control)(object)labelUserName);
		((Control)tabPageGeneral).Controls.Add((Control)(object)labelNumber);
		((Control)tabPageGeneral).Controls.Add((Control)(object)textBoxInformation);
		((Control)tabPageGeneral).Controls.Add((Control)(object)textBoxFramework);
		((Control)tabPageGeneral).Controls.Add((Control)(object)textBoxUserName);
		((Control)tabPageGeneral).Controls.Add((Control)(object)textBoxNumber);
		((Control)tabPageGeneral).Controls.Add((Control)(object)labelVersion);
		((Control)tabPageGeneral).Controls.Add((Control)(object)textBoxVersion);
		((Control)tabPageGeneral).Controls.Add((Control)(object)labelApplication);
		((Control)tabPageGeneral).Controls.Add((Control)(object)textBoxApplication);
		((Control)tabPageGeneral).Controls.Add((Control)(object)picGeneral);
		((Control)tabPageGeneral).Controls.Add((Control)(object)textBoxMessage);
		tabPageGeneral.Location = new Point(8, 39);
		((Control)tabPageGeneral).Margin = new Padding(6, 6, 6, 6);
		((Control)tabPageGeneral).Name = "tabPageGeneral";
		((Control)tabPageGeneral).Padding = new Padding(6, 6, 6, 6);
		((Control)tabPageGeneral).Size = new Size(1120, 738);
		tabPageGeneral.TabIndex = 0;
		((Control)tabPageGeneral).Text = "General";
		((Control)labelInformation).ForeColor = Color.DimGray;
		((Control)labelInformation).Location = new Point(24, 475);
		((Control)labelInformation).Margin = new Padding(6, 0, 6, 0);
		((Control)labelInformation).Name = "labelInformation";
		((Control)labelInformation).Size = new Size(1066, 38);
		((Control)labelInformation).TabIndex = 40;
		((Control)labelInformation).Text = "Please enter detailed information about events which cause this exception. ";
		labelInformation.TextAlign = (ContentAlignment)16;
		((Control)labelFramework).ForeColor = Color.DimGray;
		((Control)labelFramework).Location = new Point(24, 292);
		((Control)labelFramework).Margin = new Padding(6, 0, 6, 0);
		((Control)labelFramework).Name = "labelFramework";
		((Control)labelFramework).Size = new Size(140, 38);
		((Control)labelFramework).TabIndex = 40;
		((Control)labelFramework).Text = "Framework";
		labelFramework.TextAlign = (ContentAlignment)16;
		((Control)labelUserName).ForeColor = Color.DimGray;
		((Control)labelUserName).Location = new Point(24, 358);
		((Control)labelUserName).Margin = new Padding(6, 0, 6, 0);
		((Control)labelUserName).Name = "labelUserName";
		((Control)labelUserName).Size = new Size(140, 38);
		((Control)labelUserName).TabIndex = 40;
		((Control)labelUserName).Text = "User Name";
		labelUserName.TextAlign = (ContentAlignment)16;
		((Control)labelNumber).ForeColor = Color.DimGray;
		((Control)labelNumber).Location = new Point(24, 417);
		((Control)labelNumber).Margin = new Padding(6, 0, 6, 0);
		((Control)labelNumber).Name = "labelNumber";
		((Control)labelNumber).Size = new Size(140, 38);
		((Control)labelNumber).TabIndex = 40;
		((Control)labelNumber).Text = "Number";
		labelNumber.TextAlign = (ContentAlignment)16;
		textBoxInformation.AcceptsReturn = true;
		((TextBoxBase)textBoxInformation).AcceptsTab = true;
		((Control)textBoxInformation).BackColor = SystemColors.Window;
		((Control)textBoxInformation).Location = new Point(24, 519);
		((Control)textBoxInformation).Margin = new Padding(6, 6, 6, 6);
		((TextBoxBase)textBoxInformation).Multiline = true;
		((Control)textBoxInformation).Name = "textBoxInformation";
		textBoxInformation.ScrollBars = (ScrollBars)2;
		((Control)textBoxInformation).Size = new Size(1068, 185);
		((Control)textBoxInformation).TabIndex = 8;
		((Control)textBoxFramework).Anchor = (AnchorStyles)13;
		((Control)textBoxFramework).BackColor = SystemColors.Window;
		((Control)textBoxFramework).Location = new Point(176, 292);
		((Control)textBoxFramework).Margin = new Padding(6, 6, 6, 6);
		((Control)textBoxFramework).Name = "textBoxFramework";
		((TextBoxBase)textBoxFramework).ReadOnly = true;
		((Control)textBoxFramework).Size = new Size(916, 31);
		((Control)textBoxFramework).TabIndex = 4;
		((Control)textBoxUserName).Anchor = (AnchorStyles)13;
		((Control)textBoxUserName).BackColor = SystemColors.Window;
		((Control)textBoxUserName).Location = new Point(176, 354);
		((Control)textBoxUserName).Margin = new Padding(6, 6, 6, 6);
		((Control)textBoxUserName).Name = "textBoxUserName";
		((Control)textBoxUserName).Size = new Size(916, 31);
		((Control)textBoxUserName).TabIndex = 4;
		((Control)textBoxNumber).Anchor = (AnchorStyles)13;
		((Control)textBoxNumber).BackColor = SystemColors.Window;
		((Control)textBoxNumber).Location = new Point(176, 413);
		((Control)textBoxNumber).Margin = new Padding(6, 6, 6, 6);
		((Control)textBoxNumber).Name = "textBoxNumber";
		((TextBoxBase)textBoxNumber).ReadOnly = true;
		((Control)textBoxNumber).Size = new Size(916, 31);
		((Control)textBoxNumber).TabIndex = 4;
		((Control)labelVersion).ForeColor = Color.DimGray;
		((Control)labelVersion).Location = new Point(24, 231);
		((Control)labelVersion).Margin = new Padding(6, 0, 6, 0);
		((Control)labelVersion).Name = "labelVersion";
		((Control)labelVersion).Size = new Size(128, 38);
		((Control)labelVersion).TabIndex = 30;
		((Control)labelVersion).Text = "Version";
		labelVersion.TextAlign = (ContentAlignment)16;
		((Control)textBoxVersion).Anchor = (AnchorStyles)13;
		((Control)textBoxVersion).BackColor = SystemColors.Window;
		((Control)textBoxVersion).Location = new Point(176, 231);
		((Control)textBoxVersion).Margin = new Padding(6, 6, 6, 6);
		((Control)textBoxVersion).Name = "textBoxVersion";
		((TextBoxBase)textBoxVersion).ReadOnly = true;
		((Control)textBoxVersion).Size = new Size(916, 31);
		((Control)textBoxVersion).TabIndex = 2;
		((Control)labelApplication).ForeColor = Color.DimGray;
		((Control)labelApplication).Location = new Point(24, 171);
		((Control)labelApplication).Margin = new Padding(6, 0, 6, 0);
		((Control)labelApplication).Name = "labelApplication";
		((Control)labelApplication).Size = new Size(128, 38);
		((Control)labelApplication).TabIndex = 28;
		((Control)labelApplication).Text = "Application";
		labelApplication.TextAlign = (ContentAlignment)16;
		((Control)textBoxApplication).Anchor = (AnchorStyles)13;
		((Control)textBoxApplication).BackColor = SystemColors.Window;
		((Control)textBoxApplication).Location = new Point(176, 169);
		((Control)textBoxApplication).Margin = new Padding(6, 6, 6, 6);
		((Control)textBoxApplication).Name = "textBoxApplication";
		((TextBoxBase)textBoxApplication).ReadOnly = true;
		((Control)textBoxApplication).Size = new Size(916, 31);
		((Control)textBoxApplication).TabIndex = 1;
		picGeneral.Image = (Image)componentResourceManager.GetObject("picGeneral.Image");
		((Control)picGeneral).Location = new Point(24, 23);
		((Control)picGeneral).Margin = new Padding(6, 6, 6, 6);
		((Control)picGeneral).Name = "picGeneral";
		((Control)picGeneral).Size = new Size(128, 123);
		picGeneral.SizeMode = (PictureBoxSizeMode)1;
		picGeneral.TabIndex = 26;
		picGeneral.TabStop = false;
		((Control)tabPageException).BackColor = Color.White;
		((Control)tabPageException).Controls.Add((Control)(object)labelStackTrace);
		((Control)tabPageException).Controls.Add((Control)(object)labelSource);
		((Control)tabPageException).Controls.Add((Control)(object)labelMessage1);
		((Control)tabPageException).Controls.Add((Control)(object)textBoxStackTrace);
		((Control)tabPageException).Controls.Add((Control)(object)textBoxSource);
		((Control)tabPageException).Controls.Add((Control)(object)textBoxMessage2);
		tabPageException.Location = new Point(8, 39);
		((Control)tabPageException).Margin = new Padding(6, 6, 6, 6);
		((Control)tabPageException).Name = "tabPageException";
		((Control)tabPageException).Padding = new Padding(6, 6, 6, 6);
		((Control)tabPageException).Size = new Size(1120, 738);
		tabPageException.TabIndex = 1;
		((Control)tabPageException).Text = "Exception";
		((Control)labelStackTrace).ForeColor = Color.DimGray;
		((Control)labelStackTrace).Location = new Point(24, 323);
		((Control)labelStackTrace).Margin = new Padding(6, 0, 6, 0);
		((Control)labelStackTrace).Name = "labelStackTrace";
		((Control)labelStackTrace).Size = new Size(1064, 38);
		((Control)labelStackTrace).TabIndex = 41;
		((Control)labelStackTrace).Text = "Stack Trace";
		labelStackTrace.TextAlign = (ContentAlignment)16;
		((Control)labelSource).ForeColor = Color.DimGray;
		((Control)labelSource).Location = new Point(24, 210);
		((Control)labelSource).Margin = new Padding(6, 0, 6, 0);
		((Control)labelSource).Name = "labelSource";
		((Control)labelSource).Size = new Size(1070, 38);
		((Control)labelSource).TabIndex = 41;
		((Control)labelSource).Text = "Source";
		labelSource.TextAlign = (ContentAlignment)16;
		((Control)labelMessage1).ForeColor = Color.DimGray;
		((Control)labelMessage1).Location = new Point(24, 23);
		((Control)labelMessage1).Margin = new Padding(6, 0, 6, 0);
		((Control)labelMessage1).Name = "labelMessage1";
		((Control)labelMessage1).Size = new Size(1070, 38);
		((Control)labelMessage1).TabIndex = 41;
		((Control)labelMessage1).Text = "Message";
		labelMessage1.TextAlign = (ContentAlignment)16;
		((Control)textBoxStackTrace).Anchor = (AnchorStyles)15;
		((Control)textBoxStackTrace).BackColor = SystemColors.Window;
		((Control)textBoxStackTrace).Location = new Point(24, 369);
		((Control)textBoxStackTrace).Margin = new Padding(6, 6, 6, 6);
		((TextBoxBase)textBoxStackTrace).Multiline = true;
		((Control)textBoxStackTrace).Name = "textBoxStackTrace";
		((TextBoxBase)textBoxStackTrace).ReadOnly = true;
		textBoxStackTrace.ScrollBars = (ScrollBars)2;
		((Control)textBoxStackTrace).Size = new Size(1066, 214);
		((Control)textBoxStackTrace).TabIndex = 2;
		((Control)textBoxSource).Anchor = (AnchorStyles)13;
		((Control)textBoxSource).BackColor = SystemColors.Window;
		((Control)textBoxSource).Location = new Point(24, 256);
		((Control)textBoxSource).Margin = new Padding(6, 6, 6, 6);
		((TextBoxBase)textBoxSource).Multiline = true;
		((Control)textBoxSource).Name = "textBoxSource";
		((TextBoxBase)textBoxSource).ReadOnly = true;
		((Control)textBoxSource).Size = new Size(1066, 41);
		((Control)textBoxSource).TabIndex = 1;
		((Control)textBoxMessage2).Anchor = (AnchorStyles)13;
		((Control)textBoxMessage2).BackColor = SystemColors.Window;
		((Control)textBoxMessage2).Location = new Point(24, 67);
		((Control)textBoxMessage2).Margin = new Padding(6, 6, 6, 6);
		((TextBoxBase)textBoxMessage2).Multiline = true;
		((Control)textBoxMessage2).Name = "textBoxMessage2";
		((TextBoxBase)textBoxMessage2).ReadOnly = true;
		((Control)textBoxMessage2).Size = new Size(1066, 119);
		((Control)textBoxMessage2).TabIndex = 0;
		((Control)tabPageAssemblies).BackColor = Color.White;
		((Control)tabPageAssemblies).Controls.Add((Control)(object)listViewAssemblies);
		tabPageAssemblies.Location = new Point(8, 39);
		((Control)tabPageAssemblies).Margin = new Padding(6, 6, 6, 6);
		((Control)tabPageAssemblies).Name = "tabPageAssemblies";
		((Control)tabPageAssemblies).Padding = new Padding(6, 6, 6, 6);
		((Control)tabPageAssemblies).Size = new Size(1120, 738);
		tabPageAssemblies.TabIndex = 2;
		((Control)tabPageAssemblies).Text = "Assemblies";
		listViewAssemblies.Activation = (ItemActivation)1;
		((Control)listViewAssemblies).Anchor = (AnchorStyles)15;
		listViewAssemblies.FullRowSelect = true;
		listViewAssemblies.HideSelection = false;
		listViewAssemblies.HotTracking = true;
		listViewAssemblies.HoverSelection = true;
		((Control)listViewAssemblies).Location = new Point(24, 23);
		((Control)listViewAssemblies).Margin = new Padding(6, 6, 6, 6);
		((Control)listViewAssemblies).Name = "listViewAssemblies";
		((Control)listViewAssemblies).Size = new Size(1066, 687);
		((Control)listViewAssemblies).TabIndex = 22;
		listViewAssemblies.UseCompatibleStateImageBehavior = false;
		listViewAssemblies.View = (View)1;
		((Control)buttonClipboard).Anchor = (AnchorStyles)10;
		((Control)buttonClipboard).Location = new Point(84, 833);
		((Control)buttonClipboard).Margin = new Padding(6, 6, 6, 6);
		((Control)buttonClipboard).Name = "buttonClipboard";
		((Control)buttonClipboard).Size = new Size(48, 46);
		((Control)buttonClipboard).TabIndex = 1;
		((ButtonBase)buttonClipboard).UseVisualStyleBackColor = true;
		((Control)buttonClipboard).Click += ButtonClipboard_Click;
		((Control)buttonSaveToFile).Anchor = (AnchorStyles)10;
		((Control)buttonSaveToFile).Location = new Point(24, 833);
		((Control)buttonSaveToFile).Margin = new Padding(6, 6, 6, 6);
		((Control)buttonSaveToFile).Name = "buttonSaveToFile";
		((Control)buttonSaveToFile).Size = new Size(48, 46);
		((Control)buttonSaveToFile).TabIndex = 1;
		((ButtonBase)buttonSaveToFile).UseVisualStyleBackColor = true;
		((Control)buttonSaveToFile).Click += ButtonSaveToFile_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(12f, 25f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(1184, 900);
		((Control)this).Controls.Add((Control)(object)tabControlGeneral);
		((Control)this).Controls.Add((Control)(object)buttonSaveToFile);
		((Control)this).Controls.Add((Control)(object)buttonClipboard);
		((Control)this).Controls.Add((Control)(object)btCancel);
		((Control)this).Controls.Add((Control)(object)btSend);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).KeyPreview = true;
		((Form)this).Margin = new Padding(6, 6, 6, 6);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiExceptionForm";
		((Form)this).ShowIcon = false;
		((Form)this).ShowInTaskbar = false;
		((Form)this).SizeGripStyle = (SizeGripStyle)2;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "Exception Report";
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((Control)tabControlGeneral).ResumeLayout(false);
		((Control)tabPageGeneral).ResumeLayout(false);
		((Control)tabPageGeneral).PerformLayout();
		((ISupportInitialize)picGeneral).EndInit();
		((Control)tabPageException).ResumeLayout(false);
		((Control)tabPageException).PerformLayout();
		((Control)tabPageAssemblies).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
