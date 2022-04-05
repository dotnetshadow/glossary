using Glossary.Web.Models;
using Glossary.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Glossary.Web.Pages.Glossary
{
    public class CreateModel : PageModel
    {
        private readonly ILogger<CreateModel> _logger;
        private readonly GlossaryService _glossaryService;

        public CreateModel(ILogger<CreateModel> logger, GlossaryService glossaryService)
        {
            _logger = logger;
            _glossaryService = glossaryService;
        }

        [BindProperty]
        public GlossaryItem GlossaryItem { get; set; } = new();
        
        public IActionResult OnGet()
        {
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

            var result = await _glossaryService.CreateGlossaryItem(GlossaryItem);

            if(result.IsSuccess)
            {
                TempData["Glossary:Success"] = "The glossary item was successfully created";
                return RedirectToPage("Index");
            }
            
            TempData["Glossary:Error"] = result.Error;
            return Page();
            
        }
    }
}