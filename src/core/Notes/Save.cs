using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace core.Notes
{
    public class Save
    {
        public class Command : Add.Command
        {
            [Required]
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private IPortfolioStorage _storage;

            public Handler(IPortfolioStorage storage)
            {
                _storage = storage;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var note = await _storage.GetNote(request.UserId, request.Id);
                if (note == null)
                {
                    return new Unit();
                }

                note.Update(request.Note, request.RelatedToTicker, request.PredictedPrice);

                await _storage.Save(note);

                return new Unit();
            }
        }
    }
}