using System;
using System.Collections;
using System.Xml.Serialization;

namespace Stimulsoft.Editor;

public class ColorThemes : IColorThemes, ICloneable
{
	private int activeThemeIndex = -1;

	private ArrayList colorThemes = new ArrayList();

	public int ActiveThemeIndex
	{
		get
		{
			return activeThemeIndex;
		}
		set
		{
			activeThemeIndex = value;
		}
	}

	[XmlIgnore]
	public IColorTheme ActiveTheme
	{
		get
		{
			return (IColorTheme)colorThemes[activeThemeIndex];
		}
		set
		{
			colorThemes[activeThemeIndex] = value;
		}
	}

	public IColorTheme[] Themes
	{
		get
		{
			IColorTheme[] array = new ColorTheme[colorThemes.Count];
			IColorTheme[] array2 = array;
			for (int i = 0; i < colorThemes.Count; i++)
			{
				array2[i] = (IColorTheme)colorThemes[i];
			}
			return array2;
		}
		set
		{
			colorThemes.Clear();
			for (int i = 0; i < value.Length; i++)
			{
				colorThemes.Add(value[i]);
			}
		}
	}

	public void Add(IColorTheme colorTheme)
	{
		colorThemes.Add(colorTheme);
	}

	public bool Remove(int index)
	{
		if (index >= 0 && index < colorThemes.Count)
		{
			colorThemes.RemoveAt(index);
			return true;
		}
		return false;
	}

	public void Clear()
	{
		colorThemes.Clear();
	}

	public object Clone()
	{
		ColorThemes colorThemes = new ColorThemes();
		colorThemes.activeThemeIndex = activeThemeIndex;
		foreach (ColorTheme colorTheme in this.colorThemes)
		{
			colorThemes.Add((ColorTheme)colorTheme.Clone());
		}
		return colorThemes;
	}
}
