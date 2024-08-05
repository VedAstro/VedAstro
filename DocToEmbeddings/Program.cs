using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace DocToEmbeddings
{
    class Chunk
    {
        private string Text;
    }

    /// <summary>
    /// Simple program to convert text to chunks
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("App starting ");

            // location pdf file is stored
            var pdfFilePath = @"C:\Users\ASUS\Desktop\Projects\VedAstro\Others\NotCode\Books\Muhurtha Or Electional Astrology (text only).pdf";

            // convert pdf to raw text
            using (var pdfReader = new PdfReader(pdfFilePath))
            {
                var rawText = string.Concat(Enumerable.Range(1, pdfReader.NumberOfPages).Select(pageNumber =>
                {
                    var pageText = PdfTextExtractor.GetTextFromPage(pdfReader, pageNumber);
                    return pageText;
                }));
                Console.WriteLine(rawText);
            }

            // hold control
            Console.ReadLine();
        }
    }
}