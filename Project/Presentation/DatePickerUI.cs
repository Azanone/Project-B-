using System;

public static class DatePicker
{

    public static DateTime PickDate()
    {
        int month = DateSelection.monthMenu();

        MenuNavigation dayMenu = new(
            DateSelection.DaysInSelectedMonth(month),
            DateSelection.Months[month - 1]);

        DateTime date = DateSelection.GetDateFromCoordinate(dayMenu.Start2D(), 2026, month);

        // if (date < DateTime.Now.Date)
        //     throw new InvalidOperationException("Cannot pick a date in the past.");

        return date;
    }
}