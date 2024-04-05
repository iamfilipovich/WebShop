using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebShop.Repositories.Abstract;
using WebShop.Repositories.Implement;

namespace WebShop.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public IActionResult Index()
        {
            var kategorije = _categoryService.GetAll();
            return View(kategorije);
        }
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Category model)
        {
            if (!ModelState.IsValid) 
            {
                return View(model);
            }
            var result = _categoryService.Add(model);
            if (result==null)
            {
                TempData["msg"] = "Error";
                return View(model);
            }
            TempData["msg"] = "Successfull";
            return RedirectToAction("Index");
        }

        public IActionResult Update(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var productFromDb = _categoryService.FindById(id);
            if (productFromDb == null)
            {
                return NotFound(); // Možete dodati dodatnu logiku za rukovanje ako proizvod nije pronađen
            }

            var model = new Category
            {
                CategoryID = productFromDb.CategoryID,
                Name = productFromDb.Name
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Update(Category model)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(model);
            //}
            var result = _categoryService.Update(model);
            if (result)
            {
                TempData["msg"] = "Successfull";
                return RedirectToAction("Index");
            }
            TempData["msg"] = "Error";
            return View(model);
        }
        public IActionResult Delete(int id)
        {
            var result = _categoryService.Delete(id);
            return RedirectToAction("Index");
        }

        public IActionResult GetAll()
        {
            var data = _categoryService.GetAll();
            return View(data);
        }
    }
}
