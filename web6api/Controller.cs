using Microsoft.AspNetCore.Mvc;

namespace web7;

[ApiController]
[Route("api")]
public class Controller : ControllerBase
{
	private static List<Record> _storage = [];

	[HttpPost("save")]
	public IActionResult SaveRecords([FromBody] Record? newRecord)
	{
		if (newRecord is null) return BadRequest(new { message = "Invalid data" });
		newRecord.Message = DateTime.Now.ToString("G") + newRecord.Message;
		_storage.Add(newRecord);
		return Ok(new { message = "Record saved" });
	}

	[HttpGet("get")]
	public IActionResult GetRecords()
	{
		_storage.Sort((r1, r2) => int.Parse(r1.Id!).CompareTo(int.Parse(r2.Id!)));
		if (_storage.Count == 0) return BadRequest(new { message = "Empty storage" });
		var storage = _storage;
		_storage = [];
		return Ok(storage);
	}

	public class Record(string? id, string? message)
	{
		public string? Id { get; } = id;

		public string? Message { get; set; } = message;
	}
}