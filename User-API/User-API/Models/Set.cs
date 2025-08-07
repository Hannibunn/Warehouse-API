using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace User_API.Models
{
    public class Set : ISet
    {
        [Key]
        public int Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? setID { get; set; }
        public Box box { get; set; }
        public int? boxID { get; set; } = null;
        public string number { get; set; }
        public int numberVariant { get; set; }
        public string name { get; set; }
        public int year { get; set; }
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

        private LegoComInfo? _legoCom;
        public LegoComInfo LEGOCom
        {
            get => _legoCom ??= new LegoComInfo();
            set => _legoCom = value;
        }
        public ImagePreview? Image { get; set; }
        public AgeRangeInfo? AgeRange { get; set; }
        public Dimensions? Dimensions { get; set; }
        public Barcode? Barcode { get; set; }
        public ExtendedData? ExtendedData { get; set; }
    }
}
