// Pages/Index.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using Week5Project.Models;
using System.ComponentModel.DataAnnotations;

namespace Week5Project.Pages
{
    public class IndexModel : PageModel
        {
            public static List<ClassInformationModel> ClassList { get; set; } = new();
            private static int _nextId = 1;

            [BindProperty]
            public ClassInformationModel NewClass { get; set; }

            [BindProperty]
            public bool IsEditing { get; set; }

            public void OnGet()
            {
            }

            public IActionResult OnPostAdd()
            {
                if (!ModelState.IsValid)
                    return Page();

                NewClass.Id = _nextId++;
                ClassList.Add(NewClass);
                return RedirectToPage();
            }

            public IActionResult OnPostDelete(int id)
            {
                var item = ClassList.FirstOrDefault(c => c.Id == id);
                if (item != null)
                    ClassList.Remove(item);

                return RedirectToPage();
            }

            public IActionResult OnPostSelectForEdit(int id)
            {
                var classToEdit = ClassList.FirstOrDefault(c => c.Id == id);
                if (classToEdit != null)
                {
                    NewClass = new ClassInformationModel
                    {
                        Id = classToEdit.Id,
                        ClassName = classToEdit.ClassName,
                        StudentCount = classToEdit.StudentCount,
                        Description = classToEdit.Description
                    };
                    IsEditing = true;
                }

                return Page();
            }

            public IActionResult OnPostUpdate()
            {
                if (!ModelState.IsValid)
                    return Page();

                var existing = ClassList.FirstOrDefault(c => c.Id == NewClass.Id);
                if (existing != null)
                {
                    existing.ClassName = NewClass.ClassName;
                    existing.StudentCount = NewClass.StudentCount;
                    existing.Description = NewClass.Description;
                }

                return RedirectToPage();
            }
        }

}
