using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DAL.Models.Gpt
{
	public class Choice
    {
		[JsonProperty(PropertyName = "index")]
        public int Index { get; set; }
		[JsonProperty(PropertyName = "message")]
        public Message Message { get; set; } = new();
		[JsonProperty(PropertyName = "finish_reason")]
        public string FinishReason { get; set; } = "";
	}
}
