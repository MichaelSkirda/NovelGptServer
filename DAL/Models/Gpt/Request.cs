using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DAL.Models.Gpt
{
    public class Request
    {
		[JsonProperty(PropertyName = "model")]
        public string ModelId { get; set; } = "";
		[JsonProperty(PropertyName = "messages")]
        public List<Message> Messages { get; set; } = new();

		[JsonProperty(PropertyName = "temperature")]
        public float Temperature { get; set; } = 0.2f;
	}
}
