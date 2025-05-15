using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NextWebApi.DTOs;
using NextWebApi.Models;
using NextWebApi.Utils;

namespace NextWebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    ///
    ///<summary>
    ///账户控制器
    ///</summary>
    public class AccountController : ControllerBase
    {
        private readonly NextToDoDbContext _context;
        public readonly IMapper _mapper;

        public AccountController(NextToDoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
       /// <summary>
       /// 注册
       /// </summary>
       /// <param name="account"></param>
       /// <returns></returns>
        [HttpPost]
        public IActionResult Register(AccountDTO account)
        {
            Result result = new Result();
            var accountExists = _context.Account.Any(a => a.AccountName == account.AccountName);
            if (accountExists == true)
            {
                result.Code = -1;
                result.Msg = "Account already exists";
            }

            else
            {
                var Info=_mapper.Map<Account>(account);
                _context.Account.Add(Info);
                var res = _context.SaveChanges();
                if(res > 0)
                {
                    result.Code = 1;
                    result.Msg = "Account created successfully";
                }
            }
            return Ok(result);
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Login(LoginInfoDTO account)
        {
            Result result = new Result();
            var accountExists = _context.Account.Any(a => a.NickName == account.NickName && a.Password == account.Password);
            if (accountExists == true)
            {
                result.Code = 1;
                result.Msg = "Login successful";
            }
            else
            {
                result.Code = -1;
                result.Msg = "Invalid username or password";
            }
            return Ok(result);
        }
    }
}
