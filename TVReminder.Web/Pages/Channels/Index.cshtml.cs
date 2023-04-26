using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TVReminder.Web.Data;
using TVReminder.Web.Entities;

namespace TVReminder.Web.Pages.Channels
{
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;

        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<Model> Data { get; private set; }
        public async Task OnGetAsync()
        {
            Data = await _mediator.Send(new Query() {});
        }

        public class Query : IRequest<List<Model>> { 
        }
        public class QueryHandler : IRequestHandler<Query, List<Model>>
        {
            private readonly ApplicationDbContext _db;

            public QueryHandler(ApplicationDbContext db)
            {
                _db = db;
            }

            public async Task<List<Model>> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await _db.Set<Channel>().ToListAsync();
                return result
                    .Select(x => new Model() { Id = x.Id, Name = x.Name })
                    .ToList();
            }
        }
        public record Model
        {
            public int Id { get; set; }
            public string Name { get; set; } = "";
        }
    }
}
