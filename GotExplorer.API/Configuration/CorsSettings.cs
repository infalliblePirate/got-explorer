using Microsoft.AspNetCore.Cors.Infrastructure;

namespace GotExplorer.API.Configuration
{
    public class CorsSettings
    {
        public string[]? Origins { get; set; }
        public string[]? Headers { get; set; }
        public string[]? Methods { get; set; }
    }
}
