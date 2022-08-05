using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Domain;
using Infrastructure;

namespace Presentation.WebApp.Controllers;

public class juegoController : Controller
{
    private readonly ProductosDbContext _productosDbContext;
    public juegoController(IConfiguration configuration)
    {
        _productosDbContext = new ProductosDbContext(configuration.GetConnectionString("DefaultConnection"));
    }

    //[Authorize]
    public IActionResult Index()
    {
        var data = _productosDbContext.List();
        return View(data);
    }

    //[Authorize]
    public IActionResult Details(Guid id)
    {
        var data = _productosDbContext.Details(id);
        return View(data);
    }

    //[Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    //[Authorize(Roles = "Admin")]
    public IActionResult Create(Producto data)
    {
        _productosDbContext.Create(data);
        return RedirectToAction("Index");
    }

    //[Authorize(Roles = "Admin")]
    public IActionResult Edit(Guid id)
    {
        var data = _productosDbContext.Details(id);
        return View(data);
    }
    [HttpPost]
    //[Authorize(Roles = "Admin")]
    public IActionResult Edit(Producto data)
    {
        _productosDbContext.Edit(data);
        return RedirectToAction("Index");
    }

    //[Authorize(Roles = "Admin")]
    public IActionResult Delete(Guid id)
    {
        _productosDbContext.Delete(id);
        return RedirectToAction("Index");
    }
}