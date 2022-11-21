using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SharedTrip.Core.Contracts;
using SharedTrip.Core.Models.Comments;

namespace SharedTrip.Controllers
{
    public class CommentController : BaseController
    {
        private readonly ICommentService commentService;

        public CommentController(ICommentService commentService)
        {
            this.commentService = commentService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] AddCommentViewModel model)
        {
            var isCreated = false;

            if (!ModelState.IsValid)
            {
                return Json(isCreated);
            }

            isCreated = await this.commentService.CreateComment(model);

            return Json(isCreated);
        }

        [HttpGet]
        public async Task<IActionResult> GetComments([FromQuery] AllCommentsQueryModel query)
        {
            var queryResult = await this.commentService.GetAllByCommentsByUserIdAsync(query.ReceiverId, query.CurrentPage, AllCommentsQueryModel.CommentsPerPage);

            query.Comments = queryResult.Comments;
            query.TotalCommentsCount = queryResult.TotalCommentsCount;

            return Json(query);
        }
    }
}