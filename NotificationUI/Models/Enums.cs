using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NotificationUI.Models
{
    public class Enums
    {
        public enum RepetitionFrequency
        {
            Daily = 0,
            [Display(Name = "Every Second Day")]
            SecondDay = 1,
            Weekly = 2,
            [Display(Name = "Every Second Week")]
            SecondWeek = 3,
            [Display(Name = "Every Forth Week")]
            ForthWeek = 4,
            Monthly = 5,
            [Display(Name = "Every Second Month")]
            SecondMonth = 6,
            Once = 7,
            Now = 8
        }
        public enum DayOfTheWeek
        {
            
            Monday = 0,
            Tuesday = 1,
            Wednesday = 2,
            Thursday = 3,
            Friday = 4,
            Saturday = 5,
            Sunday = 6
        }
        public enum ExecutionTime
        {
            [Display(Name = "00")]
            _00 = 0,
             [Display(Name = "01")]
            _01 = 1,
             [Display(Name = "02")]
            _02 = 2,
             [Display(Name = "03")]
            _03 = 3,
             [Display(Name = "04")]
            _04 = 4,
             [Display(Name = "05")]
            _05 = 5,
             [Display(Name = "06")]
            _06 = 6,
             [Display(Name = "07")]
            _07 = 7,
             [Display(Name = "08")]
            _08 = 8,
             [Display(Name = "09")]
            _09 = 9,
             [Display(Name = "10")]
            _10 = 10,
             [Display(Name = "11")]
            _11 = 11,
             [Display(Name = "12")]
            _12 = 12,
             [Display(Name = "13")]
            _13 = 13,
             [Display(Name = "14")]
            _14 = 14,
             [Display(Name = "15")]
            _15 = 15,
             [Display(Name = "16")]
            _16 = 16,
             [Display(Name = "17")]
            _17 = 17,
             [Display(Name = "18")]
            _18 = 18,
             [Display(Name = "19")]
            _19 = 19,
             [Display(Name = "20")]
            _20 = 20,
             [Display(Name = "21")]
            _21 = 21,
             [Display(Name = "22")]
            _22 = 22,
             [Display(Name = "23")]
            _23 = 23
        }
    }
}