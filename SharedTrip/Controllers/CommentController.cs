using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateComment([FromBody] AddCommentViewModel model)
        {
            var isCreated = false;

            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(isCreated);
                }

                isCreated = await this.commentService.CreateComment(model);

                return Json(isCreated);
            }
            catch (Exception)
            {
                return Json(isCreated);
            }
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetComments([FromQuery] AllCommentsQueryModel query)
        {
            try
            {
                var queryResult = await this.commentService.GetAllByCommentsByUserIdAsync(query.ReceiverId, query.CurrentPage, AllCommentsQueryModel.CommentsPerPage);

                query.Comments = queryResult.Comments;
                query.TotalCommentsCount = queryResult.TotalCommentsCount;

                return Json(query);
            }
            catch (Exception)
            {
                return Json(query);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromBody] int commentId)
        {
            var isDeleted = false;

            try
            {
                await this.commentService.DeleteCommentAsync(commentId);
                isDeleted = true;
            }
            catch (Exception)
            {
                isDeleted = false;
            }

            return Json(isDeleted);
        }
    }
}