using PDF_Creator_Test.Models;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SixLabors.ImageSharp.Formats.Png;

namespace PDF_Creator_Test.Documents
{
    public class PlaneDocument: IDocument
    {
        public Plane Model { get; }
        public PlaneDocument(Plane plane)
        {
            Model = plane;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container
            .Page(page =>
            {
                page.Margin(50);

                page.Header().Element(ComposeHeader);

                page.Content().Element(ComposeContent);

                page.Footer().Element(ComposeFooter);
            });
        }
        public void ComposeHeader(IContainer container)
        {
            container.Background(Colors.Grey.Lighten3).Padding(10).
                Row(row =>
                {
                    row.Spacing(5);

                    row.RelativeItem()
                        .Width(100)
                        .Height(100)
                        .Image("Logo.png");

                    row.RelativeItem().AlignCenter().Column(column =>
                    {
                        column.Spacing(5);
                        column.Item().Text("Mohawk College").FontSize(14);
                        column.Item().Text("Fake address 123").FontSize(12);
                        column.Item().Text("Fake phone number and email address").FontSize(12);
                    });
                });
        }

        public void ComposeFooter(IContainer container)
        {
            container.Background(Colors.Grey.Lighten3).Padding(10).Column(column =>
            {
                column.Spacing(5);
                column.Item().Text("Copyright " + DateTime.Now.Year).FontSize(12);
                column.Item().AlignRight().Text(x =>
                {
                    x.CurrentPageNumber().FontSize(12);
                    x.Span(" / ").FontSize(12);
                    x.TotalPages().FontSize(12);
                });
            });
        }

        void ComposeContent(IContainer container)
        {
            container.PaddingVertical(1).Column(column =>
            {
                column.Item().Element(ComposePlane);

                column.Item().AlignMiddle().AlignCenter().Element(ComposePicture);
            });
        }
        void ComposePlane(IContainer container)
        {
            container
               .PaddingVertical(5)
               .Background(Colors.Grey.Lighten3)
               .AlignCenter()
               .Column(column =>
               {
                   column.Item().Text("Manufacturer: " + Model.Manufacturer).FontSize(14);
                   column.Item().Text("Model: " + Model.Model).FontSize(14);
                   column.Item().Text("Max Speed: " + Model.MaxSpeed).FontSize(14);
                   column.Item().Text("Max Altitude: " + Model.MaxAltitude).FontSize(14);
                   column.Item().Text("Max Range: " + Model.MaxRange).FontSize(14);
                   column.Item().Text("Wingspan: " + Model.Wingspan).FontSize(14);
                   column.Item().Text("Length: " + Model.Length).FontSize(14);
                   column.Item().Text("Weight: " + Model.Weight).FontSize(14);

               });
        }

        void ComposePicture(IContainer container)
        {
            container
                .PaddingVertical(5)
                .AlignMiddle()

                .Column(col =>
                {

                    string signature = Model.Picture.Replace("data:image/png;base64,", "");
                    byte[] imageBytes = Convert.FromBase64String(signature);
                    using (var ms = new MemoryStream(imageBytes))
                    {
                        using (var image = Image.Load(ms))
                        {
                            var pngEncoder = new PngEncoder();
                            var outputStream = new MemoryStream();
                            image.Save(outputStream, pngEncoder);
                            var imageData = outputStream.ToArray();

                            col.Item()
                                .Background(Colors.White)
                                .BorderColor(Colors.Black)
                                .Border(1)
                                .MaxWidth(400)
                                .MaxWidth(300)
                                .Image(imageData);
                        }
                    }
                });
        }

    }
}
