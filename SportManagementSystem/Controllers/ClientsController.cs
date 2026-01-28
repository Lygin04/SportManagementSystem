using Microsoft.AspNetCore.Mvc;
using SportManagementSystem.Modules.Users.Contracts.Requests;
using SportManagementSystem.Modules.Users.Domain.Entities;
using SportManagementSystem.Modules.Users.Domain.Repositories;

namespace SportManagementSystem.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class ClientsController(IClientRepository clientRepository) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateClientRequest request, CancellationToken ct)
    {
        var client = new DbClient
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Patronymic = request.Patronymic,
            BirthDate = request.BirthDate,
            Phone = request.Phone,
            RegisterDate = DateTime.UtcNow,
        };
        await clientRepository.CreateAsync(client, ct);
        return CreatedAtAction("GetById", new { id = client.Id }, client);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await clientRepository.DeleteAsync(id, ct);
        return NoContent();
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(CreateClientRequest request, long id, CancellationToken ct)
    {
        var client = new DbClient
        {
            Id = id,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Patronymic = request.Patronymic,
            BirthDate = request.BirthDate,
            Phone = request.Phone,
            Modified = DateTime.UtcNow,
        };
        await clientRepository.UpdateAsync(client, ct);
        return NoContent();
    }
    
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        return Ok(await clientRepository.GetByIdAsync(id, ct));
    }
}