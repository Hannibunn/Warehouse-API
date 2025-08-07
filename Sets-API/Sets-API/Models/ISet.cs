namespace Sets_API.Models
{
    public class ISet
    {
        public int Id { get; set; }
        public int? setID { get; set; }
        public string number { get; set; }
        public int numberVariant { get; set; }
        public string name { get; set; }
        public string? theme { get; set; }
        public string? themeGroup { get; set; }
        public string? subTheme { get; set; }
        public string? category { get; set; }
        public bool? released { get; set; }
        public int? pieces { get; set; }
        public int? minifigs { get; set; }
        public string? packagingType { get; set; }
        public string? availability { get; set; }

        public int? instructionsCount { get; set; }
        public LegoComInfo LEGOCom { get; set; }
        public ImagePreview? Image { get; set; }
        public AgeRangeInfo? AgeRange { get; set; }
        public Dimensions? Dimensions { get; set; }
        public Barcode? Barcode { get; set; }
        public ExtendedData? ExtendedData { get; set; }
    }
}
