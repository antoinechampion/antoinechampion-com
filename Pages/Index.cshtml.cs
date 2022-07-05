using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace antoinechampion_com.Pages
{
    public class WorkItem
    {
        public string Label { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public List<(string, string)> Links { get; set; } = new List<(string, string)>();
        public string ImageUri { get; set; }
        public string ImagePlaceholder { get; set; }
    }

    public class IndexModel : PageModel
    {
        public List<WorkItem> WorkItems;

        public IndexModel()
        {
            WorkItems = new List<WorkItem>
            {
                new WorkItem
                {
                    Label = "How to generate procedural music using code",
                    Type = "Article",
                    Description = "Creating a synthesizer that plays Tetris theme in 70 lines of C code.",
                    Links = {
                        ("Read on Medium", "https://medium.com/@antoine.champion/how-to-generate-music-using-code-c0413909f02") ,
                    },
                    ImageUri = "/images/thumbs/tetris-music.jpg",
                    ImagePlaceholder = "Music composer behind a computer"
                },
                new WorkItem
                {
                    Label = "Stockfish: Technical overview of a Chess Engine",
                    Type = "Article",
                    Description = "Stockfish, one of the best modern chess engines, is orders of magnitude stronger than DeepBlue.",
                    Links = {
                        ("Part 1: Generating candidate moves", "https://towardsdatascience.com/dissecting-stockfish-part-1-in-depth-look-at-a-chess-engine-7fddd1d83579") ,
                        ("Part 2: Evaluating a position", "https://towardsdatascience.com/dissecting-stockfish-part-2-in-depth-look-at-a-chess-engine-2643cdc35c9a") ,
                    },
                    ImageUri = "/images/thumbs/stockfish.jpg",
                    ImagePlaceholder = "Chess pieces"
                },
                new WorkItem
                {
                    Label = "Machine Code Metamorphism using Recurrent Neural Networks",
                    Type = "Scientific Paper",
                    Description = "Code metamorphism is a dynamic approach of obfuscation in which a program refactors "
                        + "itself at run time while preserving its semantics. This paper presents a new metamorphic framework based on a predictive model",
                    Links = {("View preprint", "/download/Machine_Code_Metamorphism_using_Recurrent_Neural_Networks.pdf") },
                    ImageUri = "/images/thumbs/metamorphism.jpg",
                    ImagePlaceholder = "Disassembler"
                },
                new WorkItem
                {
                    Label = "When you should use Constraint Solvers instead of Machine Learning",
                    Type = "Article",
                    Description = "Machine Learning and Deep Learning are ongoing buzzwords in the industry. "
                        + "Branding ahead of functionalities led to Deep Learning being overused "
                        + "in many artificial intelligence applications.",
                    Links = {("Read on Medium", "https://towardsdatascience.com/where-you-should-drop-deep-learning-in-favor-of-constraint-solvers-eaab9f11ef45") ,
                             ("View notebook", "https://colab.research.google.com/drive/1vFkt5yIQtyelqvCh2TsJ9UDeM5miXqui") },
                    ImageUri = "/images/thumbs/constraint-solvers.jpg",
                    ImagePlaceholder = "Rubik's cube"
                },
                new WorkItem
                {
                    Label = "Statistical Decision Theory for Predictive Models",
                    Type = "Article",
                    Description = "Can you write a predictive function from observations "
                        + "which would minimize the error deviation? Or the average error? "
                        + "Can you find the best theoretical predictor which would minimize any given loss function?",
                    Links = {("Read on Medium", "https://towardsdatascience.com/the-math-you-need-to-develop-your-own-predictive-models-fdb771cc1ddf") },
                    ImageUri = "/images/thumbs/statistical-decision-theory.jpg",
                    ImagePlaceholder = "Computer with statistical graphics"
                },
                new WorkItem
                {
                    Label = "Beware of the AI hype",
                    Type = "Article",
                    Description = "An Australian PhD candidate in artificial intelligence made a recent post "
                        + "on LinkedIn about his researches on SARS-CoV-2. The post gathered thousands of views, likes, and shares.",
                    Links = {("Read on Medium", "https://towardsdatascience.com/detecting-covid-19-with-97-accuracy-beware-of-the-ai-hype-9074248af3e1") },
                    ImageUri = "/images/thumbs/covid.jpg",
                    ImagePlaceholder = "Virus in 3D"
                },
                new WorkItem
                {
                    Label = "Optional: A Library for the R Programming Language",
                    Type = "Library",
                    Description = "This library package aims to add abilities to the R programming language such as nullable types "
                        + "and pattern matching in order to benefit from the functional programming paradigm.",
                    Links =
                    {
                        ("embed", "<img src=\"https://cranlogs.r-pkg.org/badges/optional\" alt=\"CRAN Downloads\" />"),
                        ("CRAN package", "https://cran.r-project.org/web/packages/optional/index.html"),
                        ("Github repository", "https://github.com/antoinechampion/optional"),
                    },
                    ImageUri = "/images/thumbs/optional.jpg",
                    ImagePlaceholder = "R programming language"
                },
                new WorkItem
                {
                    Label = "Musical compositions",
                    Type = "Music",
                    Description = "Collection of various musical compositions for classical Piano.",
                    Links = {
                         ("Nocturne in G Minor: Download sheet", "/download/1-2_nocturne_en_sol_mineur.pdf"),
                         ("Nocturne in F Minor: Download sheet", "/download/1-1_nocturne_en_fa_mineur.pdf"),
                         ("embed", "<iframe width=\"100%\" height=\"450\" scrolling=\"no\" frameborder=\"no\" allow=\"autoplay\" src=\"https://w.soundcloud.com/player/?url=https%3A//api.soundcloud.com/playlists/1301423938&color=%2354b176&auto_play=false&hide_related=false&show_comments=true&show_user=true&show_reposts=false&show_teaser=true\"></iframe><div style=\"font-size: 10px; color: #cccccc;line-break: anywhere;word-break: normal;overflow: hidden;white-space: nowrap;text-overflow: ellipsis; font-family: Interstate,Lucida Grande,Lucida Sans Unicode,Lucida Sans,Garuda,Verdana,Tahoma,sans-serif;font-weight: 100;\"><a href=\"https://soundcloud.com/antoine-champion\" title=\"Antoine Champion\" target=\"_blank\" style=\"color: #cccccc; text-decoration: none;\">Antoine Champion</a> · <a href=\"https://soundcloud.com/antoine-champion/sets/antoine-champion-piano\" title=\"Antoine Champion - Piano\" target=\"_blank\" style=\"color: #cccccc; text-decoration: none;\">Antoine Champion - Piano</a></div>"),
                    },
                    ImageUri = "/images/thumbs/piano.jpg",
                    ImagePlaceholder = "A piano"
                }

            };
        }

        public void OnGet()
        {

        }

    }
}
