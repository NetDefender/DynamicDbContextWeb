using DynamicDbContextWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicDbContextWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly CascadeDeleteContext _dbContext;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public ParentTable Parent
        {
            get;
            set;
        }

        public Task OnGet()
        {
            return Task.CompletedTask;
        }
    }
}
