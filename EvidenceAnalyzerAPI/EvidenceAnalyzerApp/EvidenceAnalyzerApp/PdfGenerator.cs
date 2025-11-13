using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Drawing;
using QuestPDF.Elements;
using System;
using System.Collections.Generic;
using System.IO;

namespace EvidenceAnalyzerApp
{
    public static class PdfGenerator
    {
        public static void GenerateAnalysisReport(List<Detail> details, string transcriptionText, string outputPath, string title)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            try
            {
                Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Margin(20);
                        page.Size(PageSizes.A4);

                        page.Header().AlignCenter().Text(title).FontSize(24).Bold();

                        page.Content().Column(content =>
                        {
                            // Transcription Section
                            content.Item().Text("Transcription:").FontSize(14).Bold().Underline();

                            content.Item().PaddingBottom(10).Text(text =>
                            {
                                text.DefaultTextStyle(x => x.FontSize(12).LineHeight(1.4f));
                                

                                var lines = transcriptionText.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

                                foreach (var line in lines)
                                {
                                    if (string.IsNullOrWhiteSpace(line))
                                    {
                                        text.Line("");
                                        continue;
                                    }

                                    var colonIndex = line.IndexOf(':');
                                    if (colonIndex != -1 && colonIndex < 40)
                                    {
                                        var before = line.Substring(0, colonIndex + 1);
                                        var after = line.Substring(colonIndex + 1);

                                        text.Span(before.Trim() + " ").SemiBold();
                                        text.Span(after.Trim());
                                    }
                                    else
                                    {
                                        text.Span(line);
                                    }

                                    text.Line("");
                                }
                            });

                            // Spacing and Object List Title
                            content.Item().PaddingVertical(10).Text("Detected Objects:")
                                .FontSize(14).Bold().Underline();

                            // Object list
                            content.Item().Column(col =>
                            {
                                foreach (var detail in details)
                                {
                                    col.Item().PaddingBottom(20).BorderBottom(1).Row(row =>
                                    {
                                        row.RelativeColumn(1).Element(imageCol =>
                                        {
                                            if (File.Exists(detail.ImagePath))
                                            {
                                                imageCol
                                                    .Image(detail.ImagePath)
                                                    .FitWidth();
                                            }
                                            else
                                            {
                                                imageCol.Text("[Image not found]").Italic();
                                            }
                                        });

                                        row.RelativeColumn(2).PaddingLeft(15).Column(info =>
                                        {
                                            info.Item().Text($"Object Name: {detail.ObjectName}").Bold();
                                            info.Item().Text($"Found Time: {detail.ObjectFoundTime}");
                                            info.Item().Text($"Duration: {detail.ObjectDuration}");
                                        });
                                    });
                                }
                            });
                        });

                        page.Footer().AlignCenter().Text(text =>
                        {
                            text.Span("Generated on ").FontSize(10);
                            text.Span(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")).SemiBold().FontSize(10);
                        });
                    });
                })
                .GeneratePdf(outputPath);
                MessageBox.Show($"An analysis report is generated on {outputPath}", "Analyis Report", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
             
            }
        }
        public static void GenerateAnalysisReport(List<ImageInterPretations> details, string outputPath, string title)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            try
            {
                Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Margin(20);
                        page.Size(PageSizes.A4);

                        page.Header().AlignCenter().Text(title).FontSize(24).Bold();

                        page.Content().Column(content =>
                        {
                            
                            // Spacing and Object List Title
                            content.Item().PaddingVertical(10).Text("Image Analysis")
                                .FontSize(14).Bold().Underline();

                            // Object list
                            content.Item().Column(col =>
                            {
                                foreach (var detail in details)
                                {
                                    col.Item().PaddingBottom(20).BorderBottom(1).Row(row =>
                                    {
                                        row.RelativeColumn(1).Element(imageCol =>
                                        {
                                            if (File.Exists(detail.ImagePath))
                                            {
                                                imageCol
                                                    .Image(detail.ImagePath)
                                                    .FitWidth();
                                            }
                                            else
                                            {
                                                imageCol.Text("[Image not found]").Italic();
                                            }
                                        });

                                        row.RelativeColumn(2).PaddingLeft(15).Column(info =>
                                        {
                                            info.Item().Text($"Image : {detail.ImagePath}").Bold();
                                            info.Item().Text($"Detedcted Objects: {string.Join(Environment.NewLine, detail.ImageObjects)}");
                                            info.Item().Text($"Interpretations: {detail.ImageSummary}");
                                        });
                                    });
                                }
                            });
                        });

                        page.Footer().AlignCenter().Text(text =>
                        {
                            text.Span("Generated on ").FontSize(10);
                            text.Span(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")).SemiBold().FontSize(10);
                        });
                    });
                })
                .GeneratePdf(outputPath);
                MessageBox.Show($"An analysis report is generated on {outputPath}", "Analyis Report", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {

            }
        }

    }
}



