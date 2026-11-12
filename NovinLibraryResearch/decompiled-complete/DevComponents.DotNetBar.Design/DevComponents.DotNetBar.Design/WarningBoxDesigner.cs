using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.IO;
using System.Windows.Forms.Design;
using DevComponents.DotNetBar.Controls;
using Microsoft.Win32;

namespace DevComponents.DotNetBar.Design;

public class WarningBoxDesigner : ControlDesigner
{
	private DesignerActionListCollection _ActionLists;

	public override DesignerActionListCollection ActionLists
	{
		get
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			if (_ActionLists == null)
			{
				_ActionLists = new DesignerActionListCollection();
				_ActionLists.Add((DesignerActionList)(object)new WarningBoxActionList(this));
			}
			return _ActionLists;
		}
	}

	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		((ControlDesigner)this).InitializeNewComponent(defaultValues);
		SetDefaults();
	}

	private void SetDefaults()
	{
		if (((ControlDesigner)this).Control is WarningBox component)
		{
			TypeDescriptor.GetProperties(component)["Text"].SetValue(component, "<b>Warning Box</b> control with <i>text-markup</i> support.");
			Image defaultImage = GetDefaultImage();
			if (defaultImage != null)
			{
				TypeDescriptor.GetProperties(component)["Image"].SetValue(component, defaultImage);
			}
		}
	}

	private Image GetDefaultImage()
	{
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
		string text = "images\\Warning.png";
		try
		{
			RegistryKey val = Registry.LocalMachine;
			string text2 = "";
			try
			{
				if (val != null)
				{
					val = val.OpenSubKey("Software\\DevComponents\\DotNetBar");
				}
				if (val != null)
				{
					text2 = val.GetValue("InstallationFolder", (object)"").ToString();
				}
			}
			finally
			{
				if (val != null)
				{
					val.Close();
				}
			}
			if (text2 != "")
			{
				if (text2.Substring(text2.Length - 1, 1) != "\\")
				{
					text2 += "\\";
				}
				text2 = ((!File.Exists(text2 + text)) ? "" : (text2 + text));
			}
			if (text2 != "")
			{
				return (Image)new Bitmap(text2);
			}
		}
		catch (Exception)
		{
		}
		return null;
	}
}
