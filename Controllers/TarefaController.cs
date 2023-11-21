using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Models;

namespace API.Controllers;

[Route("api/tarefa")]
[ApiController]
public class TarefaController : ControllerBase
{
    private readonly AppDataContext _context;

    public TarefaController(AppDataContext context) =>
        _context = context;

    // GET: api/tarefa/listar
    [HttpGet]
    [Route("listar")]
    public IActionResult Listar()
    {
        try
        {
            List<Tarefa> tarefas = _context.Tarefas.Include(x => x.Categoria).ToList();
            return Ok(tarefas);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // POST: api/tarefa/cadastrar
    [HttpPost]
    [Route("cadastrar")]
    public IActionResult Cadastrar([FromBody] Tarefa tarefa)
    {
        try
        {
            Categoria? categoria = _context.Categorias.Find(tarefa.CategoriaId);
            if (categoria == null)
            {
                return NotFound();
            }
            tarefa.Categoria = categoria;
            _context.Tarefas.Add(tarefa);
            _context.SaveChanges();
            return Created("", tarefa);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPatch]
    [Route("alterar/{id}")]
    public IActionResult Alterar([FromRoute] int id, [FromBody] Tarefa Tarefa)
    {
        try
        {
            //Expressões lambda
            Tarefa? tarefaCadastrado =
                _context.Tarefas.FirstOrDefault(x => x.TarefaId == id);

            if (tarefaCadastrado != null)
            {

                Tarefa? tarefa = _context.Tarefas.Find(tarefaCadastrado.TarefaId);
                if (tarefa == null)
                {
                    return NotFound();
                }
                tarefaCadastrado.Titulo = tarefa.Titulo;
                tarefaCadastrado.Descricao = tarefa.Descricao;
                _context.Tarefas.Update(tarefaCadastrado);
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


    [HttpGet]
    [Route("concluidas")]
    public IActionResult ListarConcluidas()
    {
        try
        {
            Tarefa? tarefaCadastrado = _context.Tarefas.FirstOrDefault(x => x.Concluida == true);
            List<Tarefa> tarefas = _context.Tarefas.Include(x => x.Categoria).ToList();
            return Ok(tarefas);     
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet]
    [Route("naoconcluidas")]
    public IActionResult ListarNaoConcluidas()
    {
        try
        {
            Tarefa? tarefaCadastrado = _context.Tarefas.FirstOrDefault(x => x.Concluida == false);
            List<Tarefa> tarefas = _context.Tarefas.Include(x => x.Categoria).ToList();
            return Ok(tarefas);     
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}