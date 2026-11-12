using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Engine;

namespace Stimulsoft.Report.Dictionary.Design;

public class StiSqlExpressionsForm : StiFormUX
{
	[ToolboxItem(false)]
	internal class StiLabel : Label
	{
		protected override void OnPaint(PaintEventArgs e)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Expected O, but got Unknown
			StringFormat val = new StringFormat();
			try
			{
				val.Alignment = (StringAlignment)0;
				val.LineAlignment = (StringAlignment)1;
				val.FormatFlags = (StringFormatFlags)4096;
				val.Trimming = (StringTrimming)3;
				SolidBrush val2 = new SolidBrush(StiUX.IsDark ? StiUX.InputForeground : SystemColors.ControlText);
				try
				{
					e.Graphics.DrawString(((Control)this).Text, ((Control)this).Font, (Brush)(object)val2, (RectangleF)((Control)this).ClientRectangle, val);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
	}

	private StiButtonUX btCancel;

	private StiButtonUX btOk;

	private Container components;

	private ArrayList texts = new ArrayList();

	private List<string> exps = new List<string>();

	private StiReport report;

	private Hashtable parameters = new Hashtable();

	private StiSqlSource sqlSource;

	public string Value { get; set; }

	public bool CheckResult { get; set; }

	public StiSqlExpressionsForm(StiSqlSource source, List<string> exps, StiReport report)
	{
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Expected O, but got Unknown
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_0346: Unknown result type (might be due to invalid IL or missing references)
		//IL_0353: Expected O, but got Unknown
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (((Component)(object)this).DesignMode)
		{
			return;
		}
		sqlSource = source;
		Value = source.SqlCommand;
		this.exps = exps;
		((Control)this).Text = StiLocalization.Get("FormTitles", "SqlExpressionsForm");
		this.report = report;
		int num = StiScale.I12;
		bool flag = true;
		if (source.AllowExpressions)
		{
			for (int i = 0; i < exps.Count; i++)
			{
				string text = exps[i];
				if (text != null && text.Length > 1 && text[0] == '{')
				{
					StiTextBox stiTextBox = new StiTextBox();
					((Control)stiTextBox).Left = StiScale.I(154);
					((Control)stiTextBox).Top = num;
					((Control)stiTextBox).Width = StiScale.I(100);
					((Control)stiTextBox).Text = GetExpressionResult(text);
					StiTextBox stiTextBox2 = stiTextBox;
					texts.Add(stiTextBox2);
					StiLabel stiLabel = new StiLabel();
					((Control)stiLabel).ForeColor = Color.DimGray;
					((Control)stiLabel).Left = StiScale.I12;
					((Control)stiLabel).Top = num;
					((Control)stiLabel).Width = StiScale.I(120);
					((Control)stiLabel).Height = ((Control)stiTextBox2).Height;
					((Control)stiLabel).Text = ((text.StartsWith("{") && text.EndsWith("}")) ? text.Substring(1, text.Length - 2) : text);
					StiLabel stiLabel2 = stiLabel;
					((Control)this).Controls.Add((Control)(object)stiLabel2);
					((Control)this).Controls.Add((Control)(object)stiTextBox2);
					if (flag)
					{
						((Control)stiTextBox2).Focus();
					}
					num += StiScale.I(32);
					((Control)this).Height = ((Control)this).Height + StiScale.I(32);
					CheckResult = true;
					flag = false;
				}
			}
		}
		if (sqlSource != null)
		{
			foreach (StiDataParameter parameter in source.Parameters)
			{
				Type type = source.ConvertDbTypeToType(parameter.Type);
				StiLabel stiLabel3 = new StiLabel();
				((Control)stiLabel3).ForeColor = Color.DimGray;
				((Control)stiLabel3).Left = StiScale.I12;
				((Control)stiLabel3).Top = num;
				((Control)stiLabel3).Width = StiScale.I(120);
				((Control)stiLabel3).Text = parameter.Name;
				StiLabel stiLabel4 = stiLabel3;
				((Control)this).Controls.Add((Control)(object)stiLabel4);
				if (type == typeof(DateTime))
				{
					DateTimePicker val = new DateTimePicker
					{
						Left = StiScale.I(154),
						Top = num,
						Width = StiScale.I(100),
						Format = (DateTimePickerFormat)2
					};
					object parameterValue = GetParameterValue(parameter, report);
					if (parameterValue is DateTime)
					{
						val.Value = (DateTime)parameterValue;
					}
					((Control)stiLabel4).Height = ((Control)val).Height;
					texts.Add(val);
					parameters[parameter] = val;
					((Control)this).Controls.Add((Control)(object)val);
					if (flag)
					{
						((Control)val).Focus();
					}
				}
				else if (type == typeof(bool))
				{
					CheckBox val2 = new CheckBox
					{
						Left = StiScale.I(154),
						Top = num,
						Width = StiScale.I(100),
						Height = StiScale.I16
					};
					((Control)stiLabel4).Height = ((Control)val2).Height;
					texts.Add(val2);
					parameters[parameter] = val2;
					((Control)this).Controls.Add((Control)(object)val2);
					if (flag)
					{
						((Control)val2).Focus();
					}
				}
				else
				{
					StiTextBox stiTextBox3 = new StiTextBox();
					((Control)stiTextBox3).Left = StiScale.I(154);
					((Control)stiTextBox3).Top = num;
					((Control)stiTextBox3).Width = StiScale.I(100);
					((Control)stiTextBox3).Text = GetExpressionResult(parameter.Expression);
					StiTextBox stiTextBox4 = stiTextBox3;
					((Control)stiLabel4).Height = ((Control)stiTextBox4).Height;
					if (!(type == typeof(string)))
					{
						stiTextBox4.TextAlign = (HorizontalAlignment)1;
					}
					texts.Add(stiTextBox4);
					parameters[parameter] = stiTextBox4;
					((Control)this).Controls.Add((Control)(object)stiTextBox4);
					if (flag)
					{
						((Control)stiTextBox4).Focus();
					}
				}
				num += StiScale.I(32);
				((Control)this).Height = ((Control)this).Height + StiScale.I(32);
				CheckResult = true;
				flag = false;
			}
		}
		((Control)btCancel).TabIndex = 1001;
		((Control)btOk).TabIndex = 1000;
		((Control)this).Height = ((Control)this).Height + StiScale.I12;
		((Form)this).ShowIcon = false;
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
		btCancel = new StiButtonUX();
		btOk = new StiButtonUX();
		((Control)this).SuspendLayout();
		((Control)btCancel).Anchor = (AnchorStyles)10;
		((Button)btCancel).DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(331, 8);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 23);
		((Control)btCancel).TabIndex = 6;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Button)btOk).DialogResult = (DialogResult)1;
		((Control)btOk).Location = new Point(250, 8);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 23);
		((Control)btOk).TabIndex = 5;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.FromArgb(250, 250, 250);
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(418, 39);
		((Control)this).Controls.Add((Control)(object)btOk);
		((Control)this).Controls.Add((Control)(object)btCancel);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiSqlExpressionsForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiSqlExpressionForm";
		((Form)this).Closed += This_Closed;
		((Control)this).ResumeLayout(false);
	}

	private object GetParameterValue(StiDataParameter parameter, StiReport report)
	{
		object parameterValue = parameter.GetParameterValue();
		if (parameterValue != null)
		{
			return parameterValue;
		}
		if (string.IsNullOrWhiteSpace(parameter.Expression))
		{
			return null;
		}
		StiText stiText = new StiText();
		stiText.Name = "**ParameterValueCalculation**";
		stiText.Page = report.Pages[0];
		string text = parameter.Expression;
		if (!text.TrimStart(new char[0]).StartsWith("{"))
		{
			text = "{" + text;
		}
		if (!text.TrimEnd(new char[0]).EndsWith("}"))
		{
			text += "}";
		}
		return StiParser.ParseTextValue(text, stiText);
	}

	private string GetExpressionResult(string exp2)
	{
		if (string.IsNullOrEmpty(exp2))
		{
			return string.Empty;
		}
		string text = null;
		try
		{
			text = ((exp2.StartsWith("{") && exp2.EndsWith("}")) ? exp2.Substring(1, exp2.Length - 2) : exp2);
			foreach (StiVariable variable in report.Dictionary.Variables)
			{
				if (variable.Name == text)
				{
					return Convert.ToString(variable.Eval(report));
				}
			}
			int num = text.LastIndexOf(".", StringComparison.InvariantCulture);
			if (num != -1)
			{
				string text2 = text.Substring(num + 1);
				if (text2.ToLowerInvariant() == "text" || text2.ToLowerInvariant() == "value")
				{
					text = text.Substring(0, num);
					StiComponentsCollection stiComponentsCollection = report.GetComponents();
					StiComponent stiComponent = stiComponentsCollection[text];
					if (stiComponent != null)
					{
						if (stiComponent is IStiTextBoxControl && text2.ToLowerInvariant() == "text")
						{
							return ((IStiTextBoxControl)stiComponent).Text;
						}
						if (stiComponent is IStiDateTimePickerControl && text2.ToLowerInvariant() == "value")
						{
							return ((IStiDateTimePickerControl)stiComponent).Value.ToString();
						}
					}
				}
			}
		}
		catch
		{
		}
		StiText stiText = new StiText
		{
			Page = report.Pages[0],
			Name = "*SQLParameter*"
		};
		string inputExpression = ((exp2.StartsWith("{") && exp2.EndsWith("}")) ? exp2 : ("{" + exp2 + "}"));
		bool storeToPrint = false;
		object obj2 = StiParser.ParseTextValue(inputExpression, stiText, stiText, ref storeToPrint, executeIfStoreToPrint: false, returnAsmList: true);
		if (obj2 == null)
		{
			return text;
		}
		return Convert.ToString(StiParser.ParseTextValue(inputExpression, stiText));
	}

	private void This_Closed(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		if ((int)((Form)this).DialogResult == 2)
		{
			return;
		}
		string text = string.Empty;
		int num = 0;
		for (int i = 0; i < exps.Count; i++)
		{
			string text2 = exps[i];
			text = ((text2 == null || text2.Length <= 1 || text2[0] != '{') ? (text + text2) : (text + ((Control)(StiTextBox)texts[num++]).Text));
		}
		if (!string.IsNullOrWhiteSpace(text))
		{
			Value = text;
		}
		if (sqlSource == null)
		{
			return;
		}
		string numberDecimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
		foreach (StiDataParameter parameter in sqlSource.Parameters)
		{
			try
			{
				object? obj = parameters[parameter];
				Control val = (Control)((obj is Control) ? obj : null);
				if (val is DateTimePicker)
				{
					parameter.ParameterValue = ((DateTimePicker)val).Value;
					continue;
				}
				if (val is CheckBox)
				{
					parameter.ParameterValue = ((CheckBox)val).Checked;
					continue;
				}
				Type type = sqlSource.ConvertDbTypeToType(parameter.Type);
				string text3 = ((Control)(StiTextBox)(object)val).Text;
				if (type == typeof(string))
				{
					parameter.ParameterValue = text3;
					continue;
				}
				if (type == typeof(long))
				{
					parameter.ParameterValue = long.Parse(text3);
					continue;
				}
				if (type == typeof(decimal))
				{
					parameter.ParameterValue = decimal.Parse(text3.Replace(".", ",").Replace(",", numberDecimalSeparator));
					continue;
				}
				if (type == typeof(double))
				{
					parameter.ParameterValue = double.Parse(text3.Replace(".", ",").Replace(",", numberDecimalSeparator));
					continue;
				}
				if (type == typeof(Guid))
				{
					parameter.ParameterValue = new Guid(text3);
					continue;
				}
				try
				{
					parameter.ParameterValue = StiConvert.ChangeType(text3, type);
				}
				catch
				{
				}
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}
	}
}
