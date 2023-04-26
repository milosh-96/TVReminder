using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using TVReminder.Web.Data;
using TVReminder.Web.Entities;

namespace TVReminder.Web.Pages.Channels
{
    public class CreateModel : PageModel
    {
        private readonly IMediator _mediator;

        public int RandomNumber { get; set; } = -1;


        [BindProperty]
        public Command Data { get; set; }

        public CreateModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task OnGetAsync() {
            this.RandomNumber = await _mediator.Send(
                new GetARandomNumber()
                {
                    Minimum = 5,
                    Maximum = 36
                }
                );
        }
        public async Task<IActionResult> OnPostAsync() {
            var channelId = await _mediator.Send(Data);
            TempData["Status"] = "Success! Channel with ID #" + channelId + " has been created.";
            return RedirectToPage("../Index");
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
                Channel channel = new Channel() { Name = request.Name };
                await _db.Set<Channel>().AddAsync(channel,cancellationToken);
                await _db.SaveChangesAsync(cancellationToken);
                return channel.Id;
            }
        }


        public class GetARandomNumber : IRequest<int> {
            public int Minimum { get; set; } = 0;
            public int Maximum { get; set; } = 255;
        }
        public class GetARandomNumberHandler : IRequestHandler<GetARandomNumber, int>
        {
            public Task<int> Handle(GetARandomNumber request, CancellationToken cancellationToken)
            {
                return Task.FromResult(new Random().Next(request.Minimum,request.Maximum));
            }
        }
    }


}
