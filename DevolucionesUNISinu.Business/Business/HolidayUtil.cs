using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevolucionesUNISinu.Business.Business
{
    public class HolidayUtil
    {        
        private readonly int year;
        private readonly int easterMonth;
        private readonly int easterDay;

        private List<string> holiDays;
        public HolidayUtil(int newYear)
        {
            year = newYear;
            int a = year % 19;
            int b = year / 100;
            int c = year % 100;
            int d = b / 4;
            int e = b % 4;
            int g = (8 * b + 13) / 25;
            int h = (19 * a + b - d - g + 15) % 30;
            int j = c / 4;
            int k = c % 4;
            int m = (a + 11 * h) / 319;
            int r = (2 * e + 2 * j - k - h + m + 32) % 7;
            easterMonth = (h - m + r + 90) / 25;
            easterDay = (h - m + r + easterMonth + 19) % 32;
            //easterMonth--;

            holiDays = new List<string>
            {
                "1:1", //Primero de enero
                "5:1", // Dia del trabajo 1 de mayo
                "7:20", //Independencia 20 de Julio
                "8:7",  //Batalla de boyaca 7 de agosto
                "12:8", //Maria inmaculada 8 de diciembre
                "12:25" //Navidad 25 de diciembre
            };

            CalculateEmiliani(1, 6);           // Reyes magos 6 de enero
            CalculateEmiliani(3, 19);          //San jose 19 de marzo
            CalculateEmiliani(6, 29);          //San pedro y san pablo 29 de junio
            CalculateEmiliani(8, 15);          //Asuncion 15 de agosto
            CalculateEmiliani(10, 12);          //Descubrimiento de america 12 de octubre
            CalculateEmiliani(11, 1);          //Todos los santos 1 de noviembre
            CalculateEmiliani(11, 11);         //Independencia de cartagena 11 de noviembre

            CalculateOtherHoliday(-3, false);  //jueves santos
            CalculateOtherHoliday(-2, false);  //viernes santo
            CalculateOtherHoliday(43, true);   //Asención del señor de pascua
            CalculateOtherHoliday(64, true);   //Corpus cristi
            CalculateOtherHoliday(71, true);   //Sagrado corazon
        }

        private void CalculateEmiliani(int month, int day)
        {
            DateTime date = new DateTime(year, month, day);
            int dayOfWeek = int.Parse(date.DayOfWeek.ToString("D"));
            switch (dayOfWeek)
            {
                case 0://DOMINGO
                    date = date.AddDays(1);
                    break;
                case 2://MARTES
                    date = date.AddDays(6);
                    break;
                case 3://MIERCOLES
                    date = date.AddDays(5);
                    break;
                case 4://JUEVES
                    date = date.AddDays(4);
                    break;
                case 5://VIERNES
                    date = date.AddDays(3);
                    break;
                case 6://SABADO
                    date = date.AddDays(2);
                    break;
                default:
                    break;
            }

            holiDays.Add($"{date.Month}:{date.Day}");
        }

        private void CalculateOtherHoliday(int days, bool emiliani)
        {
            DateTime date = new DateTime(year, easterMonth, easterDay);
            date = date.AddDays(days);
            if (emiliani)
            {
                CalculateEmiliani(date.Month, date.Day);
            }
            else
            {
                holiDays.Add($"{date.Month}:{date.Day}");
            }
        }

        public bool IsHoliday(int month, int day)
        {
            return holiDays.Contains($"{month}:{day}");
        }

        public static int CountBusinessDays(DateTime dateStart, DateTime dateEnd)
        {
            HolidayUtil holidayUtil = new HolidayUtil(dateStart.Year);

            int days = 0;

            while (dateStart <= dateEnd)
            {
                if (dateStart.Year != holidayUtil.year)
                {
                    holidayUtil = new HolidayUtil(dateStart.Year);
                }

                int numDay = int.Parse(dateStart.DayOfWeek.ToString("D"));
                if (numDay != 0 && numDay != 6 && !holidayUtil.IsHoliday(dateStart.Month, dateStart.Day))
                {
                    days++;
                }

                dateStart = dateStart.AddDays(1);
            }

            return days;
        }
    }
}
