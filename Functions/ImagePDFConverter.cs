using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using PdfiumViewer;
using PdfSharp.Drawing;

namespace Functions
{
    public class ImagePDFConverter
    {
		public bool ConvertPDFToImage(string pathPDF, string pathToSaveIMG, int dpi, long quality)
		{
			if (File.Exists(pathPDF))
			{
				try
				{
					using (var document = PdfDocument.Load(pathPDF))
					{
						var pageCount = document.PageCount;

						for (int i = 0; i < pageCount; i++)
						{
							using (var image = document.Render(i, dpi, dpi, PdfRenderFlags.CorrectFromDpi))
							{
								var encoder = ImageCodecInfo.GetImageEncoders().First(c => c.FormatID == ImageFormat.Jpeg.Guid);
								var encParams = new EncoderParameters(1);
								encParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
								image.Save(pathToSaveIMG + Path.GetFileNameWithoutExtension(pathPDF) + " - Pag. " + (i + 1).ToString("D6") + ".jpg", encoder, encParams);
							}
						}
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
					return false;
				}
				return true;
			}
			else
			{
				Console.WriteLine("Falha ao buscar o arquivo " + pathPDF + ".");
				return false;
			}
		}

		public bool ConvertImageToPDF(List<string> imageFileNameWithPath, string pdfFileNameWithPath, int width)
		{
			if (imageFileNameWithPath.Count > 0)
			{
				try
				{
					Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
					using (var document = new PdfSharp.Pdf.PdfDocument())
					{
						foreach (string imageFileName in imageFileNameWithPath)
						{
							PdfSharp.Pdf.PdfPage page = document.AddPage();
							using (XImage img = XImage.FromFile(imageFileName))
							{
								// Calculate new height to keep image ratio
								var height = (int)(((double)width / (double)img.PixelWidth) * img.PixelHeight);

								// Change PDF Page size to match image
								page.Width = width;
								page.Height = height;

								XGraphics gfx = XGraphics.FromPdfPage(page);
								gfx.DrawImage(img, 0, 0, width, height);
							}
						}
						document.Save(pdfFileNameWithPath);
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
					return false;
				}

				return true;
			}
			else
			{
				Console.WriteLine("Nenhuma imagem foi localizada na pasta.");
				return false;
			}
		}
	}
}
