using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using GenerateWordFile.Data;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;
using DocumentFormat.OpenXml;
using Microsoft.EntityFrameworkCore;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GenerateWordFile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordController : ControllerBase
    {
        private readonly AppDbContext _context;
        public WordController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateWordDocument(IFormFile file, int personId)
        {
            var person = await _context.People.FirstOrDefaultAsync(p => p.Id == personId);

            if (person == null)
            {
                return NotFound();
            }

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                stream.Position = 0;

                using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(stream, true))
                {
                    var body = wordDocument.MainDocumentPart.Document.Body;

                    foreach (var text in body.Descendants<Text>())
                    {
                        if (text.Text.Contains("{FirstName}"))
                        {
                            text.Text = text.Text.Replace("{FirstName}", person.FirstName);
                        }
                        if (text.Text.Contains("{LastName}"))
                        {
                            text.Text = text.Text.Replace("{LastName}", person.LastName);
                        }
                        else if (text.Text.Contains("{Birthday}"))
                        {
                            text.Text = text.Text.Replace("{Birthday}", person.Birthday.ToShortDateString());
                        }
                        else if (text.Text.Contains("{FatherName}"))
                        {
                            text.Text = text.Text.Replace("{FatherName}", person.FatherName);
                        }
                        else if (text.Text.Contains("{EmailAddress}"))
                        {
                            text.Text = text.Text.Replace("{EmailAddress}", person.EmailAddress);
                        }
                        else if (text.Text.Contains("{FinCode}"))
                        {
                            text.Text = text.Text.Replace("{FinCode}", person.FinCode);
                        }
                        else if (text.Text.Contains("{CardNumber}"))
                        {
                            text.Text = text.Text.Replace("{CardNumber}", person.CardNumber);
                        }
                    }

                    wordDocument.MainDocumentPart.Document.Save();
                }

                stream.Position = 0;
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "GeneratedPersonInfo.docx");
            }
        }
    }
}