namespace Intake.Common
{
    public class AwsOptions
    {
        public const string SectionName = "AWS";
        public string ResultBucketName { get; set; }
        public string CrvStatusBucketName { get; set; }
        public string CrvImportBucketName { get; set; }
    }
}
