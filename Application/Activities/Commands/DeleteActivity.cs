using MediatR;
using Persistence;

namespace Application.Activities.Commands;

public class DeleteActivity
{
    public class Command : IRequest<string>
    {
        public required string Id { get; set; }
    }
    
    public class Handler(AppDbContext context) : IRequestHandler<Command, string>
    {
        public async Task<string> Handle(Command request, CancellationToken cancellationToken)
        {
            var existingActivity = await context.Activities.FindAsync([request.Id], cancellationToken);
            if (existingActivity == null)
            {
                throw new Exception("Activity not found");
            }

            context.Activities.Remove(existingActivity);
            await context.SaveChangesAsync(cancellationToken);
            return existingActivity.Id;
        }
    }
}