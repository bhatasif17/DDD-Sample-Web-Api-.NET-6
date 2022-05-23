using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using DDDSampleWebApi.DTOs;
using DDDSampleWebApi.Filters;
using DDDSampleWebApi.Models;
using DDDSampleWebApi.Persistence.Repositories;
using DDDSampleWebApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DDDSampleWebApi.Controllers;

[ApiController]
[Route("api/v1/")]
public class HikerController : ControllerBase
{
    private readonly IHikerService _hikerService;
    private readonly IHikerRepository _hikerRepository;
    private readonly IMapper _mapper;

    public HikerController(IHikerService hikerService, IHikerRepository hikerRepository, IMapper mapper)
    {
        _hikerService = hikerService;
        _hikerRepository = hikerRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Registers a new Hiker
    /// </summary>
    /// <param name="dto">Model</param>
    /// <returns>Secret Key</returns>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Post(HikerDto dto)
    {
        var hiker = _mapper.Map<Hiker>(dto);
        hiker.Secret = Guid.NewGuid().ToString();
        await _hikerService.RegisterHiker(hiker);
        return Created($"{HttpContext.Request.Scheme}/{HttpContext.Request.Host}/GetById/{hiker.Id}",hiker.Secret);
    }
    /// <summary>
    /// Get all hikers
    /// </summary>
    /// <returns>Hikers list</returns>
    [HttpGet("getAll")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
       var hikers = await _hikerService.GetAllHikers();
        return Ok(hikers);
    }
    /// <summary>
    /// Get a hiker by ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Hiker</returns>
    [HttpGet("GetById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById([NotNull] uint id)
    {
        if (id == 0) return NotFound("Id not found.");
        var hiker = await _hikerService.GetById(id);
        if (hiker is null) return NotFound("Id not found.");
        
        return Ok(hiker);
    }
    
    /// <summary>
    /// Update hiker's location - Uses secret key authorization
    /// </summary>
    /// <param name="longitude"></param>
    /// <param name="latitude"></param>
    /// <returns>Message</returns>
    [HttpPut("updateLocation")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorized]
    public IActionResult UpdateLocation([FromQuery] [NotNull] float longitude, [NotNull] float latitude)
    {
        var secret = HttpContext.Items["secret"] as string;
        _hikerRepository.UpdateLocation(longitude, latitude, secret);
        _hikerRepository.Save();
        
        return Ok("Location updated.");
    }
    
    /// <summary>
    /// Report a hiker as ill/injured/sick
    /// </summary>
    /// <param name="hikerId">Id of the injured/sick/ill hiker</param>
    /// <param name="illness">Illness of the hiker</param>
    /// <returns>Message</returns>
    [HttpPost("report")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorized]
    public async Task<IActionResult> Post([FromQuery] string username, string illness)
    {
        var reportedBy = HttpContext.Items["secret"] as string;

        if (! await _hikerRepository.HikerExists(username))
            return NotFound("Hiker not found");
        if (_hikerRepository.SelfReport(username, reportedBy))
            return Unauthorized("You cannot report yourself.");
        if(_hikerRepository.AlreadyReported(username,reportedBy))
            return Unauthorized("You have already reported.");
        
        _hikerService.Report(username, reportedBy, illness);
        _hikerRepository.Save();
        
        return Ok($"Hiker {username} reported as ill.");
    }

    /// <summary>
    /// Trade items between hikers
    /// </summary>
    /// <param name="dto">Model</param>
    /// <returns>Message</returns>
    [HttpPost("trade")]
    [Authorized]
    public async Task<IActionResult> TradeItems(TradeItemsDto dto)
    {
        if (!await _hikerRepository.HikerExists(dto.From.Username))
            return NotFound("Sender not found.");
        if (!await _hikerRepository.HikerExists(dto.To.Username))
            return NotFound("Receiver not found.");
        
        await _hikerService.TradeItems(dto);
        _hikerRepository.Save();

        return Ok("Items traded successfully.");
    }
    
    /// <summary>
    /// Generate report
    /// </summary>
    /// <returns>Report</returns>
    [HttpGet("getReport")]
    public async Task<IActionResult> GetReport()
    {
       var report = await _hikerService.GenerateReport();
        return Ok(report);
    }
}