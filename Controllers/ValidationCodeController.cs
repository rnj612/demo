﻿using Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Util;

namespace Api.Controllers
{
    public class ValidationCodeController : ApiController
    {
        static Dictionary<string, DateTime> cDict = new Dictionary<string, DateTime>();
        [HttpGet]
        [Route("ValidationCode/SendImg")]
        public async Task<HttpResponseMessage> SendImg(string uuid)
        {
            try
            {
                DateTime now = DateTime.Now;
                if (cDict.ContainsKey(uuid))
                {
                    TimeSpan ts = now - cDict[uuid];
                    if (ts.TotalSeconds <= 1.5)
                    {
                        return null;
                    }
                    cDict[uuid] = now;
                }
                else
                    cDict.Add(uuid, now);
            }
            catch
            {

            }

            if (string.IsNullOrWhiteSpace(uuid)) return null;
            Util.ValidationCode vc = new Util.ValidationCode();
            Image img = vc.NextImage(4, System.Drawing.Drawing2D.HatchStyle.DiagonalCross, false, uuid);
            System.IO.MemoryStream Ms = new System.IO.MemoryStream();
            img.Save(Ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] imgByte = new byte[Ms.Length];
            Ms.Position = 0;
            await Ms.ReadAsync(imgByte, 0, Convert.ToInt32(Ms.Length));
            Ms.Close();

            var resp = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(imgByte)
            };
            resp.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpg");
            return resp;
        }
        [HttpGet]
        [Route("ValidationCode/ValidCode")]
        public async Task<bool> ValidCode(string uuid, string code)
        {
            return await Util.ValidationCode.GetCode(uuid, code);
        }
        [HttpGet]
        [Route("ValidationCode/ValidMobile")]
        public async Task<bool> ValidMobile(string uuid, string code)
        {
            return await Util.Mobile.GetCode(uuid, code);
        }
        [HttpGet]
        [Route("ValidationCode/ValidCodeMobile")]
        public async Task<string> ValidCodeMobile(string mobile, string code, string type)
        {
            if (string.IsNullOrWhiteSpace(mobile) || string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(type))
                return "参数不全";

            try
            {
                DateTime now = DateTime.Now;
                if (cDict.ContainsKey(mobile))
                {
                    TimeSpan ts = now - cDict[mobile];
                    if (ts.TotalSeconds <= 1.2)
                    {
                        return null;
                    }
                    cDict[mobile] = now;
                }
                else
                    cDict.Add(mobile, now);
            }
            catch
            {

            }

            bool result = await Util.ValidationCode.GetCode(mobile, code);
            if (result)
            {
                if (string.IsNullOrWhiteSpace(mobile) || string.IsNullOrWhiteSpace(type)) return "参数不全";
                string msg = string.Empty;

                string ip = ((System.Web.HttpContextWrapper)Request.Properties["MS_HttpContext"]).Request.UserHostAddress;
                if (!Util.Mobile.SmsCanIp(ip, 50, 20)) return "该ip调用接口太频繁";
                if (mobile.Length != 11) return "手机号格式错误";
                if (!Util.Mobile.SmsCanIp(mobile, 50, 5)) return "该号码调用接口太频繁";

                UserModel user = await DBOper.User.GetOne(0, mobile, string.Empty);
                int template = 0;
                if (type.Equals("Register"))
                {
                    if (user != null)
                        return "该号码已注册";
                    code = await Util.Mobile.SetCode(mobile);
                    msg = ("您正在使用手机号注册，验证码：" + code);
                    template = 5;
                }
                else if (type.Equals("ForgetPwd"))
                {
                    if (user == null)
                        return "该号码未注册";
                    code = await Util.Mobile.SetCode(mobile);
                    msg = ("您正在使用密码找回功能，验证码：" + code);
                    template = 6;
                }
                string rst = await Util.Mobile.SendSms(mobile, msg);

                Util.Mobile.SetCanIp(ip);
                Util.Mobile.SetCanIp(mobile);

                return rst;
            }
            else return "验证码错误";
        }
    }
}
