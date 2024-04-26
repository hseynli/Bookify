using Bookify.Domain.Abstractions;
using MediatR;

namespace Bookify.Application.Abstractions.Messaging;

internal interface IQueryHandler<Tquery, TReponse> : IRequestHandler<Tquery, Result<TReponse>> where Tquery : IQuery<TReponse>
{
}