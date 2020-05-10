using cw3.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cw3.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, IStudentDbService dbService)
        {
            httpContext.Request.EnableBuffering();
            var sciezka = httpContext.Request.Path;
            var querystring = httpContext.Request?.QueryString.ToString();
            var metoda = httpContext.Request.Method.ToString();
            var cialo = "";

            using (StreamReader reader =
                new StreamReader(httpContext.Request.Body, Encoding.UTF8, true, 1024, true))
            {
                cialo = await reader.ReadToEndAsync();
            }

            var sb = new StringBuilder();
            sb.Append("Sciezka: ");
            sb.Append(sciezka);
            sb.Append(" query string: ");
            sb.Append(querystring);
            sb.Append(" metoda: ");
            sb.Append(metoda);
            sb.Append(" cialo: ");
            sb.Append(cialo);

            StreamWriter writer;
            if (!File.Exists("requestsLog.txt"))
            {
                writer = new StreamWriter("requestsLog.txt");
            }
            else {
                writer = File.AppendText("requestsLog.txt");
            }
            writer.WriteLine(sb.ToString());
            writer.Close();


            await _next(httpContext);
        }
    }
}
