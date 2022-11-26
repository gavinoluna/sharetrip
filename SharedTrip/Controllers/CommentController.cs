using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SharedTrip.Core.Contracts;
using SharedTrip.Core.Models.Comments;

namespace SharedTrip.Controllers
{
    public class CommentController : BaseController
    {
        private readonly ICommentService commentService;
        private readonly INotyfService notyfService;

        public CommentController(
            ICommentService commentService,
            INotyfService notyfService)
        {
            this.commentService = commentService;
            this.notyfService = notyfService;
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
    }
}