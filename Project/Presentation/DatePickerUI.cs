using System;

public static class DatePicker
{

    public static DateTime PickDate(string label = "")
    {
        int month = DateSelection.monthMenu(label);

        string dayTitle = string.IsNullOrEmpty(label)
            ? DateSelection.Months[month - 1]
            : $"{label} - {DateSelection.Months[month - 1].Trim()} (pick a day)";

        MenuNavigation dayMenu = new(
            DateSelection.DaysInSelectedMonth(month),
            dayTitle);

        DateTime date = DateSelection.GetDateFromCoordinate(dayMenu.Start2D(), 2026, month);

        // if (date < DateTime.Now.Date)
        //     throw new InvalidOperationException("Cannot pick a date in the past.");

        return date;
    }
}