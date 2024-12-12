namespace DevTask.CreditProcessor.Application.Credits.StatusReport;

public record StatusReportDto(ReportItem Paid, ReportItem AwatingPayment);

public record ReportItem(decimal Amount, decimal Percent);
