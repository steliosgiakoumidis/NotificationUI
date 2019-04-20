using System;
using System.ComponentModel.DataAnnotations;

namespace NotificationUI.Models
{
    public class Sendout
    {
        public int Id { get; set; }
        public string ReminderName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime StartDate { get; set; }
        public Enums.RepetitionFrequency RepetitionFrequency { get; set; }
        public Enums.ExecutionTime ExecutionTime { get; set; }
        public Enums.DayOfTheWeek DayOfTheWeek { get; set; }
        public DateTime? LastRunAt { get; set; }
        public string Parameters { get; set; }
        public string Username { get; set; }
        public string UserGroup { get; set; }
    }
}