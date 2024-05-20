using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DAL.Models.Gpt
{
    public class Message
    {
		[JsonProperty(PropertyName = "role")]
        public string Role { get; set; } = "";
		[JsonProperty(PropertyName = "content")]
        public string Content { get; set; } = "";
	}
}
