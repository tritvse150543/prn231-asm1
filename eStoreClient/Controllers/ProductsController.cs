using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using System.Security.Policy;
using Newtonsoft.Json;
using eStoreAPI.DTOs.Product;
using Microsoft.AspNetCore.Authorization;

namespace eStoreClient.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly HttpClient client;
        private readonly string PRODUCT_ENDPOINT = "api/products";
        public ProductsController(IConfiguration configuration)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(configuration["BaseUrl"]);
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var response = await client.GetAsync( PRODUCT_ENDPOINT);
            var dtos = JsonConvert.DeserializeObject<List<ProductResponseDTO>>(await response.Content.ReadAsStringAsync());
            return View(dtos);
        }

        public async Task<IActionResult> Search([FromQuery] decimal? UnitPrice, [FromQuery] string? ProductName)
        {
            var url =  PRODUCT_ENDPOINT + "/search";
            var queryParameters = new List<string>();
            if (UnitPrice.HasValue)
                queryParameters.Add($"UnitPrice={UnitPrice}");
            if (!string.IsNullOrEmpty(ProductName))
                queryParameters.Add($"ProductName={Uri.EscapeDataString(ProductName)}");

            if (queryParameters.Count > 0)
            {
                var queryString = string.Join("&", queryParameters);
                url = $"{url}?{queryString}";
            }
            var response = await client.GetAsync(url);
            var dtos = JsonConvert.DeserializeObject<List<ProductResponseDTO>>(await response.Content.ReadAsStringAsync());
            ViewBag.UnitPrice = UnitPrice;
            ViewBag.ProductName = ProductName;
            return View("Index",dtos);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var response = await client.GetAsync( PRODUCT_ENDPOINT +  $"/{id}");
            var product = JsonConvert.DeserializeObject<ProductResponseDTO>(await response.Content.ReadAsStringAsync());
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        
        private async Task<List<Category>> GetCategories()
        {
            var response = await client.GetAsync( "api/categories");
            return JsonConvert.DeserializeObject<List<Category>>(await response.Content.ReadAsStringAsync());           
        }
        // GET: Products/Create
        public async Task<IActionResult> Create()
        {
            ViewData["CategoryId"] = new SelectList(await GetCategories(), "CategoryId", "CategoryName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,CategoryId,ProductName,Weight,UnitPrice,UnitsInStock")] ProductCreateRequestDTO request)
        {
            if (ModelState.IsValid)
            {
                var response = await client.PostAsJsonAsync( PRODUCT_ENDPOINT, request);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(await GetCategories(), "CategoryId", "CategoryName", request.CategoryId);
            return View(request);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var response = await client.GetAsync( PRODUCT_ENDPOINT + $"/{id}");
            var product = JsonConvert.DeserializeObject<ProductResponseDTO>(await response.Content.ReadAsStringAsync());

            ViewData["CategoryId"] = new SelectList(await GetCategories(), "CategoryId", "CategoryName", product.CategoryId);
            ProductUpdateRequestDTO productUpdateRequestDTO = new ProductUpdateRequestDTO()
            {
                id = product.ProductId,
                CategoryId = product.CategoryId,
                ProductName = product.ProductName,
                UnitPrice = product.UnitPrice,
                UnitsInStock = product.UnitsInStock,
                Weight = product.Weight,
            };
            ViewBag.id = id;
            return View(productUpdateRequestDTO);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( [Bind("id,CategoryId,ProductName,Weight,UnitPrice,UnitsInStock")] ProductUpdateRequestDTO request)
        {
            if (ModelState.IsValid)
            {
                var response = await client.PutAsJsonAsync( PRODUCT_ENDPOINT + $"/{request.id}", request);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }     
                
            }
            ViewData  ["CategoryId"] = new SelectList(await GetCategories(), "CategoryId", "CategoryName", request.CategoryId);
            return View(request);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {

            var response = await client.GetAsync( PRODUCT_ENDPOINT + $"/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var product = JsonConvert.DeserializeObject<ProductResponseDTO>(await response.Content.ReadAsStringAsync());
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await client.DeleteAsync( PRODUCT_ENDPOINT + $"/{id}");
            return RedirectToAction(nameof(Index));
        }

    }
}