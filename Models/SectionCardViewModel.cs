using System.Text.Json;

namespace Taply.Models;

public sealed class SectionCardViewModel
{
    public PageSection Section { get; init; } = null!;
    public string Type => Section.Type.ToLowerInvariant();
    public string Title => Section.Title;
    public string? Subtitle => Section.Subtitle;
    public string Icon => string.IsNullOrWhiteSpace(Section.Icon) ? "bi-info-circle" : Section.Icon;
    public string Text { get; private init; } = "";
    public string Ssid { get; private init; } = "";
    public string Password { get; private init; } = "";
    public string Phone { get; private init; } = "";
    public string Zalo { get; private init; } = "";
    public string ZaloUrl { get; private init; } = "";
    public string Code { get; private init; } = "";
    public string Note { get; private init; } = "";
    public string CheckoutTime { get; private init; } = "";
    public bool IsSensitive { get; private init; }
    public IReadOnlyList<string> Items { get; private init; } = [];
    public string? ImageUrl => Section.ImageUrl;
    public string? VideoUrl => Section.VideoUrl;

    public static SectionCardViewModel From(PageSection section)
    {
        var model = new SectionCardViewModel { Section = section, Text = section.Content ?? "" };
        if (string.IsNullOrWhiteSpace(section.Content)) return model;
        try
        {
            using var document = JsonDocument.Parse(section.Content);
            var root = document.RootElement;
            if (root.ValueKind != JsonValueKind.Object) return model;
            string Read(string key) => root.TryGetProperty(key, out var value) && value.ValueKind == JsonValueKind.String ? value.GetString() ?? "" : "";
            var items = new List<string>();
            foreach (var key in new[] { "items", "steps", "rules" })
                if (root.TryGetProperty(key, out var list) && list.ValueKind == JsonValueKind.Array)
                    items.AddRange(list.EnumerateArray().Where(x => x.ValueKind == JsonValueKind.String).Select(x => x.GetString() ?? ""));
            return new SectionCardViewModel
            {
                Section = section, Text = Read("text") is { Length: > 0 } text ? text : section.Content,
                Ssid = Read("ssid"), Password = Read("password"), Phone = Read("phone"), Zalo = Read("zalo"),
                Code = Read("code"), Note = Read("note"), ZaloUrl = Read("zaloUrl"),
                CheckoutTime = Read("time") is { Length: > 0 } time ? time : Read("checkoutTime"),
                IsSensitive = root.TryGetProperty("isSensitive", out var sensitive) && sensitive.ValueKind == JsonValueKind.True,
                Items = items
            };
        }
        catch (JsonException) { return model; }
    }

    public static SectionCardViewModel From(PlaceSection section) => From(new PageSection
    {
        Type = section.Type,
        Title = section.Title,
        Subtitle = section.Subtitle,
        Icon = section.Icon,
        Content = section.Content,
        ImageUrl = section.ImageUrl,
        VideoUrl = section.VideoUrl,
        ButtonText = section.ButtonText,
        ButtonUrl = section.ButtonUrl,
        SortOrder = section.SortOrder,
        IsVisible = section.IsVisible
    });
}
