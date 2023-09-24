using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using Newtonsoft.Json;
using eStoreAPI.DTOs.Member;
using Microsoft.AspNetCore.Authorization;

namespace eStoreClient.Controllers
{
    [Authorize]
    public class MembersController : Controller
    {
        private readonly HttpClient client;
        private readonly string MEMBER_ENDPOINT = "api/members";
        public MembersController(IConfiguration configuration)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(configuration["BaseUrl"]);
        }
        // GET: Members
        public async Task<IActionResult> Index()
        {
              var response = await client.GetAsync( MEMBER_ENDPOINT);
              var dtos = JsonConvert.DeserializeObject<List<MemberResponseDTO>>(await response.Content.ReadAsStringAsync());
              return View(dtos);
        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            var response = await client.GetAsync( MEMBER_ENDPOINT + $"/{id}");
            var member =  JsonConvert.DeserializeObject<MemberResponseDTO>(await response.Content.ReadAsStringAsync());
            return View(member);
        }

        // GET: Members/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MemberId,Email,CompanyName,City,Country,Password")] MemberCreateRequestDTO member)
        {
            if (ModelState.IsValid)
            {
                var response = await client.PostAsJsonAsync( MEMBER_ENDPOINT, member);
                if (response.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    ViewBag.msg = "Duplicated Email";
                }
            }
            return View(member);
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            var response = await client.GetAsync( MEMBER_ENDPOINT + $"/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }
            var member = JsonConvert.DeserializeObject<MemberResponseDTO>(await response.Content.ReadAsStringAsync());
            var dto = new MemberUpdateRequestDTO()
            {
                City = member.City,
                CompanyName = member.CompanyName,
                Country = member.Country,
                Password = member.Password
            };
            ViewBag.id = id;
            return View(dto);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Email,CompanyName,City,Country,Password")] MemberUpdateRequestDTO member)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var response = client.PutAsJsonAsync( MEMBER_ENDPOINT + $"/{member.id}", member);
                }
                catch (DbUpdateConcurrencyException)
                {
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var response = await client.GetAsync( MEMBER_ENDPOINT + $"/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }
            var member = JsonConvert.DeserializeObject<MemberResponseDTO>(await response.Content.ReadAsStringAsync());

            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await client.DeleteAsync( MEMBER_ENDPOINT + $"/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}
