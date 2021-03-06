﻿using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using MyShop.DAL.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        public InMemoryRepository<Product> context;
        public InMemoryRepository<ProductCategory> contextCategoryRepository;

        public ProductManagerController()
        {
            context = new InMemoryRepository<Product>();
            contextCategoryRepository = new InMemoryRepository<ProductCategory>();
        }
        // GET: ProductManager
        public ActionResult Index()
        {
            List<Product> products = context.Collection().ToList();
            return View(products);
        }

        public ActionResult Create()
        {
            ProductManagerViewModel productViewModel = new ProductManagerViewModel();
            productViewModel.Product = new Product();
            productViewModel.ProductCategories = contextCategoryRepository.Collection();
            return View(productViewModel);
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            context.Insert(product);
            context.Commit();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(string Id)
        {
            Product product = context.Find(Id);
            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                ProductManagerViewModel productViewModel = new ProductManagerViewModel();
                productViewModel.Product = product;
                productViewModel.ProductCategories = contextCategoryRepository.Collection();
                return View(productViewModel);
            }
        }

        [HttpPost]
        public ActionResult Edit(Product product, string Id)
        {
            Product productToUpdate = context.Find(Id);
            if(productToUpdate == null)
            {
                return HttpNotFound();
            }
            else
            {
                if(!ModelState.IsValid)
                {
                    return View(product);
                }
                productToUpdate.Name = product.Name;
                productToUpdate.Description = product.Description;
                productToUpdate.Price = product.Price;
                productToUpdate.Category = product.Category;
                productToUpdate.Image = product.Image;
                //context.Update(product);
                context.Commit();
                return RedirectToAction("Index");
            }
        }

        public ActionResult Delete(string Id)
        {
            Product product = context.Find(Id);
            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(product);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            Product productToDelete = context.Find(Id);
            if (productToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(productToDelete);
                }
                context.Delete(Id);
                context.Commit();
                return RedirectToAction("Index");
            }
        }
    }
}