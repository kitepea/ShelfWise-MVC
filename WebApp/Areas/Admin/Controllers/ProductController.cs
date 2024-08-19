﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShelfWise.DataAccess.Repository.IRepository;
using ShelfWise.Models;
using ShelfWise.Models.ViewModels;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Product> objProductsList = _unitOfWork.Product.GetAll().ToList();
            return View(objProductsList);
        }

        public IActionResult Upsert(int? id)
        {
            ProductViewModel viewModel = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                Product = new Product()
            };
            if (id == null || id == 0)
            {
                // Create
                return View(viewModel);
            }
            else
            {
                // Update
                viewModel.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(viewModel);
            }
        }
        [HttpPost]
        public IActionResult Upsert(ProductViewModel productViewModel, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath; // Give wwwroot path
                if (imageFile != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var uploads = Path.Combine(wwwRootPath, @"images\product");

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                    {
                        imageFile.CopyTo(fileStreams);
                    }
                    productViewModel.Product.ImageUrl = @"\images\products\" + fileName;
                }
                else
                {
                    if (productViewModel.Product.Id != 0)
                    {
                        Product objFromDb = _unitOfWork.Product.Get(u => u.Id == productViewModel.Product.Id);
                        productViewModel.Product.ImageUrl = objFromDb.ImageUrl;
                    }
                }
                _unitOfWork.Product.Add(productViewModel.Product);
                _unitOfWork.Save();
                TempData["success"] = "Product added successfully.";
                return RedirectToAction("Index", "Product");
            }
            else
            {
                productViewModel.CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                return View(productViewModel);
            }
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product? obj = _unitOfWork.Product.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product? product = _unitOfWork.Product.Get(u => u.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            _unitOfWork.Product.Remove(product);
            _unitOfWork.Save();
            TempData["success"] = "Product deleted successfully.";
            return RedirectToAction("Index", "Product");
        }
    }
}
