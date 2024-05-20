using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
	public class Dialog
	{
		public int Id { get; set; }	
		public List<MessageDto>? Messages { get; set; }
		public string? UserMessage { get; set; }
		public string? Answer { get; set; }
		public string Location { get; set; } = "";
		public string? Accepted { get; set; }
	}
}
