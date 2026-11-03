using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace AccSqlServerPersistence.Entities;

[Table("Acc_Gps")]
public class Gps : IGPS, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDeliverableEntity
{
	[Key]
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; } = true;


	public decimal Accuracy { get; set; }

	public EnGpsActivity Activity { get; set; }

	public decimal Altitude { get; set; }

	public decimal Bearing { get; set; }

	public string? ColorHex { get; set; }

	public DateTime DateM { get; set; }

	[NotMapped]
	public string? Description
	{
		get
		{
			using (new JaegerMonitor("Description", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Entities\\Gps.cs"))
			{
				try
				{
					string text = "زمان:" + Calendar.Minutes2TimeFormat(GPSTime.Hour * 60 + GPSTime.Minute);
					if (PointDurationSeconds > 120m)
					{
						text = text + " ** مدت توقف:" + PointDurationTime;
					}
					if (Math.Round(FromLastPointDistance, 0) > 0m)
					{
						text = text + " ** فاصله از نقطه قبلی:" + FromLastPointDistance.ToString("N0") + "متر";
					}
					if (Math.Round(FromStartPointDistance, 0) > 0m)
					{
						text = text + " ** فاصله از ایتدای مسیر:" + FromStartPointDistance.ToString("N0") + "متر";
					}
					return text;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "Description", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Entities\\Gps.cs", 40);
					return null;
				}
			}
		}
		set
		{
		}
	}

	public decimal Distance { get; set; }

	public decimal FromLastPointDistance { get; set; }

	public decimal FromStartPointDistance { get; set; }

	public DateTime GPSTime { get; set; }

	public string? Icon { get; set; }

	public decimal Latitude { get; set; }

	public decimal Longitude { get; set; }

	public decimal PointDurationSeconds { get; set; }

	[NotMapped]
	public string PointDurationTime => Calendar.Minutes2TimeFormat((int)(PointDurationSeconds / 60m));

	public int PreviewsID { get; set; }

	public decimal Speed { get; set; }

	public decimal Time { get; set; }

	public int UId { get; set; }

	public Guid UserAccGuid { get; set; }

	public int ReferencedId { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string? DeliveryMessage { get; set; }

	public string? DeliveryShortMessage { get; set; }

	public string? HostRefrenced { get; set; }
}
