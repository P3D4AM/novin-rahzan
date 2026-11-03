using System.Collections;
using System.Drawing;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Components;

public class StiConditionHelper : StiFilter
{
	public static void Apply(object comp, string styleName)
	{
		StiComponent stiComponent = comp as StiComponent;
		if (styleName.StartsWith("##") && styleName.EndsWith("##"))
		{
			styleName = styleName.Substring(2, styleName.Length - 4);
			foreach (StiStyle style in StiOptions.Designer.Styles)
			{
				if (style.Name == styleName)
				{
					style.SetStyleToComponent(stiComponent);
					ApplyParentStyle(style, stiComponent);
					return;
				}
			}
		}
		StiBaseStyle stiBaseStyle = stiComponent.Report.Styles[styleName];
		if (stiBaseStyle != null)
		{
			stiBaseStyle.SetStyleToComponent(stiComponent);
			ApplyParentStyle(stiBaseStyle, stiComponent);
			return;
		}
		stiComponent.Report.WriteToReportRenderingMessages("'" + styleName + "' style not found in conditions of '" + stiComponent.Name + "' component!");
	}

	private static void ApplyParentStyle(StiBaseStyle style, StiComponent comp)
	{
		if (comp != null && comp.Report != null && comp.Report.Engine != null)
		{
			if (comp.Report.Engine.HashParentStyles == null)
			{
				comp.Report.Engine.HashParentStyles = new Hashtable();
			}
			comp.Report.Engine.HashParentStyles[comp] = style;
		}
	}

	public static void ApplyFont(object component, Font font, StiConditionPermissions perms)
	{
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		if (!(component is IStiFont stiFont))
		{
			return;
		}
		bool flag = false;
		string name = stiFont.Font.Name;
		float size = stiFont.Font.Size;
		bool bold = stiFont.Font.Bold;
		bool italic = stiFont.Font.Italic;
		bool underline = stiFont.Font.Underline;
		bool strikeout = stiFont.Font.Strikeout;
		if ((perms & StiConditionPermissions.Font) > StiConditionPermissions.None)
		{
			flag = true;
			name = font.Name;
		}
		if ((perms & StiConditionPermissions.FontSize) > StiConditionPermissions.None)
		{
			flag = true;
			size = font.Size;
		}
		if ((perms & StiConditionPermissions.FontStyleBold) > StiConditionPermissions.None)
		{
			flag = true;
			bold = font.Bold;
		}
		if ((perms & StiConditionPermissions.FontStyleItalic) > StiConditionPermissions.None)
		{
			flag = true;
			italic = font.Italic;
		}
		if ((perms & StiConditionPermissions.FontStyleUnderline) > StiConditionPermissions.None)
		{
			flag = true;
			underline = font.Underline;
		}
		if ((perms & StiConditionPermissions.FontStyleStrikeout) > StiConditionPermissions.None)
		{
			flag = true;
			strikeout = font.Strikeout;
		}
		if (flag)
		{
			FontStyle val = (FontStyle)0;
			if (bold)
			{
				val = (FontStyle)(val | 1);
			}
			if (italic)
			{
				val = (FontStyle)(val | 2);
			}
			if (underline)
			{
				val = (FontStyle)(val | 4);
			}
			if (strikeout)
			{
				val = (FontStyle)(val | 8);
			}
			stiFont.Font = StiFontCollection.CreateFont(name, size, val);
		}
	}
}
