using System;
using System.Collections.Generic;

namespace Warehouse_API_Test.Models;

public partial class Set
{
    public int Id { get; set; }

    public int? BoxId { get; set; }

    public string Number { get; set; } = null!;

    public int NumberVariant { get; set; }

    public string Name { get; set; } = null!;

    public int Year { get; set; }

    public string? Theme { get; set; }

    public string? ThemeGroup { get; set; }

    public string? Subtheme { get; set; }

    public string? Category { get; set; }

    public int? Released { get; set; }

    public int? Pieces { get; set; }

    public int? Minifigs { get; set; }

    public int? ImageId { get; set; }

    public string? ImageThumbnailUrl { get; set; }

    public string? ImageImageUrl { get; set; }

    public int? LegocomId { get; set; }

    public string? LegocomUsRetailPrice { get; set; }

    public string? LegocomUsDateFirstAvailable { get; set; }

    public string? LegocomUsDateLastAvailable { get; set; }

    public string? LegocomUkRetailPrice { get; set; }

    public string? LegocomUkDateFirstAvailable { get; set; }

    public string? LegocomUkDateLastAvailable { get; set; }

    public string? LegocomCaRetailPrice { get; set; }

    public string? LegocomCaDateFirstAvailable { get; set; }

    public string? LegocomCaDateLastAvailable { get; set; }

    public string? LegocomDeRetailPrice { get; set; }

    public string? LegocomDeDateFirstAvailable { get; set; }

    public string? LegocomDeDateLastAvailable { get; set; }

    public string? PackagingType { get; set; }

    public string? Availability { get; set; }

    public int? InstructionsCount { get; set; }

    public int? AgeRangeId { get; set; }

    public int? AgeRangeMin { get; set; }

    public int? AgeRangeMax { get; set; }

    public float? DimensionsHeight { get; set; }

    public float? DimensionsWidth { get; set; }

    public float? DimensionsDepth { get; set; }

    public float? DimensionsWeight { get; set; }

    public string? BarcodeEan { get; set; }

    public string? BarcodeUpc { get; set; }

    public string? ExtendedDataBrickTags { get; set; }

    public virtual Box? Box { get; set; }
}
