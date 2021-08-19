﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyNetCore.Web.SetUp
{

    public class DefaultImageMiddleware
    {
        private readonly RequestDelegate _next;

        public static string DefaultImagePath { get; set; }

        public DefaultImageMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == 404)
            {
                await SetDefaultImage(context);

                //var contentType = context.Request.Headers["accept"].ToString().ToLower();
                //if (contentType.StartsWith("image"))
                //{
                    
                //}
            }
        }

        private async Task SetDefaultImage(HttpContext context)
        {
            try
            {
                FileStream fs = File.OpenRead(DefaultImagePath);
                byte[] bytes = new byte[fs.Length];
                await fs.ReadAsync(bytes, 0, bytes.Length);
                //this header is use for browser cache, format like: "Mon, 15 May 2017 07:03:37 GMT". 
                //context.Response.Headers.Append("Last-Modified", $"{File.GetLastWriteTimeUtc(path).ToString("ddd, dd MMM yyyy HH:mm:ss")} GMT");
                //context.Response.StatusCode = 200;
                await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
            }
            catch (Exception ex)
            {
                await context.Response.WriteAsync(ex.Message);
            }
        }

    }


    public static class DefaultImageMiddlewareExtensions
    {
        /// <summary>
        /// 如果访问的图片不存在，则返回默认图片
        /// </summary>
        /// <param name="app"></param>
        /// <param name="defaultImagePath"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseDefaultImage(this IApplicationBuilder app, string defaultImagePath)
        {
            DefaultImageMiddleware.DefaultImagePath = defaultImagePath;

            return app.UseMiddleware<DefaultImageMiddleware>();
        }
    }
}
