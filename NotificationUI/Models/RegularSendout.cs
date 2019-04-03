using System;
using System.ComponentModel.DataAnnotations;

namespace NotificationUI.Models
{
    public class RegularSendout
    {
        public int Id { get; set; }
        public string ReminderName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime StartDate { get; set; }
        public string RepetitionFrequency { get; set; }
        public int ExecutionTime { get; set; }
        public string DayOfTheWeek { get; set; }
        public DateTime? LastRunAt { get; set; }
        public string Parameters { get; set; }
        public string Username { get; set; }
        public string UserGroup { get; set; }
        public string Priority { get; set; }
    }
}