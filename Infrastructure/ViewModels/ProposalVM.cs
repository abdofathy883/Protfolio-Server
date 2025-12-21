namespace Infrastructure.ViewModels
{
    public class ProposalVM
    {
        public string Title { get; set; }
        public string ProjectDescription { get; set; }
        public string ProjectCost { get; set; }
        public string TimelineRaw { get; set; }

        // Features with headlines and sub-features
        public List<FeatureSection> Features { get; set; } = new();

        // Phases with titles and descriptions
        public List<PhaseSection>? Phases { get; set; }

        public List<string> Timeline =>
            string.IsNullOrWhiteSpace(TimelineRaw)
            ? new List<string>()
            : TimelineRaw.Split('\n').Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).ToList();
    }

    public class FeatureSection
    {
        public string Headline { get; set; }
        public List<SubFeature> SubFeatures { get; set; } = new List<SubFeature>();
    }

    public class SubFeature
    {
        public string Title { get; set; }
        public string? BulletPointsRaw { get; set; }

        // Computed property for bullet points
        public List<string> BulletPoints =>
            string.IsNullOrWhiteSpace(BulletPointsRaw)
            ? new List<string>()
            : BulletPointsRaw.Split('\n').Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).ToList();
    }

    public class PhaseSection
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
