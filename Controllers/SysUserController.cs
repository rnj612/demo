using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Util;

namespace Api.Controllers
{
    public class SysUserController : ApiController
    {
        [HttpGet]
        [Route("SysUser/Login")]
        public async Task<SysUserModel> Login(string login_name, string login_pwd)
        {
            SysUserModel user = await DBOper.SysUser.Login(login_name, login_pwd);
            if (user != null)
            {
                string token = await DBOper.SysUser.SetToken(user.id);
                user.access_id = user.id;
                user.access_token = token;
            }
            return user;
        }
        [HttpPut]
        [Route("SysUser/UpdatePwd")]
        public async Task<string> UpdatePwd(SysUserModel user)
        {
            if (!await DBOper.SysUser.GetToken(user.access_id, user.access_token)) return "请重新登录";
            return await DBOper.SysUser.UpdatePwd(user.id, user.login_pwd, user.login_name);
        }
        [HttpPut]
        [Route("SysUser/ResetPwd")]
        public async Task<string> ResetPwd(SysUserModel user)
        {
            if (!await DBOper.SysUser.GetToken(user.access_id, user.access_token)) return "请重新登录";
            if (user.access_id != 1) return "权限不足";
            return await DBOper.SysUser.ResetPwd(user.id, user.login_pwd);
        }
        [HttpGet]
        [Route("SysUser/GetList")]
        public async Task<IEnumerable<SysUserModel>> GetList(int access_id, string access_token)
        {
            if (!await DBOper.SysUser.GetToken(access_id, access_token)) return new List<SysUserModel>();
            return await DBOper.SysUser.GetList();
        }
    }
}
