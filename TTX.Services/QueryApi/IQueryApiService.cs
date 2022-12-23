using System.Collections.Generic;

namespace TTX.Services.QueryApi;

/// <summary>
/// Primary In-Memory Db Storage and web request handler.
/// </summary>
public interface IQueryApiService
{
    List<string> Search(string keywords);

    // Metadata

    string? GetDescription(string uuid);

    string? GetMetadata(string uuid);

    string? GetIntegrityInfo(string uuid);

    string? GetMediaInfo(string uuid);

    // Tags

    string? GetTags(string uuid);

    int? AddTags(string uuid, string[] tags);

    int? RemoveTags(string uuid, string[] tags);

    // Primary data

    byte[]? GetFileSection(string uuid, long start, int length);

    // Thumbnails

    byte[]? GetThumbnail(string uuid);
}