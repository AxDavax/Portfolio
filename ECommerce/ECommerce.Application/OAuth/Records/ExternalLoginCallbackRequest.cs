using ECommerce.Application.OAuth.UseCases;
using MediatR;

namespace ECommerce.Application.OAuth.Records;

public record ExternalLoginCallbackRequest(string Provider, string Code, string State)
    : IRequest<ExternalLoginCallbackResponse>;