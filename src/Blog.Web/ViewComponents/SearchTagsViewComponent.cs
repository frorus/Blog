using Blog.Core.Interfaces;
using Blog.Core.Models;
using Blog.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.ViewComponents
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

            var tagList = tags.Where(x => x.Title.ToLower().Contains(search.ToLower()) ||
                                          x.Description != null && x.Description.ToLower().Contains(search.ToLower()));

            return View(tagList);
        }
    }
}