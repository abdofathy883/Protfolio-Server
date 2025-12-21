using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Scriban;

namespace Infrastructure.Services
{
    public class LaTexEngine
    {
        private readonly IWebHostEnvironment webHostEnv;
        public LaTexEngine(IWebHostEnvironment environment)
        {
            webHostEnv = environment;
        }
        public async Task<string> RenderLatexAsync(ProposalVM vm)
        {
            var templatePath = Path.Combine(webHostEnv.ContentRootPath, "Templates", "Proposal.tex");
            var templateText = await File.ReadAllTextAsync(templatePath);

            // Add debugging
            Console.WriteLine($"Template content length: {templateText?.Length}");

            var template = Template.Parse(templateText);

            // Check for template errors
            if (template.HasErrors)
            {
                var errors = string.Join(", ", template.Messages.Select(m => m.Message));
                throw new InvalidOperationException($"Template parsing errors: {errors}");
            }

            // Prepare data for template
            var data = new
            {
                title = EscapeLatex(vm.Title ?? ""),
                projectDescription = EscapeLatex(vm.ProjectDescription ?? ""),
                projectCost = EscapeLatex(vm.ProjectCost ?? ""),
                timeline = vm.Timeline?.Select(EscapeLatex).ToList() ?? new List<string>(),
                features = vm.Features?.Select(f => new
                {
                    headline = EscapeLatex(f.Headline ?? ""),
                    subFeatures = f.SubFeatures?.Select(sf => new
                    {
                        title = EscapeLatex(sf.Title ?? ""),
                        bulletPoints = sf.BulletPoints?.Select(EscapeLatex).ToList() ?? new List<string>()
                    }).ToList()
                }).ToList(),
                phases = vm.Phases?.Select(p => new
                {
                    title = EscapeLatex(p.Title ?? ""),
                    description = EscapeLatex(p.Description ?? "")
                }).ToList()
            };

            return template.Render(data);
        }

        public static string EscapeLatex(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";

            return input
                .Replace(@"\", @"\textbackslash{}")
                .Replace("%", @"\%")
                .Replace("$", @"\$")
                .Replace("#", @"\#")
                .Replace("_", @"\_")
                .Replace("{", @"\{")
                .Replace("}", @"\}")
                .Replace("~", @"\textasciitilde{}")
                .Replace("^", @"\textasciicircum{}")
                .Replace("&", @"\&");
        }

    }
}

