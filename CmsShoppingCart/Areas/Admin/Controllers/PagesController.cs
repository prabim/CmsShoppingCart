using CmsShoppingCart.Models.Data;
using CmsShoppingCart.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CmsShoppingCart.Areas.Admin.Controllers
{
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {
            List<PageVM> pageList;

            using(Db db = new Db())
            {
                pageList = db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PageVM(x)).ToList();
            }

            return View(pageList);
        }

        [HttpGet]
        public ActionResult AddPage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddPage(PageVM model)
        {
            //Check model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            using(Db db = new Db())
            {
                //Declare slug
                string slug;

                //Init pageDTO
                PageDTO dto = new PageDTO();

                //DTO Title
                dto.Title = model.Title;

                //Check for and set slug if needed
                if (string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    slug = model.Slug.Replace(" ", "-").ToLower();
                }

                //Make sure title and slug is unique
                if(db.Pages.Any(x => x.Title == model.Title) || db.Pages.Any(x => x.Slug == model.Slug))
                {
                    ModelState.AddModelError("", "The Title or slug already exists.");
                }

                //Dto the rest
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;
                dto.Sorting = model.Sorting;


                //Save DTO
                db.Pages.Add(dto);
                db.SaveChanges(); 
            }

            //Set TempData message
            TempData["SM"] = "Added a new page.";

            //Redirect
            return RedirectToAction("AddPage");
        }

        [HttpGet]
        public ActionResult EditPage(int id)
        {
            //Declare PageVM
            PageVM model;
            
            using(Db db = new Db())
            {
                //Get the page
                PageDTO dto = db.Pages.Find(id);

                //Confirm page exists
                if(dto == null)
                {
                    return Content("The page doesn't exists.");
                }

                //Init PageVM
                model = new PageVM(dto);
            }

            //Return view with model
            return View(model);
        }

        [HttpPost]
        public ActionResult EditPage(PageVM model)
        {
            //CHeck model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db = new Db())
            {
                //Get Page Id
                int id = model.Id;

                //Declare slug
                string slug = "home";

                //Get the page
                PageDTO dto = db.Pages.Find(id);

                //Dto the title
                dto.Title = model.Title;

                //Check for the slug and set it if needed
                if(model.Slug != "home")
                {
                    if (string.IsNullOrWhiteSpace(model.Slug))
                    {
                        slug = model.Title.Replace(" ", "-").ToLower();
                    }
                    else
                    {
                        slug = model.Slug.Replace(" ", "-").ToLower();
                    }
                }

                //Make sure title and slug are unique
                if(db.Pages.Where(x => x.Id != id).Any(x => x.Title == model.Title) 
                    || db.Pages.Where(x => x.Id != id).Any(x => x.Slug == model.Slug))
                {
                    ModelState.AddModelError("", "The title or slug already exists.");
                    return View(model);
                }

                //Dto the rest
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;

                //save the dto
                db.SaveChanges();
            }

            //set tempdata message
            TempData["SM"] = "Page has been successfully updated.";

            //redirect
            return RedirectToAction("EditPage");
        }

        public ActionResult PageDetails(int id)
        {
            //Declare PageVM
            PageVM model;

            
            using(Db db = new Db())
            {
                //Get the page
                PageDTO dto = db.Pages.Find(id);

                //Confirm page exits
                if(dto == null)
                {
                    return Content("Page doesn't exitsts.");
                }

                //Init PageVM
                model = new PageVM(dto);
            }

            return View(model);
        }

        public ActionResult DeletePage(int id)
        {
            using(Db db = new Db())
            {
                //Get the page
                PageDTO dto = db.Pages.Find(id);

                //Remove the page
                db.Pages.Remove(dto);

                //Save
                db.SaveChanges();
            }

            //Redirect
            return RedirectToAction("Index");
        }

        [HttpPost]
        public void ReorderPages(int[] id)
        {
            using (Db db = new Db())
            {
                //set intitial count
                int count = 1;

                //declare pageDto
                PageDTO dto;

                //set sorting for each page
                foreach(var pageId in id)
                {
                    dto = db.Pages.Find(pageId);
                    dto.Sorting = count;
                    db.SaveChanges();
                    count++;

                }
            }
        }

        public ActionResult EditSidebar()
        {
            //Declare model
            SidebarVM model;

            using (Db db = new Db())
            {
                //Get the dto
                SidebarDTO dto = db.Sidebar.Find(1);

                //init model
                model = new SidebarVM(dto);
            }
            //Return view with model
            return View(model);
        }

        [HttpPost]
        public ActionResult EditSidebar(SidebarVM model)
        {
            using(Db db = new Db())
            {
                //Get the dto
                SidebarDTO dto = db.Sidebar.Find(1);

                //DTO the body
                dto.Body = model.Body;

                //save
                db.SaveChanges();
            }
            //set tempdata message
            TempData["SM"] = "Sidebar updated successfully";

            //Redirect
            return RedirectToAction("EditSidebar");
        }
    }
}