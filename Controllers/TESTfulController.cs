using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using TESTful.Models;

namespace TESTful.Controllers
{
    public class TESTfulController : Controller
    {
        private static readonly HttpClient client = new HttpClient();
        private static HttpResponseMessage responseMessage;
        private static string RequestMediaType;

        public TESTfulController() {

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Privacy() {
            return View("Privacy");
        }

        [HttpGet]
        public IActionResult About() {
            return View("About");
        }

        [HttpGet]
        public IActionResult Index() {
            return View(new TESTfulViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(TESTfulViewModel Model) {
            if (ModelState.IsValid == false) return View(Model);

            BuildHeaders(Model);
            await SendRequest(Model);
            await ReadResponse(Model);

            return View(Model);
        }

        private void BuildHeaders(TESTfulViewModel Model) {
            if (Model.UseHostHeader == true) client.DefaultRequestHeaders.Host = Model.ResolvedURL.Host;

            client.DefaultRequestHeaders.UserAgent.Clear();
            Model.AgentNameHeader = (string.IsNullOrWhiteSpace(Model.AgentNameHeader) == true) ? "TESTfulRuntime" : Model.AgentNameHeader;
            Model.AgentVersionHeader = (string.IsNullOrWhiteSpace(Model.AgentVersionHeader) == true) ? "1.0" : Model.AgentVersionHeader;
            if (Model.UseAgentHeader == true) client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(Model.AgentNameHeader, Model.AgentVersionHeader));

            client.DefaultRequestHeaders.Accept.Clear();
            Model.AcceptHeader = (string.IsNullOrWhiteSpace(Model.AcceptHeader) == true) ? "*/*" : Model.AcceptHeader;
            if (Model.UseAcceptHeader == true) client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Model.AcceptHeader));

            client.DefaultRequestHeaders.Connection.Clear();
            Model.ConnectionHeader = (string.IsNullOrWhiteSpace(Model.ConnectionHeader) == true) ? "keep-alive" : Model.ConnectionHeader;
            if (Model.UseConnectionHeader == true) client.DefaultRequestHeaders.Connection.Add(Model.ConnectionHeader);

            RequestMediaType =
                (Model.RequestBodyMediaType == MediaType.Text) ? MediaTypeNames.Text.Plain :
                (Model.RequestBodyMediaType == MediaType.HTML) ? MediaTypeNames.Text.Html : MediaTypeNames.Application.Json;
        }

        private async Task SendRequest(TESTfulViewModel Model) {
            Model.RequestBody = (string.IsNullOrWhiteSpace(Model.RequestBody) == true) ? "" : Model.RequestBody;

            switch (Model.Method) {

                case HTTPMethod.GET:
                    responseMessage = await client.GetAsync(Model.ResolvedURL);
                    break;

                case HTTPMethod.POST:
                    responseMessage = await client.PostAsync(Model.ResolvedURL, new StringContent(Model.RequestBody, Encoding.UTF8, RequestMediaType));
                    break;

                case HTTPMethod.PUT:
                    responseMessage = await client.PutAsync(Model.ResolvedURL, new StringContent(Model.RequestBody, Encoding.UTF8, RequestMediaType));
                    break;

                case HTTPMethod.PATCH:
                    responseMessage = await client.PatchAsync(Model.ResolvedURL, new StringContent(Model.RequestBody, Encoding.UTF8, RequestMediaType));
                    break;

                case HTTPMethod.DELETE:
                    responseMessage = await client.DeleteAsync(Model.ResolvedURL);
                    break;

                case HTTPMethod.Length:
                    break;

                default:
                    break;
            }
        }

        private async Task ReadResponse(TESTfulViewModel Model) {
            Model.ResponseURL = Model.ResolvedURL.Host;
            Model.ResponseStatus = ((int)responseMessage.StatusCode).ToString() + " " + responseMessage.StatusCode.ToString();
            Model.ResponseSuccess = responseMessage.IsSuccessStatusCode;
            Model.ResponseHeader = responseMessage.Headers.ToString();
            Model.ResponseBody = await responseMessage.Content.ReadAsStringAsync();

            Model.ResponseCookies = "";

            if (responseMessage.Headers.Contains("Set-Cookie") == false) return;

            foreach (string cookieString in responseMessage.Headers.GetValues("Set-Cookie")) {
                if (string.IsNullOrEmpty(cookieString) == true) {
                    continue;
                }

                Model.ResponseCookies = Model.ResponseCookies + cookieString + "\n \n";
            }
        }
    }
}
