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

    public static string ToCsv(this IDataReader sqlReader)
    {
        var sb = new StringBuilder();

        var columnNames = Enumerable
            .Range(0, sqlReader.FieldCount)
            .Select(sqlReader.GetName)
            .ToList();
        var header = string.Join(',', columnNames);

        sb.Append(header);
        sb.AppendLine();
        while (sqlReader.Read())
        {
            for (int i = 0; i < sqlReader.FieldCount; i++)
            {
                string? value = sqlReader[i].ToString();
                if (value is null)
                    continue;

                if (value.Contains(","))
                    value = "\"" + value + "\"";

                sb.Append(value.Replace(Environment.NewLine, " ") + ",");
            }

            sb.Length--; // Remove the last comma
            sb.AppendLine();
        }
        
        return sb.ToString();
    }
}
