using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Entities
{
	public class DayTask
	{
		public int Id { get; set; }
		public string Task { get; set; }
		public bool IsCompleted { get; set; }
		public string TaskText { get; set; }
		public DateTime DayToComplete { get; set; }
		public TaskDecorations TaskDecorations { get; set; }
		[NotMapped]
		public Color SwipeColor
		{
			get
			{
				return IsCompleted ? Colors.Yellow : Colors.Green;
			}
		}
		[NotMapped]
		public string SwipeText
		{
			get
			{
				return IsCompleted ? "UnComplete" : "Complete";
			}
		}
	}
}
