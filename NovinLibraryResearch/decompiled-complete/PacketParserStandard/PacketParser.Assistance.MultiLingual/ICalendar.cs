using System;
using System.Collections.Concurrent;
using System.Globalization;
using PacketParser.Entities;

namespace PacketParser.Assistance.MultiLingual;

public interface ICalendar
{
	ConcurrentDictionary<string, string> LstLangauge { get; set; }

	string Content { get; }

	string[] GetListOfMonth();

	DateTime CalendarToMiladi(string dateParam);

	bool CheckDate(string dateSh);

	string MiladiToCalendar(DateTime dateM);

	string ToShortDate(DateTime dateM);

	int EndDayOfMonthInt(short civilMonth, int civilyear);

	string GetMonthName(string month);

	string GetMonthName(DateTime date);

	string ToLongDateWithoutYear(DateTime dateM);

	string ToLongTimeString(DateTime dateM);

	DateTime EndDayOfMonth(string Date, string time);

	DateTime EndDayOfMonth(short Month, int Year, string time);

	DateTime EndDayOfMonth();

	DateTime StartDayOfMonth();

	DateTime StartDayOfMonth(string Date, string time);

	DateTime StartDayOfMonth(short Month, int Year, string time);

	string Today();

	string AddToDate(int count, string date, EnPartDate part);

	string GetDayNameOfWeek(DateTime dateM);

	string AddYear(string Date, int yearCount);

	string GetPartday(string date, EnPartDate type);

	int GetDayOfDate(string dateSh);

	string Minutes2TimeFormat(int min);

	int GetYearOfDate(string dateSh);

	int GetMonthOfDate(string date);

	string AddMonth(string Date, int Count);

	string MiladitoCalenderText(DateTime dateM);

	string DateToFormatTime(DateTime dateM);

	string GetValue(string key);

	int GetWeekNumber(DateTime dateM, CalendarWeekRule role);

	DateTime GetFirstDayOfWeek(DateTime dateM, string time);

	DateTime GetEndDayOfWeek(DateTime dateM, string time);

	void Dispose();
}
