using System;
using TESTful.CustomValidations;

namespace TESTful.Models
{
    public enum HTTPMethod { GET, POST, PUT, PATCH, DELETE, Length }
    public enum MediaType { Text, HTML, Json }
    public enum SelectedView { Headers, Body, Cookies }

    public class TESTfulViewModel
    {
        public HTTPMethod Method { get; set; } = HTTPMethod.GET;
        [ValidURL] public string RequestURL { get; set; } = "";
        public Uri ResolvedURL { get; set; }

        public bool UseHostHeader { get; set; } = true;
        public string HostHeader { get; set; } = "Calculated on Send";
        public bool UseAgentHeader { get; set; } = true;
        public string AgentNameHeader { get; set; } = "TESTfulRuntime";
        public string AgentVersionHeader { get; set; } = "1.0";
        public bool UseAcceptHeader { get; set; } = true;
        public string AcceptHeader { get; set; } = "*/*";
        public bool UseConnectionHeader { get; set; } = true;
        public string ConnectionHeader { get; set; } = "keep-alive";

        public MediaType RequestBodyMediaType { get; set; } = MediaType.Json;
        public string RequestBody { get; set; } = "";

        public string ResponseURL { get; set; } = "";
        public string ResponseStatus { get; set; } = "200 OK";
        public bool ResponseSuccess { get; set; } = true;
        public SelectedView ResponseView { get; set; } = SelectedView.Body;
        public string ResponseHeader { get; set; } = "";
        public string ResponseBody { get; set; } = "";
        public string ResponseCookies { get; set; } = "";
    }
}
