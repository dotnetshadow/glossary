using Glossary.Web.Models;
using Glossary.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Glossary.Web.Pages.Glossary
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly GlossaryService _glossaryService;

        public List<GlossaryItem> Glossary { get; set; } = new List<GlossaryItem>();

        public IndexModel(ILogger<IndexModel> logger, GlossaryService glossaryService)
        {
            _logger = logger;
            _glossaryService = glossaryService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                Glossary = (await _glossaryService.GetGlossaryListAsync()).ToList();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"There is a problem accessing service {ex}");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var result = await _glossaryService.DeleteGlossaryItemById(id);

            if (!result.IsSuccess)
            {
                TempData["Glossary:Error"] = "The glossary item could not be deleted";
                return Page();
            }
            
            TempData["Glossary:Success"] = "The glossary item has been successfully deleted";
            return RedirectToPage();
        }
    }
}