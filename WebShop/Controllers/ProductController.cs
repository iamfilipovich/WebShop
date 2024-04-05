using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebShop.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly WebShopDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IImageService _imageService;

        public ProductController(ICategoryService categoryService, IProductService productService, WebShopDbContext dbContext,
                                 IWebHostEnvironment webHostEnvironment, IImageService imageService)
        {
            _categoryService = categoryService;
            _productService = productService;
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
            _imageService = imageService;
        }
        public IActionResult Index()
        {
            var produkti = _productService.GetAll();
            return View(produkti);
        }

        public IActionResult Add()
        {
            var model = new Products();
            model.CategoryList = _categoryService.GetAll().Select(a => new SelectListItem { Text = a.Name, Value = a.CategoryID.ToString() }).ToList();
            return View(model);
        }

        [HttpPost]
        public IActionResult Add(Products model)
        {

            model.CategoryList = _categoryService.GetAll().Select(a => new SelectListItem { Text = a.Name, Value = a.CategoryID.ToString(), Selected = a.CategoryID == model.CategoryID }).ToList();
            if (model.ImageFile != null)
            {
                var fileReult = _imageService.SaveImage(model.ImageFile);
                if (fileReult.Item1 == 0)
                {
                    TempData["msg"] = "File could not saved";
                    return View(model);
                }
                var imageName = fileReult.Item2;
                model.Image = imageName;
            }
            var result = _productService.Add(model);
            if (result)
            {
                TempData["msg"] = "Successful";
                return RedirectToAction(nameof(Index));
            }
            TempData["msg"] = "Error";
            return View(model);
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var model = _productService.FindById(id);
            if (model == null)
            {
                return NotFound(); // Možete dodati dodatnu logiku za rukovanje ako proizvod nije pronađen
            }

            // Postavite CategoryList na popis svih kategorija
            model.CategoryList = _categoryService.GetAll()
                .Select(a => new SelectListItem { Text = a.Name, Value = a.CategoryID.ToString(), Selected = a.CategoryID == model.CategoryID })
                .ToList();

            return View(model);
        }

        [HttpPost]
        public IActionResult Update(Products model)
        {
            model.CategoryList = _categoryService.GetAll().Select(a => new SelectListItem { Text = a.Name, Value = a.CategoryID.ToString(), Selected = a.CategoryID == model.CategoryID }).ToList();
            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                var fileResult = _imageService.SaveImage(model.ImageFile);
                if (fileResult.Item1 == 0)
                {
                    TempData["msg"] = "File could not be saved";
                    return View(model);
                }
                model.Image = fileResult.Item2; // Update the image property with the new file name
            }
            //_dbContext.Entry(model).State = EntityState.Modified;
            var result = _productService.Update(model);
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
            var result = _productService.Delete(id);
            return RedirectToAction("Index");
        }
        public IActionResult GetAll()
        {
            var data = _productService.GetAll();
            return View(data);
        }
        
    }
}
