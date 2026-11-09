using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities.Commands;

public class UpdateActivity
{
    public class Command : IRequest<string>
    {
        public required Activity Activity { get; set; }
    }
    
    public class Handler(AppDbContext context, IMapper mapper) : IRequestHandler<Command, string>
    {
        public async Task<string> Handle(Command request, CancellationToken cancellationToken)
        {
            var existingActivity = await context.Activities.FindAsync([request.Activity.Id], cancellationToken);
            if (existingActivity == null)
            {
                throw new Exception("Activity not found");
            }
            
            mapper.Map(request.Activity, existingActivity);
            
            await context.SaveChangesAsync(cancellationToken);
            return existingActivity.Id;
        }
    }
}