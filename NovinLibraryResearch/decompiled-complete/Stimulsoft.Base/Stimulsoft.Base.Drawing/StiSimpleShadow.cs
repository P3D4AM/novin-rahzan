using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Base.Drawing;

[JsonObject]
[TypeConverter(typeof(StiSimpleShadowConverter))]
public class StiSimpleShadow : ICloneable, IStiJsonReportObject
{
	private Point location = new Point(2, 2);

	private int size = 5;

	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets a border color.")]
	[RefreshProperties(RefreshProperties.All)]
	[StiOrder(100)]
	public virtual Color Color { get; set; } = StiColor.Get("#44222222");


	[StiSerializable]
	[Description("Gets or sets a shadow location.")]
	[RefreshProperties(RefreshProperties.All)]
	[StiOrder(200)]
	public virtual Point Location
	{
		get
		{
			return location;
		}
		set
		{
			if (value != location)
			{
				if (value.X < -10)
				{
					value.X = -10;
				}
				if (value.X > 10)
				{
					value.X = 10;
				}
				if (value.Y < -10)
				{
					value.Y = -10;
				}
				if (value.Y > 10)
				{
					value.Y = 10;
				}
				location = value;
			}
		}
	}

	[StiSerializable]
	[DefaultValue(5)]
	[Description("Gets or sets a shadow size.")]
	[RefreshProperties(RefreshProperties.All)]
	[StiOrder(300)]
	public virtual int Size
	{
		get
		{
			return size;
		}
		set
		{
			if (size != value)
			{
				if (value < 1)
				{
					value = 1;
				}
				if (value > 10)
				{
					value = 10;
				}
				size = value;
			}
		}
	}

	[StiSerializable]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that shadow is visible or not.")]
	[RefreshProperties(RefreshProperties.All)]
	[StiOrder(400)]
	public virtual bool Visible { get; set; }

	[Browsable(false)]
	[JsonIgnore]
	public virtual bool IsDefault
	{
		get
		{
			if (!ShouldSerializeColor() && !ShouldSerializeLocation() && Size == 5)
			{
				return !Visible;
			}
			return false;
		}
	}

	public virtual object Clone()
	{
		return (StiSimpleShadow)MemberwiseClone();
	}

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		if (IsDefault)
		{
			return null;
		}
		JObject jObject = new JObject();
		jObject.AddPropertyColor("Color", Color, "#44222222");
		jObject.AddPropertyPoint("Location", Location);
		jObject.AddPropertyInt("Size", Size, 10);
		jObject.AddPropertyBool("Visible", Visible);
		return jObject;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Color":
				Color = item.DeserializeColor();
				break;
			case "Location":
				Location = item.DeserializePoint();
				break;
			case "Size":
				Size = item.DeserializeInt();
				break;
			case "Visible":
				Visible = item.DeserializeBool();
				break;
			}
		}
	}

	private bool ShouldSerializeColor()
	{
		return Color != StiColor.Get("#44222222");
	}

	private bool ShouldSerializeLocation()
	{
		_ = Location;
		if (Location.X == 2)
		{
			return Location.Y != 2;
		}
		return true;
	}

	public override bool Equals(object obj)
	{
		StiSimpleShadow stiSimpleShadow = (StiSimpleShadow)obj;
		if (stiSimpleShadow != null && stiSimpleShadow.Color == Color && stiSimpleShadow.Location == Location && stiSimpleShadow.Size == Size)
		{
			return stiSimpleShadow.Visible == Visible;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public int GetUniqueCode()
	{
		int num = 0;
		num = (num * 397) ^ Color.GetHashCode();
		num = (num * 397) ^ Location.GetHashCode();
		num = (num * 397) ^ Size.GetHashCode();
		return (num * 397) ^ Visible.GetHashCode();
	}

	public StiSimpleShadow()
	{
	}

	public StiSimpleShadow(Color color, Point location, int size, bool visible)
	{
		Color = color;
		Location = location;
		Size = size;
		Visible = visible;
	}
}
