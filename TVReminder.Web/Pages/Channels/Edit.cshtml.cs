using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TVReminder.Web.Data;
using TVReminder.Web.Entities;

namespace TVReminder.Web.Pages.Channels
{
    public class EditModel : PageModel
    {
        private readonly IMediator _mediator;

        public int RandomNumber { get; set; } = -1;


        [BindProperty]
        public Command Data { get; set; }

        public EditModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task OnGetAsync(Query query) {
            Data = await _mediator.Send(query);
            
        }
        public async Task<IActionResult> OnPostAsync() {
            var channelId = await _mediator.Send(Data);
            TempData["Status"] = "Success! Channel with ID #" + channelId + " has been updated.";
            return RedirectToPage("../Index");
        }
        public record Model
        {
            public int Id { get; set; }
            public string Name { get; set; } = "";
        }
        public record Query : IRequest<Command>
        {
            public int Id { get; init; }
        }
        public class QueryHandler : IRequestHandler<Query, Command?>
        {
            private readonly ApplicationDbContext _db;

            public QueryHandler(ApplicationDbContext db)
            {
                _db = db;
            }

            public async Task<Command?> Handle(Query request, CancellationToken cancellationToken)
            {
                var channel = await _db.Set<Channel>().FirstOrDefaultAsync(x => x.Id == request.Id);
                return new Command() { 
                    Id = channel.Id,
                    Name = channel.Name
                };
            }
        }
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(m => m.Name).NotNull().Length(3, 50);
            }
        }

        public record Command : IRequest<int>
        {
            public int Id { get; set; }
            [StringLength(50, MinimumLength = 3)]
            [Required]
            public string Name { get; init; } = "";
        }
        public class CommandHandler : IRequestHandler<Command, int>
        {
            private readonly ApplicationDbContext _db;

            public CommandHandler(ApplicationDbContext db)
            {
                _db = db;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                Channel channel = await _db.Set<Channel>().FirstOrDefaultAsync(x=>x.Id==request.Id,cancellationToken);
                channel.Name = request.Name;
                await _db.SaveChangesAsync(cancellationToken);
                return channel.Id;
            }
        }


        
       
    }


}
