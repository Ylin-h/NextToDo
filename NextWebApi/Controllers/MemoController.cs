using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NextWebApi.Models;
using NextWebApi.Utils;

namespace NextWebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    /// <summary>
    /// 备忘录
    /// </summary>
    public class MemoController : ControllerBase
    {
        private readonly NextToDoDbContext db;
        public MemoController(NextToDoDbContext _db)
        {
            db = _db;
        }
        /// <summary>
        /// 获取备忘录统计数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            Result result = new Result();
            try
            {
                var list = db.Memo.ToList();
                result.Code = 1;
                result.Data = list.Count;
                result.Msg = "获取备忘录统计数据成功";
            }
            catch (Exception ex)
            {
                result.Code = 0;
                result.Msg = "获取备忘录统计数据失败";
            }
            return Ok(result);
        }
        /// <summary>
        /// 获取备忘录列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetMemoList(string? Title)
        {
            Result result = new Result();
            try
            {
                var list = new List<Memo>();
                if(Title!= null)
                {
                    list = db.Memo.Where(m => m.Title.Contains(Title)).ToList();
                }
                else
                list = db.Memo.ToList();
                result.Code = 1;
                result.Data = list;
                result.Msg = "获取备忘录列表成功";
            }
            catch (Exception ex)
            {
                result.Code = -1;
                result.Msg = "获取备忘录列表失败";
            }
            return Ok(result);
        }
        /// <summary>
        /// 添加备忘录
        /// </summary>
        /// <param name="memo"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Add(Memo memo)
        {
            Result result = new Result();
            try
            {
                db.Memo.Add(memo);
                db.SaveChanges();
                result.Code = 1;
                result.Msg = "添加备忘录成功";
            }
            catch (Exception ex)
            {
                result.Code = -1;
                result.Msg = "添加备忘录失败";
            }
            return Ok(result);
        }
        /// <summary>
        /// 删除备忘录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            Result result = new Result();
            try
            {
                var memo = db.Memo.Find(id);
                if (memo == null)
                {
                    result.Code = -1;
                    result.Msg = "备忘录不存在";
                    return Ok(result);
                }
                db.Memo.Remove(memo);
                db.SaveChanges();
                result.Code = 1;
                result.Msg = "删除备忘录成功";
            }
            catch (Exception ex)
            {
                result.Code = -1;
                result.Msg = "删除备忘录失败";
            }
            return Ok(result);
        }
        /// <summary>
        /// 更新备忘录
        /// </summary>  
        /// <param name="memo"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Update(Memo memo)
        {
            Result result = new Result();
            try
            {
                var oldMemo = db.Memo.Find(memo.Id);
                if (oldMemo == null)
                {
                    result.Code = -1;
                    result.Msg = "备忘录不存在";
                    return Ok(result);
                }
                oldMemo.Title = memo.Title;
                oldMemo.Content = memo.Content; 
                db.SaveChanges();
                result.Code = 1;
                result.Msg = "更新备忘录成功";
            }
            catch (Exception ex)
            {
                result.Code = -1;
                result.Msg = "更新备忘录失败";
            }
            return Ok(result);
        }
    }
}
