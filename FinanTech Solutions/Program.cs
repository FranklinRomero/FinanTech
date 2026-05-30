using FinanTech_Solutions.Application.Interfaces;
using FinanTech_Solutions.Application.Services;
using FinanTech_Solutions.Domain.Interfaces;
using FinanTech_Solutions.Infrastructure.Builders;
using FinanTech_Solutions.Infrastructure.DataSources;
using FinanTech_Solutions.Infrastructure.Decorators;
using FinanTech_Solutions.Infrastructure.Delivery;
using FinanTech_Solutions.Infrastructure.Factories;
using FinanTech_Solutions.Infrastructure.Formatters;
using FinanTech_Solutions.Infrastructure.Strategies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddSingleton<FinancialDataRepository>();

builder.Services.AddTransient<IReportStrategy, ExecutiveReportStrategy>();
builder.Services.AddTransient<IReportStrategy, AuditorReportStrategy>();
builder.Services.AddTransient<IReportStrategy, AnalystReportStrategy>();
builder.Services.AddTransient<IReportStrategyFactory, ReportStrategyFactory>();

builder.Services.AddTransient<IReportContentDecorator, HeaderDecorator>();
builder.Services.AddTransient<IReportContentDecorator, WatermarkDecorator>();
builder.Services.AddTransient<IReportContentDecorator, EncryptionDecorator>();
builder.Services.AddTransient<IReportContentDecorator, CompressionDecorator>();

builder.Services.AddTransient<IReportFormatter, PdfReportFormatter>();
builder.Services.AddTransient<IReportFormatter, ExcelReportFormatter>();
builder.Services.AddTransient<IReportFormatter, CsvReportFormatter>();
builder.Services.AddTransient<IReportFormatterFactory, ReportFormatterFactory>();

builder.Services.AddTransient<IReportDelivery, EmailReportDelivery>();
builder.Services.AddTransient<IReportDelivery, SharedFolderReportDelivery>();
builder.Services.AddTransient<IReportDelivery, ApiReportDelivery>();
builder.Services.AddTransient<IReportDeliveryFactory, ReportDeliveryFactory>();

builder.Services.AddTransient<IReportBuilder, ReportBuilder>();
builder.Services.AddTransient<IReportOrchestrator, ReportOrchestrator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
