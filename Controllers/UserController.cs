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
    public class UserController : ApiController
    {
        #region 客户
        [HttpGet]
        [Route("User/Login")]
        public async Task<UserModel> Login(string mobile, string login_pwd)
        {
            UserModel user = await DBOper.User.GetOne(0, mobile, string.Empty);
            if (user != null)
            {
                if (user.login_pwd.Equals(Md5.GetMd5(login_pwd)))
                {
                    var token = Auth.Authenticate(user.id.ToString());
                    user.access_token = token;
                    user.login_pwd = string.Empty;
                    return user;
                }
            }
            return null;
        }
        [HttpPut]
        [Route("User/Register")]
        public async Task<UserModel> Register(UserModel userx)
        {
            UserModel user = await DBOper.User.Register(userx.mobile, userx.nick_name, userx.login_pwd);
            if (user != null && user.id > int.MinValue)
            {
                var token = Auth.Authenticate(user.id.ToString());
                user.access_token = token;
                user.login_pwd = string.Empty;
                return user;
            }
            return null;
        }
        [HttpPut]
        [Route("User/UpdateNickName")]
        [Authorize]
        public async Task<string> UpdateNickName(UserModel user)
        {
            string userId = WebApi.GetUserId(User);
            if (userId.Length == 0) return "获取用户失败";
            return await DBOper.User.UpdateNickName(Convert.ToInt32(userId), user.nick_name);
        }
        [HttpPut]
        [Route("User/UpdatePwdMobile")]
        public async Task<string> UpdatePwdMobile(UserModel user)
        {
            return await DBOper.User.UpdatePwdMobile(user.mobile, user.nick_name, user.login_pwd);
        }
        [HttpGet]
        [Route("User/IsReg")]
        public async Task<bool> IsReg(string mobile)
        {
            UserModel user = await DBOper.User.GetOne(0, mobile, string.Empty);
            return user != null;
        }
        #endregion
        #region 管理员
        [HttpPut]
        [Route("User/ResetPwd")]
        public async Task<string> ResetPwd(UserModel user)
        {
            if (!await DBOper.SysUser.GetToken(user.access_id, user.access_token)) return "请重新登录";
            return await DBOper.User.ResetPwd(user.id, user.login_pwd);
        }
        [HttpGet]
        [Route("User/GetList")]
        public async Task<IEnumerable<UserModel>> GetList(string id, string mobile, string nick_name,
            string page_size, string page_index, int access_id, string access_token)
        {
            if (!await DBOper.SysUser.GetToken(access_id, access_token)) return new List<UserModel>();
            return await DBOper.User.GetList(id, mobile, nick_name, page_size, page_index);
        }
        [HttpGet]
        [Route("User/GetCount")]
        public async Task<long> GetCount(string id, string mobile, string nick_name,
            int access_id, string access_token)
        {
            if (!await DBOper.SysUser.GetToken(access_id, access_token)) return 0;
            return await DBOper.User.GetCount(id, mobile, nick_name);
        }
        #endregion
    }
}
