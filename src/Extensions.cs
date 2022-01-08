using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.Azure.Functions.Worker.Http;

namespace SqlQuery.WebApi;
public static class Extensions
{
    public static HttpResponseData ReturnBad(this HttpRequestData req, string? msg = default)
    {
        var bad = req.CreateResponse(HttpStatusCode.BadRequest);
        bad.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        if (msg != default)
        {
            bad.WriteString(msg);
        }

        return bad;
    }

    public static HttpResponseData ReturnOk(this HttpRequestData req, string? msg = default)
    {
        var ok = req.CreateResponse(HttpStatusCode.OK);
        ok.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        if (msg != default)
        {
            ok.WriteString(msg);
        }

        return ok;
    }
}
