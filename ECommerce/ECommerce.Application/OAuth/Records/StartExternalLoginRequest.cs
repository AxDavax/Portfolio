using MediatR;

namespace ECommerce.Application.OAuth.Records;

public record StartExternalLoginRequest(string Provider) : IRequest<StartExternalLoginResponse>;