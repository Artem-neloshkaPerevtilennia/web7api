using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace EventApi;

[ApiController]
[Route("api/events")]
public class EventController : ControllerBase
{
	[HttpPost("save-immediate")]
	public IActionResult SaveImmediate([FromBody] Event? newEvent)
	{
		const string filePath = "events_immediate.json";
		if (newEvent == null) return BadRequest(new { message = "Invalid data" });

		newEvent.Timestamp = DateTime.UtcNow;
		if (System.IO.File.Exists(filePath))
		{
			var jsonString = System.IO.File.ReadAllText(filePath);
			var events = JsonSerializer.Deserialize<List<Event>>(jsonString);
			events!.Add(newEvent);
			jsonString = JsonSerializer.Serialize(events, new JsonSerializerOptions { WriteIndented = true });
			System.IO.File.WriteAllText(filePath, jsonString);
		}
		else
		{
			var events = new List<Event> { newEvent };
			var jsonString = JsonSerializer.Serialize(events, new JsonSerializerOptions { WriteIndented = true });
			System.IO.File.WriteAllText(filePath, jsonString);
		}

		return Ok(new { message = "Event saved immediately!" });
	}

	[HttpPost("save-batch")]
	public IActionResult SaveBatch([FromBody] List<Event>? newEvents)
	{
		const string filePath = "events_batch.json";
		if (newEvents == null || newEvents.Count == 0) return BadRequest(new { message = "No events provided" });

		foreach (var ev in newEvents)
		{
			ev.Timestamp = DateTime.UtcNow;
		}

		if (System.IO.File.Exists(filePath))
		{
			var jsonString = System.IO.File.ReadAllText(filePath);
			var events = JsonSerializer.Deserialize<List<Event>>(jsonString);
			events!.AddRange(newEvents);
			jsonString = JsonSerializer.Serialize(events, new JsonSerializerOptions { WriteIndented = true });
			System.IO.File.WriteAllText(filePath, jsonString);
		}
		else
		{
			var events = newEvents;
			var jsonString = JsonSerializer.Serialize(events, new JsonSerializerOptions { WriteIndented = true });
			System.IO.File.WriteAllText(filePath, jsonString);
		}

		return Ok(new { message = "Batch of events saved successfully!" });
	}

	[HttpGet]
	public IActionResult GetEvents([FromQuery] string mode)
	{
		var filePath = mode switch
		{
			"immediate" => "events_immediate.json",
			"batch" => "events_batch.json",
			_ => null
		};

		if (filePath == null || !System.IO.File.Exists(filePath))
		{
			return Ok(new List<Event>());
		}

		var jsonString = System.IO.File.ReadAllText(filePath);
		var events = JsonSerializer.Deserialize<List<Event>>(jsonString);
		return Ok(events);
	}

	public class Event
	{
		public string? Name { get; set; }
		public string? Details { get; set; }
		public DateTime? Timestamp { get; set; }
	}
}
