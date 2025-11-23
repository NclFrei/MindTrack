using System.Collections.Generic;

namespace MindTrack.Domain.DTOs.Response;

public class Link
{
 public string Href { get; set; } = string.Empty;
 public string Rel { get; set; } = string.Empty;
 public string Method { get; set; } = "GET";
}

public class PagedResult<T>
{
 public List<T> Items { get; set; } = new();
 public int Page { get; set; }
 public int PageSize { get; set; }
 public int TotalCount { get; set; }
 public int TotalPages { get; set; }
 public List<Link> Links { get; set; } = new();
}
