using Blog.Data.Repository;
using Blog.Data.UnitOfWork;
using Blog.Models.DB;
using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    public class TagController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public TagController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Tag> tagList = await _unitOfWork.GetRepository<Tag>().GetAllAsync();
            return View(tagList);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        //Create tag
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(Tag model)
        {
            if (ModelState.IsValid)
            {
                var tag = new Tag
                {
                    Title = model.Title
                };

                await _unitOfWork.GetRepository<Tag>().Create(tag);

                //TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Failed to create");

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var tagFromDb = await _unitOfWork.GetRepository<Tag>().GetByIdAsync(id);

            if (tagFromDb == null)
            {
                return NotFound();
            }

            var tagModel = new TagViewModel
            {
                Title = tagFromDb.Title,
            };

            return View(tagModel);
        }

        //Edit tag
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(Guid id, Tag model)
        {
            var tagFromDb = await _unitOfWork.GetRepository<Tag>().GetByIdAsync(id);

            if (tagFromDb == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                tagFromDb.Title = model.Title;

                await _unitOfWork.GetRepository<Tag>().Update(tagFromDb);

                //TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var tagFromDb = await _unitOfWork.GetRepository<Tag>().GetByIdAsync(id);

            if (tagFromDb == null)
            {
                return NotFound();
            }

            return View(tagFromDb);
        }

        //Delete tag
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePOST(Guid id)
        {
            var tagFromDb = await _unitOfWork.GetRepository<Tag>().GetByIdAsync(id);

            if (tagFromDb == null)
            {
                return NotFound();
            }

            await _unitOfWork.GetRepository<Tag>().Delete(tagFromDb);

            //TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
