using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DAL.Models.Gpt
{
	public class ResponseData
    {
		[JsonProperty(PropertyName = "id")]
        public string Id { get; set; } = "";
		[JsonProperty(PropertyName = "object")]
        public string Object { get; set; } = "";
		[JsonProperty(PropertyName = "created")]
        public ulong Created { get; set; }
		[JsonProperty(PropertyName = "choices")]
        public List<Choice> Choices { get; set; } = new();
		[JsonProperty(PropertyName = "usage")]
        public Usage Usage { get; set; } = new();
	}
}
