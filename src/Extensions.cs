using System.Net;
using Microsoft.Azure.Functions.Worker.Http;

namespace SqlQuery.WebApi;
public static class Extensions
{
    public static HttpResponseData ReturnBad(this HttpRequestData req, string msg = null)
    {
        var bad = req.CreateResponse(HttpStatusCode.BadRequest);
        bad.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        if (msg != null)
        {
            bad.WriteString(msg);
        }

        return bad;
    }

    public static HttpResponseData ReturnOk(this HttpRequestData req)
    {
        var ok = req.CreateResponse(HttpStatusCode.OK);
        ok.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        return ok;
    }
}
