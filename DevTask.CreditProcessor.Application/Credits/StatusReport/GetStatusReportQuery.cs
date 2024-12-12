using MediatR;

namespace DevTask.CreditProcessor.Application.Credits.StatusReport;

public record GetStatusReportQuery : IRequest<StatusReportDto>;