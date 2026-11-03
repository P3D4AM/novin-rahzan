using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Controls;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Engine;

namespace Stimulsoft.Report.Win.Parameters;

internal sealed class StiParameterHelper
{
	public static string ParseVariableExpression(StiVariable variable, StiReport report, string defaultValue)
	{
		try
		{
			StiText stiText = new StiText();
			stiText.Name = "**VariableRequestFromUser**";
			stiText.Page = report.Pages[0];
			defaultValue = StiParser.ParseTextValue("{" + variable.Value + "}", stiText)?.ToString();
		}
		catch
		{
			if (report[variable.Name] != null)
			{
				defaultValue = report[variable.Name].ToString();
			}
		}
		return defaultValue;
	}

	public static Label CreateLabel(string text)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		return new Label
		{
			AutoSize = true,
			Text = text
		};
	}

	public static void CheckDropDownWidth(ComboBox comboBox, List<StiDialogInfoItem> list)
	{
		if (!StiOptions.Engine.Interaction.ComboBoxDropDownAutoWidth)
		{
			return;
		}
		Graphics globalMeasureGraphics = StiReport.GlobalMeasureGraphics;
		float num = 0f;
		Font font = ((Control)comboBox).Font;
		foreach (StiDialogInfoItem item in list.Where((StiDialogInfoItem i) => i != null && !string.IsNullOrEmpty(i.Value)))
		{
			SizeF sizeF = globalMeasureGraphics.MeasureString(item.Value, font);
			if (sizeF.Width > num)
			{
				num = sizeF.Width;
			}
		}
		if (num > 0f)
		{
			comboBox.DropDownWidth = (int)num + 4;
		}
	}

	public static void CheckDropDownWidth(StiDropDownBox comboBox, List<StiDialogInfoItem> list)
	{
		if (!StiOptions.Engine.Interaction.ComboBoxDropDownAutoWidth)
		{
			return;
		}
		Graphics globalMeasureGraphics = StiReport.GlobalMeasureGraphics;
		float num = 0f;
		Font font = ((Control)comboBox).Font;
		foreach (StiDialogInfoItem item in list.Where((StiDialogInfoItem i) => i != null && !string.IsNullOrEmpty(i.Value)))
		{
			SizeF sizeF = globalMeasureGraphics.MeasureString(item.Value, font);
			if (sizeF.Width > num)
			{
				num = sizeF.Width;
			}
		}
		if (num > 0f)
		{
			comboBox.DropDownWidth = (int)num + 4;
		}
	}

	public static int CheckDropDownWidth(ListBox listBox, ObjectCollection list)
	{
		if (!StiOptions.Engine.Interaction.ComboBoxDropDownAutoWidth)
		{
			return ((Control)listBox).Width;
		}
		Graphics globalMeasureGraphics = StiReport.GlobalMeasureGraphics;
		float num = 0f;
		Font font = ((Control)listBox).Font;
		foreach (object item in list)
		{
			if (item != null && !string.IsNullOrEmpty(item.ToString()))
			{
				SizeF sizeF = globalMeasureGraphics.MeasureString(item.ToString(), font);
				if (sizeF.Width > num)
				{
					num = sizeF.Width;
				}
			}
		}
		if (num > 0f)
		{
			((Control)listBox).Width = StiScale.I(num + 30f);
		}
		if (((Control)listBox).Width < StiScale.I(150))
		{
			((Control)listBox).Width = StiScale.I(150);
		}
		return ((Control)listBox).Width;
	}
}
