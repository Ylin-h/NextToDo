using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NextWebApi.DTOs;
using NextWebApi.Models;
using NextWebApi.Utils;

namespace NextWebApi.Controllers
{
    /// <summary>
    /// 待办事项控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        /// <summary>
        /// 待办事项数据库上下文
        /// </summary>
        private readonly NextToDoDbContext db;
        public ToDoController(NextToDoDbContext _dbContext)
        {
            db=_dbContext;
        }
        /// <summary>
        /// 获取待办事项中未完成的事项
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetWaitingToDo()
        {
            Result result = new Result();
            try
            {
                List<ToDo> list = db.ToDo.Where(x => x.Status == false).ToList(); 
                result.Code = 1;
                result.Msg = "获取成功";
                result.Data = list;
            }
            catch (Exception ex)
            {
                result.Code = -1;
                result.Msg = "获取失败";
            }
            
            return Ok(result);
        }
        /// <summary>
        /// 获取所有待办事项
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllToDo(string? Title, int? Status)
        {
            Result result = new Result();
            try {
                var list=new List<ToDo>();
                list = db.ToDo.ToList();
                if (!string.IsNullOrEmpty(Title))
                {
                    list = list.Where(x => x.Title.Contains(Title)).ToList();
                }
                if (Status.HasValue)
                {
                    if (Status == 1)
                    {
                        list = list.Where(x => x.Status == false).ToList();
                    }
                    else if (Status == 2)
                    list = list.Where(x => x.Status == true).ToList();
                }
  
                result.Code = 1;
                result.Msg = "获取成功";
                result.Data = list;
                
            }
            catch (Exception ex)
            {
                result.Code = -1;
                result.Msg = "获取失败";
            }       
                return Ok(result); 
        }
        /// <summary>
        /// 获取待办事项统计信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetToDo()
        {
            Result result = new Result();
           
            try
            {
               var totallist = db.ToDo.ToList();
                var finishedList = totallist.Where(x => x.Status == true).Count();
                ToDoDTO dto = new ToDoDTO();
                dto.Total = totallist.Count;
                dto.Completed = finishedList;
                result.Code = 1;
                result.Msg = "获取成功";
                result.Data = dto;

            }
            catch (Exception ex)
            {
                result.Code = -1;
                result.Msg = "获取失败";
            }
            return Ok(result);
        }
        /// <summary>
        /// 添加待办事项
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// 

        [HttpPost]
        public IActionResult AddToDo(ToDo dto)
        {
            Result result = new Result();
            try
            {
                ToDo todo = new ToDo();
                todo.Title = dto.Title;
                todo.Content = dto.Content;
                todo.Status = dto.Status;
                todo.CreatedDate = DateTime.Now;
                db.ToDo.Add(todo);
                db.SaveChanges();
                result.Code = 1;
                result.Msg = "添加成功";
            }
            catch (Exception ex)
            {
                result.Code = -1;
                result.Msg = "添加失败";
            }
            return Ok(result);
        }
        /// <summary>
        /// 更新待办事项
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult UpdateToDo(ToDo dto)
        {
            Result result = new Result();
            try
            {
                ToDo todo = db.ToDo.Find(dto.Id);
                if (todo == null)
                {
                    result.Code = -1;
                    result.Msg = "待办事项不存在";
                    return Ok(result);
                }
                todo.Title = dto.Title;
                todo.Content = dto.Content;
                todo.Status = dto.Status;
                db.SaveChanges();
                result.Code = 1;
                result.Msg = "更新成功";
            }
            catch (Exception ex)
            {
                result.Code = -1;
                result.Msg = "更新失败";
            }
            return Ok(result);
        }
        /// <summary>
        /// 删除待办事项
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult DeleteToDo(int id)
        {
            Result result = new Result();
            try
            {
                ToDo todo = db.ToDo.Find(id);
                if (todo == null)
                {
                    result.Code = -1;
                    result.Msg = "待办事项不存在";
                    return Ok(result);
                }
                db.ToDo.Remove(todo);
                db.SaveChanges();
                result.Code = 1;
                result.Msg = "删除成功";
            }
            catch (Exception ex)
            {
                result.Code = -1;
                result.Msg = "删除失败";
            }
            return Ok(result);
        }
      }

}
