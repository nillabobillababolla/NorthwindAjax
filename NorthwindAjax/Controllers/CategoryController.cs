using NorthwindAjax.Models;
using NorthwindAjax.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
// ReSharper disable All

namespace NorthwindAjax.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult Search(string s)
        {
            var key = s.ToLower();

            if (key.Length <= 2 && key != "*")
            {
                return Json(new ResponseData()
                {
                    message = "2 karakterden daha fazla girmelisiniz.",
                    success = false
                }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                var db = new NorthwindEntity();
                List<CategoryViewModel> data;
                data = db.Categories
                    .OrderBy(x => x.CategoryName)
                    .ThenBy(y => y.Products.Count)
                    .Select(z => new CategoryViewModel()
                    {
                        CategoryName = z.CategoryName,
                        Description = z.Description,
                        ProductCount = z.Products.Count
                    }).ToList();
                return Json(new ResponseData()
                {
                    message = $"{data.Count} adet kayıt getirildi.",
                    data = data,
                    success = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new ResponseData()
                {
                    message = $"Hata: {ex.Message}",
                    success = false
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Add(CategoryViewModel model)
        {
            try
            {
                var db = new NorthwindEntity();
                db.Categories.Add(new Category()
                {
                    CategoryName = model.CategoryName,
                    Description = model.Description
                });
                db.SaveChanges();
                return Json(new ResponseData()
                {
                    message = $"{model.CategoryName} kategorisi basariyla eklendi.",
                    success = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new ResponseData()
                {
                    message = $"Bir hata olustu {ex.Message}",
                    success = false
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                var db = new NorthwindEntity();
                var cat = db.Categories.Find(id);
                db.Categories.Remove(cat);
                db.SaveChanges();
                return Json(new ResponseData()
                {
                    message = $"{cat.CategoryName} kategorisi basariyla silindi.",
                    success = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new ResponseData()
                {
                    message = $"Kategori silme isleminde bir hata gerceklesti: {ex.Message}",
                    success = false
                }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult Detail()
        {
            throw new NotImplementedException();
        }
    }
}