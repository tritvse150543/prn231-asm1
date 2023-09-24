using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using Newtonsoft.Json;
using eStoreAPI.DTOs.Order;
using eStoreAPI.DTOs.Member;
using eStoreAPI.DTOs.Product;
using Microsoft.AspNetCore.Authorization;

namespace eStoreClient.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {

        private readonly HttpClient client;
        private readonly string ORDER_ENDPOINT = "api/orders";
        public OrdersController(IConfiguration configuration)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(configuration["BaseUrl"]);
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var response = await client.GetAsync(ORDER_ENDPOINT);
            var dto = JsonConvert.DeserializeObject<List<OrderResponseDTO>>(await response.Content.ReadAsStringAsync());
            return View(dto);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var response = await client.GetAsync( ORDER_ENDPOINT + $"/details/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }
            var dto = JsonConvert.DeserializeObject<OrderResponseDTO>(await response.Content.ReadAsStringAsync());
            return View(dto);
        }

        public async Task<List<MemberResponseDTO>> GetMembers()
        {
            var response = await client.GetAsync( "api/members");
            var dto = JsonConvert.DeserializeObject<List<MemberResponseDTO>>(await response.Content.ReadAsStringAsync());
            return dto;
        }

        // GET: Orders/Create
        public async Task<IActionResult> Create()
        {
            ViewData["MemberId"] = new SelectList(await GetMembers(), "MemberId", "Email");
            var response = await client.GetAsync("api/products");
            var products = JsonConvert.DeserializeObject<List<ProductResponseDTO>>(await response.Content.ReadAsStringAsync());
            ViewData["Products"] = products;
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MemberId,OrderDate,RequiredDate,ShippedDate,Freight")] OrderCreateRequestDTO request)
        {
            var formData = HttpContext.Request.Form;
            
            foreach(var data in formData)
            {
                if (data.Key.Contains("product_"))
                {
                    int id = int.Parse(data.Key.Substring(8));
                    int quantity = int.Parse(data.Value);
                    if (quantity > 0) request.ProductIds.Add(new BuyProductRequest { Id = id, Quantity = quantity });
                }
            }
            var response = await client.PostAsJsonAsync(ORDER_ENDPOINT, request);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            ViewData["MemberId"] = new SelectList(await GetMembers(), "MemberId", "Email", request.MemberId);
            return View(request);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var response = await client.GetAsync( ORDER_ENDPOINT + $"/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }
            var dto = JsonConvert.DeserializeObject<OrderUpdateRequestDTO>(await response.Content.ReadAsStringAsync());
            var order = new OrderUpdateRequestDTO()
            {
                OrderId = dto.OrderId,
                OrderDate = dto.OrderDate,
                Freight = dto.Freight,
                MemberId = dto.MemberId,
                RequiredDate   = dto.RequiredDate,
                ShippedDate = dto.ShippedDate
            }; 
            ViewData["MemberId"] = new SelectList(await GetMembers(), "MemberId", "Email", dto.MemberId);


            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("OrderId,MemberId,OrderDate,RequiredDate,ShippedDate,Freight")] OrderUpdateRequestDTO order)
        {
            if (ModelState.IsValid)
            {
                var response = await client.PutAsJsonAsync( ORDER_ENDPOINT + $"/{order.OrderId}", order);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["MemberId"] = new SelectList(await GetMembers(), "MemberId", "Email", order.MemberId) ;
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {

            var response = await client.GetAsync( ORDER_ENDPOINT + $"/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }
            var dto = JsonConvert.DeserializeObject<OrderResponseDTO>(await response.Content.ReadAsStringAsync());
            return View(dto);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await client.DeleteAsync( ORDER_ENDPOINT + $"/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Report(DateTime? startDate, DateTime? endDate)
        {
            if (startDate == null || endDate == null)
            {
                return View(new List<OrderReportDTO>());
            }
            var response = await client.GetAsync(ORDER_ENDPOINT + $"/report?startDate={startDate}&endDate={endDate}");
            var dto = JsonConvert.DeserializeObject<List<OrderReportDTO>>(await response.Content.ReadAsStringAsync());
            return View(dto);
        }
    }
}