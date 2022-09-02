using Blog.Data.Repository;
using Blog.Data.UnitOfWork;
using Blog.Models.DB;
using Microsoft.AspNetCore.Mvc;

namespace Blog.ViewComponents
{
    public class SearchTagsViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public SearchTagsViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync(string search)
        {
            var repository = _unitOfWork.GetRepository<Tag>() as TagRepository;
            IEnumerable<Tag> tags = await repository.GetAllTags();

            var tagList = tags.Where(x => (x.Title.ToLower().Contains(search.ToLower()) ||
                                          (x.Description != null && x.Description.ToLower().Contains(search.ToLower()))));

            return View(tagList);
        }
    }
}