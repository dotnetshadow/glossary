using Glossary.Web.Models;
using Glossary.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Glossary.Web.Pages.Glossary
{
    public class EditModel : PageModel
    {
        private readonly ILogger<EditModel> _logger;
        private readonly GlossaryService _glossaryService;

        public EditModel(ILogger<EditModel> logger, GlossaryService glossaryService)
        {
            _logger = logger;
            _glossaryService = glossaryService;
        }

        [BindProperty]
        public GlossaryItem GlossaryItem { get; set; } = new();
        
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var glossaryItem = await _glossaryService.GetGlossaryItemById(id);

            if (glossaryItem != null)
            {
                GlossaryItem = glossaryItem;
            }
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            GlossaryItem.Term = GlossaryItem.Term?.Trim();
            GlossaryItem.Definition = GlossaryItem.Definition?.Trim();
            
            var result = await _glossaryService.EditGlossaryItem(GlossaryItem);

            if(result.IsSuccess)
            {
                TempData["Glossary:Success"] = "The glossary item was successfully updated";
                return RedirectToPage("Index");
            }
            
            TempData["Glossary:Error"] = result.Error;
            return Page();
            
        }
    }
}