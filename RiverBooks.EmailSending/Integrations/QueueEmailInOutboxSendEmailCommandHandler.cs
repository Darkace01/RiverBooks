using Ardalis.Result;
using MediatR;
using RiverBooks.EmailSending.Contracts;

namespace RiverBooks.EmailSending.Integrations;

internal class QueueEmailInOutboxSendEmailCommandHandler(IOutboxService outboxService) :
  IRequestHandler<SendEmailCommand, Result<Guid>>
{
  public async Task<Result<Guid>> Handle(SendEmailCommand request, CancellationToken cancellationToken)
  {
    var newEntity = new EmailOutboxEntity()
    {
      Body = request.Body,
      From = request.From,
      To = request.To,
      Subject = request.Subject,
    };
    await outboxService.QueueEmailForSending(newEntity);

    return newEntity.Id;
  }
}
