using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Week5Project.Pages
{
    public class IndexModel : PageModel
    {
        public static List<ClassInformationModel> ClassList { get; set; } = new List<ClassInformationModel>();
        private static int _nextId = 1;

        private const int PageSize = 10; // Items per page

        [BindProperty]
        public ClassInformationModel NewClass { get; set; } = new ClassInformationModel();

        [BindProperty]
        public bool IsEditing { get; set; }

        // Filter properties
        [BindProperty(SupportsGet = true)]
        public string ClassNameFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? MinStudentFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? MaxStudentFilter { get; set; }

        // Pagination properties
        public PaginatedList<ClassInformationTable> PaginatedClasses { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public void OnGet(int pageIndex = 1)
        {
            if (!ClassList.Any())
            {
                GenerateSampleData();
            }

            var filteredData = ClassList.AsQueryable();

            if (!string.IsNullOrEmpty(ClassNameFilter))
            {
                filteredData = filteredData.Where(c => c.ClassName.Contains(ClassNameFilter));
            }

            if (MinStudentFilter.HasValue)
            {
                filteredData = filteredData.Where(c => c.StudentCount >= MinStudentFilter.Value);
            }

            if (MaxStudentFilter.HasValue)
            {
                filteredData = filteredData.Where(c => c.StudentCount <= MaxStudentFilter.Value);
            }

            var tableData = filteredData.Select(c => new ClassInformationTable
            {
                ClassName = c.ClassName,
                StudentCount = c.StudentCount,
                Description = c.Description,
                HiddenId = c.Id
            }).AsQueryable();

            PaginatedClasses = PaginatedList<ClassInformationTable>.Create(tableData, pageIndex, PageSize);

            CurrentPage = pageIndex;
            TotalPages = (int)Math.Ceiling(filteredData.Count() / (double)PageSize);
        }

        public IActionResult OnPostAdd()
        {
            if (!ModelState.IsValid)
            {
                OnGet();
                return Page();
            }

            NewClass.Id = _nextId++;
            ClassList.Add(NewClass);
            NewClass = new ClassInformationModel();

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

            IsEditing = false;
            NewClass = new ClassInformationModel();
            return RedirectToPage();
        }

        private void GenerateSampleData()
        {
            var classNames = new[] { "Math", "Science", "History", "English", "Art", "Music", "Physics", "Chemistry", "Biology", "Geography" };
            var random = new Random();

            for (int i = 0; i < 100; i++)
            {
                var className = classNames[random.Next(classNames.Length)];
                var studentCount = random.Next(1, 101);
                var description = $"{className} class for {studentCount} students";

                ClassList.Add(new ClassInformationModel
                {
                    Id = _nextId++,
                    ClassName = $"{className} {i % 10 + 1}",
                    StudentCount = studentCount,
                    Description = description
                });
            }
        }
    }

    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);
        }

        public static PaginatedList<T> Create(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
