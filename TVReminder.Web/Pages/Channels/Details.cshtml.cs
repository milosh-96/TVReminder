using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TVReminder.Web.Data;
using TVReminder.Web.Entities;

namespace TVReminder.Web.Pages.Channels
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly IMediator _mediator;
        public Model Data { get; private set; }
        public DetailsModel(ApplicationDbContext db, IMediator mediator)
        {
            _db = db;
            _mediator = mediator;
        }

        public async Task OnGetAsync(int id)
        {
            Data = await _mediator.Send(new Query() { Id=id});
        }

        public record Model
        {
            public int Id { get; set; }
            public string Name { get; set; } = "";
        }
        public record Query : IRequest<Model>
        {
            public int Id { get; init; }
        }
        public class QueryHandler : IRequestHandler<Query, Model?>
        {
            private readonly ApplicationDbContext _db;

            public QueryHandler(ApplicationDbContext db)
            {
                _db = db;
            }

            public async Task<Model?> Handle(Query request, CancellationToken cancellationToken)
            {
                var channel = await _db.Set<Channel>().FirstOrDefaultAsync(x => x.Id == request.Id);
                return new Model()
                {
                    Id = channel.Id,
                    Name = channel.Name
                };
            }
        }
    }
}
