using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Taply.Models;
namespace Taply.Controllers;
public class PublicController(TaplyDbContext db, IConfiguration configuration) : Controller
{
 public async Task<IActionResult> Page(string slug) { Response.Headers.CacheControl="no-store, no-cache, must-revalidate"; Response.Headers.Pragma="no-cache"; var page=await db.Pages.Include(p=>p.Place).ThenInclude(p=>p!.SharedSections).Include(p=>p.Sections).FirstOrDefaultAsync(p=>p.Slug==slug&&p.IsActive&&p.Place!.IsActive); return page is null?NotFound():View(page); }
 public async Task<IActionResult> Section(string slug,string sectionSlug) { var exists=await db.Pages.AnyAsync(p=>p.Slug==slug&&p.IsActive&&p.Place!.IsActive&&p.Sections.Any(s=>s.IsVisible&&Slug(s.Title)==sectionSlug)); return exists?RedirectToAction(nameof(Page),new{slug}):NotFound(); }
 public IActionResult Qr(string slug) { var baseUrl=configuration["PublicBaseUrl"]?.TrimEnd('/')??$"{Request.Scheme}://{Request.Host}"; var url=$"{baseUrl}/p/{slug}"; using var gen=new QRCoder.QRCodeGenerator(); using var data=gen.CreateQrCode(url,QRCoder.QRCodeGenerator.ECCLevel.Q); var png=new QRCoder.PngByteQRCode(data); return File(png.GetGraphic(8),"image/png"); }
 public static string Slug(string s) { var norm=s.Normalize(System.Text.NormalizationForm.FormD); var chars=norm.Where(c=>System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c)!=System.Globalization.UnicodeCategory.NonSpacingMark).ToArray(); return System.Text.RegularExpressions.Regex.Replace(new string(chars).ToLowerInvariant(),"[^a-z0-9]+","-").Trim('-'); }
}
