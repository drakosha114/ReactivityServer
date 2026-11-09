using Domain;
using MediatR;
using Persistence;

namespace Application.Activities.Queries;

public class GetActivityById
{
    public class Query : IRequest<Activity?>
    {
        public string Id { get; set; } = string.Empty;
    }
    
    public class Handler(AppDbContext context) : IRequestHandler<Query, Activity?>
    {
        public async Task<Activity?> Handle(Query request, CancellationToken cancellationToken)
        {
           return  await context.Activities.FindAsync([request.Id], cancellationToken);
     
        }
    }
}