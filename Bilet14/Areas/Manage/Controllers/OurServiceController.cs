using Bilet14.DAL;
using Bilet14.Models;
using Bilet14.Utilities.Extensions;
using Bilet14.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Bilet14.Areas.Manage.Controllers;

[Area("Manage")]
[Authorize(Roles ="Admin")]
public class OurServiceController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public OurServiceController(AppDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    public IActionResult Index(int page = 1, int take = 3)
    {
        var items = _context.OurServices.Skip((page - 1) * take).Take(take).ToList();

        PaginateVM<OurService> paginateVM = new PaginateVM<OurService>()
        {
            Items = items,
            CurrentPage = page,
            PageCount = GetPageCount(take)
        };

        return View(paginateVM);

    }

    private int GetPageCount(int take)
    {
        int serviceCount = _context.OurServices.Count();

        return (int)Math.Ceiling((decimal)serviceCount / take);
    }


    #region Create
    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(OurService ourService)
    {
        if (!ModelState.IsValid) return View();

        if (ourService.ImageFile.CheckFileSize(200))
        {
            ModelState.AddModelError("ImageFile", "File size must be less than 200");
            return View();
        }

        if (!ourService.ImageFile.CheckFileType("image"))
        {
            ModelState.AddModelError("ImageFile", "File type must be image");
            return View();
        }

        ourService.Image = await ourService.ImageFile.SaveFileAsync(_environment.WebRootPath, "img");

        await _context.OurServices.AddAsync(ourService);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));


    }
    #endregion

    #region Edit
    public async Task<IActionResult> Edit(int id)
    {
        OurService? exists = await _context.OurServices.FirstOrDefaultAsync(x => x.Id == id);
        return View(exists);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(OurService ourService)
    {
        if (!ModelState.IsValid) return View();

        OurService? exists = await _context.OurServices.FirstOrDefaultAsync(x => x.Id == ourService.Id);

        if (exists == null)
        {
            ModelState.AddModelError("", "Service is not found!");
            return View();
        }

        if (ourService.ImageFile.CheckFileSize(200))
        {
            ModelState.AddModelError("", "File size must be less than 200");
            return View();
        }

        if (!ourService.ImageFile.CheckFileType("image"))
        {
            ModelState.AddModelError("", "File type must be image");
            return View();
        }

        exists.Image = await ourService.ImageFile.SaveFileAsync(_environment.WebRootPath, "img");
        exists.Title = ourService.Title;
        exists.Description = ourService.Description;

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));

    }
    #endregion

    #region Delete
    public async Task<IActionResult> Delete(int id)
    {
        OurService? exists = await _context.OurServices.FirstOrDefaultAsync(x => x.Id == id);

        if (exists == null)
        {
            ModelState.AddModelError("", "Service is not found!");
            return View();
        }

        _context.OurServices.Remove(exists);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));

    }
    #endregion



}