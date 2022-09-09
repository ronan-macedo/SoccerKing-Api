using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerKing.Api.Domain.Dtos.Login
{
    public class LoginDtoResult
    {
        [JsonProperty("authenticated")]
        public bool Authenticated { get; set; }
        [JsonProperty("create")]
        public DateTime Create { get; set; }
    }
}
