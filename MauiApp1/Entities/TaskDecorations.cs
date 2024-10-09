using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MauiApp1.Entities
{
	public class TaskDecorations
	{
		public int Id { get; set; }
		public int EditorFontSize { get; set; }
		public string TaskTitleColor { get; set; }
		public string TaskDescriptionColor { get; set;}

		[NotMapped]
		public Color TaskTitleColorAsColor
		{
			get { return Color.Parse(TaskTitleColor); }
			set 
			{
				if (value != null)
					TaskTitleColor = value.ToString();
				else
					TaskTitleColor = "White";
			}
		}

		[NotMapped]
		public Color TaskDescriptionColorAsColor
		{
			get { return Color.Parse(TaskDescriptionColor); }
			set
			{
				if (value != null)
					TaskDescriptionColor = value.ToString();
				else
					TaskDescriptionColor = "White";
			}
		}
	}
}
