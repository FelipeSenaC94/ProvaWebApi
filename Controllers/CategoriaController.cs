using Microsoft.AspNetCore.Mvc;
using API.Data;
using API.Models;

namespace API.Controllers;

[Route("api/categoria")]
[ApiController]
public class CategoriaController : ControllerBase
{
    private readonly AppDataContext _context;

    public CategoriaController(AppDataContext context) =>
        _context = context;

    // GET: api/categoria/listar
    [HttpGet]
    [Route("listar")]
    public IActionResult Listar()
    {
        try
        {
            List<Categoria> categorias = _context.Categorias.ToList();
            return Ok(categorias);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    [Route("cadastrar")]
    public IActionResult Cadastrar([FromBody] Categoria categoria)
    {
        try
        {
            _context.Categorias.Add(categoria);
            _context.SaveChanges();
            return Created("", categoria);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    
    
    [HttpPatch]
    [Route("alterar/{id}")]
    public IActionResult Alterar([FromRoute] int id, [FromBody] Categoria Categoria)
    {
        try
        {
            //Expressões lambda
            Categoria? categoriaCadastrado =
                _context.Categorias.FirstOrDefault(x => x.CategoriaId == id);

            if (categoriaCadastrado != null)
            {

                Categoria? categoria = _context.Categorias.Find(categoriaCadastrado.CategoriaId);
                if (categoria == null)
                {
                    return NotFound();
                }
                categoriaCadastrado.Nome = categoria.Nome;
                _context.Categorias.Update(categoriaCadastrado);
                _context.SaveChanges();
                return Ok();
            }
            return NotFound();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }




}




}