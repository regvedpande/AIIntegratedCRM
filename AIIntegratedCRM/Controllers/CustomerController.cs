using AIIntegratedCRM.Models.Entities;
using AIIntegratedCRM.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AIIntegratedCRM.Controllers
{
    public class CustomerController : Controller
    {
        // 1. Declare the fields for injection
        private readonly ICustomerService _customerService;
        private readonly IAIService _aiService;

        // 2. Constructor: receive those services via DI
        public CustomerController(ICustomerService customerService, IAIService aiService)
        {
            _customerService = customerService;
            _aiService = aiService;
        }

        // ========================
        // Standard CRUD Actions
        // ========================

        // GET: /Customer/Index
        public async Task<IActionResult> Index()
        {
            var allCustomers = await _customerService.GetAllCustomersAsync();
            return View(allCustomers);
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

            // Preserve CreatedAt inside model (it was posted as a hidden field)
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

        // ========================
        // AI / Gemini Integration
        // ========================

        // GET: /Customer/GenerateSummary/{id}
        [HttpGet]
        public async Task<IActionResult> GenerateSummary(int id)
        {
            // 1. Fetch the Customer by id (using the injected service)
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
                return NotFound();

            // 2. Call the AI service to generate a summary
            string summary = await _aiService.GenerateCustomerSummaryAsync(customer);

            // 3. Return JSON containing that summary
            return Json(new { summary });
        }
    }
}
