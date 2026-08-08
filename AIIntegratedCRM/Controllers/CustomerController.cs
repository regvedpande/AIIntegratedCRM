using AIIntegratedCRM.Models.Entities;
using AIIntegratedCRM.Models.ViewModels;
using AIIntegratedCRM.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AIIntegratedCRM.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IAIService _aiService;

        public CustomerController(ICustomerService customerService, IAIService aiService)
        {
            _customerService = customerService;
            _aiService = aiService;
        }

        // GET: /Customer/Index
        public async Task<IActionResult> Index(string? searchTerm, string sortBy = CustomerSortOptions.Name)
        {
            var allCustomers = await _customerService.GetAllCustomersAsync();
            var normalizedSearchTerm = searchTerm?.Trim();

            var filteredCustomers = allCustomers;
            if (!string.IsNullOrWhiteSpace(normalizedSearchTerm))
            {
                filteredCustomers = filteredCustomers.Where(customer =>
                    Contains(customer.FullName, normalizedSearchTerm) ||
                    Contains(customer.Email, normalizedSearchTerm) ||
                    Contains(customer.Company, normalizedSearchTerm) ||
                    Contains(customer.Phone, normalizedSearchTerm));
            }

            filteredCustomers = sortBy switch
            {
                CustomerSortOptions.Company => filteredCustomers
                    .OrderBy(customer => customer.Company)
                    .ThenBy(customer => customer.FullName),
                CustomerSortOptions.Newest => filteredCustomers
                    .OrderByDescending(customer => customer.CreatedAt)
                    .ThenBy(customer => customer.FullName),
                _ => filteredCustomers.OrderBy(customer => customer.FullName)
            };

            var visibleCustomers = filteredCustomers.ToList();
            var model = new CustomerIndexViewModel
            {
                Customers = visibleCustomers,
                SearchTerm = normalizedSearchTerm,
                SortBy = sortBy,
                TotalCustomers = allCustomers.Count(),
                VisibleCustomers = visibleCustomers.Count
            };

            return View(model);
        }

        // GET: /Customer/Create
        public IActionResult Create()
        {
            return View(new Customer());
        }

        // POST: /Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _customerService.CreateCustomerAsync(model);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Customer/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            var existing = await _customerService.GetCustomerByIdAsync(id);
            if (existing == null)
                return NotFound();

            return View(existing);
        }

        // POST: /Customer/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Customer model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var exists = await _customerService.CustomerExistsAsync(model.Id);
            if (!exists)
                return NotFound();

            await _customerService.UpdateCustomerAsync(model);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Customer/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
                return NotFound();

            return View(customer);
        }

        // GET: /Customer/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
                return NotFound();

            return View(customer);
        }

        // POST: /Customer/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _customerService.DeleteCustomerAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Customer/GenerateSummary/{id}
        [HttpGet]
        public async Task<IActionResult> GenerateSummary(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
                return NotFound();

            string summary = await _aiService.GenerateCustomerSummaryAsync(customer);
            return Json(new { summary });
        }

        private static bool Contains(string? value, string searchTerm)
        {
            return value?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true;
        }
    }
}
