using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DAL.Models
{
	public class Usage
    {
		[JsonProperty(PropertyName = "prompt_tokens")]
		public int PromptTokens { get; set; }
		[JsonProperty(PropertyName = "completion_tokens")]
		public int CompletionTokens { get; set; }
		[JsonProperty(PropertyName = "total_tokens")]
		public int TotalTokens { get; set; }
    }
}
