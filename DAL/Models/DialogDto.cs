using DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class DialogDto
    {
        public List<MessageDto> Messages { get; set; } = new List<MessageDto>();
        public string? Answer { get; set; }
        public string Location { get; set; } = "";
        public string? UserMessage { get; set; }
	}
}
